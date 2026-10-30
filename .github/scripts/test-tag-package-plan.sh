#!/usr/bin/env bash
set -euo pipefail
SCRIPT_DIRECTORY=$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)
"${PYTHON_COMMAND:-python3}" "$SCRIPT_DIRECTORY/test-tag-package-plan.py"
