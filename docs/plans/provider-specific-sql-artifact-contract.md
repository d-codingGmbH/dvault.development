# Provider-Specific SQL Artifact Contract

Status: v1 planning contract
Ticket: 06F8KZTNG44XDPMVTVCV4WJSHG

## Purpose

This document defines the v1 contract for reviewed design-time provider-specific SQL or stored-procedure artifacts in DVault. It fixes the opt-in workflow, authoritative artifact format, review rules, deployment and invocation ownership, evidence prerequisites, and explicit non-goals.

The contract is architecture-level. It does not implement runtime dispatch, automatic deployment, automatic EF migration synchronization, or a new standalone DVault CLI.

## Source Of Truth Boundaries

Use these repository surfaces as the authoritative anchors for implementation:

| Responsibility | Source |
| --- | --- |
| Consumer-owned design-time host, single-project ownership, and no standalone DVault CLI | `docs/architecture/dvault-dotnet-ef-design-time-workflow.md`, `src/DCoding.Data.DVault/DataVaultDesignTimeCommand.cs` |
| Current provider-specific artifact gate and required prerequisite categories | `docs/performance-profiles.md` |
| Default explicit save-service boundary and no automatic stored-procedure dispatch | `docs/architecture/dvault-v1-explicit-save-service.md` |
| Current v0.32 review-only dry-run release summary | `docs/releases/v0.32.0.md` |
| Historical stored-procedure escape-hatch and non-goal wording | `docs/releases/v0.20.0.md`, `docs/releases/v0.26.0.md`, `docs/releases/v0.31.0.md` |
| Finite supported-provider baseline and provider profile facts | `docs/plans/provider-identifier-ddl-guardrail-contract.md`, `src/DCoding.Data.DVault/DataVaultAnnotationNames.cs` |
| Shared benchmark artifact and before/after evidence rules | `docs/plans/performance-evidence-benchmark-artifact-contract.md` |
| Existing reviewed artifact conventions and support-bundle ownership | `docs/model-first-governance.md`, `src/DCoding.Data.DVault/DataVaultSupportBundle.cs` |

## Supported Provider Baseline

The artifact lane is limited to the supported provider set already visible in the repository:

- SQLite
- PostgreSQL
- SQL Server
- MySQL
- Oracle

Each reviewed artifact manifest must bind exactly one provider and one representative workload. This contract does not add DB2 or any other new provider baseline, and it does not let an unrecognized provider inherit safety or performance claims from one of the supported profiles.

## Opt-In Workflow

The v1 workflow is consumer-owned and design-time only:

1. The application project that already owns the configured `DbContext`, `IDesignTimeDbContextFactory<TContext>`, migrations, and DVault design-time command host explicitly opts into artifact generation.
2. The consumer runs the normal reviewed design-time checks first: validation, artifact drift when a reviewed model artifact exists, migration guardrail review when schema changes are involved, and representative request-bound diagnostics for the exact provider and workload.
3. The consumer captures the required benchmark and semantic parity evidence for the same provider and representative workload.
4. A consumer-owned design-time command emits a deterministic dry-run artifact manifest for review. The current v0.32 lane stays dry-run only and must not deploy, invoke, or auto-register runtime dispatch.
5. The consumer reviews the manifest and any referenced SQL payload files in source control, then owns deployment, invocation, versioning, rollback, cleanup, environment selection, credentials, transaction policy, and operational observability outside DVault.

Artifact generation stays inside the existing single-project design-time boundary. It must not require a separate runtime service, a package-owned migration hook, or a standalone `dvault` executable.

## Artifact Format

The authoritative review surface is one deterministic manifest JSON document with schema version `dvault.sql-artifact.v1`.

The manifest is design-time output only. DVault does not auto-discover it at runtime, auto-route requests through it, or treat it as a replacement for `IDataVaultSaveService`, `IDataVaultReadService`, support bundles, benchmark artifacts, or EF migrations.

The manifest must carry enough reviewed metadata to make an artifact proposal auditable without relying on unstored local context:

- exact provider name and selected provider capability profile;
- representative workload label or scenario identity;
- authoritative metadata-source kind and fingerprint for the DVault model that the artifact was generated from;
- artifact kind for each entry, such as script or stored procedure;
- deterministic generated object identity and lifecycle intent for each entry;
- semantic parity scope and evidence references for the exact provider and workload;
- relative payload file paths and deterministic content hashes when executable SQL is emitted as sidecar files;
- explicit dry-run status when the output is a reviewed prototype rather than an executable deployment artifact.

The manifest must stay deterministic and automation-friendly:

- use ordinal comparisons and invariant formatting;
- avoid wall-clock timestamps, random ids, machine-specific paths, credentials, connection strings, raw business data, raw diagnostics text, provider exception text, and raw benchmark logs;
- preserve traceable reviewed identifiers such as provider/profile names, metadata-source fingerprint, workload label, and content hashes.

The manifest is the authoritative contract surface. SQL text, when emitted, should live in manifest-relative sidecar files so the metadata review surface and the provider-authored payload review surface remain explicit and diffable. Dry-run prototype work may emit manifest entries before the lane emits deployable SQL payload files.

## Review Rules

Artifact review follows the same posture as existing reviewed DVault artifacts:

- Store the manifest and any sidecar SQL files in a consumer-owned source-controlled location.
- Review changes like source code, not as opaque generated output.
- Reject manifests whose provider, provider profile, metadata-source fingerprint, workload label, or evidence references do not match the reviewed proposal.
- Reject manifests that include secrets, connection strings, raw request payloads, unsanitized provider messages, raw SQL capture unrelated to the reviewed artifact payload, or nondeterministic timestamps.
- Reject manifests that imply automatic runtime dispatch, automatic deployment, automatic migration synchronization, or unsupported-provider guarantees.

Reviewers must confirm that the artifact proposal stays within the bounded semantic contract already documented for DVault writes and reads. A reviewed artifact is an explicit opt-in escape hatch, not a second default runtime path.

## Consumer-Owned Responsibilities

The consuming application owns all operational responsibilities outside artifact generation:

- storage and retention policy for manifests and sidecar SQL files;
- deployment packaging and environment routing;
- invocation and transaction policy;
- versioning, rollback, and cleanup;
- credential handling and secret management;
- migration synchronization and model-change compatibility review;
- runtime observability, alerting, and failure handling;
- release-note and change-management communication for the adopting application.

DVault may define the reviewed artifact contract, but it does not assume DBA workflow ownership or application operations ownership.

## Evidence Gate

No provider-specific SQL or stored-procedure artifact proposal is implementation-ready until it has the evidence categories already required by `docs/performance-profiles.md` and the shared benchmark artifact contract.

At minimum, an artifact proposal must provide:

- one exact provider and one representative workload;
- request-bound diagnostics for the same provider and workload;
- benchmark evidence recorded under the shared benchmark artifact contract;
- an explicit semantic parity checklist covering ordering, load timestamp, record source, hash key, hash diff, satellite latest-state behavior, PIT or bridge maintenance when relevant, cancellation, cleanup, and caller-owned transaction behavior;
- a consumer-owned migration compatibility plan that does not rely on DVault automatic synchronization.

Tickets that do not have that evidence should remain contract, prototype, documentation, or measurement work. They must not claim a production-ready provider-specific artifact lane.

## Related Ticket Split

This contract is the parent refinement boundary for the current artifact-lane child tickets:

- `06F8KZVCVRPS3NAGQA7J55EAA4` defines the concrete benchmark and semantic parity evidence requirements.
- `06F8KZV18BQ0GN3CE4G02ATVA0` prototypes one SQL Server dry-run artifact manifest for one representative workload.
- `06F8KZVRARQPG482YKCQ686PNM` updates the v0.32 documentation and non-goal wording around the artifact lane.

Those tickets should refine implementation detail and evidence collection inside this contract instead of reopening the architecture boundary.

## Non-Goals

This contract does not:

- make provider-specific SQL or stored procedures a default DVault runtime path;
- add a runtime dispatcher, interceptor, scheduler, background worker, or automatic invocation surface;
- auto-generate, auto-deploy, auto-run, or auto-clean up provider-specific artifacts;
- automatically synchronize artifacts with EF migrations, live schema, metadata changes, model-first import or export, or support-bundle refreshes;
- add a standalone DVault CLI, startup-project discovery, or multi-project design-time orchestration;
- broaden the supported-provider baseline beyond SQLite, PostgreSQL, SQL Server, MySQL, and Oracle;
- replace the shared benchmark artifact contract, support-bundle contract, or existing provider-neutral save and read services;
- promise raw SQL capture, physical-plan capture, provider service-level objectives, or DBA automation as part of the artifact lane itself.

## V0.32 Documentation Decisions And Follow-Up

The v0.32 release and architecture docs summarize this contract as a current review-only dry-run manifest lane inside the existing consumer-owned design-time boundary. The first visible dry-run exporter is SQL Server for the `provider-native-bulk-ingestion` representative workload. That scope does not broaden the implemented exporter baseline to SQLite, PostgreSQL, MySQL, or Oracle.

These decisions remain intentionally deferred to later work and do not block this contract:

- the exact consumer repository path convention for storing reviewed manifests and sidecar SQL payloads;
- whether a future non-dry-run lane should emit deployable SQL payload files after the prototype and evidence tickets land.
