[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F7Y0H83H29E1D9K5RK3K7Y9W'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F7Y0H83H29E1D9K5RK3K7Y9W`.
- Optimistic claim succeeded (`expectedRevision=06F7Y0YCP30J1R9V1BWBFMFYEW`, `currentRevision=06F87RQNF2JWJE55HTZXHHVBV8`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F7Y0H83H29E1D9K5RK3K7Y9W': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F7Y0H83H29E1D9K5RK3K7Y9W': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F7Y0H83H29E1D9K5RK3K7Y9W-story-generate-typed-pit-read-helpers-from-suppo' from source '4049da3f301c0da72d5b7594ac175264815a5278'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06F7Y0H83H29E1D9K5RK3K7Y9W-story-generate-typed-pit-read-helpers-from-suppo` as `27a75a4d098e`.

Open questions / Risiken
- If support-bundle export does not actually carry the required request-bound `readShape.pit` facts for parent identity, segment snapshot references, deterministic ordering, and column bindings, supported runtime PIT shapes will still collapse to diagnostics instead of helper em...
- Shared-driving-key multi-active PIT support is only safe when the support bundle proves one canonical driving-key name or order family; mismatches must keep diagnostic-only behavior.
- Link-parent PIT helper emission must stay constrained to unique non-multi-active satellites on one declared link parent so the generator does not imply model-first link-parent PIT artifact support or broader runtime semantics.
- Live relation state still includes a historical `blocks` edge from done contract ticket `06F7Y0GT7A5QT77TADMRZBVYN8`; ticket metadata is currently `isBlocked: false`, but dependency-graph cleanup may still be needed later.
- Split recommendation: No further child split is justified from current evidence: PIT implementation is already separated from the bridge-helper story `06F7Y0HJ1ZPY7ND9N8RVS92H4C` and the downstream documentation task `06F7Y0HZKHBHMYX9EYDYFRYXZ0`.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.6462`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `9f6bc1900fa142ab92fe2409a145b574`
- completed-at-utc: `<redacted>-01T16:11:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F7Y0H83H29E1D9K5RK3K7Y9W/runs/20260601T161126232Z-9f6bc1900fa142ab92fe2409a145b574.json`