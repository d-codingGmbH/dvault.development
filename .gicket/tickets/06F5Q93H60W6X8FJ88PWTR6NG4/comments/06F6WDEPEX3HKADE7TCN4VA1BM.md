[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F5Q93H60W6X8FJ88PWTR6NG4'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F5Q93H60W6X8FJ88PWTR6NG4`.
- Optimistic claim succeeded (`expectedRevision=06F5Q99QP3BRH7VVCG526D2BPM`, `currentRevision=06F6WA88PV8R4QM5YJVN9SEREC`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F5Q93H60W6X8FJ88PWTR6NG4': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F5Q93H60W6X8FJ88PWTR6NG4': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F5Q93H60W6X8FJ88PWTR6NG4-task-update-v0-22-0-typed-read-and-hash-governan' from source 'c7ac736dbb30d48cc9a4c9782f3b47825c519737'.
- Interactive PO tool loop hit bounded stop reason 'tool_call_limit_reached' and fell back to legacy planning.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- 1 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- The biggest documentation risk is overstating the generator boundary by implying PIT or bridge helpers, direct raw `dvault.model.v1` parsing, or runtime request compilation that the current implementation does not ship.
- Hash-governance wording can become misleading if metadata-source fingerprint drift and stable-hash compatibility are blended together; the docs should keep those as separate governance topics.
- Linking to non-existent generator snapshot artifacts or analyzer public API snapshot files would create false evidence claims, because the visible repo evidence uses generator tests and runtime/provider public API snapshots instead.
- If the targeted docs do not all move to the same v0.22.0 current-baseline wording, readers will get mixed release guidance between README, the checklist, model-first guidance, and release notes.
- Split recommendation: If the work expands into new quality infrastructure such as dedicated generator approval snapshots or analyzer API snapshot coverage, split that into a separate quality/evidence ticket.
- Split recommendation: If the release also needs public docs for PIT or bridge generated helpers, split that into a later ticket tied to the actual shipped implementation rather than broadening this documentation ticket beyond current behavior.

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
- run-id: `9f23e8441708465e9003478478490d95`
- completed-at-utc: `<redacted>-28T10:51:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F5Q93H60W6X8FJ88PWTR6NG4/runs/20260528T105122607Z-9f23e8441708465e9003478478490d95.json`