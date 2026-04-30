[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06EXB7FF1J9NR2849WKDR8DKPG'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EXB7FF1J9NR2849WKDR8DKPG`.
- Optimistic claim succeeded (`expectedRevision=06EXW5MKW6H57D0DSGR0V4W5W0`, `currentRevision=06EY0RZ9QYCMWNZ2BXKSP8JZ1R`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-write-p...
- Bounded PO planning surfaces enabled for ticket '06EXB7FF1J9NR2849WKDR8DKPG': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06EXB7FF1J9NR2849WKDR8DKPG': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06EXB7FF1J9NR2849WKDR8DKPG-story-integrate-with-ef-core-model-building' from source '6c2f37e269fcb16fbdaebd33317505d5330f7d51'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06EXB7FF1J9NR2849WKDR8DKPG-story-integrate-with-ef-core-model-building` as `c92bb1cd1750`.

Open questions / Risiken
- If implementation or tests drift from the established naming-policy baseline, EF metadata shape can become inconsistent with the existing deterministic model contracts.
- If downstream provider-specific work relies on inferred names instead of the explicit DVault-owned annotations, semantics may become brittle across future naming changes.
- Because this story sits above already-refined EF tasks, description drift between the story and those downstream contracts is the main planning risk; the story should remain an umbrella over the existing bounded task split rather than reopening scope.
- Split recommendation: No additional split is recommended; the current story is already bounded by the visible separation between the conventions-only EF entry-point task and the provider-neutral EF metadata translation task.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `45571`
- cached-tokens: `2432`
- effective-cache-ratio: `0.0534`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `68316d8f4e2e43bebe055dda7bbcfb2d`
- completed-at-utc: `<redacted>-30T22:00:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EXB7FF1J9NR2849WKDR8DKPG/runs/20260430T220003028Z-68316d8f4e2e43bebe055dda7bbcfb2d.json`