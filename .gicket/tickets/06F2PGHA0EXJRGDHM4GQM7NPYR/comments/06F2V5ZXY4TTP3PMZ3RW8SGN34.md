[gicket-bot] PO refinement contract

Summary
- Repository-backed refinement ratifies `06F2PGHA0EXJRGDHM4GQM7NPYR` as the bounded v0.11.0 documentation roll-up: create `docs/releases/v0.11.0.md` and update current public docs to match the already-shipped design-time command surface, CI guidance, and built-in multi-provider live-schema readers. No child tickets, relation writes, attachments, or planning documents were materialized in this PO pass.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Local `.gicket` evidence shows this ticket is a `parentOf` child of epic `06F2PGFT8Z406HFBJGQSY7YRJ0` for release `v0.11.0 - Design-Time Drift and CI Guardrails`, with incoming `blocks` relations from done stories `06F2PGFZWC5PXSDH46RCZPN1CG`, `06F2PGGEY26Y65G97NGFKH381M`, and `06F2PGGW8ZBW80V6B8RPWNVM70`; treat those as completed upstream inputs for this documentation roll-up, not as scope to reopen.
- Repository evidence already includes `DataVaultDesignTimeCommand`, `DataVaultDesignTimeCommandHost`, `DataVaultDesignTimeExportSource`, the consumer-owned `validate`/`export`/`drift`/`guardrail` verbs, and GitHub Actions-oriented workflow guidance in `docs/architecture/dvault-dotnet-ef-design-time-workflow.md`.
- Repository code already includes built-in live-schema readers for SQLite, PostgreSQL, SQL Server, Oracle, and MySQL in `src/DCoding.Data.DVault/DataVaultLiveSchemaReader.cs`, so v0.11.0 docs must stop describing non-SQLite providers as unsupported first-class readers.
- `docs/releases/v0.11.0.md` is currently missing, and `README.md` plus `examples/README.md` still pin install snippets to `0.10.0` and point readers at v0.10.0 release notes.
- Current ticket comments are automation claim or lease comments only; no human clarification, attachment context, child-ticket split, or planning artifact is pending from this refinement pass.

Scope In
- Create `docs/releases/v0.11.0.md` for release `v0.11.0 - Design-Time Drift and CI Guardrails` using the established release-note shape: package scope, highlights, documentation updates, compatibility notes, and validation evidence.
- Update `README.md` and `examples/README.md` so package snippets, release-note references, and user-facing release summaries are version-aligned to `0.11.0`.
- Update current public guidance in `README.md`, `examples/README.md`, `docs/production-adoption-checklist.md`, and `docs/model-first-governance.md` to reflect the implemented command surface and built-in live-schema reader support for PostgreSQL, SQL Server, Oracle, and MySQL.
- Keep the public wording aligned with the bounded implementation: consumer-owned command host, reviewed-artifact drift as the default blocking lane, optional live-schema drift, no DVault-owned CLI shim, no EF command interception, and no automatic migration or schema-repair behavior.
- Capture doc-level verification evidence and any intentionally deferred operational guidance in the new release notes and affected public docs.

Scope Out
- No new product code, diagnostics, provider readers, command verbs, or CI workflow behavior; this ticket documents behavior already present in the repository.
- No expansion into provider-specific secret management, container provisioning, or full production runbooks beyond the existing opt-in connection-string and external-provider guidance.
- No rewrite of historical release notes as if v0.10.0 or v0.8.0 had already shipped v0.11.0 behavior; preserve older release notes as historical context and make v0.11.0 the current public summary.
- No new child tickets, relation restructuring, or planning-document split is required from the current repository evidence.

Open questions
- none

Follow-up questions
- After v0.11.0 lands, should a later docs pass add runnable non-SQLite live-schema examples, or keep non-SQLite providers documented only as external opt-in validation lanes?
- Should future documentation add provider-specific operational appendices for external live-schema checks instead of keeping all cross-provider guidance in the shared README and adoption documents?

Risks
- Current public docs still understate shipped live-schema support and still point at v0.10.0, so users can misunderstand the actual v0.11.0 baseline if this ticket slips.
- Overcorrecting the docs would also be wrong: they must distinguish built-in reader support from developer-managed external databases and opt-in validation lanes.
- If release notes summarise design-time commands differently from the architecture and adoption docs, adopters may incorrectly assume DVault ships a standalone CLI or intercepts `dotnet ef`.
- `docs/releases/v0.11.0.md` is missing today, so the release currently has no authoritative public summary artifact.

Split recommendations
- No additional split is recommended; the current evidence supports a single bounded docs and release-note rollout across the already-implemented command surface, CI guidance, and provider drift reader changes.
- If future work wants provider-specific operational tutorials or board-level relation cleanup, track those as separate follow-up tickets rather than widening this ticket.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 5
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment