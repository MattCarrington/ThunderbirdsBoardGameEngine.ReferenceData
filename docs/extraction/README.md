# Extraction provenance

This repository was filtered from the original repository's merged `main` branch.
[provenance.json](provenance.json) records the original commit, filtered baseline,
tool version, and file/commit counts. [retained-paths.txt](retained-paths.txt)
records the path selection, including historical rename predecessors.
[commit-map.txt](commit-map.txt) maps original hashes to filtered hashes; zero
new hashes indicate commits pruned by filtering.

The clone used `--no-local --single-branch --branch main --no-tags`, with an
independent object store. `git filter-repo --paths-from-file` ran only inside
that clone. The source checkout and remote were not rewritten. All retained HEAD
file blobs and modes were checked against the original before standalone edits.

History includes relevant earlier enum and fake-catalogue paths without importing
entire former Catalog or Rules services. Unmerged branches and unrelated tags
were intentionally excluded. Relevant authors, dates, and change history remain;
commit hashes change as contents and ancestry are filtered. Historical filtered
commits are evidence, not a promise that each intermediate tree builds standalone.

Subsequent standalone changes move Reference Data helpers into their own owned
TestUtils assembly and link two generic theory-data files privately into tests.
Build/package settings are trimmed to this solution, with no dependency on the
application's source or package feed. Original package identities, embedded
snapshot bytes, spreadsheet bytes, and archived snapshots are preserved.

Older architecture records describe their historical context. Missing application
cross-references link to the original source revision rather than duplicating the
application's deployment or release documentation. General game-state concerns
remain owned by the application; Reference Data supplies the data contracts and
snapshot support required by later game-pinning work.
## Local verification

- Standalone restore from nuget.org and Release solution build passed.
- All 354 tests passed, including two consumer-isolation checks added for the split.
- `dotnet format --verify-no-changes --no-restore` passed.
- Core, Runtime, and TestUtils candidate packages were produced locally with
  version `0.0.0-extraction.1` and compatible transitive package dependencies.
- A separate package-only console consumer, using an isolated package cache,
  exercised the test factory/fake catalogue and embedded Runtime hydration.
- The Linux CI job is defined but has not been executed locally.

No snapshot schema, content, spreadsheet, or archive changes are part of extraction.
Source-to-package application migration and authenticated publishing remain future
work. Nothing has been pushed as part of these local checks.
The original imported test directory names produced a 261-character Debug editorconfig path. Test project folders were shortened without changing their depth, project filenames, assembly names, or namespaces; the solution paths were updated accordingly. Debug and Release validation cover the new locations.

All component directories now use short names consistently: src/Core, src/Runtime, src/TestUtils, and tools/Compiler, matching the test directory convention. Only file-system locations and their references change; public names and package identities remain the same.
