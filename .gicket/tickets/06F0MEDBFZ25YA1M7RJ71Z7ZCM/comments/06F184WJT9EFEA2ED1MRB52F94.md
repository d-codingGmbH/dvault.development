[gicket-bot] Run report (outcome: po-critic-blocking-apply)

Summary
- PO-critic review completed with a blocking assessment for ticket '06F0MEDBFZ25YA1M7RJ71Z7ZCM'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F0MEDBFZ25YA1M7RJ71Z7ZCM`.
- Optimistic claim succeeded (`expectedRevision=06F180KCPGCAEZMT9JJMPZ2EGR`, `currentRevision=06F1830K0MEP3MQ381JVPQPJ5M`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06F0MEDBFZ25YA1M7RJ71Z7ZCM-task-add-runnable-sqlite-and-postgresql-quicksta' from source '12e330289ec423aa87d7f898c728a7ce89fc34a8'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed]).
- Published runtime write-group comment template 'handover-po'.
- Committed transactional ticket writeback for TP `TP8` on branch `ticket/06F0MEDBFZ25YA1M7RJ71Z7ZCM-task-add-runnable-sqlite-and-postgresql-quicksta` as `8e42a91f6bf9`.

Open questions / Risiken
- Blocking finding: The contract requires a `code-first plus registry-backed` quickstart, but the visible public surface does not expose a way to build a `DataVaultMetadataModel`/`DataVaultMetadataRegistry` from code-first declarations. As written, the ticket leaves developers t...
- Blocking finding: The contract depends on teaching the correct runnable Postgres wiring, but the repository currently disagrees about that wiring. Source says `AddDVaultPostgres()` auto-registers the Postgres capability profile; README and the architecture note still say it do...
- Required PO action: Clarify how the example may satisfy `code-first plus registry-backed`: either explicitly allow duplicated declarations on the current public surface, or change the acceptance criteria to one authoritative public-surface pattern that the repo actually expose...
- Required PO action: Add one explicit Postgres setup instruction to the contract, naming the intended registration path for the example (`AddDVaultPostgres()` plus registry-backed `UseDataVaultMetadata()` if that is the desired flow) so developers do not follow stale docs.
- Required PO action: Define the missing-configuration contract for the Postgres example more tightly: env var name, whether absence is a success-path skip or a non-zero fail-fast, and what action-oriented message must be shown.
- Risky assumption: Assuming developers can demonstrate `code-first plus registry-backed` behavior without either duplicating metadata declarations or using non-public APIs.
- Risky assumption: Assuming developers will infer that source should override stale docs for Postgres provider-profile registration without an explicit PO clarification.
- Risky assumption: Assuming example-local documentation alone will be sufficiently discoverable while `README.md` still presents the older metadata-model/raw-read quickstart and broader README work is deferred to ticket `06F0MEDJC732GDD77H60R259P0`.
- Split recommendation: No split recommended. Once the public-surface setup contract is corrected, the work remains a bounded two-example task with example-local docs.

Next steps
- Resolve blocking findings and required Product Owner actions before implementation continues.
- Re-run PO-critic after clarification updates are applied.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9405`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `35f3d5fa008044fc925fb74e7cd2983b`
- completed-at-utc: `<redacted>-10T22:48:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F0MEDBFZ25YA1M7RJ71Z7ZCM/runs/20260510T224806565Z-35f3d5fa008044fc925fb74e7cd2983b.json`