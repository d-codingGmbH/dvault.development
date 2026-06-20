[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06FE4QR3DD7EFZ4F35SBTFGWSR'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FE4QR3DD7EFZ4F35SBTFGWSR`.
- Optimistic claim succeeded (`expectedRevision=06FE4QTGNND3W8A214RQWCG06R`, `currentRevision=06FE87DGK4X1N5PH5Z6XYW3G4R`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06FE4QR3DD7EFZ4F35SBTFGWSR': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06FE4QR3DD7EFZ4F35SBTFGWSR': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06FE4QR3DD7EFZ4F35SBTFGWSR-task-tune-db2-optimized-save-and-read-evidence-p' from source '53621d2d6d7dfe0916db92185bf473e2b0550501'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Detected directly written bounded PO planning artifact for transactional writeback: docs/plans/db2-hotspot-evidence-refinement-06FE4QR3DD7EFZ4F35SBTFGWSR.md.
- 3 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- Without strict evidence-posture wording, downstream work could overread selectedStrategy or plannedReadStrategy tokens, or smoke-test success, as measured timing evidence.
- DB2 support remains configuration-sensitive: missing DVAULT_TEST_DB2_CONNECTION_STRING, dirty save contexts, unsupported read shapes, incomplete read-shape evidence, or stale PIT/bridge maintenance all force provider-neutral fallback.
- This ticket must stay within the already implemented DB2 provider boundary; widening runtime capability and collecting timing evidence are separate concerns and should not be conflated.
- The downstream docs-update ticket depends on this evidence handoff; partial evidence or ambiguous promotion rules would propagate inconsistent v0.42 documentation claims.
- Split recommendation: No additional PO split is recommended. 06FE4QR3DD7EFZ4F35SBTFGWSR already owns the provider-configured DB2 hotspot evidence slice left open by the done guardrail task 06FE4QPEZW97YR6YT7MQD1MXTG.
- Split recommendation: Keep the existing downstream blocks relation to 06FE4QRMXVGJVA65ZR5MZ817K8; documentation and release-surface promotion should remain a separate follow-up that consumes this ticket's evidence output rather than being merged back into this ticket.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9393`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `65652583981746e193ae6f856ad0e007`
- completed-at-utc: `<redacted>-20T08:31:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FE4QR3DD7EFZ4F35SBTFGWSR/runs/20260620T083106724Z-65652583981746e193ae6f856ad0e007.json`