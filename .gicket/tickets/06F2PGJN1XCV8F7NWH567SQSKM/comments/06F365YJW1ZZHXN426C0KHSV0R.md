[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F2PGJN1XCV8F7NWH567SQSKM'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F2PGJN1XCV8F7NWH567SQSKM`.
- Optimistic claim succeeded (`expectedRevision=06F2PNK2AS24B2X0SR0WXYAX80`, `currentRevision=06F363FYF6TPZRZR6JMSPW11KW`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F2PGJN1XCV8F7NWH567SQSKM': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F2PGJN1XCV8F7NWH567SQSKM': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F2PGJN1XCV8F7NWH567SQSKM-task-define-source-generator-input-and-output-co' from source '3f344b0af972ac6b1bba34c493a3012f4874bd82'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06F2PGJN1XCV8F7NWH567SQSKM-task-define-source-generator-input-and-output-co` as `44bf8769d21c`.

Open questions / Risiken
- If implementation treats the generator input as a new authoritative metadata declaration system instead of a helper layer over existing logical names, it will reopen code-first, metadata-first, and model-first ownership and expand scope.
- If generated output hides `loadTimestamp`, `recordSource`, or save orchestration, it can violate the explicit `IDataVaultSaveService` boundary already ratified elsewhere in the repository.
- Satellite or link scope can sprawl quickly if v1 tries to absorb link-parent satellites or same-hub repeated-participant links that current typed-mapper ergonomics already constrain.
- `docs/releases/v0.12.0.md` is still absent on the branch snapshot, so public communication of this generator contract remains a downstream documentation dependency until `06F2PGJYY6S97B4Z8044D34K5C` lands.
- Split recommendation: No additional split is required for this contract ticket; the existing story already separates contract definition, implementation (`06F2PGJSXP18VKKV52QZA4NP30`), and release and documentation (`06F2PGJYY6S97B4Z8044D34K5C`).
- Split recommendation: If implementation work proves too large, split follow-on generator support by excluded shape families such as link-parent satellites, repeated-participant and self-link handling, or higher-level save wrappers instead of widening the initial v1 contract.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9002`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `cd4346b689d84308be8d1171a19025d3`
- completed-at-utc: `<redacted>-16T23:20:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F2PGJN1XCV8F7NWH567SQSKM/runs/20260516T232059095Z-cd4346b689d84308be8d1171a19025d3.json`