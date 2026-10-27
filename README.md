# Thunderbirds Board Game Engine Reference Data

Immutable game definitions, spreadsheet compilation, runtime catalogues, and
consumer test utilities, extracted from ThunderbirdsBoardGameEngine with relevant
Git history preserved. See [extraction provenance](docs/extraction/README.md).

## Projects

| Project | Purpose |
|---|---|
| ReferenceData.Core | Public definition types and identities; owned internal construction |
| ReferenceData.Runtime | Embedded snapshot hydration and read-only catalogues |
| ReferenceData.TestUtils | Test-only factories, builders, fixtures, fake catalogues, and DI registration |
| ReferenceData.Compiler | Spreadsheet validation and snapshot generation |

Core and Runtime keep their package IDs and public namespaces. Test utilities
use `ThunderbirdsBoardGameEngine.ReferenceData.TestUtils`. General xUnit theory
inputs are private linked test sources, not part of the published test package.

## Build and verify

Install a .NET 10 SDK satisfying `global.json`, then from this repository root:

```powershell
dotnet restore ThunderbirdsBoardGameEngine.ReferenceData.sln
dotnet build ThunderbirdsBoardGameEngine.ReferenceData.sln -c Release --no-restore
dotnet test ThunderbirdsBoardGameEngine.ReferenceData.sln -c Release --no-build --no-restore
dotnet format ThunderbirdsBoardGameEngine.ReferenceData.sln --verify-no-changes --no-restore
```

Restore uses nuget.org only. No application checkout, private application
packages, database, or cloud resources are required for these tests. CI verifies
Windows and Linux and produces candidate packages as workflow artifacts; it does
not publish to a package feed or Blob storage.

## Current delivery and release boundary

Runtime still loads its single embedded snapshot. Compilation/copying and archive
handling remain manual as described in the [compiler guide](tools/README.md).
CI's compiler component test validates a generated snapshot without replacing or
regenerating the committed embedded artifact. Extraction does not add Blob loading,
version selection, or game pinning.

Core and Runtime's imported declared versions are preserved for this local
extraction; already published versions must not be republished. CI overrides
candidate versions with `0.0.0-extraction.<run>` for artifact inspection only.
TestUtils starts at `0.1.0`. Stable release version selection and authenticated
publication are follow-up work. The original application remains unchanged until
candidate package consumption has been verified.

## Documentation

- [Reference Data overview](src/Readme.md)
- [Compiler operation and relationship validation](tools/README.md)
- [Versioning and compatibility policy](docs/architecture/decisions/0008-reference-data-versioning-and-compatibility.md)
- [Evolution roadmap](docs/architecture/reference-data-evolution.md)
- [Consumer test support](src/TestUtils/README.md)
Project directories consistently use short component names across src/, tests/, and tools/. Project filenames, assembly identities, namespaces, and package IDs retain their full names. This keeps directory context clear and generated paths short.

The dedicated repository uses src/, tests/, and tools/ directly, without an additional ReferenceData nesting level. The overview is src/Readme.md, the compiler guide is tools/README.md, and historical snapshots are in src/Archive/.

## Directory convention

```text
src/
  Core/
  Runtime/
  TestUtils/
  Archive/
tests/
  UnitTests/
    Core/
    Runtime/
    Compiler/
  ComponentTests/
    Runtime/
  Infrastructure/
tools/
  Compiler/
```

For example, `src/Runtime/ThunderbirdsBoardGameEngine.ReferenceData.Runtime.csproj`.