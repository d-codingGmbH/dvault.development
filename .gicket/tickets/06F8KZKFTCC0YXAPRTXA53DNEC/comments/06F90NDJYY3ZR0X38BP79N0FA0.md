[gicket-bot] PO refinement contract

Summary
- Refined the v0.28.0 provider read optimization docs ticket into a coordinated documentation-baseline rollover: current repository evidence supports a consistent v0.28.0 story across README, performance guidance, read-plan architecture notes, checklist, and release notes without additional PO clarification.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Repository evidence already bounds the provider matrix: SQLite is the only optimized latest-satellite read path with completed root benchmark timings, while PIT/bridge optimized read strategies exist for SQLite, PostgreSQL, SQL Server, MySQL, and Oracle through provider registrations, diagnostics coverage, benchmark guidance rows, and strategy-parity tests.
- Current active docs are inconsistent today: README, the production checklist, and the PIT/bridge boundary note still narrow optimized PIT/bridge read wording to SQLite/PostgreSQL/SQL Server, while performance profiles, provider packages, benchmark artifacts, and tests already include MySQL and Oracle PIT/bridge strategy paths.
- The root benchmark triplet preserves external-provider read rows as optional evidence that can remain skipped when provider connection strings are unset; v0.28.0 documentation must distinguish those skipped rows from completed SQLite timing rows instead of presenting them as unconditional live benchmark measurements.
- No child tickets, relation writes, description updates, attachments, or planning documents were materialized during this refinement run; existing ticket relations were only verified.

Scope In
- Create the v0.28.0 coordinated documentation baseline in docs/releases/v0.28.0.md and move current-baseline README/checklist guidance from v0.27.0 to v0.28.0 for provider read optimization topics.
- Align current adopter-facing and architecture docs on the supported provider matrix: SQLite-only optimized latest-satellite reads, plus diagnostics-gated PIT/bridge optimized read paths for SQLite, PostgreSQL, SQL Server, MySQL, and Oracle.
- Document the bounded evidence posture using the root benchmark triplet, benchmark scenario guidance rows, provider read strategy registrations, and existing diagnostics/parity tests.
- Explain provider-neutral fallback guidance for unsupported providers, unsupported request shapes, incomplete read-shape evidence, and stale PIT/bridge maintenance evidence through IDataVaultReadService and IDataVaultReadDiagnosticsService.
- Carry forward explicit non-goals around implicit maintenance, scheduling, raw SQL/query-plan disclosure, auto-index advice, and new provider-specific latest-satellite claims outside SQLite.

Scope Out
- Changing provider read implementation code, telemetry behavior, benchmark harness logic, or EF/runtime behavior.
- Adding new provider-specific latest-satellite read strategies for PostgreSQL, SQL Server, MySQL, or Oracle.
- Producing new external-provider benchmark executions or treating currently skipped optional-provider rows as completed performance measurements.
- Rewriting historical release records except where the new v0.28.0 baseline links back to them as historical context.

Open questions
- none

Follow-up questions
- After v0.28.0 docs land, should a later release rerun external-provider PIT/bridge benchmarks with configured PostgreSQL, SQL Server, MySQL, and Oracle connections so public docs can cite completed non-SQLite timings instead of skipped optional-provider rows?
- Should a later documentation-cleanup pass normalize older historical README sections so past embedded summaries do not reintroduce the pre-v0.28 provider matrix into current reader guidance?

Risks
- If the v0.28.0 docs overstate skipped optional-provider rows as measured live benchmarks, the release note will misrepresent current repository evidence.
- If only README and performance profiles are updated while active architecture/checklist guidance remains unchanged, adopters will continue to receive conflicting provider-matrix instructions.
- External-provider read behavior still depends on consumer-managed provider configuration and explicit PIT/bridge maintenance; the docs must avoid suggesting turnkey runtime enablement where the repository only documents diagnostics-gated and maintenance-dependent paths.

Split recommendations
- No split recommended; current evidence supports one coordinated documentation-baseline update across the existing current-baseline surfaces.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 5
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment