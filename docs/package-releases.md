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

The snapshot remains manually compiled and embedded in Runtime. This pipeline does
not compile or publish independent snapshots.
