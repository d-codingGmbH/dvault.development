[gicket-bot] PO refinement contract

Summary
- Verified the ticket, comments, relations, branch state, and streaming/diagnostics source. The refinement fixes this story to an additive bounded chunked-save fallback/remediation surface over existing telemetry vocabularies; no child tickets, relation changes, attachments, or planning documents were materialized.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Verified persisted context: the ticket has no human comments or closure amendments, remains a child of 06F5Q8WVYMV8KQPAENPEEE3YM4, blocks 06F5Q8Y3WW9FFV7HA289VHCEAM, and is itself blocked by 06F5Q8X8Q72TQ5B7F2JSAJWPR8 and 06F5Q8XF9DPKFW9VY0F3Y32BH4; no relation changes were applied in this refinement pass.
- Verified branch state: ticket/06F5Q8XPXEQPJTKGJ7BQGCY438-story-explain-streaming-fallback-and-remediation is still at scratch source 0b1caa35633951259af2b6b13dac2283ba55e298 apart from the PO claim commit, so this is refining an unimplemented slice rather than ratifying landed code.
- The authoritative streaming contract already names DataVaultSaveTelemetrySummary as the bounded diagnostics surface for chunked saves and fixes the retained-state limit vocabulary to RetainedSatelliteSeriesLimitReached and RetainedSatelliteSeriesLimitExceeded.
- Current save diagnostics already expose finite provider-fallback enums such as ProviderNameMismatch, UnknownOrUnregisteredProviderName, NoProviderSpecificStrategyRegistered, DirtyDbContext, MultiActiveSatelliteOperations, and provider threshold causes; the missing slice is actionable explanation and remediation over that bounded vocabulary.
- Transaction guidance is already decided by the streaming contract: chunked execution participates in the caller's current transaction and callers who need all-or-nothing across chunks must open the transaction before invoking the save service.

Scope In
- Add an additive bounded explanation/remediation surface for chunked-save and provider-fallback outcomes rooted in the existing explicit save telemetry contract.
- Cover provider-neutral fallback causes already evidenced in source: dirty tracked DbContext, provider mismatch or unknown provider registration, missing provider strategy wiring, multi-active satellite batches, and current SQL Server/MySQL/Oracle threshold gates.
- Cover chunked retained-state fallback and unsupported-shape reporting for the finite RetainedSatelliteSeriesLimitReached and RetainedSatelliteSeriesLimitExceeded vocabulary.
- Keep explanations/redaction compatible with the current telemetry contract by avoiding raw hash keys, payload values, record sources, table-level dumps, or per-parent retained-state listings.
- Update focused tests and public API snapshots for any new public helper types or summary members added to support the explanation/remediation surface.

Scope Out
- Do not add provider-native chunk execution, new save-strategy gates, or new provider capability-profile registration behavior.
- Do not add unbounded debug dumps, raw SQL, per-chunk trace logs, or high-cardinality diagnostic payloads.
- Do not repurpose EF SaveChanges interception, background ingestion, scheduler behavior, or support-bundle automation into the streaming save path.
- Do not broaden this story into general preflight or migration diagnostics work unrelated to explicit save fallback explanation.
- Do not require relation cleanup, child-ticket creation, or planning-document materialization unless new evidence appears; none was justified in this pass.

Open questions
- none

Follow-up questions
- Should a later story project the same fallback/remediation vocabulary into DataVaultPreflight or design-time command output, or keep that broader explain surface separate from runtime save telemetry?
- After this bounded v1 slice lands, do we want provider-specific documentation examples that show how applications consume the remediation output from custom IDataVaultTelemetryObserver implementations?

Risks
- Because the default AddDVault() path stays telemetry-free, consumers that do not register AddDVaultTelemetry() or a custom IDataVaultTelemetryObserver may miss the new streaming remediation guidance unless docs clearly show how to opt in.
- Cause-to-remediation mappings can drift when provider strategy gates change; tests should assert that every currently exposed fallback enum and retained-state classification has a stable bounded explanation.
- Chunked attempts aggregate causes across chunks, so remediation text must stay aggregate and deterministic rather than implying a raw per-chunk execution trace.

Split recommendations
- none

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment