[gicket-bot] Run report (outcome: po-critic-blocking-apply)

Summary
- PO-critic review completed with a blocking assessment for ticket '06FGX5VQ9Y665A727EFJ677SBC'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FGX5VQ9Y665A727EFJ677SBC`.
- Optimistic claim succeeded (`expectedRevision=06FH60A7X4X75ZE6K6F4G0ACR4`, `currentRevision=06FH60MTS0QGDRCVSKH34AQZJR`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06FGX5VQ9Y665A727EFJ677SBC-story-make-binary-hash-storage-migration-manifes' from source 'c42f73b9bc8ffc6cf372bebb45c73c076ad08adb'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed]).
- Published runtime write-group comment template 'handover-po'.
- Committed transactional ticket writeback for TP `TP8` on branch `ticket/06FGX5VQ9Y665A727EFJ677SBC-story-make-binary-hash-storage-migration-manifes` as `20a6f8f916f8`.

Open questions / Risiken
- Blocking finding: Repository contract inconsistency: `docs/plans/hash-key-storage-profile-contract.md:69-77` still defines validation findings as part of the manifest's top-level facts, while the implemented exporter (`src/DCoding.Data.DVault/DataVaultHashKeyStorageMigrationMa...
- Required PO action: Reopen `06FGX67TZV1F6S949F96ZE201W` or create one bounded follow-up ticket to reconcile `docs/plans/hash-key-storage-profile-contract.md` with the implemented six-key v1 manifest shape.
- Required PO action: Keep the parent ticket in PO until the ticket contract and cited repository contract both say the same thing about findings being validator output rather than serialized manifest input.
- Risky assumption: Assuming `docs/plans/hash-key-storage-profile-contract.md` is merely historical or ignorable is risky because the parent ticket implementation notes still cite it as part of the bounded contract surface.
- Split recommendation: No broader split is needed; one narrow contract-alignment reopen/follow-up for `docs/plans/hash-key-storage-profile-contract.md` is sufficient.

Next steps
- Resolve blocking findings and required Product Owner actions before implementation continues.
- Re-run PO-critic after clarification updates are applied.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9185`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `906afd1e38224c2aa393149892c31bf3`
- completed-at-utc: `<redacted>-29T11:00:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FGX5VQ9Y665A727EFJ677SBC/runs/20260629T110012700Z-906afd1e38224c2aa393149892c31bf3.json`