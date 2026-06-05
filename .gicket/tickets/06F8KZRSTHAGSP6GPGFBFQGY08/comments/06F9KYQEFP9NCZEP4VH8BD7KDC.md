[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F8KZRSTHAGSP6GPGFBFQGY08'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F8KZRSTHAGSP6GPGFBFQGY08`.
- Optimistic claim succeeded (`expectedRevision=06F9JFCJJNZD6479NX7DA0F6JM`, `currentRevision=06F9KWJ6Y9J3XB73X4RRG0DH50`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F8KZRSTHAGSP6GPGFBFQGY08': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F8KZRSTHAGSP6GPGFBFQGY08': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F8KZRSTHAGSP6GPGFBFQGY08-task-add-bounded-performance-decision-tree-docum' from source '2ced96059fb82a1ed41ea12a933669c2097e4e50'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06F8KZRSTHAGSP6GPGFBFQGY08-task-add-bounded-performance-decision-tree-docum` as `11128b56e970`.

Open questions / Risiken
- If the examples merely paraphrase the contract instead of showing adopter decision points and fallback cases, the ticket will not materially improve the practical guidance gap this follow-up is supposed to cover.
- If non-SQLite latest-satellite reads or optional-provider PIT or bridge rows are presented as measured wins instead of diagnostics-gated or skipped evidence, the docs will overclaim beyond the repository baseline.
- If the guide omits maintenance freshness, `ReadShape`, or diagnostic fallback examples, adopters may treat PIT or bridge optimization as automatic rather than caller-owned and evidence-gated.
- If the checklist is expanded beyond a short pointer, it risks becoming a second source of truth that duplicates `docs/performance-profiles.md`.
- Split recommendation: No additional split is justified. Keep this ticket as the bounded practical-example follow-up under epic `06F8KZQNH8CCMTJW9P95W1N388` and leave release-note or README coordination to the existing downstream release-doc work.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9367`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `776780988ea54d5dba8719ef5afe6940`
- completed-at-utc: `<redacted>-05T22:50:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F8KZRSTHAGSP6GPGFBFQGY08/runs/20260605T225019636Z-776780988ea54d5dba8719ef5afe6940.json`