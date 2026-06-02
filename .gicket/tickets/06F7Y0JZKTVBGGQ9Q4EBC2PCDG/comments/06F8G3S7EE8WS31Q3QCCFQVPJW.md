[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F7Y0JZKTVBGGQ9Q4EBC2PCDG'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F7Y0JZKTVBGGQ9Q4EBC2PCDG`.
- Optimistic claim succeeded (`expectedRevision=06F7Y0ZKMKM6JTV9DTKK08VBAC`, `currentRevision=06F8FZ70W6D6K0G6BFWPHXZ4FG`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F7Y0JZKTVBGGQ9Q4EBC2PCDG': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F7Y0JZKTVBGGQ9Q4EBC2PCDG': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F7Y0JZKTVBGGQ9Q4EBC2PCDG-story-add-provider-strategy-eligibility-and-thre' from source 'd80ebb757f8c211a046ccb3fa6b46b58913b754a'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06F7Y0JZKTVBGGQ9Q4EBC2PCDG-story-add-provider-strategy-eligibility-and-thre` as `fea9208e472c`.

Open questions / Risiken
- Recommendation mappings can drift from the checked-in profile and benchmark baseline unless the closed category set stays anchored to those documents.
- Read guidance can overpromise provider-specific behavior if implementation turns profile hints into non-SQLite optimized read claims that the repository does not prove.
- Redaction can regress if provider exception text or workload values leak into serialized diagnostics instead of staying behind finite fallback messages and omitted optional fields.
- Split recommendation: Keep the current split unchanged: historical contract story `06F7Y0JQ2FZQZVTNFX2T25DAS4` remains background evidence, this story owns the diagnostics implementation, `06F7Y0K95VW0PX21F6R2YGP8DM` owns verification, and `06F7Y0NBHXQ6CK8R3AH4DEP9V4` owns doc...
- Split recommendation: If the team later wants new benchmark profiles, provider-specific read thresholds, attachment-backed support material, transport or reporting surfaces, or automatic tuning behavior, create separate follow-up work instead of widening this ticket.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.7410`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `905641daac8543a0a95425e8c6b4e9a6`
- completed-at-utc: `<redacted>-02T11:19:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F7Y0JZKTVBGGQ9Q4EBC2PCDG/runs/20260602T111915056Z-905641daac8543a0a95425e8c6b4e9a6.json`