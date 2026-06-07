[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F8KZTNG44XDPMVTVCV4WJSHG'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F8KZTNG44XDPMVTVCV4WJSHG`.
- Optimistic claim succeeded (`expectedRevision=06F8M02T2QPWQ96JMZQPTAEF1C`, `currentRevision=06F9XEDGNQN6BR43NH0JCVY66G`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F8KZTNG44XDPMVTVCV4WJSHG': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F8KZTNG44XDPMVTVCV4WJSHG': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F8KZTNG44XDPMVTVCV4WJSHG-story-define-design-time-provider-specific-sql-a' from source '3bb170c47e7be09105c00ad1a2101a3b12f91262'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Detected directly written bounded PO planning artifact for transactional writeback: docs/plans/provider-specific-sql-artifact-contract.md.
- 3 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- Future implementation work could accidentally widen this lane into runtime dispatch or automatic migration behavior unless it stays anchored to the explicit non-goals now documented.
- A prototype that skips the benchmark or semantic parity evidence gate could create unmeasured provider-specific claims that conflict with the repository's existing performance-evidence posture.
- If later work stores executable SQL payloads without a deterministic manifest-relative convention, review and drift management could become inconsistent across consumer projects.
- Split recommendation: No new split is needed now; the current parent and child tickets already separate architecture contract, evidence rules, dry-run prototype, and v0.32 documentation work.
- Split recommendation: If a later phase wants deployable SQL payload emission, runtime invocation helpers, or provider-specific validators, create separate follow-up tickets instead of widening this contract or the dry-run prototype ticket.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9411`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `a17c602702fc4f9cb961fc158e29c124`
- completed-at-utc: `<redacted>-06T21:10:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F8KZTNG44XDPMVTVCV4WJSHG/runs/20260606T211038019Z-a17c602702fc4f9cb961fc158e29c124.json`