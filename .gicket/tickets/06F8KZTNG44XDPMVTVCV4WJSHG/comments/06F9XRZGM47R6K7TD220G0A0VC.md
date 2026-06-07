[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F8KZTNG44XDPMVTVCV4WJSHG'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F8KZTNG44XDPMVTVCV4WJSHG`.
- Optimistic claim succeeded (`expectedRevision=06F9XPVHZ11YZ11GVNA708CZGR`, `currentRevision=06F9XQ2FVE624RW0M423FRM2B0`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F8KZTNG44XDPMVTVCV4WJSHG': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F8KZTNG44XDPMVTVCV4WJSHG': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F8KZTNG44XDPMVTVCV4WJSHG-story-define-design-time-provider-specific-sql-a' from source 'f588798af68c9817e16c82ec688c8fe14ea2dee3'.
- Removed bot-owned orchestration details from the persisted PO delivery contract.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- 1 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- If later edits desynchronize the ticket narrative from the durable planning surfaces again, PO-critic should reopen the ticket.
- Future implementation work could widen this lane into runtime dispatch or automatic migration behavior unless it stays anchored to the explicit non-goals in the contract.
- A prototype that skips benchmark or semantic-parity evidence could create unmeasured provider-specific claims that conflict with the repository's performance-evidence posture.
- Split recommendation: No new split is justified now; the current parent and child tickets still separate architecture contract, evidence rules, dry-run prototype, and v0.32 documentation alignment.
- Split recommendation: If a later phase wants deployable SQL payload emission, runtime invocation helpers, or provider-specific validators, create separate follow-up tickets instead of widening this contract or the dry-run prototype ticket.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `44665`
- cached-tokens: `7552`
- effective-cache-ratio: `0.1691`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `b04af28f06f345f89d4b4ed107307fba`
- completed-at-utc: `<redacted>-06T21:43:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F8KZTNG44XDPMVTVCV4WJSHG/runs/20260606T214318942Z-b04af28f06f345f89d4b4ed107307fba.json`