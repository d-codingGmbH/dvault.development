[gicket-bot] PO refinement contract

Summary
- Refinement stays within one provider-neutral migration-operation validator ticket. Repository and ticket evidence already support a bounded handoff: existing diagnostics test surfaces are present, the ticket remains a child of 06F1XPTCGWTJHHQVNPN13KANMG, and it is currently blocked by 06F1XPS7KGKBP5SVMQPJC49J2G; no child tickets, relation edits, attachments, or planning documents were materialized.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The validator scope is the first provider-neutral pass over EF migration operation fixtures, not SQL text or live database state.
- The bounded v1 operation set is AddColumn, DropColumn, DropTable, RenameColumn, CreateIndex, and AlterColumn.
- Deterministic output means stable finding content and stable finding order for the same fixture input so CI comparison can use exact assertions or snapshots.
- Repository evidence already includes diagnostics-oriented test surfaces in tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs and tests/DCoding.Data.DVault.Tests/Integration/DataVaultDiagnosticsIntegrationTests.cs; no separate test project split is needed for this ticket.
- Live relation state was verified: ticket 06F1XPV0YJ8Z9HQVT6BYR397Q8 is a child of 06F1XPTCGWTJHHQVNPN13KANMG and currently has an incoming blocks relation from 06F1XPS7KGKBP5SVMQPJC49J2G; no relation changes were made.

Scope In
- Implement the first migration-operation guardrail validator for the six listed EF migration operation fixture types.
- Emit findings through the existing diagnostics surface with catalog-backed diagnostic id, severity, location, and remediation text.
- Add automated coverage for both no-finding and finding-producing cases and assert deterministic output suitable for CI comparison.

Scope Out
- Provider-specific DDL or SQL parsing.
- Runtime database access, schema inspection, or live migration execution.
- Validation coverage for migration operation types beyond AddColumn, DropColumn, DropTable, RenameColumn, CreateIndex, and AlterColumn.
- Automatic migration rewriting, autofix behavior, or broader migration-planning features.

Open questions
- none

Follow-up questions
- After the first six-operation baseline lands, which additional EF migration operations should be prioritized in the next validator expansion ticket?
- Should a later follow-up add provider-specific enrichment or migration authoring guidance once the provider-neutral guardrail baseline is stable?
- Should the validated findings later feed a broader drift or migration review report surface beyond the current test-driven diagnostics contract?

Risks
- Migration-specific diagnostic ids and severities must align with the existing catalog; mismatches can create snapshot or contract churn in downstream diagnostics tests.
- Determinism can regress if finding order or rendered location/remediation text depends on non-stable enumeration or provider details.
- Some operations are context-sensitive rather than categorically safe or unsafe, so overly blunt rules could under-report or over-report invariant risk.
- Implementation sequencing still depends on upstream ticket 06F1XPS7KGKBP5SVMQPJC49J2G because it currently blocks this ticket in live relation state.

Split recommendations
- No split recommended; current scope is already tightly bounded to one provider-neutral validator pass, six operation fixtures, and deterministic diagnostics coverage.

Persisted contract coverage
- acceptance-criteria items: 4
- definition-of-done items: 4
- implementation-notes items: 4

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment