[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F2PGJYY6S97B4Z8044D34K5C'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F2PGJYY6S97B4Z8044D34K5C`.
- Optimistic claim succeeded (`expectedRevision=06F2PNK5TRAR95WWJQW3ZXM4PC`, `currentRevision=06F37WDDHRYY47HJDD94VHH9PG`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F2PGJYY6S97B4Z8044D34K5C': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F2PGJYY6S97B4Z8044D34K5C': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F2PGJYY6S97B4Z8044D34K5C-task-update-v0-12-0-documentation-and-release-no' from source '8458bf1de930549138ac6bce8ff6b4311a8cf5ba'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06F2PGJYY6S97B4Z8044D34K5C-task-update-v0-12-0-documentation-and-release-no` as `198e03f0924e`.

Open questions / Risiken
- If broader docs duplicate the package-local suppression contract, README-level guidance can drift from the packaged analyzer README that consumers actually receive.
- If v0.12 docs describe generated helpers as a new metadata authority or hidden persistence layer, adopters may misunderstand the preserved explicit `IDataVaultSaveService` boundary.
- If `docs/releases/v0.12.0.md` fails to distinguish older DMV1901/DMV1902 baseline behavior from new v0.12 additions, release history will be misleading even if the technical behavior description is otherwise correct.
- If touched docs update version snippets but leave `v0.11.0` current-baseline prose in place elsewhere in the same surfaces, the public release narrative will remain inconsistent.
- Split recommendation: No additional split is recommended. The existing separation is already sufficient: package-local analyzer configuration and suppression guidance stays with done task `06F2PGJ28KVSZAAFRA40D94128`, generator contract and implementation stay with done tasks ...
- Split recommendation: If the team later wants a runnable generator sample or a broader diagnostics catalog page, create that as a separate follow-on documentation ticket instead of widening this release-closure task.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9360`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `14a1e7244ff5458e93e8d4a6e2cfad42`
- completed-at-utc: `<redacted>-17T03:29:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F2PGJYY6S97B4Z8044D34K5C/runs/20260517T032911170Z-14a1e7244ff5458e93e8d4a6e2cfad42.json`