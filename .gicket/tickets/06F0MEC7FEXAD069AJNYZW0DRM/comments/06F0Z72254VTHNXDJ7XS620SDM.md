[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F0MEC7FEXAD069AJNYZW0DRM'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F0MEC7FEXAD069AJNYZW0DRM`.
- Optimistic claim succeeded (`expectedRevision=06F0Z44SG0RVB9AGH8H46ET0YM`, `currentRevision=06F0Z4BZPBQXYAZ5SZCXWR46AR`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F0MEC7FEXAD069AJNYZW0DRM': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F0MEC7FEXAD069AJNYZW0DRM': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F0MEC7FEXAD069AJNYZW0DRM-task-define-typed-hub-link-and-satellite-mapper' from source 'fad6c60d4ec6ac10ac948462f91be78868021ccb'.
- Interactive PO tool loop fell back to legacy planning after MODEL-TOOL-INVOCATION-RESULT-TOOL-CALL-ARGUMENTS-JSON-INVALID.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- 1 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- If implementation tries to accept same-hub or self-link typed links without changing the participant identity shape, distinct participants will collapse because the current registry-backed operation is keyed only by participant hub metadata name and rejects duplicates.
- If docs or tests keep promising missing required values before save orchestration begins, the ticket will misstate the existing repository boundary and force an unplanned validator abstraction.
- If implementation targets metadata-object-based save operations instead of the chosen registry-backed operation family, typed helpers will drift from the authoritative-registry path already established in the repository.
- If hidden CLR-type metadata inference is added in v1, metadata-first or code-first registrations without DataVaultMetadataClrMapping will fail unpredictably even though current repository evidence makes CLR mappings optional.
- Because current operation inputs are string-based, weak coverage around mapper-produced string values could still allow inconsistent caller-side business-key, participant-hash-key, or hash-diff formatting unless tests pin the contract down clearly.
- Split recommendation: No split is needed for this v1 contract ticket as refined.
- Split recommendation: If same-hub or self-link typed link support becomes a real requirement, open a separate follow-up ticket for participant role-, ordinal-, or alias-based identity and any necessary save-operation shape changes instead of stretching this mapper v1 contract.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9586`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `b483037bd9d24dabb3e42318c07d650b`
- completed-at-utc: `<redacted>-10T01:59:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F0MEC7FEXAD069AJNYZW0DRM/runs/20260510T015918862Z-b483037bd9d24dabb3e42318c07d650b.json`