[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F9G8F4RQ0T7RV82M3H2H3FVG'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F9G8F4RQ0T7RV82M3H2H3FVG`.
- Optimistic claim succeeded (`expectedRevision=06F9GF2T7G31XVPSG733N5MSG0`, `currentRevision=06FAQMCAFKVDRM78P9Q4KXVY0G`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F9G8F4RQ0T7RV82M3H2H3FVG': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F9G8F4RQ0T7RV82M3H2H3FVG': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F9G8F4RQ0T7RV82M3H2H3FVG-story-add-ef-core-provider-version-matrix-tests' from source '387a2dd1752691475eb2f5dacd5ecf38c522fc4d'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06F9G8F4RQ0T7RV82M3H2H3FVG-story-add-ef-core-provider-version-matrix-tests` as `1b93897f0cb1`.

Open questions / Risiken
- Because several provider PackageReferences are conditioned on connection-string properties, matrix coverage that relies only on live external-provider execution could miss drift when those opt-in properties are absent; at least one deterministic non-live assertion path is requ...
- Package-artifact dependency proof can overlap with ticket 06F9G8FBQTAPXXS1Y4NR5QKVG8 if implementation expands beyond EF/provider dependency groups into broader nuspec, README, symbol, or metadata verification.
- Repository policy supports a broader MySQL provider-name baseline than this story's checked-in MySql.EntityFrameworkCore matrix, so Pomelo-specific drift would remain outside this ticket unless separately scheduled.
- Split recommendation: No additional split is required; the story is bounded if it stays focused on exact provider-version assertions and package dependency-line proof, while broader verifier and CI coverage remains with ticket 06F9G8FBQTAPXXS1Y4NR5QKVG8.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `48460`
- cached-tokens: `2432`
- effective-cache-ratio: `0.0502`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `295ec0cd18514c8ca19e167ab621b80e`
- completed-at-utc: `<redacted>-09T10:05:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F9G8F4RQ0T7RV82M3H2H3FVG/runs/20260609T100555879Z-295ec0cd18514c8ca19e167ab621b80e.json`