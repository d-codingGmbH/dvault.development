[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06FE4R9ZC210EE5AW4WCWQN32G'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FE4R9ZC210EE5AW4WCWQN32G`.
- Optimistic claim succeeded (`expectedRevision=06FE4RCG3NMWWHV2B9X1J93DTG`, `currentRevision=06FERD0HFNVHQ9C45Y443H6VHW`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06FE4R9ZC210EE5AW4WCWQN32G': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06FE4R9ZC210EE5AW4WCWQN32G': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06FE4R9ZC210EE5AW4WCWQN32G-task-design-personal-data-satellite-field-metada' from source '612cbaeabd38e5bfaddf4ff5fec4c3ccaf58965b'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06FE4R9ZC210EE5AW4WCWQN32G-task-design-personal-data-satellite-field-metada` as `cec2491b057f`.

Open questions / Risiken
- If the contract tries to replace the existing `payload` declaration shape instead of augmenting it, it will reopen the already-fixed `dvault.model.v1` artifact contract and create parser and exporter churn.
- If provider-specific ciphertext storage or crypto choices leak into the shared contract, the provider-neutral EF boundary approved by the privacy story will erode quickly.
- If the metadata contract is vague about history compatibility, later implementation work may accidentally couple privacy mapping to changed hash-diff or multi-active behavior.
- Split recommendation: No split is needed if this ticket stays as the authoritative contract-definition lane for personal-data satellite field metadata.
- Split recommendation: Keep parser or API changes, privacy package behavior, and provider-specific execution or storage optimization as follow-on implementation tickets rather than widening this ticket.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9493`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `6f85fd989d164451ad38f5d91f1e0420`
- completed-at-utc: `<redacted>-21T22:22:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FE4R9ZC210EE5AW4WCWQN32G/runs/20260621T222248602Z-6f85fd989d164451ad38f5d91f1e0420.json`