# Reference Data Test Utilities

Test-only helpers for consumers of Reference Data Core and Runtime:

- `ReferenceDefinitionFactory` creates individual definitions.
- `ReferenceDataSnapshotBuilder` assembles snapshots for loader and validation tests.
- `Fakes` contains in-memory catalogue implementations accepting controlled definitions.
- `TestData` supplies representative fixtures, independent of production snapshot loading.
- `AddFakeCatalogs()` registers the fixture catalogues through dependency injection.

Namespaces begin with `ThunderbirdsBoardGameEngine.ReferenceData.TestUtils`.
Only test projects should reference this package. Fixtures are test inputs, not
an authoritative copy of production reference data. Tests can create explicit
cases using the factory and fake catalogue constructors instead of shared fixtures.

This assembly has owned constructor access to Core and Runtime. Application
production code obtains definitions through Runtime catalogues. The package has
no dependency on GameState, Rules, UI, xUnit, the compiler, or production snapshot
loading. The Runtime package remains a dependency for its public catalogue contracts.