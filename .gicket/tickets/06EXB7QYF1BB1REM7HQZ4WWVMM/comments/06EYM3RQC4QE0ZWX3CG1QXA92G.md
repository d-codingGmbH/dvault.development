[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06EXB7QYF1BB1REM7HQZ4WWVMM'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EXB7QYF1BB1REM7HQZ4WWVMM`.
- Optimistic claim succeeded (`expectedRevision=06EXBF535NAD8M8SYN7S7P4X48`, `currentRevision=06EYM2KXK1JF30N9A6QA35W3JC`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-write-p...
- Bounded PO planning surfaces enabled for ticket '06EXB7QYF1BB1REM7HQZ4WWVMM': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06EXB7QYF1BB1REM7HQZ4WWVMM': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06EXB7QYF1BB1REM7HQZ4WWVMM-story-write-getting-started-documentation' from source '327c5d020ebafc694e1ffa7307fce29bd88c1d04'.
- Removed bot-owned orchestration details from the persisted PO delivery contract.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- 1 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- README snippets can drift from the tested API surface if later library changes update code without keeping docs aligned with integration tests.
- Project-reference guidance will confuse future package consumers unless it stays clearly framed as the pre-publication path.
- The shared-type query example may be misread as the long-term preferred consumer API if the docs do not keep calling it the current v1 baseline.
- If future NuGet wording becomes too concrete before publication, the story will emit false or failing install guidance.
- Split recommendation: No additional split recommended; this story already has the right decomposition through child task 06EXB7R6MTJW1PYRN172MW34DM for the README quickstart and child task 06EXB7REMY41DF7RE8J3N1RZYC for project-reference and future NuGet wording.
- Split recommendation: Keep the parent story as the umbrella documentation contract and leave release/publication workflow to blocked story 06EXB8202A88KJJP7WEGBESBYM.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9273`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `d195c9597e2b4732b1b917c9c4268787`
- completed-at-utc: `<redacted>-02T18:59:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EXB7QYF1BB1REM7HQZ4WWVMM/runs/20260502T185910117Z-d195c9597e2b4732b1b917c9c4268787.json`