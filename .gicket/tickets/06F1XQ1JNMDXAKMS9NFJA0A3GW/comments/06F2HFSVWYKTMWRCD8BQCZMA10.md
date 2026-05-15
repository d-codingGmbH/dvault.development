[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F1XQ1JNMDXAKMS9NFJA0A3GW'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F1XQ1JNMDXAKMS9NFJA0A3GW`.
- Optimistic claim succeeded (`expectedRevision=06F1XTQAWGH8SXYYFVC16DN5A4`, `currentRevision=06F2HD832B24130VNHNQQ44WJ0`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F1XQ1JNMDXAKMS9NFJA0A3GW': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F1XQ1JNMDXAKMS9NFJA0A3GW': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F1XQ1JNMDXAKMS9NFJA0A3GW-task-implement-first-analyzer-rules-and-tests' from source 'e6851bfc769587b279a2b812647f7a502759aac2'.
- Interactive PO tool loop fell back to legacy planning after MODEL-TOOL-INVOCATION-RESULT-TOOL-CALL-ARGUMENTS-JSON-INVALID.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- 1 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- If the analyzer attempts to infer builder state across locals, helper methods, or complex control flow in v1, false positives and false negatives will rise quickly.
- Because current test projects disable analyzer execution, the ticket can appear complete without real analyzer coverage unless the explicit test harness is added.
- If new analyzer diagnostics do not follow the existing catalog conventions, DVault diagnostic ids and categories may drift across runtime and Roslyn surfaces.
- Expanding this ticket beyond the two high-confidence Code-First rules will likely turn it into a broader analyzer-foundation effort and slow delivery.
- Split recommendation: No additional split is required for PO-critic readiness; this ticket is already well-bounded if it stays on the two high-confidence Code-First rules plus the minimal harness needed to test them.
- Split recommendation: If the team wants broader analyzer coverage such as missing business keys or link-participant validation, create a follow-up task after this first low-noise rule pair ships.
- Split recommendation: If packaging polish, installation guidance, or suppression documentation is still missing after the minimal scaffolding is in place, keep that work on the parent analyzer-foundation story rather than expanding this task.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9294`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `0b8484c98f4142dea49fefa8f1faac37`
- completed-at-utc: `<redacted>-14T23:08:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F1XQ1JNMDXAKMS9NFJA0A3GW/runs/20260514T230801747Z-0b8484c98f4142dea49fefa8f1faac37.json`