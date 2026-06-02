[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F7Y0KVHGTTVS216ERSG4XNMM'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F7Y0KVHGTTVS216ERSG4XNMM`.
- Optimistic claim succeeded (`expectedRevision=06F7Y0ZRY0GEASKW4RZ3R7JN6C`, `currentRevision=06F8GBN9YGGRWBQ0PK4VCD03E0`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F7Y0KVHGTTVS216ERSG4XNMM': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F7Y0KVHGTTVS216ERSG4XNMM': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F7Y0KVHGTTVS216ERSG4XNMM-story-add-provider-idempotency-constraint-and-in' from source '3e338299b22f186f6cc6e8cbe7b39ee6c89e6049'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06F7Y0KVHGTTVS216ERSG4XNMM-story-add-provider-idempotency-constraint-and-in` as `b12d69a682a5`.

Open questions / Risiken
- If the comparison logic ignores provider-capability normalization, providers with redundant-index suppression or non-native included-index behavior will false-fail even when their generated schema is correct.
- If implementation reuses general drift reporting without filtering, the preflight output will become noisy and blur the ticket's idempotency-specific purpose.
- If live schema is opened implicitly on ordinary preflight runs, the implementation will violate the documented consumer-owned design-time boundary and make default local/CI execution brittle.
- If remediation output includes raw provider messages, connection details, or database values, it will break the repository's redaction contract.
- Split recommendation: No child-ticket split is required for PO-critic readiness; the repository already has the necessary preflight, live-schema, diagnostics, and provider-capability building blocks, so this remains one bounded additive story.
- Split recommendation: Keep README, architecture-guide, checklist, and release-note rollout on the existing blocked documentation task `06F7Y0NBHXQ6CK8R3AH4DEP9V4` instead of broadening this story's delivery contract.
- Split recommendation: If future work wants non-idempotency live-schema drift aggregation, automatic repository discovery, or first-class support-bundle/command integration, track those as separate follow-up tickets rather than widening this ticket.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9541`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `fd6cdad419c649999f919da126c62ff7`
- completed-at-utc: `<redacted>-02T12:09:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F7Y0KVHGTTVS216ERSG4XNMM/runs/20260602T120918303Z-fd6cdad419c649999f919da126c62ff7.json`