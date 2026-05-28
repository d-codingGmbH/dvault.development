[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F5Q91V0YGSA6SH9WDS02GH0M'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F5Q91V0YGSA6SH9WDS02GH0M`.
- Optimistic claim succeeded (`expectedRevision=06F6XEBTH9F6NPF7EN6CMXQGFM`, `currentRevision=06F6XEMZRG8G3T54PJ67WB0NXW`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F5Q91V0YGSA6SH9WDS02GH0M': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F5Q91V0YGSA6SH9WDS02GH0M': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F5Q91V0YGSA6SH9WDS02GH0M-epic-typed-read-models-and-hash-governance' from source '4a486aba6f9f81e5f6eb1bfe17cc283593d63bae'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06F5Q91V0YGSA6SH9WDS02GH0M-epic-typed-read-models-and-hash-governance` as `6d9fbfbfe568`.

Open questions / Risiken
- Until the queued develop-branch replay lands, readers of docs/plans/typed-read-model-generator-contract.md or docs/plans/README.md on develop can still encounter the older PIT or bridge helper wording that this epic now explicitly supersedes.
- Future docs or implementation work could overstate the shipped typed-read boundary by implying PIT or bridge helper emission before a separate additive ticket lands.
- Any unversioned change to the sha256-v1 canonicalization rules or published vectors would break the compatibility contract this epic establishes.
- If DMV196x unsupported-shape behavior regresses, consumers may no longer distinguish unsupported metadata from misconfiguration, which would blur the current satellite-only boundary.
- Split recommendation: No additional split is recommended now; the existing seven-child decomposition is already persisted and complete for this epic.
- Split recommendation: If future work expands into shipped PIT or bridge helpers, automatic hashDiff generation, or new hash encodings, create additive follow-up tickets instead of reopening this parent epic.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `65558`
- cached-tokens: `2432`
- effective-cache-ratio: `0.0371`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `2d26e35498b94f2998ecacb111e0b6d4`
- completed-at-utc: `<redacted>-28T13:55:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F5Q91V0YGSA6SH9WDS02GH0M/runs/20260528T135558594Z-2d26e35498b94f2998ecacb111e0b6d4.json`