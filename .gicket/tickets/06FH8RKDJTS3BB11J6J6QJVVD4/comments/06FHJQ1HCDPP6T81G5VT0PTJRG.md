[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06FH8RKDJTS3BB11J6J6QJVVD4'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FH8RKDJTS3BB11J6J6QJVVD4`.
- Optimistic claim succeeded (`expectedRevision=06FH8SN51A4MEHPZ7VP2RV2ENM`, `currentRevision=06FHJMDDRZPM9VE38SRQPQNFJ0`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06FH8RKDJTS3BB11J6J6QJVVD4': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06FH8RKDJTS3BB11J6J6QJVVD4': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06FH8RKDJTS3BB11J6J6QJVVD4-task-add-privacy-configuration-api-for-custom-or' from source '57ebadd747b19ede8edaa6a987c077e5577309c5'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06FH8RKDJTS3BB11J6J6QJVVD4-task-add-privacy-configuration-api-for-custom-or` as `d3d02cf5c4bc`.

Open questions / Risiken
- The current ticket title and short draft description can invite over-scoping into a shared cross-provider native runtime feature unless implementers follow the provider-package boundary documented in the repo.
- A silent downgrade from an explicitly requested native capability to some other behavior would violate the existing fail-closed privacy posture and create user-visible ambiguity.
- If future provider-specific APIs drift away from the reviewed capability-fact matrix, diagnostics, documentation, and runtime behavior could diverge.
- Because capability-reporting work is already done, teams may incorrectly assume native execution support already exists unless this ticket keeps discovery/reporting clearly separate from execution/configuration.
- Split recommendation: Keep provider-native execution split to one provider and one exact capability per ticket; let 06FH8RMFZSVNW0KKTZT9HMGM8G own the first bounded proof plus fallback tests.
- Split recommendation: Keep documentation rollout or consumer guidance updates separate from this configuration-contract ticket instead of widening the current scope.
- Split recommendation: If future work needs environment probing, key-store validation, or secret-handling review, split that into a separate opt-in diagnostics/preflight ticket rather than expanding this selection contract.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.7552`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `b27dd52a2a1447899d627cea0e1cd5a8`
- completed-at-utc: `<redacted>-30T16:28:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FH8RKDJTS3BB11J6J6QJVVD4/runs/20260630T162820191Z-b27dd52a2a1447899d627cea0e1cd5a8.json`