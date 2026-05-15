[gicket-bot] Run report (outcome: po-critic-blocking-apply)

Summary
- PO-critic review completed with a blocking assessment for ticket '06F2PGG8ZKSYGC8863118H56G8'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F2PGG8ZKSYGC8863118H56G8`.
- Optimistic claim succeeded (`expectedRevision=06F2QG6NJ0CJ671NDB3H7FTKWM`, `currentRevision=06F2QGBXAKYT1M2Y1K1TXTXK3G`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06F2PGG8ZKSYGC8863118H56G8-task-implement-provider-catalog-readers' from source '05d9653055555e1cd312fec1d7dff81477a301a2'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed]).
- Published runtime write-group comment template 'handover-po'.
- Committed transactional ticket writeback for TP `TP8` on branch `ticket/06F2PGG8ZKSYGC8863118H56G8-task-implement-provider-catalog-readers` as `d165c3df8f9b`.

Open questions / Risiken
- Blocking finding: Definition of Done items 1-2 are not met: compared with `develop`, the branch contains ticket metadata updates only and no non-ticket `src/` or `tests/` implementation evidence for provider catalog readers.
- Blocking finding: Acceptance criteria 1, 2, and 5 are not met because `DataVaultLiveSchemaReader.ReadAsync(...)` still routes only SQLite and classifies recognized non-SQLite providers as `UnsupportedProvider`.
- Blocking finding: Acceptance criterion 3 is not met because direct PostgreSQL, SQL Server, Oracle, and MySQL live-schema reader execution is not evidenced; the only observed direct `ReadAsync(...)` tests are SQLite tests.
- Required PO action: Return ticket `06F2PGG8ZKSYGC8863118H56G8` to PO refinement instead of developer handoff; the current `ready_for_po_critic` routing is unsupported by repository evidence.
- Required PO action: Do not resend this ticket to PO-critic until the ticket cites an exact implementation branch/ref/commit with matching non-ticket `src/` and `tests/` evidence for the provider readers.
- Required PO action: If product intends to keep the release SQLite-only, update the delivery contract and ticket routing accordingly, and move first-class PostgreSQL/SQL Server/Oracle/MySQL live-schema readers into a separate implementation ticket rather than leaving them impli...
- Risky assumption: Assuming `ExternalProviderLiveSchemaFixture` or conditional provider package references prove live-schema reader delivery; the observed direct reader calls remain SQLite-only.
- Risky assumption: Assuming existing provider capability selection automatically supplies live-schema reader dispatch; `DataVaultLiveSchemaReader.cs:31-34` bypasses non-SQLite capability profiles and returns `UnsupportedProvider`.
- Risky assumption: Assuming the persisted `ready_for_po_critic` handoff can override current branch evidence even when no implementation ref or non-ticket code/test changes are attached.
- Split recommendation: Keep this as one bounded implementation ticket if product still wants PostgreSQL, SQL Server, Oracle, and MySQL live-schema readers in the same release slice.
- Split recommendation: Only split if product explicitly narrows scope back to SQLite-only or if provider-specific external setup becomes independently reviewable enough to justify separate implementation tickets.

Next steps
- Resolve blocking findings and required Product Owner actions before implementation continues.
- Re-run PO-critic after clarification updates are applied.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8740`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `6fb992420ccd4a459e0e35c446c00c63`
- completed-at-utc: `<redacted>-15T13:14:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F2PGG8ZKSYGC8863118H56G8/runs/20260515T131448116Z-6fb992420ccd4a459e0e35c446c00c63.json`