import importlib.util
import unittest
from pathlib import Path
from unittest.mock import patch

spec = importlib.util.spec_from_file_location('tagger', Path(__file__).with_name('tag-package-plan.py'))
tagger = importlib.util.module_from_spec(spec)
spec.loader.exec_module(tagger)


def package(version='4.0.1', publish=True, prefix='core/v'):
    return dict(version=version, should_publish=publish, tag_prefix=prefix)


class TagPlanTests(unittest.TestCase):
    def test_stable_packages_push_atomically(self):
        with patch.object(tagger, 'git', side_effect=['abc', '', '', '', '', '', '', '']) as git:
            tags = tagger.tag_plan([package(), package(prefix='runtime/v')], 'HEAD')
            self.assertEqual(tags, ['core/v4.0.1', 'runtime/v4.0.1'])
            git.assert_called_with('push', '--atomic', 'origin', 'refs/tags/core/v4.0.1', 'refs/tags/runtime/v4.0.1')

    def test_skipped_packages_do_not_tag(self):
        with patch.object(tagger, 'git', return_value='abc') as git:
            self.assertEqual(tagger.tag_plan([package(publish=False)], 'HEAD'), [])
            self.assertEqual(git.call_count, 1)

    def test_beta_is_rejected(self):
        with patch.object(tagger, 'git', return_value='abc'):
            with self.assertRaises(ValueError):
                tagger.tag_plan([package('4.0.1-beta.12')], 'HEAD')

    def test_same_commit_tag_is_idempotent(self):
        with patch.object(tagger, 'git', side_effect=['abc', '', 'core/v4.0.1', 'abc']):
            self.assertEqual(tagger.tag_plan([package()], 'HEAD'), [])

    def test_existing_tag_cannot_move(self):
        with patch.object(tagger, 'git', side_effect=['abc', '', 'core/v4.0.1', 'old']) as git:
            with self.assertRaises(ValueError):
                tagger.tag_plan([package()], 'HEAD')
            self.assertFalse(any(c.args[0] == 'push' for c in git.call_args_list))


if __name__ == '__main__':
    unittest.main()
