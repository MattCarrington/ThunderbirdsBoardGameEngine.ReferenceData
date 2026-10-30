#!/usr/bin/env python3
"""Tag newly published stable packages after consumption verification succeeds."""
import json
import os
import re
import subprocess


def git(*args):
    return subprocess.check_output(['git', *args], text=True).strip()


def tag_plan(plan, ref):
    commit = git('rev-parse', f'{ref}^{{commit}}')
    tags = []
    for package in plan:
        if not package.get('should_publish'):
            continue
        version = package['version']
        if not re.fullmatch(r'\d+\.\d+\.\d+', version):
            raise ValueError(f'Expected stable version, got {version}')
        tag = package['tag_prefix'] + version
        git('check-ref-format', 'refs/tags/' + tag)
        existing = git('tag', '--list', tag)
        if existing:
            if git('rev-parse', f'refs/tags/{tag}^{{commit}}') != commit:
                raise ValueError(f'{tag} already points to another commit; refusing to move it')
            continue
        tags.append(tag)
    # Check every tag before creating any. Push together or fail together.
    for tag in tags:
        git('tag', tag, commit)
    if tags:
        git('push', '--atomic', 'origin', *['refs/tags/' + tag for tag in tags])
    return tags


if __name__ == '__main__':
    tag_plan(json.loads(os.environ['PACKAGE_PLAN']), os.environ['GITHUB_SHA'])
