[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F7Y0JQ2FZQZVTNFX2T25DAS4'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F7Y0JQ2FZQZVTNFX2T25DAS4`.
- Optimistic claim succeeded (`expectedRevision=06F7Y0Z97TH2JE609B3KRQ1RJ8`, `currentRevision=06F8EYH13MH29NNF427BSYEEJR`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F7Y0JQ2FZQZVTNFX2T25DAS4': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F7Y0JQ2FZQZVTNFX2T25DAS4': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F7Y0JQ2FZQZVTNFX2T25DAS4-story-define-provider-performance-tuning-diagnos' from source '897e416454092e0435c9e145012c3b759b7ea1b9'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06F7Y0JQ2FZQZVTNFX2T25DAS4-story-define-provider-performance-tuning-diagnos` as `ba79ffd3bed5`.

Open questions / Risiken
- Recommendation prose can drift from checked-in evidence unless the implementation keeps the machine-readable category set anchored to the four current performance profiles.
- Read tuning could overpromise provider-specific behavior if developers infer query-plan or numeric-threshold claims that the current repository does not prove.
- Redaction can regress if provider exception text or workload values leak into example payloads instead of staying behind finite fallback messages and omitted optional fields.
- Split recommendation: Keep the current split: this ticket defines the contract, 06F7Y0JZKTVBGGQ9Q4EBC2PCDG implements eligibility, threshold, and recommendation diagnostics, and 06F7Y0K95VW0PX21F6R2YGP8DM owns benchmark-artifact verification.
- Split recommendation: If the team later wants new benchmark profiles, provider-specific read thresholds, or transport and reporting surfaces, create separate follow-up stories rather than widening this contract ticket.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `44751`
- cached-tokens: `7552`
- effective-cache-ratio: `0.1688`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `ffa904dc041c49c886661f44b745ec6f`
- completed-at-utc: `<redacted>-02T08:46:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F7Y0JQ2FZQZVTNFX2T25DAS4/runs/20260602T084602691Z-ffa904dc041c49c886661f44b745ec6f.json`