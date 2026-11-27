#!/usr/bin/env python3
"""Reuse an existing snapshot release or retain a validated snapshot without overwriting."""
import argparse
import json
import os
from pathlib import Path
import re
import shutil
import subprocess
import tempfile
from urllib.parse import quote


def ensure_same_content(candidate, retained):
    expected = json.loads(Path(candidate).read_text(encoding="utf-8-sig"))
    actual = json.loads(Path(retained).read_text(encoding="utf-8-sig"))
    expected.pop("generatedAt", None)
    actual.pop("generatedAt", None)
    if expected != actual:
        raise ValueError("Published content version differs from compiler output. Increment the content version.")


def process_snapshot(path, retain=False):
    path = Path(path)
    snapshot = json.loads(path.read_text(encoding="utf-8-sig"))
    version = snapshot["contentVersion"]
    if not re.fullmatch(r"\d+\.\d+\.\d+", version):
        raise ValueError("Snapshot content version must be a stable semantic version.")
    repository = os.environ["GITHUB_REPOSITORY"]
    tag = f"snapshot/v{version}"
    result = subprocess.run(
        ["gh", "api", f"repos/{repository}/releases/tags/{quote(tag, safe='')}"],
        capture_output=True, text=True,
    )
    if result.returncode:
        if "(HTTP 404)" not in result.stderr:
            raise RuntimeError(result.stderr)
        if retain:
            subprocess.run([
                "gh", "release", "create", tag, str(path),
                "--repo", repository, "--target", os.environ["GITHUB_SHA"],
                "--title", f"Reference Data {version}", "--latest=false",
                "--notes", f"Validated snapshot from commit {os.environ['GITHUB_SHA']}. Preserve these bytes for future package builds.",
            ], check=True)
        return
    with tempfile.TemporaryDirectory() as directory:
        subprocess.run([
            "gh", "release", "download", tag, "--repo", repository,
            "--pattern", "snapshot.json", "--dir", directory,
        ], check=True)
        retained = Path(directory) / "snapshot.json"
        if retain:
            # Retention happens after validation. Never substitute different bytes now.
            if path.read_bytes() != retained.read_bytes():
                raise ValueError("Retained snapshot bytes differ from the validated artifact. Retry the workflow.")
        else:
            ensure_same_content(path, retained)
            shutil.copyfile(retained, path)


if __name__ == "__main__":
    parser = argparse.ArgumentParser()
    parser.add_argument("path")
    parser.add_argument("--retain", action="store_true")
    args = parser.parse_args()
    process_snapshot(args.path, args.retain)
