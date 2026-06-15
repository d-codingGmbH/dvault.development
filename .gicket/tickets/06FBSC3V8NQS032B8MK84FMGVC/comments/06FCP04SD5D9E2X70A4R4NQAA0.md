[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06FBSC3V8NQS032B8MK84FMGVC'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FBSC3V8NQS032B8MK84FMGVC`.
- Optimistic claim succeeded (`expectedRevision=06FBSCXVX56Z5MJ2ZE4BPXACHG`, `currentRevision=06FCNXRJSBTMW0GZ3TH3DMDEZC`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06FBSC3V8NQS032B8MK84FMGVC': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06FBSC3V8NQS032B8MK84FMGVC': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06FBSC3V8NQS032B8MK84FMGVC-task-add-provider-evidence-manifest-shape' from source '10d01ba019883f9167bcd75c85cbca33aea1d853'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06FBSC3V8NQS032B8MK84FMGVC-task-add-provider-evidence-manifest-shape` as `c15ca7fde360`.

Open questions / Risiken
- If the new manifest duplicates provider facts that still evolve independently inside executionDetail, benchmark artifacts and docs can drift; implementation should derive both from the same closed vocabularies or shared mapping.
- Docs-side evidence already includes non-timing postures such as diagnostics-only and smoke-only; omitting those from v1 would force follow-up tickets back into parallel ad hoc shapes.
- If the ticket expands beyond provider optimization evidence rows into every benchmark row family, the contract will sprawl and lose the bounded purpose justified by current repository evidence.
- Split recommendation: No split recommended for the current ticket; contract definition and alignment of existing benchmark and docs evidence surfaces are one bounded unit.
- Split recommendation: If later work wants a new exporter or checked-in manifest artifact, track that as a follow-up after this ticket lands the shared row contract.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9317`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `95d56ed139b14c12ba91ebd28d286b22`
- completed-at-utc: `<redacted>-15T11:17:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FBSC3V8NQS032B8MK84FMGVC/runs/20260615T111754657Z-95d56ed139b14c12ba91ebd28d286b22.json`