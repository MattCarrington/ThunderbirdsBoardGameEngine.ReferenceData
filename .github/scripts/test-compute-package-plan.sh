#!/usr/bin/env bash

set -euo pipefail

SCRIPT_DIRECTORY=$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)
PLANNER="$SCRIPT_DIRECTORY/compute-package-plan.py"
PYTHON_COMMAND="${PYTHON_COMMAND:-python3}"
TEST_ROOT=$(mktemp -d)
trap 'rm -rf "$TEST_ROOT"' EXIT

PASSED=0

run_test() {
  local name="$1"
  local changed_files="$2"
  local expected="$3"
  local package_props_diff="${4:-}"
  local changed_file="$TEST_ROOT/changed-$PASSED.txt"
  local package_diff_file="$TEST_ROOT/package-diff-$PASSED.txt"
  local output_file="$TEST_ROOT/output-$PASSED.txt"

  printf '%s\n' "$changed_files" > "$changed_file"
  printf '%s\n' "$package_props_diff" > "$package_diff_file"

  CHANGED_FILES_PATH="$changed_file" \
    PACKAGE_PROPS_DIFF_PATH="$package_diff_file" \
    GITHUB_OUTPUT="$output_file" \
    "$PYTHON_COMMAND" "$PLANNER" >/dev/null

  actual=$(sed -n 's/^plan=//p' "$output_file" | "$PYTHON_COMMAND" -c \
    'import json, sys; print(json.dumps([item["package_id"] for item in json.load(sys.stdin)], separators=(",", ":")))')

  if [ "$actual" != "$expected" ]; then
    echo "FAIL: $name: expected $expected, got $actual" >&2
    exit 1
  fi

  PASSED=$((PASSED + 1))
  echo "PASS: $name"
}

run_test \
  "ReferenceData.Core selects Runtime" \
  "src/ReferenceData/ThunderbirdsBoardGameEngine.ReferenceData.Core/Model.cs" \
  '["ThunderbirdsBoardGameEngine.ReferenceData.Core","ThunderbirdsBoardGameEngine.ReferenceData.Runtime"]'

run_test \
  "ReferenceData.Runtime selects only itself" \
  "src/ReferenceData/ThunderbirdsBoardGameEngine.ReferenceData.Runtime/Runtime.cs" \
  '["ThunderbirdsBoardGameEngine.ReferenceData.Runtime"]'

run_test \
  "ReferenceData readme selects Runtime" \
  "src/ReferenceData/Readme.md" \
  '["ThunderbirdsBoardGameEngine.ReferenceData.Runtime"]'

run_test \
  "Test-only dependency selects no packages" \
  "Directory.Packages.props" \
  '[]' \
  '+    <PackageVersion Include="xunit.v3" Version="3.3.0" />'

run_test \
  "Unrelated changes select no packages" \
  "docs/package-lifecycle.md" \
  '[]'

echo "All $PASSED compute-package-plan tests passed."
