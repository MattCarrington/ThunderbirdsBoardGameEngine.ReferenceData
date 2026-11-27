import importlib.util
import json
from pathlib import Path
import tempfile
import unittest
from unittest.mock import patch
import subprocess

spec = importlib.util.spec_from_file_location("selector", Path(__file__).with_name("select-release-snapshot.py"))
selector = importlib.util.module_from_spec(spec)
spec.loader.exec_module(selector)


class SnapshotSelectionTests(unittest.TestCase):
    def test_timestamp_difference_is_allowed_but_content_change_is_rejected(self):
        with tempfile.TemporaryDirectory() as directory:
            candidate = Path(directory) / "candidate.json"
            retained = Path(directory) / "retained.json"
            candidate.write_text(json.dumps(dict(contentVersion="2.0.0", generatedAt="today", cards=["a"])))
            retained.write_text(json.dumps(dict(contentVersion="2.0.0", generatedAt="yesterday", cards=["a"])))
            selector.ensure_same_content(candidate, retained)
            retained.write_text(json.dumps(dict(contentVersion="2.0.0", cards=["b"])))
            with self.assertRaises(ValueError):
                selector.ensure_same_content(candidate, retained)

    def test_api_authentication_failure_cannot_be_treated_as_missing_release(self):
        with tempfile.TemporaryDirectory() as directory:
            candidate = Path(directory) / "snapshot.json"
            candidate.write_text('{"contentVersion":"2.0.0"}')
            failure = subprocess.CompletedProcess([], 1, "", "Forbidden (HTTP 403)")
            with patch.dict(selector.os.environ, GITHUB_REPOSITORY="owner/repo"):
                with patch.object(selector.subprocess, "run", return_value=failure) as run:
                    with self.assertRaises(RuntimeError):
                        selector.process_snapshot(candidate, retain=True)
                    self.assertEqual(run.call_count, 1)

    def test_missing_release_is_only_created_when_explicitly_retaining(self):
        with tempfile.TemporaryDirectory() as directory:
            candidate = Path(directory) / "snapshot.json"
            candidate.write_text('{"contentVersion":"2.0.0"}')
            missing = subprocess.CompletedProcess([], 1, "", "Not Found (HTTP 404)")
            with patch.dict(selector.os.environ, GITHUB_REPOSITORY="owner/repo", GITHUB_SHA="commit"):
                with patch.object(selector.subprocess, "run", return_value=missing) as run:
                    selector.process_snapshot(candidate)
                    self.assertEqual(run.call_count, 1)
                with patch.object(selector.subprocess, "run", side_effect=[missing, None]) as run:
                    selector.process_snapshot(candidate, retain=True)
                    self.assertEqual(run.call_args.args[0][:3], ["gh", "release", "create"])
                    self.assertNotIn("--clobber", run.call_args.args[0])

    def test_existing_release_reuses_original_bytes(self):
        with tempfile.TemporaryDirectory() as directory:
            candidate = Path(directory) / "snapshot.json"
            candidate.write_text('{"contentVersion":"2.0.0","generatedAt":"today"}')
            original = b'{"contentVersion":"2.0.0","generatedAt":"yesterday"}'
            def mock_command(command, **kwargs):
                if command[1] == "api":
                    return subprocess.CompletedProcess(command, 0, "{}", "")
                destination = Path(command[command.index("--dir") + 1]) / "snapshot.json"
                destination.write_bytes(original)
                return subprocess.CompletedProcess(command, 0)
            with patch.dict(selector.os.environ, GITHUB_REPOSITORY="owner/repo"):
                with patch.object(selector.subprocess, "run", side_effect=mock_command):
                    selector.process_snapshot(candidate)
                    self.assertEqual(candidate.read_bytes(), original)

    def test_retention_cannot_replace_previously_published_bytes(self):
        with tempfile.TemporaryDirectory() as directory:
            candidate = Path(directory) / "snapshot.json"
            candidate.write_text('{"contentVersion":"2.0.0","generatedAt":"today"}')
            def mock_command(command, **kwargs):
                if command[1] == "api":
                    return subprocess.CompletedProcess(command, 0, "{}", "")
                destination = Path(command[command.index("--dir") + 1]) / "snapshot.json"
                destination.write_text('{"contentVersion":"2.0.0","generatedAt":"yesterday"}')
                return subprocess.CompletedProcess(command, 0)
            with patch.dict(selector.os.environ, GITHUB_REPOSITORY="owner/repo"):
                with patch.object(selector.subprocess, "run", side_effect=mock_command) as run:
                    with self.assertRaises(ValueError):
                        selector.process_snapshot(candidate, retain=True)
                    self.assertEqual(run.call_count, 2)


if __name__ == "__main__":
    unittest.main()
