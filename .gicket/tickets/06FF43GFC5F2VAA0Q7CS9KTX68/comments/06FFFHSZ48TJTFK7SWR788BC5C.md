[gicket-bot] Run report (outcome: dev-workflow-apply)

Summary
- Developer workflow prepared branch 'ticket/06FF43GFC5F2VAA0Q7CS9KTX68-task-review-postgresql-pit-maintenance-transacti' and commit '90e433e93217' for ticket '06FF43GFC5F2VAA0Q7CS9KTX68'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FF43GFC5F2VAA0Q7CS9KTX68`.
- Optimistic claim succeeded (`expectedRevision=06FFF4Y2S68TZK0N2XDGEBV3TR`, `currentRevision=06FFF83QRHD0NRFHV9Q8319X8G`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded developer ticket mutation surface enabled: gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-relation.
- Prepared interactive developer scratch worktree for target branch 'ticket/06FF43GFC5F2VAA0Q7CS9KTX68-task-review-postgresql-pit-maintenance-transacti' from source 'ticket/06FF43GFC5F2VAA0Q7CS9KTX68-task-review-postgresql-pit-maintenance-transacti'.
- Planned implementation step: Added DataVaultPitMaintenanceStrategyFallbackCauseKind.CurrentTransactionSavepointUnavailable and wired PostgreSQL PIT gate evaluation to detect active DbContext transactions.
- Planned implementation step: Updated DefaultDataVaultPitMaintenanceService to record known provider PIT strategy fallback causes before running the provider-neutral rebuild path, and to record selected strategy names when provider strategies are used.
- Planned implementation step: Added a defensive PostgresDataVaultPitMaintenanceStrategy.RebuildAsync gate recheck so direct internal strategy calls cannot run provider SQL when the request is outside the approved boundary.
- Planned implementation step: Added unit coverage for the PostgreSQL caller-transaction fallback cause and integration coverage for provider-neutral fallback under an ambient PostgreSQL transaction.
- Planned implementation step: Updated architecture, performance profile, release-note, and changelog wording to state that PostgreSQL optimized PIT rebuilds require no active caller transaction.
- Resolved branch route (fallback): base 'develop', work 'ticket/06FF43GFC5F2VAA0Q7CS9KTX68-task-review-postgresql-pit-maintenance-transacti'.
- Executed runtime-orchestration sync-first fetch/pull before workspace automation.
- 20 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- Risk: Live PostgreSQL execution of the new ambient-transaction integration test was not observed locally because the PostgreSQL connection string is absent; the test is present and will execute in a configured provider environment.
- Risk: Callers that previously ran PostgreSQL optimized PIT rebuilds inside a caller transaction will now get provider-neutral maintenance by design.
- Risk: NuGet emitted NU1900 vulnerability-cache warnings because the sandbox cannot write to the NuGet HTTP cache path under /home/davidullrich/.local/share/NuGet/http-cache.

Next steps
- Push branch 'ticket/06FF43GFC5F2VAA0Q7CS9KTX68-task-review-postgresql-pit-maintenance-transacti' manually if remote collaboration is required.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9691`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `dev`
- run-id: `8d3da6d0d8c14ad8b612239f5b3ad9c2`
- completed-at-utc: `<redacted>-24T03:58:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FF43GFC5F2VAA0Q7CS9KTX68/runs/20260624T035810716Z-8d3da6d0d8c14ad8b612239f5b3ad9c2.json`