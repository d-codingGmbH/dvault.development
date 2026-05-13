[gicket-bot] PO refinement contract

Summary
- Refinement now defines the six-operation safe/unsafe matrix, fixes the diagnostics ownership contract without public API expansion, and removes the stale blocks relation from 06F1XPS7KGKBP5SVMQPJC49J2G; no child tickets or planning documents were created.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - Added a concrete matrix bound to named invariants: MI-1 identity-only Hub/Link tables, MI-2 required technical columns, MI-3 stable key and parent/participant shapes, MI-4 default DVault index coverage, and MI-5 deterministic DVault-owned names. Safe examples are limited to non-DVault or non-key payload changes; finding-producing examples map AddColumn to DVM2001, DropColumn to DVM2002 or DVM2003, DropTable to DVM2006, RenameColumn to DVM2005, CreateIndex to DVM2004, and AlterColumn to DVM2002 or DVM2003.
- critic-item-2: `answered` - Migration-operation analysis stays under the existing IDataVaultDiagnosticsService ownership boundary, specifically the current Analyze(DbContext) service family and returned DataVaultDiagnosticsResult.Issues. Internal helpers or fixture harnesses are allowed, but this ticket must not add a new public diagnostics service entrypoint or a public Analyze(MigrationOperation...) overload.
- critic-item-3: `answered` - The live blocks relation from 06F1XPS7KGKBP5SVMQPJC49J2G to this ticket was removed. This ticket remains a child of 06F1XPTCGWTJHHQVNPN13KANMG but is no longer blocked by 06F1XPS7KGKBP5SVMQPJC49J2G, so the contract and risks now treat that dependency as historical context only.
- critic-item-4: `answered` - The invariant decision matrix is now explicit so the developer does not have to invent product rules. Examples: AddColumn SatCustomerContact.EmailAddress is safe; AddColumn HubCustomer.CustomerStatus emits DVM2001. DropColumn SatCustomerContact.DeprecatedNickname is safe; DropColumn HubCustomer.RecordSource emits DVM2002. DropTable LegacyAuditScratch is safe; DropTable HubCustomer emits DVM2006. RenameColumn SatCustomerContact.CustomerStatus to StatusCode is safe; RenameColumn HubCustomer.LoadTimestamp to LoadedAt emits DVM2005. CreateIndex IX_SatCustomerContact_EmailAddress is safe; a fake or wrong-semantics IxHubCustomerBusinessKeyCustomerId emits DVM2004. AlterColumn SatCustomerContact.EmailAddress widening is safe; AlterColumn on LoadTimestamp, RecordSource, HashDiff, hash-key, participant, parent-hash-key, or driving-key shape emits DVM2002 or DVM2003.
- critic-item-5: `answered` - Findings enter the existing DataVaultDiagnosticsResult.Issues list and keep the published public shape of Code, Severity, Message, and Path only. Migration remediation text stays catalog-backed internal data keyed by code and is asserted in tests by catalog lookup; public DataVaultDiagnosticsIssue and DataVaultDiagnosticsResult shape changes are out of scope and the approved API snapshot should remain unchanged.

Clarifications
- Named invariants for this ticket are MI-1 identity-only Hub/Link tables, MI-2 required technical columns, MI-3 stable key and parent/participant shapes, MI-4 default DVault index coverage, and MI-5 deterministic DVault-owned names and path labels.
- Diagnostic Path values are stable and deterministic: migration/{OperationType}/{TableOrObject} with /{MemberName} appended for column or index targets; examples include migration/AddColumn/SatCustomerContact/EmailAddress and migration/DropTable/HubCustomer.
- AddColumn matrix: safe when adding a non-key payload column to a satellite or a non-DVault table; finding DVM2001 error when a descriptive payload column is added to Hub* or Link* tables because hub and link rows remain identity-only or relationship-only.
- DropColumn matrix: safe when dropping a non-key, non-technical satellite payload column; finding DVM2002 error for dropping LoadTimestamp, RecordSource, or satellite HashDiff, and DVM2003 error for dropping a hash-key or driving-key column that changes DVault key or parent shape.
- DropTable matrix: safe when the table is outside current DVault-produced objects; finding DVM2006 error when Hub*, Link*, or Sat* produced tables are dropped.
- RenameColumn matrix: safe when renaming a non-DVault payload column inside a satellite; finding DVM2005 warning when a DVault-owned technical, key, or deterministic produced column name is renamed away from the current naming-policy output.
- CreateIndex matrix: safe for supplemental non-DVault lookup indexes that do not reuse DVault-owned default index names; finding DVM2004 warning when a business-key, relationship, or satellite-parent/latest-history index is created with the wrong uniqueness, column order, or target columns.
- AlterColumn matrix: safe for non-key satellite payload widenings or provider-neutral shape changes that leave DVault semantics intact; finding DVM2002 error for LoadTimestamp, RecordSource, or HashDiff alterations and DVM2003 error for hash-key, participant-reference, parent-hash-key, or driving-key shape changes.
- The diagnostics contract for this ticket stays on the existing IDataVaultDiagnosticsService and DataVaultDiagnosticsResult.Issues public surface; remediation text remains catalog-backed and internal or test-visible by code lookup rather than new public issue or result fields.
- The stale blocks relation from 06F1XPS7KGKBP5SVMQPJC49J2G to 06F1XPV0YJ8Z9HQVT6BYR397Q8 was removed during refinement; no child tickets, attachments, or planning documents were materialized.

Scope In
- Provider-neutral validation for AddColumn, DropColumn, DropTable, RenameColumn, CreateIndex, and AlterColumn against current Hub*, Link*, and Sat* DVault invariants.
- Stable migration diagnostic catalog entries DVM2001 through DVM2006 with fixed severities, summaries, and remediation text used by deterministic tests.
- Deterministic fixtures proving at least one safe and one finding-producing case for each supported operation type.
- Use of the existing diagnostics issue and path surface without public API growth.

Scope Out
- New public IDataVaultDiagnosticsService or IDataVaultReadDiagnosticsService members, including a public Analyze(MigrationOperation...) overload.
- Public DataVaultDiagnosticsIssue or DataVaultDiagnosticsResult shape changes.
- Provider-specific SQL or DDL parsing, runtime database inspection, or live migration execution.
- Bridge-specific, PIT-specific, or broader non-Hub/Link/Satellite rule families beyond the v1 matrix in this ticket.
- Automatic migration rewriting or autofix behavior.

Open questions
- none

Follow-up questions
- Should a later ticket extend the same matrix to Bridge* and Pit* produced tables once the hub, link, and satellite baseline proves stable?
- After the internal helper ships, is there a separate need for a public Analyze(MigrationOperation...) diagnostics API and corresponding approved API snapshot changes?
- Which EF migration operation types beyond the initial six should be prioritized next: RenameTable, DropIndex, AddPrimaryKey, DropPrimaryKey, or foreign-key operations?
- Should later guardrails add provider-specific remediation enrichment after the provider-neutral baseline is stable?

Risks
- Prefix-only table matching would create false positives; the implementation should derive DVault-owned names from current metadata, naming rules, and schema baselines rather than matching any table that starts with Hub, Link, or Sat.
- If migration codes or remediations are not catalog-backed and stable, downstream diagnostics assertions will churn.
- An implementation that adds public diagnostics members instead of internal helpers will break the approved API snapshot and exceed this ticket's scope.
- Operations that combine multiple changes may emit multiple findings; test fixtures should lock the deterministic issue ordering contract.

Split recommendations
- No immediate split; keep this ticket bounded to the six-operation hub, link, and satellite baseline, and open follow-up tickets for bridge or PIT guardrails or any future public migration-analysis API.

Persisted contract coverage
- acceptance-criteria items: 7
- definition-of-done items: 5
- implementation-notes items: 7

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment