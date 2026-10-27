# ADR 0009: Independent Reference Data Repository

- Status: Accepted for the extraction slice
- Date: 2026-10-27

## Context

Reference Data has its own authoring input, compiler, snapshots, packages, and
tests. Its production lifecycle differs from application behaviour and deployment.
Project references and shared CI allow changes to several contracts to be made
together, obscuring independent consumption and release requirements.

Consumer tests already use factories, controlled fixtures, fake catalogue
implementations, and DI registration without loading the production snapshot.
Those helpers live inside an application-wide TestUtils assembly that also
references GameState and Rules. Moving that whole assembly would retain an
unwanted dependency back into the application.

## Decision

Own Reference Data in `ThunderbirdsBoardGameEngine.ReferenceData`, with a
standalone solution and build/test CI. Preserve existing Core and Runtime package
IDs, definition/identity namespaces, validation behaviour, and embedded snapshot
delivery during extraction. Automated snapshot publication and Blob retrieval
remain later slices under ADR 0008.

Publishable projects are Core, Runtime, and ReferenceData.TestUtils. The compiler
is an executable tool, not a NuGet package in this slice. ReferenceData.TestUtils
owns the definition factory, snapshot builder, fake catalogues, representative
fixtures, and registration extension. It uses the namespace and assembly prefix
`ThunderbirdsBoardGameEngine.ReferenceData.TestUtils` and has no application or
xUnit dependencies. Core and Runtime grant constructor access to that specific
owned assembly instead of the original shared TestUtils assembly.

Only consumer test projects should reference TestUtils. Production consumers
continue to obtain definitions through Runtime catalogue contracts. Fixtures
remain controlled test inputs, not the authority for production game facts.
Reference Data's own production-snapshot integration checks remain separate from
consumer isolation checks.

Two generic xUnit theory-data helpers are private linked sources in this
repository's tests. Do not publish them in ReferenceData.TestUtils or import
application-specific assertions/dependencies to support them.

Preserve relevant merged history by filtering a separate clone, including known
historical rename paths. Keep the source repository and checkout unchanged.
Record the source revision, filter selection, tool version, and hash mapping.
Historical filtered commits are not guaranteed to build independently.

Build/test CI verifies Windows and Linux and creates inspectable prerelease
candidate package artifacts. It does not publish to GitHub Packages or Blob.
Imported stable package versions must not be republished; candidate overrides are
for verification, with subsequent stable releases following the agreed policies.

Verify the standalone solution and a consumer using only generated packages
before migrating the application. Keep its projects and workflows until that
migration is validated; no cross-repository source project references are allowed
in the standalone solution.

## Consequences

- Reference Data can evolve independently while preserving catalogue consumption.
- Consumers gain owned test support without loading the production snapshot.
- Package adoption requires changing test namespaces and removing superseded
  local helpers in the application; that migration is separate from extraction.
- General test helpers are intentionally local until broader reuse justifies
  another package; no PublishedLanguage repository is introduced.
- Commit hashes change during filtering. Original authors, dates, relevant
  history, and the recorded source/hash mapping preserve traceability.
- The new CI definition must still be exercised on GitHub after publication.
  Local Windows verification does not prove the Linux job passed.

## Related documentation

- [Extraction provenance](../../extraction/README.md)
- [Construction boundary](0007-reference-data-owned-construction.md)
- [Versioning and compatibility](0008-reference-data-versioning-and-compatibility.md)
- [Repository overview](../../../README.md)