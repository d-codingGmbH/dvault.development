[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06FE4R1XJVQZTQ8S9WN2YE3ZKW'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FE4R1XJVQZTQ8S9WN2YE3ZKW`.
- Optimistic claim succeeded (`expectedRevision=06FE4R3NA6RXJHX6ZN5Q92VD3C`, `currentRevision=06FEG5T237SZVJF99PECEQH79G`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06FE4R1XJVQZTQ8S9WN2YE3ZKW': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06FE4R1XJVQZTQ8S9WN2YE3ZKW': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06FE4R1XJVQZTQ8S9WN2YE3ZKW-task-profile-hash-canonicalization-and-hash-diff' from source '44e9cf9189d183ca5636a5f3a5b21ac3137755e2'.
- Interactive PO tool loop fell back to legacy planning after MODEL-TOOL-INVOCATION-RESULT-TOOL-CALL-ARGUMENTS-JSON-INVALID.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- 1 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- Whole-scenario allocation numbers can hide the true in-memory hotspot order if the evidence does not separately isolate DB/EF overhead from DVault-owned canonicalization and save-preparation work.
- Because satellite `HashDiff` values are caller-supplied, the ticket can be overread if upstream payload-hash generation costs are mixed into the DVault hotspot summary.
- A ranking taken only on the default SQLite/`sha256-v1`/`HexString` baseline should not be generalized to provider-specific or non-default hash-key variants without follow-up validation.
- Split recommendation: Keep this ticket evidence-only. If the ranking surfaces independent hotspot families, land follow-up implementation tickets separately for stable-hash canonicalization/hash generation and for satellite replay/save-preparation allocation reduction.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8964`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `272600b337ae49a4a614ccfd30bc2676`
- completed-at-utc: `<redacted>-21T03:00:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FE4R1XJVQZTQ8S9WN2YE3ZKW/runs/20260621T030048175Z-272600b337ae49a4a614ccfd30bc2676.json`