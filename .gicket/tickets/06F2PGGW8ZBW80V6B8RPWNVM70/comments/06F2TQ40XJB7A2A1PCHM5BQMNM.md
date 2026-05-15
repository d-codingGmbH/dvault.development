[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F2PGGW8ZBW80V6B8RPWNVM70'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F2PGGW8ZBW80V6B8RPWNVM70`.
- Optimistic claim succeeded (`expectedRevision=06F2PNHQJRFA20MDC4BTWD287W`, `currentRevision=06F2TN72MR341FJS085ZYRMDKC`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F2PGGW8ZBW80V6B8RPWNVM70': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F2PGGW8ZBW80V6B8RPWNVM70': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F2PGGW8ZBW80V6B8RPWNVM70-story-harden-migration-guardrails-for-ci-enforce' from source 'e591eb55bbc9eafc827a246391e75b9d4c88b64e'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06F2PGGW8ZBW80V6B8RPWNVM70-story-harden-migration-guardrails-for-ci-enforce` as `b652524ffccb`.

Open questions / Risiken
- Broadening beyond provider-neutral structural invariants would create noisy false positives across providers and weaken CI trust.
- The current guardrail lane has no authoritative prior-schema state, so absence-based or rename-based inference would be unstable if added here.
- If create-table and existing operation findings stop sorting deterministically, downstream `guardrail` output and test baselines will churn.
- Broader v0.11 discoverability still depends on separate documentation task `06F2PGHA0EXJRGDHM4GQM7NPYR`.
- Split recommendation: No new split is required now; keep implementation-level rule coverage in already-done child `06F2PGH42B6BT1708MYGMXP5GM`.
- Split recommendation: Keep broader README and release-note rollout in existing blocked task `06F2PGHA0EXJRGDHM4GQM7NPYR`.
- Split recommendation: Treat rename-table, missing-table inference, and provider-specific facet checks as later follow-up tickets rather than widening this story.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9402`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `670cc30f39bf411bbfebc45cef3cb1b3`
- completed-at-utc: `<redacted>-15T20:38:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F2PGGW8ZBW80V6B8RPWNVM70/runs/20260515T203817433Z-670cc30f39bf411bbfebc45cef3cb1b3.json`