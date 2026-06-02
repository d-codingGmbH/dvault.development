[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F7Y0NBHXQ6CK8R3AH4DEP9V4'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F7Y0NBHXQ6CK8R3AH4DEP9V4`.
- Optimistic claim succeeded (`expectedRevision=06F7Y0ZYZN7M28VKSQ2R18A7EC`, `currentRevision=06F8JWCG06RECNQ6627G27RA70`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F7Y0NBHXQ6CK8R3AH4DEP9V4': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F7Y0NBHXQ6CK8R3AH4DEP9V4': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F7Y0NBHXQ6CK8R3AH4DEP9V4-task-update-v0-26-0-provider-performance-and-sch' from source 'da968453a7d5bc310919abe8111024525300e6b1'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06F7Y0NBHXQ6CK8R3AH4DEP9V4-task-update-v0-26-0-provider-performance-and-sch` as `dfe2bbf32ce9`.

Open questions / Risiken
- Documentation can drift from the checked-in diagnostics and verifier contracts if any surface rephrases recommendation categories, thresholds, or provider claims instead of reusing the established bounded vocabulary.
- The docs can overpromise unsupported behavior if provider-specific read guidance or stored-procedure language goes beyond the SQLite-proven read baseline or beyond the explicit opt-in artifact boundary.
- Because the current checklist and performance-profile docs still advertise older baselines, partial updates could leave contradictory current-release signals across surfaces.
- Split recommendation: No immediate split is needed; the current task remains bounded as documentation alignment over already-completed diagnostics, verifier, and stored-procedure-boundary work.
- Split recommendation: If the team later wants new benchmark-backed profile categories, provider-specific tutorials, release-process automation, or broader operational governance material, open separate follow-up tickets instead of widening this documentation task.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `41918`
- cached-tokens: `7552`
- effective-cache-ratio: `0.1802`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `11a5d0f4c90e4c7c86e221e6952b8524`
- completed-at-utc: `<redacted>-02T17:55:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F7Y0NBHXQ6CK8R3AH4DEP9V4/runs/20260602T175526487Z-11a5d0f4c90e4c7c86e221e6952b8524.json`