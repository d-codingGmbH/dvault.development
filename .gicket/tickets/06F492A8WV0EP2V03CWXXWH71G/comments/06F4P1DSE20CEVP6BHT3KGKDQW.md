[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F492A8WV0EP2V03CWXXWH71G'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F492A8WV0EP2V03CWXXWH71G`.
- Optimistic claim succeeded (`expectedRevision=06F4NV0BFAN31QTNFH9ZCSY4QR`, `currentRevision=06F4NYEGPKCPK1P3E6HZHZFFA4`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F492A8WV0EP2V03CWXXWH71G': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F492A8WV0EP2V03CWXXWH71G': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F492A8WV0EP2V03CWXXWH71G-story-strengthen-migration-guardrail-reports' from source '2dd473d44ac88e4b33d1a6b80a30de616070ccd9'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06F492A8WV0EP2V03CWXXWH71G-story-strengthen-migration-guardrail-reports` as `3a599c2d8205`.

Open questions / Risiken
- If implementation only changes ToDisplayString() and does not add a machine-readable ordered operation surface, downstream automation and the preflight aggregator will still need to parse text or reimplement classification.
- Provider-aware wording can become misleading if it hard-codes engine claims instead of reflecting the actual diagnostics baseline, especially when provider or profile selection defaulted.
- Building safe, risky, or incompatible summaries from unordered dictionaries or merged finding sets instead of the input operation order will destabilize CI baselines and human review output.
- Because the current guardrail engine remains provider-neutral structural analysis, consumers may overread provider-aware wording as provider-specific validation unless non-goals stay explicit in code, tests, and downstream docs.
- Split recommendation: No new split is recommended; repository evidence already shows the guardrail rule catalog and operation matrix are in place, so this story can stay focused on report-surface strengthening.
- Split recommendation: Keep story 06F492BG6BZYYFMBE5WK7CB024 and task 06F492BNDPWS9P4EDSV0W7G6VM as downstream consumers of the finalized report contract rather than pulling their scope into this ticket.
- Split recommendation: If later work needs provider-specific SQL or store-type hints, migration-history reasoning, or ModelSnapshot-aware inference, raise that as a separate follow-up story instead of widening this ticket.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8297`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `cdabc908b9264f3eb3efcfccdcffda2f`
- completed-at-utc: `<redacted>-21T14:52:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F492A8WV0EP2V03CWXXWH71G/runs/20260521T145206753Z-cdabc908b9264f3eb3efcfccdcffda2f.json`