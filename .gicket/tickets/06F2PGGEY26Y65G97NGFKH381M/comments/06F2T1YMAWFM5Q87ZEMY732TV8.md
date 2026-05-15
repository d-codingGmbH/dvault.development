[gicket-bot] PO refinement contract

Summary
- Refinement ratifies the current v1 DVault design-time command baseline: a consumer-owned command surface for validate, export, drift, and guardrail, with focused CI examples split from broader v0.11 documentation rollout.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Ratify the current public v1 command surface already present on the branch: DataVaultDesignTimeCommand, DataVaultDesignTimeCommandHost, DataVaultDesignTimeExportSource, and the verbs validate, export, drift, and guardrail.
- Keep the supported design-time boundary consumer-owned and single-project: the application that owns the configured DbContext also owns IDesignTimeDbContextFactory<TContext>, the executable entrypoint, export-source selection, migration resolution, and dotnet ef invocation.
- Ratify the explicit export-source baseline already supported by the repository: export comes from Code-First declarations, DataVaultMetadataModel, or DataVaultMetadataRegistry, not reflective DbContext or ModelBuilder export.
- Use artifact-versus-design-time-model comparison as the default drift lane; live-schema drift stays opt-in through the existing live-schema APIs and must not become the default blocking gate.
- This story owns the command surface and its usage guidance; migration-rule taxonomy expansion, additional live-schema reader work, and broader release-note cleanup stay in separate tickets.

Scope In
- Provide or maintain the minimal reusable command surface in DCoding.Data.DVault for validate, export, drift, and guardrail.
- Keep hosting consumer-owned: the consumer project supplies the configured design-time DbContext factory, diagnostics service wiring, explicit export source, migration UpOperations resolver, and optional live-schema reader.
- Define deterministic parser, help, and exit-code behavior for the four verbs so local scripts and CI can treat the surface as automation-safe.
- Document the command surface in the existing design-time workflow and focused adopter examples/checklist guidance needed for pre-integration use.
- Cover the public command surface with unit tests and public API snapshot updates where applicable.

Scope Out
- No standalone DVault executable or dotnet tool package, no dotnet ef shim, no IDesignTimeServices integration, and no EF CLI interception.
- No startup-project or target-project split workflow, host auto-discovery, or multi-project design-time orchestration beyond the current single-project v1 boundary.
- No reflective export-from-DbContext or export-from-ModelBuilder path; export stays limited to the explicit source types already supported by DataVaultModelArtifactExporter.
- No new live-schema reader implementations or provider-support expansion in this story; the command surface only consumes the existing live-schema drift contract.
- No migration-rule taxonomy hardening, automatic migration execution, schema repair, SQL parsing, or broad v0.11 README and release-note consolidation beyond focused command-surface guidance.

Open questions
- none

Follow-up questions
- Should a later tooling ticket add optional JSON command output, or are deterministic text plus direct structured APIs sufficient for v1 consumers?
- Should the broader v0.11 documentation task add one consolidated consumer-host example in README or release notes, or keep the detailed wiring only in the design-time workflow document?
- Should future design-time work broaden startup-project and target-project split support, or keep the current single-project composition boundary as the long-term default?

Risks
- Over-designing the public command host or runner API beyond the current minimal surface would create avoidable long-term support obligations.
- If documentation teaches live-schema drift or export as the default blocking gate, adopters may build noisy or unsafe CI checks instead of using validate and reviewed-artifact drift by default.
- If command examples blur the consumer-owned boundary, teams may incorrectly assume DVault intercepts dotnet ef or applies migrations automatically.
- Broader v0.11 documentation and release-note rollout is separate work; if it slips, discoverability may lag behind the implemented command surface.

Split recommendations
- Keep the current split: one track for the core command implementation, one for CI and adopter examples, and one for broader v0.11 documentation and release-note cleanup.
- Keep migration-guardrail rule hardening and live-schema provider evolution outside this story so the command surface stays bounded to hosting and orchestration concerns.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 5
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment