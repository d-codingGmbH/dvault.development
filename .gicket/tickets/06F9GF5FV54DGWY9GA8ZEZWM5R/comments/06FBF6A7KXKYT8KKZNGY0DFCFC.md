[gicket-bot] Run report (outcome: test-workflow-returned)

Summary
- Tester workflow returned ticket '06F9GF5FV54DGWY9GA8ZEZWM5R' for rework because persisted acceptance criteria, definition-of-done expectations, or checklist gates were not fully confirmed.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F9GF5FV54DGWY9GA8ZEZWM5R`.
- Optimistic claim succeeded (`expectedRevision=06FBF3TFE4EE6CMQ2ZSZBCEVWC`, `currentRevision=06FBF4JE4QRTBEZR1M5K3ZTTGG`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Selected verification source branch 'ticket/06F9GF5FV54DGWY9GA8ZEZWM5R-story-define-hash-key-storage-profile-contract' and commit '12b989cfb189' (ticket-comment branch+commit reference).
- Prepared interactive tester scratch worktree for target branch 'ticket/06F9GF5FV54DGWY9GA8ZEZWM5R-story-define-hash-key-storage-profile-contract' from source '12b989cfb189'.
- Interactive tester tool loop completed review for branch 'ticket/06F9GF5FV54DGWY9GA8ZEZWM5R-story-define-hash-key-storage-profile-contract'.
- Evidence: `git diff --name-status develop...12b989cfb189` shows a new `docs/plans/hash-key-storage-profile-contract.md`, documentation updates, source changes in translator/diagnostics/guardrails/model conventions, and updated tests in provider capability, metadata translation...
- Evidence: `docs/plans/hash-key-storage-profile-contract.md` defines the two-profile vocabulary (`HexString`, `Binary`), fixed sizing for `sha256-v1`, `sha1-v1`, `sha256-128-v1`, and `sha256-160-v1`, and names the reviewed support bundle as the authoritative preflight baseline.
- Evidence: `tests/DCoding.Data.DVault.Tests/Unit/DataVaultProviderCapabilityProfileTests.cs:408-483` covers six built-in profiles for HexString sizing and explicit Binary opt-in mapping behavior.
- Evidence: `tests/DCoding.Data.DVault.Tests/Unit/DataVaultEfMetadataTranslationTests.cs:409-425` verifies algorithm-sized hash-key store types/annotations, and `:<redacted>` verifies hash-key/hash-key-reference storage annotations are present only where expected.
- Evidence: `tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs:58-159` verifies diagnostics/support-bundle exposure of `algorithmId`, `digestByteLength`, and redaction, including the `sha1-v1` versus `sha256-160-v1` same-width distinction.
- Evidence: `tests/DCoding.Data.DVault.Tests/Unit/DataVaultMigrationOperationDiagnosticsTests.cs:565-603` verifies same-width stable-hash algorithm drift is blocked for both a hub hash key and a link participant reference.
- 43 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- DoD check failed: Migration or preflight guardrail tests cover unsupported HexString-to-Binary transitions, digest-length mismatches, same-length `algorithmId` drift, and provider-shape mismatches for DVault-owned hash-key columns. (`DataVaultMigrationOperationDiagnosticsTests...
- Definition of Done 4 is not met: the migration/preflight guardrail tests cover same-width `algorithmId` drift, but they do not directly cover hash-key `HexString` to `Binary` transitions or hash-key digest-byte-length mismatches.

Next steps
- Close the remaining definition-of-done gaps before handing the ticket to the integrator gate.
- Re-run tester verification after completing the missing implementation, test, or documentation work.
- Add explicit migration or preflight guardrail tests for hash-key `HexString` to `Binary` transitions and digest-byte-length mismatches in `tests/DCoding.Data.DVault.Tests/Unit/DataVaultMigrationOperationDiagnosticsTests.cs`.
- After the missing guardrail cases are added, rerun `dotnet test DVault.slnx --nologo` and `bash tools/check-format.sh` in the supported environment.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8879`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `test`
- run-id: `8c4c6649f5d7482ea84ea66b0fe5355c`
- completed-at-utc: `<redacted>-11T16:52:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F9GF5FV54DGWY9GA8ZEZWM5R/runs/20260611T165216405Z-8c4c6649f5d7482ea84ea66b0fe5355c.json`