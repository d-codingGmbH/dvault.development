[gicket-bot] PO refinement contract

Summary
- Verified the local ticket store, comment and attachment state, and live relations from .gicket, then bounded this story to request-bound save/read strategy explanation on the existing diagnostics contract; no child tickets, relation edits, attachments, or planning documents were created.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The live parent relation is 06F2PGQ27NWVZ1B1R651S7SM4M -> 06F2PGQ6T5TGNWCBQBX3700D84 (parentOf), so this story stays inside the Epic: Observability and operations boundary.
- The live outbound blocks relations already separate downstream work into telemetry 06F2PGQBGNZPEEJE4KBET4JG24, support bundle 06F2PGQJ7THHNSYYBFFPBG4174, and v0.16 documentation 06F2PGQQJB5FJGDB16M2G7CPCM.
- The inbound blocks relation from 06F2PGP7HM8F39K3J0H5JHB3B4 (Epic: Maintenance and query operations) is historical context because that ticket is done; it is not an open blocker for this story.
- Repository evidence already establishes the intended contract surface through DataVaultDiagnosticsResult, DataVaultSaveStrategyDiagnostics, DataVaultReadStrategyDiagnostics, IDataVaultDiagnosticsService, and IDataVaultReadDiagnosticsService; this story should complete that surface rather than introduce a parallel explain API.
- The read-side explain baseline is finite and request-bound: analyze latest/as-of satellite requests through DataVaultLatestSatelliteReadRequest, PIT requests through DataVaultPitAsOfReadRequest, and bridge requests through DataVaultBridgeReadRequest.
- No child tickets, relation edits, attachments, or planning documents were materialized during this refinement pass.

Scope In
- Human-readable explanation of whether DVault selected a provider-specific save/read strategy or used provider-neutral fallback.
- Structured save-strategy decision data for analyzed save requests, including provider identity, selected strategy metadata when applicable, candidate ordering, and fallback causes.
- Structured read-strategy decision data for analyzed latest/as-of satellite, PIT, and bridge read requests.
- Tests and public contract coverage that lock the strategy-explanation behavior and keep it reusable by downstream observability tickets.
- Source-local documentation or API snapshot updates needed to ship the explain surface safely.

Scope Out
- Changing actual save or read dispatch semantics, provider thresholds, provider capability profiles, or persistence/read results.
- Telemetry hooks, counters, or metric export; that remains the separate story 06F2PGQBGNZPEEJE4KBET4JG24.
- Support bundle packaging, redaction, or artifact export; that remains the separate story 06F2PGQJ7THHNSYYBFFPBG4174.
- The coordinated v0.16 documentation/release wrap-up tracked by 06F2PGQQJB5FJGDB16M2G7CPCM, beyond any minimal source-local docs needed for this story.
- New provider-specific read optimizations, PIT/bridge maintenance changes, or broader operations tooling.

Open questions
- none

Follow-up questions
- What subset of the structured strategy explanation should the future support-bundle story serialize by default, and what should be redacted or collapsed?
- Which strategy-decision events or counters should the telemetry story emit, and at what aggregation/cardinality boundary?
- Should the downstream v0.16 documentation story add standalone troubleshooting examples for common fallback causes, or keep that detail limited to API/source docs and release notes?

Risks
- Explanation logic can drift from real runtime dispatch if it duplicates strategy-selection rules instead of reusing the same gate evaluation behavior.
- Documentation drift is likely unless README and release-note updates explicitly catch up with the existing read-diagnostics surface as well as the save-diagnostics surface.
- Scope can sprawl into telemetry, support-bundle, and release-wrap work unless the existing downstream blocks relations continue to own those deliverables.

Split recommendations
- No additional split is recommended. The live relation set already separates downstream telemetry, support-bundle, and v0.16 documentation work into tickets 06F2PGQBGNZPEEJE4KBET4JG24, 06F2PGQJ7THHNSYYBFFPBG4174, and 06F2PGQQJB5FJGDB16M2G7CPCM.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment