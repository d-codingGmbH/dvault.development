[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06FBSC08W24BJGFZ87RSFS21WC'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FBSC08W24BJGFZ87RSFS21WC`.
- Optimistic claim succeeded (`expectedRevision=06FBSCXDHRPB8K5S4VP482Q1G4`, `currentRevision=06FCCGCNNA4G97WWE7J4AEK2E0`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06FBSC08W24BJGFZ87RSFS21WC': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06FBSC08W24BJGFZ87RSFS21WC': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06FBSC08W24BJGFZ87RSFS21WC-task-report-selected-hash-storage-profile-in-dia' from source 'b040df049e3a7905cf37705d5d1f6a0cc00c412a'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06FBSC08W24BJGFZ87RSFS21WC-task-report-selected-hash-storage-profile-in-dia` as `af5888188b51`.

Open questions / Risiken
- The acceptance wording can be misread as requiring a third storage-profile enum; without the bounded clarification above, implementation could accidentally widen the v1 contract.
- If tests only cover HexString and one Binary selection path, a regression in a provider/profile-preselected Binary path could escape even though the shared diagnostics surface is the same.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9440`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `f8cfb84a39554c28907d2257e8b1d762`
- completed-at-utc: `<redacted>-14T13:21:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FBSC08W24BJGFZ87RSFS21WC/runs/20260614T132101506Z-f8cfb84a39554c28907d2257e8b1d762.json`