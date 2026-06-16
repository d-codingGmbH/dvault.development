[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06FBSCF61N0TYPYH7008TRD6VR'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FBSCF61N0TYPYH7008TRD6VR`.
- Optimistic claim succeeded (`expectedRevision=06FBSDBGPRZ7828T6W0YZ7GJ04`, `currentRevision=06FD10YS824BFDJCFRX0X5HSTW`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06FBSCF61N0TYPYH7008TRD6VR': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06FBSCF61N0TYPYH7008TRD6VR': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06FBSCF61N0TYPYH7008TRD6VR-story-define-provider-read-parity-acceptance-cri' from source '8c518692a462d13dcb266b9654d7b404ddc39a80'.
- Removed bot-owned orchestration details from the persisted PO delivery contract.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- 1 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- Downstream tickets can overclaim external-provider performance if they treat parity-only, `skipped-placeholder`, `diagnostics-only`, or `smoke-only` evidence as completed timing.
- DB2 remains intentionally narrower than the other provider lanes: PIT and bridge candidate behavior may be cited, but DB2 latest-satellite optimization and completed DB2 timing still require deliberate scope expansion.
- Split recommendation: No new split recommended; the live graph already covers the next bounded work as PIT and bridge audit ticket `06FBSCGBG8CJ0QNRX4JZJA638G` plus latest-satellite gap tickets `06FBSCFDFFYQXBK17RT3E8W4CM`, `06FBSCFKWGQMBEF5Q96AZ5Q0X0`, `06FBSCFVT3SBHKMDGNEXWV...
- Split recommendation: Do not pre-split PIT and bridge implementation tickets before `06FBSCGBG8CJ0QNRX4JZJA638G` applies the refined criteria and classifies each provider lane.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9195`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `8ae5f9f79fb5485c8c5b1da65a4760fc`
- completed-at-utc: `<redacted>-16T13:09:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FBSCF61N0TYPYH7008TRD6VR/runs/20260616T130927402Z-8ae5f9f79fb5485c8c5b1da65a4760fc.json`