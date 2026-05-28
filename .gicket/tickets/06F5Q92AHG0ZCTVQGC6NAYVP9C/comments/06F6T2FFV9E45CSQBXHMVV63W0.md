[gicket-bot] Run report (outcome: dev-workflow-apply)

Summary
- Developer workflow prepared branch 'ticket/06F5Q92AHG0ZCTVQGC6NAYVP9C-story-generate-typed-latest-and-as-of-satellite' and commit '06dcc4104508' for ticket '06F5Q92AHG0ZCTVQGC6NAYVP9C'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F5Q92AHG0ZCTVQGC6NAYVP9C`.
- Optimistic claim succeeded (`expectedRevision=06F6SNJD2HY4D76YSP9PAHQAWW`, `currentRevision=06F6SNVPRJ2ZTCAG3N9N5VCWKG`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive developer scratch worktree for target branch 'ticket/06F5Q92AHG0ZCTVQGC6NAYVP9C-story-generate-typed-latest-and-as-of-satellite' from source 'ticket/06F5Q92AHG0ZCTVQGC6NAYVP9C-story-generate-typed-latest-and-as-of-satellite'.
- Planned implementation step: Added dvault.support-bundle.v1 additional-file parsing to the typed read-model generator and made it authoritative when present, preventing source-syntax fallback from competing with projected EF/DVault metadata.
- Planned implementation step: Generated satellite helpers from diagnostics.explain entity/property descriptors, using metadata source kind/fingerprint, produced entity and property names, parent reference, property roles, logical provider kinds, ordinals, CLR type names, value ...
- Planned implementation step: Extended DataVaultPropertyExplain with ClrTypeName and IsNullable so support-bundle explain output carries the nullability/type facts needed by the generator.
- Planned implementation step: Added analyzer tests for authoritative support-bundle generation and support-bundle non-string payload diagnostics, and updated the public API snapshot for the new diagnostics fields.
- Planned implementation step: Updated analyzer README guidance to document the projected support-bundle input boundary.
- Resolved branch route (fallback): base 'develop', work 'ticket/06F5Q92AHG0ZCTVQGC6NAYVP9C-story-generate-typed-latest-and-as-of-satellite'.
- Executed runtime-orchestration sync-first fetch/pull before workspace automation.
- Checked out existing branch 'ticket/06F5Q92AHG0ZCTVQGC6NAYVP9C-story-generate-typed-latest-and-as-of-satellite'.
- 15 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- Risk: The source-visible Code-First/metadata-first/model-first parser remains as a fallback for compile-time-only samples; projected dvault.support-bundle.v1 additional files are now the authoritative path when present.
- Risk: NuGet audit cache warnings appear in this sandbox because the default HTTP cache path is read-only; they did not block build or test completion.

Next steps
- Push branch 'ticket/06F5Q92AHG0ZCTVQGC6NAYVP9C-story-generate-typed-latest-and-as-of-satellite' manually if remote collaboration is required.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9762`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `dev`
- run-id: `cda14fe47a9c4408b3db6ffc8e8eb268`
- completed-at-utc: `<redacted>-28T05:23:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F5Q92AHG0ZCTVQGC6NAYVP9C/runs/20260528T052348310Z-cda14fe47a9c4408b3db6ffc8e8eb268.json`