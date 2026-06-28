[gicket-bot] Run report (outcome: po-critic-blocking-apply)

Summary
- PO-critic review completed with a blocking assessment for ticket '06FGX69QJYHGNKBV8MJ1HG7MMG'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FGX69QJYHGNKBV8MJ1HG7MMG`.
- Optimistic claim succeeded (`expectedRevision=06FGZG95SM4FGDSSNRJ8A64PDM`, `currentRevision=06FGZGMEAEGCACRS555HHG39NM`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06FGX69QJYHGNKBV8MJ1HG7MMG-task-implement-hash-key-storage-migration-manife' from source 'a9364c3b6a088aa180f6bad8978e63fae81f09e8'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed]).
- Published runtime write-group comment template 'handover-po'.
- Committed transactional ticket writeback for TP `TP8` on branch `ticket/06FGX69QJYHGNKBV8MJ1HG7MMG-task-implement-hash-key-storage-migration-manife` as `6bd9c5f2e6a3`.

Open questions / Risiken
- Blocking finding: The authoritative contract and the checked-in producer disagree on the `dvault.hash-key-storage-migration.v1` top-level shape. The ticket does not say whether the validator must consume the current emitted `dryRun/source/target/comparison/entries` manifest, r...
- Blocking finding: The ticket expects deterministic `error`/`warning`/`info` validation findings for invalid manifests, but the only checked-in producer currently fails closed and writes no manifest on blocking drift. The input source for invalid-manifest scenarios is unspecified.
- Required PO action: Reconcile the authoritative v1 manifest schema across `docs/hash-key-storage-migration.md`, `docs/plans/hash-key-storage-profile-contract.md`, and the checked-in `hash-key-storage-migration` exporter/tests. State clearly whether this ticket preserves the cu...
- Required PO action: State whether this ticket also owns updates to the existing dry-run manifest producer and its tests/docs, or whether producer-shape changes belong to a separate ticket.
- Required PO action: Specify how invalid-manifest fixtures are expected to exist for this validator when the current producer exits with an error and writes no output file.
- Required PO action: Refresh the stale dependency wording in the delivery contract so it reflects that ticket `06FGX67TZV1F6S949F96ZE201W` is already `done`.
- Risky assumption: Assuming `dvault.hash-key-storage-migration.v1` can change top-level JSON shape without breaking the existing design-time command, tests, or any consumer already reading the emitted manifest.
- Risky assumption: Assuming invalid manifests will come from some external or future producer even though the current checked-in producer does not emit them.
- Risky assumption: Assuming developers can infer whether both the current `comparison` summary and the proposed `validation` findings must coexist in the same artifact.
- Split recommendation: No scope split is needed if PO simply reconciles the manifest contract. If PO decides the existing dry-run producer must change shape under this work, consider separating producer-schema migration from validator-only logic because downstream wiring alread...

Next steps
- Resolve blocking findings and required Product Owner actions before implementation continues.
- Re-run PO-critic after clarification updates are applied.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8986`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `a4e825927d994532aba6d9eea47f7b50`
- completed-at-utc: `<redacted>-28T19:51:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FGX69QJYHGNKBV8MJ1HG7MMG/runs/20260628T195113336Z-a4e825927d994532aba6d9eea47f7b50.json`