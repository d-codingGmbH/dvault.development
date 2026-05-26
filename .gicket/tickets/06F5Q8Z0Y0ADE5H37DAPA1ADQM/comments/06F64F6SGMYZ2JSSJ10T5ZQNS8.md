## Developer Delivery

Implemented staged-provider bulk fallback diagnostics as additive save explainability over the existing `IDataVaultSaveService` diagnostics and telemetry surfaces.

### Repository changes

- Added finite staged-provider lifecycle and provider-caveat vocabulary.
- Added bounded staged-provider bulk diagnostics carrying request, hub, link, satellite, and total operation counts without raw values.
- Extended existing save fallback causes and deterministic remediation text for dirty-context, unsupported-shape, transaction-participation, cleanup, and provider-limitation staged outcomes.
- Added an optional provider save-strategy diagnostics interface so provider strategies can report staged decline/fallback details without introducing a new save entrypoint.
- Surfaced staged diagnostics through save-strategy candidates, representative top-level save diagnostics, telemetry summaries, support-bundle JSON, display text, and low-cardinality meter tags.
- Added unit and integration coverage for telemetry explanations, staged counts, candidate ordering, selected-strategy preservation, support-bundle serialization, and redaction boundaries.

### Verification

- `dotnet build DVault.slnx --nologo` passed. The sandbox emitted `NU1900` warnings because the global NuGet vulnerability HTTP cache path is read-only.
- `dotnet test DVault.slnx --nologo` passed. External PostgreSQL, SQL Server, Oracle, and MySQL live tests remained skipped because their opt-in connection strings are not configured.
- `bash tools/check-format.sh` passed.