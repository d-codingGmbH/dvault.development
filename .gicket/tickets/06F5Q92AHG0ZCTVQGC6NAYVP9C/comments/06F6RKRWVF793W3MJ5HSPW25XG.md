[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F5Q92AHG0ZCTVQGC6NAYVP9C'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F5Q92AHG0ZCTVQGC6NAYVP9C`.
- Optimistic claim succeeded (`expectedRevision=06F5Q99BHAYDT9PY9DK3A35WAR`, `currentRevision=06F6RFGN6EGGC9NP07FBDKXVNR`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F5Q92AHG0ZCTVQGC6NAYVP9C': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F5Q92AHG0ZCTVQGC6NAYVP9C': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F5Q92AHG0ZCTVQGC6NAYVP9C-story-generate-typed-latest-and-as-of-satellite' from source '35f3916c23af4cd9e9a81cc515e7b89f6c8ba107'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06F5Q92AHG0ZCTVQGC6NAYVP9C-story-generate-typed-latest-and-as-of-satellite` as `bc8250d72e59`.

Open questions / Risiken
- The contract intentionally bounds generated satellite properties to shapes that can be proven from authoritative metadata; consumers with provider-specific or non-string payload mappings will receive diagnostics or skipped generation rather than helpers.
- Metadata-source ambiguity or source-fingerprint drift will hard-fail generation by design, so teams using multiple declaration paths must keep one authoritative source visible per generated scope.
- If any supported shape uses the optional stable direct EF projection path, implementation must stay inside the repository's compiled-model and compiled-query compatibility boundary to avoid subtle provider regressions.
- Split recommendation: No further split is recommended: the repository already isolates PIT and bridge generation into `06F5Q92R02HB7FCE1AWKXPTMRW`, and the remaining satellite generator and analyzer slice is bounded enough for one implementation story.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.7344`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `4c6ec651c11b43a694a03c10406357a3`
- completed-at-utc: `<redacted>-28T01:59:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F5Q92AHG0ZCTVQGC6NAYVP9C/runs/20260528T015944595Z-4c6ec651c11b43a694a03c10406357a3.json`