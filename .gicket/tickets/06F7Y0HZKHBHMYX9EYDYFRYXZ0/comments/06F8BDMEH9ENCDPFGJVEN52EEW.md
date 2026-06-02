[gicket-bot] Run report (outcome: dev-workflow-apply)

Summary
- Developer workflow prepared branch 'ticket/06F7Y0HZKHBHMYX9EYDYFRYXZ0-task-update-v0-25-0-read-plan-and-typed-helper-d' and commit 'ae4272889217' for ticket '06F7Y0HZKHBHMYX9EYDYFRYXZ0'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F7Y0HZKHBHMYX9EYDYFRYXZ0`.
- Optimistic claim succeeded (`expectedRevision=06F8B4EACNFVQJ79V28ND1XFFC`, `currentRevision=06F8B4SQ4ERW728VN8QDXE2XG0`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive developer scratch worktree for target branch 'ticket/06F7Y0HZKHBHMYX9EYDYFRYXZ0-task-update-v0-25-0-read-plan-and-typed-helper-d' from source 'ticket/06F7Y0HZKHBHMYX9EYDYFRYXZ0-task-update-v0-25-0-read-plan-and-typed-helper-d'.
- Planned implementation step: Updated the root README to point to v0.25.0, replace satellite-only typed helper guidance with the implemented satellite/PIT/bridge helper baseline, add generated PIT and bridge helper examples, and include a redacted readShape support-bundle example.
- Planned implementation step: Updated the analyzer README to document generated bridge helpers, support-bundle readShape requirements, DMV1964 behavior, and v0.25.0 package guidance.
- Planned implementation step: Updated the production checklist to make v0.25.0 current, demote v0.24.0/v0.22.0 to historical context, describe generated helper shape limits, and cite v0.25.0 validation evidence.
- Planned implementation step: Promoted the typed PIT/bridge architecture contract from additive/future wording to the current implemented v1 helper contract.
- Planned implementation step: Updated the redacted read-plan explain contract to name v0.25.0, explain ReadShape as support-bundle helper evidence, and add a redacted support-bundle JSON example.
- Planned implementation step: Added v0.25.0 release notes with compatibility posture, DMV1960-DMV1969 diagnostics, validation commands, evidence surfaces, documentation baseline, and non-goals.
- Planned implementation step: Clarified the planning index so the satellite-only typed-read generator plan is historical v0.22 context rather than the current baseline.
- Resolved branch route (fallback): base 'develop', work 'ticket/06F7Y0HZKHBHMYX9EYDYFRYXZ0-task-update-v0-25-0-read-plan-and-typed-helper-d'.
- 18 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- Risk: dotnet build and dotnet test still emit pre-existing warnings unrelated to this documentation change, including NU1900 warnings from the sandbox read-only NuGet vulnerability cache.
- Risk: The helper examples are documentation examples, not a new runnable consumer sample; they assume a reviewed support bundle with representative ReadShape evidence.

Next steps
- Push branch 'ticket/06F7Y0HZKHBHMYX9EYDYFRYXZ0-task-update-v0-25-0-read-plan-and-typed-helper-d' manually if remote collaboration is required.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9513`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `dev`
- run-id: `9f02879db09b45dc9501b40c01844517`
- completed-at-utc: `<redacted>-02T00:23:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F7Y0HZKHBHMYX9EYDYFRYXZ0/runs/20260602T002314308Z-9f02879db09b45dc9501b40c01844517.json`