[gicket-bot] PO refinement contract

Summary
- Restated the story against source-backed save/read boundaries and helper surfaces, and removed any reliance on an assumed public tracing type.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - The contract now cites only source-backed boundaries: three IDataVaultSaveService.SaveAsync overloads, IDataVaultReadService.ReadLatestSatelliteRowsAsync, IDataVaultReadService.ReadPitRowsAsync, and existing helper surfaces. Any missing tracing support code may be created explicitly as internal implementation code.
- critic-item-2: `answered` - The refined contract no longer infers an existing public tracing API or type. Span coverage is anchored to the evidenced save/read boundaries, and the ActivitySource-owning code is treated as work this story may add if needed.
- critic-item-3: `answered` - The contract now treats any ActivitySource holder or helper as explicit implementation work, not as a pre-existing public API requirement. The source-backed tracing requirement is the DCoding.Data.DVault ActivitySource name plus the closed span, tag, and event vocabulary.
- critic-item-4: `answered` - The contract explicitly does not assume a pre-existing public tracing API. If a public tracing surface is intentionally added, the same change must update the approved public API snapshot; otherwise tracing support stays internal.

Clarifications
- This pass is a contract restatement only: no child-ticket, relation, attachment, planning-document, or ticket-description writes were materialized.
- Live relations remain unchanged: incoming parentOf from 06F5Q93R4633D41Z21WQW3SVGR, incoming blocks from 06F5Q93YXHSKABD2SABWY85S78, and outgoing blocks to 06F5Q94SQ086B2DZ1AKFDXGV94.
- The contract now relies only on the evidenced save/read interfaces, helper surfaces, and tracing vocabulary document.

Scope In
- Add repo-owned Activity tracing that uses an ActivitySource named DCoding.Data.DVault; the supporting ActivitySource field or helper may be introduced as new internal implementation code if needed.
- Instrument the three public IDataVaultSaveService.SaveAsync overload boundaries in src/DCoding.Data.DVault/DataVaultSaveService.cs.
- Instrument the existing repo-owned read entry points for latest/current/as-of satellite reads, PIT reads, and bridge reads across IDataVaultReadService, current/as-of and registry helpers, PIT typed projection helper, bridge helpers, and the existing typed satellite helper contract documented for v0.22.0.
- Reuse the bounded telemetry and diagnostics vocabulary already documented for provider name, strategy status and type, counts, duration bucket, fallback causes, outcome, and bounded failure classification.
- Add focused coverage for no-listener behavior, sampled-listener behavior, success, fault, cancellation, bridge fallback, helper non-duplication, redaction, telemetry compatibility, and public API snapshot compatibility when a public API addition is intentional.

Scope Out
- Do not assume or require any pre-existing public tracing API or public ActivitySource accessor.
- Do not add new IDataVaultReadService members solely to cover bridge, current/as-of, PIT typed projection, or typed satellite helper tracing.
- Do not instrument PIT or bridge maintenance operations; those remain in 06F5Q94D0JDMMWDXSRGWX1E4F0.
- Do not change provider strategy selection, SQL shape, batching thresholds, persistence semantics, exporters, dashboards, collectors, hosting, or OpenTelemetry package requirements.
- Do not capture raw SQL, metadata names, table names, hash keys, payload values, record-source values, exception messages, or stack traces in Activity names, tags, or events.
- Do not require tracing coverage for consumer-owned custom IDataVaultSaveService or IDataVaultReadService implementations beyond repo-owned core code and helper surfaces.

Open questions
- none

Follow-up questions
- If consumers later need a public code-facing ActivitySource accessor instead of listener registration by source name, should that ship in a separate additive API-shaping ticket with explicit public API snapshot review?

Risks
- If tracing is added only around ReadLatestSatelliteRowsAsync, typed satellite helper executions can still bypass that hook unless the delegated execution path shares the same span creation.
- If bridge tracing covers only the DefaultDataVaultReadService branch, callers that flow through DataVaultBridgeReadPipeline can miss dvault.read.bridge spans.
- If helper layers add wrapper spans instead of reusing the delegated execution path, duplicate root spans can leak into listener output.
- If tag or event construction happens before listener or sampling checks, the implementation can violate the no-listener minimal-overhead baseline even when StartActivity returns null.

Split recommendations
- No split is recommended; current branch evidence supports one bounded story for explicit save and read tracing, while PIT and bridge maintenance tracing remains separate in 06F5Q94D0JDMMWDXSRGWX1E4F0.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 5
- implementation-notes items: 7

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment