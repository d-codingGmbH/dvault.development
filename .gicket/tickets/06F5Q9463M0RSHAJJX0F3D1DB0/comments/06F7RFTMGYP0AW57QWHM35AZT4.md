[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F5Q9463M0RSHAJJX0F3D1DB0'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F5Q9463M0RSHAJJX0F3D1DB0`.
- Optimistic claim succeeded (`expectedRevision=06F7QJ9JWGJBPKSRD5M1KJVN0M`, `currentRevision=06F7RD8517SG715Q9R9T26DGM0`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F5Q9463M0RSHAJJX0F3D1DB0': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F5Q9463M0RSHAJJX0F3D1DB0': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F5Q9463M0RSHAJJX0F3D1DB0-story-add-activity-tracing-for-save-and-read-ope' from source '0c87601d7f532fa4b7e027b4b0d04495ebb6ee56'.
- Interactive PO tool loop fell back to legacy planning after MODEL-TOOL-INVOCATION-RESULT-TOOL-CALL-ARGUMENTS-JSON-INVALID.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- 1 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- If bridge tracing is added only inside DefaultDataVaultReadService, callers that use the non-default branch in DataVaultReadServiceBridgeExtensions can miss spans.
- If current/as-of, latest typed-projection, PIT typed-projection, or bridge helper layers add wrapper spans instead of relying on the underlying execution path, duplicate root spans can leak into listener output.
- If tag/event construction happens before listener or sampling checks, the implementation can violate the contract's minimal-overhead no-listener baseline even when StartActivity returns null.
- If a public ActivitySource holder is introduced accidentally, ApiSurfaceSnapshotTests will fail and the package surface changes will require deliberate approval.
- Split recommendation: No further split is recommended; save/read tracing is already separated from PIT and bridge maintenance tracing in ticket 06F5Q94D0JDMMWDXSRGWX1E4F0.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.7701`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `a9ffb4aa41c74526aa6f84743643fde4`
- completed-at-utc: `<redacted>-31T04:16:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F5Q9463M0RSHAJJX0F3D1DB0/runs/20260531T041625730Z-a9ffb4aa41c74526aa6f84743643fde4.json`