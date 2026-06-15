[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06FBSC4QXYQ0SWB1DPMGJJ5XX0'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FBSC4QXYQ0SWB1DPMGJJ5XX0`.
- Optimistic claim succeeded (`expectedRevision=06FCT7FVHK2D4MJ6YETJYWREXW`, `currentRevision=06FCT7JNXTHR07XFR1AQJE6DYG`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06FBSC4QXYQ0SWB1DPMGJJ5XX0': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06FBSC4QXYQ0SWB1DPMGJJ5XX0': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06FBSC4QXYQ0SWB1DPMGJJ5XX0-task-update-performance-docs-with-provider-evide' from source 'c472d4c01f46c9ef23886c0ec64360035ba4609e'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06FBSC4QXYQ0SWB1DPMGJJ5XX0-task-update-performance-docs-with-provider-evide` as `e922c83cf0a8`.

Open questions / Risiken
- If another ticket later introduces repo-backed `8.39.0` / `10.39.0` version alignment, the docs-only `v0.39.0` wording may need a follow-up update to stay aligned with the new release baseline.
- Live ticket comment and relation reads were trust-policy blocked earlier in the session, so duplicate and relation conclusions still rely on the provided ticket snapshot; no blocking duplicate evidence is present in that snapshot.
- Split recommendation: No split recommended. The remaining work is one bounded docs-only task across `docs/performance-profiles.md`, `docs/releases/v0.39.0.md`, and `CHANGELOG.md`.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.7936`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `6e0f80491cef4d298c86d6c8d064892a`
- completed-at-utc: `<redacted>-15T21:15:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FBSC4QXYQ0SWB1DPMGJJ5XX0/runs/20260615T211515907Z-6e0f80491cef4d298c86d6c8d064892a.json`