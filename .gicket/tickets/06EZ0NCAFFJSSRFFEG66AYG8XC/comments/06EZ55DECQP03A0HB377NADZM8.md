[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06EZ0NCAFFJSSRFFEG66AYG8XC'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EZ0NCAFFJSSRFFEG66AYG8XC`.
- Optimistic claim succeeded (`expectedRevision=06EZ54199A7KC04A9HJXX6XNVC`, `currentRevision=06EZ544XXAXW86HB6W1JSC56KC`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-write-p...
- Bounded PO planning surfaces enabled for ticket '06EZ0NCAFFJSSRFFEG66AYG8XC': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06EZ0NCAFFJSSRFFEG66AYG8XC': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06EZ0NCAFFJSSRFFEG66AYG8XC-story-consolidate-provider-benchmark-reporting' from source '337a1b0a34d4ae039e00bbe435250f555ef3c041'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06EZ0NCAFFJSSRFFEG66AYG8XC-story-consolidate-provider-benchmark-reporting` as `1e6676c26b6b`.

Open questions / Risiken
- Reusing a test-named environment variable for benchmarks may confuse users unless the README and skipped-row reasons are explicit.
- If skipped-provider reason normalization drifts, archived artifacts may still be hard to compare across machines.
- Absolute PostgreSQL timings can vary substantially across local environments, so the report must keep scenario metadata and skip semantics prominent.
- Split recommendation: Keep this story focused on the consolidated artifact plus the SQLite-required and PostgreSQL-optional contract; move SQL Server, Oracle, and MySQL expansion into separate provider tickets.
- Split recommendation: If benchmark-specific configuration surfaces or CI provisioning grow beyond straightforward env-var discovery, split that infrastructure work into separate follow-up tickets rather than widening this story.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `63119`
- cached-tokens: `2432`
- effective-cache-ratio: `0.0385`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `f463aa67ebf14f88a4fa8f109caad738`
- completed-at-utc: `<redacted>-04T10:43:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EZ0NCAFFJSSRFFEG66AYG8XC/runs/20260504T104308278Z-f463aa67ebf14f88a4fa8f109caad738.json`