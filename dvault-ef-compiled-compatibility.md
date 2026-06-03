# DVault EF Compiled Compatibility

Status: relocated documentation entrypoint

The authoritative EF compiled compatibility note lives at [docs/architecture/dvault-ef-compiled-compatibility.md](docs/architecture/dvault-ef-compiled-compatibility.md).

This root entrypoint is retained for ticket and release validation surfaces that reference `dvault-ef-compiled-compatibility.md` directly. Use the architecture note for EF runtime-model, compiled-query, DbContext pooling, validation, and benchmark-boundary guidance.

For the v0.27.0 EF lifecycle analyzer boundary, this entrypoint also defers to the architecture note. `DMV1912` through `DMV1914` remain analyzer-only source-visible guardrails; they do not add runtime guards, runtime behavior changes, compiled-model generation, provider-specific lifecycle guarantees, cross-assembly inference, or whole-application inference.

For the v0.21.0 PIT and bridge documentation boundary, this file is adjacent compatibility context only. It does not add provider-specific PIT/bridge optimization claims, dynamic request compilation claims, new PIT read APIs, automatic maintenance, or delete-aware bridge maintenance.
