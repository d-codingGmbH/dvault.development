[gicket-bot] Run report (outcome: po-critic-non-blocking-apply)

Summary
- PO-critic review completed with a non-blocking assessment for ticket '06F492AKGMKPCRJYF4Z1EC9WY4'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F492AKGMKPCRJYF4Z1EC9WY4`.
- Optimistic claim succeeded (`expectedRevision=06F4PB0V43EZ42DVMK56VXTQV4`, `currentRevision=06F4QM15D7MTF5XGTQ22DAY2DC`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06F492AKGMKPCRJYF4Z1EC9WY4-story-verify-dvault-ef-model-cache-key-isolation' from source '9848aca88798f5fecfe22a92d2f182a3d8f61fe5'.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-dev]; removed [critic-needed]).
- Published runtime write-group comment template 'handover-dev'.
- Committed transactional ticket writeback for TP `TP2` on branch `ticket/06F492AKGMKPCRJYF4Z1EC9WY4-story-verify-dvault-ef-model-cache-key-isolation` as `58f310f2e8e6`.

Open questions / Risiken
- Risky assumption: Assumes the developer will keep the custom-key proof on a repository-supported local baseline, as requested in the contract, instead of relying only on external-provider schema fixtures.
- Risky assumption: Assumes the documentation update will explicitly distinguish registry-backed isolation from caller-owned `OnModelCreating` variability so readers do not infer automatic protection for tenant, profile, or schema-dependent state.
- Risky assumption: Assumes the model-first import lane can be covered through the existing `UseDataVaultMetadata(DataVaultModelImportResult)` surface without reopening metadata-source selection architecture.

Next steps
- Continue with role `dev` according to policy.
- Keep non-blocking findings visible during implementation and test planning.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.7137`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `041aaf9d7492490f919e2a599382a557`
- completed-at-utc: `<redacted>-21T18:38:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F492AKGMKPCRJYF4Z1EC9WY4/runs/20260521T183854047Z-041aaf9d7492490f919e2a599382a557.json`