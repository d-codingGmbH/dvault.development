[gicket-bot] Run report (outcome: test-workflow-returned)

Summary
- Tester workflow returned ticket '06F5Q92AHG0ZCTVQGC6NAYVP9C' for rework because persisted acceptance criteria or definition-of-done expectations were not fully confirmed.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F5Q92AHG0ZCTVQGC6NAYVP9C`.
- Optimistic claim succeeded (`expectedRevision=06F6V4NV82SGDJ7KGM2SB7WHVW`, `currentRevision=06F6V521HCC2WKJD0V01XVB44G`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Selected verification source branch 'ticket/06F5Q92AHG0ZCTVQGC6NAYVP9C-story-generate-typed-latest-and-as-of-satellite' (developer-delivery-outcome contract).
- Prepared interactive tester scratch worktree for target branch 'ticket/06F5Q92AHG0ZCTVQGC6NAYVP9C-story-generate-typed-latest-and-as-of-satellite' from source 'ticket/06F5Q92AHG0ZCTVQGC6NAYVP9C-story-generate-typed-latest-and-as-of-satellite'.
- Interactive tester tool loop completed review for branch 'ticket/06F5Q92AHG0ZCTVQGC6NAYVP9C-story-generate-typed-latest-and-as-of-satellite'.
- Evidence: git -C /mnt/c/Projects/DVault diff --name-only develop...ticket/06F5Q92AHG0ZCTVQGC6NAYVP9C-story-generate-typed-latest-and-as-of-satellite shows implementation changes in src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelSourceGenerator.cs, src/DCoding.Data.DV...
- Evidence: src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelSourceGenerator.cs:294-318 rejects reserved technical-name collisions for driving keys and non-string payloads, but src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelSourceGenerator.cs:353-356 still maps...
- Evidence: src/DCoding.Data.DVault/DataVaultReadServiceTypedProjectionExtensions.cs:75-106 validates both driving-key and payload mapped names against reserved projection names, so a generated helper with payload metadata name ParentHashKey, HashDiff, LoadTimestamp, or RecordSo...
- Evidence: src/DCoding.Data.DVault/DataVaultReadServiceCurrentSatelliteExtensions.cs:75-126 and src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelSourceGenerator.cs:564-640 wire Current, AsOf, and Latest helpers through the existing latest-satellite request and projectio...
- Evidence: tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests.cs:12-82 covers hub-parent, link-parent, and only a single-key multi-active satellite example; tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests....
- Evidence: src/DCoding.Data.DVault.Analyzers/README.md:52-58 documents the typed satellite generator boundary and DMV196x behavior; src/DCoding.Data.DVault/DataVaultDiagnostics.cs:237-245 and <redacted> add CLR type and nullability fields that feed support-bundle explain data, a...
- 54 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- AC check failed: When authoritative metadata cannot be resolved deterministically, fingerprints drift, bindings or normalized public names collide, or the requested shape falls outside the bounded satellite contract, generation stops or skips with the documented `DMV196x` diag...
- AC check failed: Repository tests cover positive generation for representative hub-parent, link-parent, and multi-active satellite shapes plus negative diagnostics for stale fingerprints, unsupported bindings, nullability fallback, and naming-collision edge cases. (The test su...
- DoD check failed: Generated satellite helpers behave consistently with the current/latest/as-of satellite semantics already exposed by `DataVaultReadServiceCurrentSatelliteExtensions` and `DataVaultSatelliteProjectionRow`. (Because the generator can emit helpers whose payload ...
- DoD check failed: Regression coverage protects deterministic naming, metadata-source and fingerprint handling, payload nullability, multi-active driving-key ordering, and unsupported-shape diagnostics. (Regression coverage protects fingerprints, nullability fallback, and deter...
- High: The generator does not reject payload metadata names that collide with reserved projection names. Source generation accepts the payload path in src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelSourceGenerator.cs:309-358, but runtime projection validation in src/D...
- Medium: Required regression coverage is still incomplete. The multi-active positive test in tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests.cs:31-43 and 75-77 uses only one driving key, so it does not protect metadata-order behavior for mu...

Next steps
- Extend/adjust automated tests and evidence so every acceptance-criteria item is explicitly observed.
- Re-run tester verification after updating tests or implementation.
- Close the remaining definition-of-done gaps before handing the ticket to the integrator gate.
- Re-run tester verification after completing the missing implementation, test, or documentation work.
- Add generator-time rejection for payload mapped-name collisions against ParentHashKey, HashDiff, LoadTimestamp, and RecordSource, using the documented DMV196x path before emitting helpers.
- Add analyzer regressions for a payload-name collision case and for a multi-active satellite with at least two driving keys whose generated order is asserted from metadata ordinals.
- After the code and regression gaps are fixed, rerun the required verification commands in a writable verification environment: dotnet test DVault.slnx --nologo and bash tools/check-format.sh.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9090`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `test`
- run-id: `4a9f03cf7d2e46dfa977546ca0c382bc`
- completed-at-utc: `<redacted>-28T08:02:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F5Q92AHG0ZCTVQGC6NAYVP9C/runs/20260528T080242096Z-4a9f03cf7d2e46dfa977546ca0c382bc.json`