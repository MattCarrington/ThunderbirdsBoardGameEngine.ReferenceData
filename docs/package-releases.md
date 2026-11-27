# Package releases

Package versions are declared in each project. Feature-branch pushes publish
`<version>-beta.<run-number>`; pushes to main publish the declared stable version.
Increment the version of every affected package before starting a new release line.
The package manifest selects changed packages and their dependent packages.
Commit titles do not determine whether a package is released.

Publication waits for the build/test matrix and CI script tests. After all selected
packages have been published and their exact versions restored and built in isolated
consumers, newly published stable packages receive repository tags:

- Core: `core/v<version>`
- Runtime: `runtime/v<version>`
- Testing: `testing/v<version>`

The prefixes are configured in `.github/packages.json`. Beta and skipped packages
receive no tags. Existing tags are never moved. Tag pushes do not trigger CI.

If publication succeeds but later verification or tagging fails, packages may already
exist without tags. The existing version script skips published stable versions on
rerun, so check the original release commit and complete verification/tag recovery
explicitly; do not bump versions or tag a different commit merely to retry.

After the cross-platform test matrix passes, the snapshot workflow compiles,
selects and validates one snapshot. The package job downloads that artifact and
embeds it before packing Runtime. Isolated consumers verify its SHA-256 against
the embedded resource in the published Runtime assembly.

Stable content is retained as the snapshot.json asset of a
`snapshot/v<content-version>` GitHub Release before Runtime publication.
Subsequent runs reuse those exact bytes and reject content changes that reuse
the version. This store is the interim delivery mechanism before Blob storage.
The [compiler guide](../tools/README.md) describes authoring and local validation.
