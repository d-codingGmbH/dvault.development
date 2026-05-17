[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F2PGKAQVVF8GEZVVC8SHFASG'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F2PGKAQVVF8GEZVVC8SHFASG`.
- Optimistic claim succeeded (`expectedRevision=06F3EEGG1P5MRKDE2DMJVMPCE0`, `currentRevision=06F3EEKTTATD4V8KF44B5AFBQR`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F2PGKAQVVF8GEZVVC8SHFASG': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F2PGKAQVVF8GEZVVC8SHFASG': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F2PGKAQVVF8GEZVVC8SHFASG-story-add-code-first-link-parent-satellites' from source 'fa15f59e79163ea061f9a6f144d6f16c53b661c9'.
- Interactive PO tool loop fell back to legacy planning after MODEL-TOOL-INVOCATION-RESULT-TOOL-CALL-ARGUMENTS-JSON-INVALID.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- 1 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- The main scope-creep risk is accidentally folding participant-role/alias support, recursive same-hub links, effectivity, same-as, or other advanced link shapes into this story because those capabilities are adjacent but not required for the bounded Code-First parity gap.
- If implementation adds the new API but misses Code-First metadata/schema/export regression coverage, the branch could still ship a partial feature that diverges from the existing metadata-first CustomerOrder/State baseline.
- Public documentation currently still describes the implemented Code-First surface as hub-parent-satellite-only; if task 06F2PGM9038RXVJH0RJFYEJEV0 does not land promptly after implementation, supported behavior and docs will diverge.
- Split recommendation: No additional split recommended. The product split already exists: this ticket covers the additive Code-First API and projection gap, task 06F2PGM9038RXVJH0RJFYEJEV0 covers documentation and release-note follow-through, and any future mapping/example work...

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9488`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `a22d57ff06034a1789a6bc29a44f5699`
- completed-at-utc: `<redacted>-17T18:43:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F2PGKAQVVF8GEZVVC8SHFASG/runs/20260517T184342793Z-a22d57ff06034a1789a6bc29a44f5699.json`