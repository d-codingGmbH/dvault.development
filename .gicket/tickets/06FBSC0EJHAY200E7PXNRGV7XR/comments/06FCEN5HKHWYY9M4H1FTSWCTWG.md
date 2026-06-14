[gicket-bot] PO-critic review contract

Summary
- Return to PO: the delivery contract is clear, but this run is in closure-only posture and the owner branch still shows no landed quickstart updates for the scoped binary-first documentation work.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- Ticket 06FBSC0EJHAY200E7PXNRGV7XR shows `## Open Questions` = `none`, `Recent comments` = `<none>`, and `Closure evidence amendments` = `<none>` in the persisted snapshot provided to this review.
- `git -C /mnt/c/Projects/DVault rev-parse HEAD` returned `344c8ae7d5395a43bdb602fed2a28cdff414287f`, which matches the provided `scratch-source-ref`, and `git -C /mnt/c/Projects/DVault diff --name-only 344c8ae7d5395a43bdb602fed2a28cdff414287f..ticket/06FBSC0EJHAY200E7PXNRGV7XR-task-update-new-project-quickstart-for-binary-fi` returned no files.
- `README.md:62-76` still shows the quickstart using `services.AddDVault();` plus `services.AddDVaultSqlite();` with no binary-first profile call, and `README.md:78-90` keeps the code-first snippet on the default conventions path.
- `docs/getting-started.md:17-25` still shows default-only service registration; `docs/getting-started.md:66-70` preserves the storage baseline (`HexString` default compatible, `Binary` opt-in) but not the requested quickstart-path recommendation/caveat.
- `examples/README.md:57-68` still documents `AddDVault(options => options.UseMetadataModel(...))` without `UseBinaryFirstProfile()`, and `examples/DCoding.Data.DVault.SqliteQuickstart/Program.cs:11-17` plus `examples/DCoding.Data.DVault.PostgresQuickstart/Program.cs:16-22` still register metadata without the binary-first profile.
- Direct source evidence confirms the named APIs already exist: `src/DCoding.Data.DVault/DataVaultOptions.cs:87-91` defines `UseBinaryFirstProfile()`, and `src/DCoding.Data.DVault/DataVaultModelBuilderExtensions.cs:26-64` defines `UseDataVaultBinaryFirstProfile(...)`.

Blocking findings
- This cannot be approved as a closure-only ticket: the owner branch is still at the scratch/source commit and contains no landed updates in the quickstart surfaces named by the contract.
- The repository still contradicts the closure claim because the primary quickstart path continues to model the default-only setup instead of the required binary-first recommendation in the root README, getting-started guide, examples README, and runnable SQLite/PostgreSQL quickstarts.
- The required quickstart-path compatibility caveat is not yet visible where the new-project recommendation is introduced; existing storage-contract text exists elsewhere, but the closure contract requires that caveat in the primary quickstart path itself.

Required PO actions
- Remove the closure-only posture for this ticket or otherwise correct the routing so it reflects remaining implementation work; current repository evidence does not support closure.
- Keep the current delivery contract, but re-handoff the ticket as normal development work once status/routing no longer treats it as closure-ready.
- Require landed repository evidence on the named quickstart surfaces before sending this ticket back through PO-critic as closure-ready.

Open issues ledger
- critic-item-1 [required-po-action] Remove the closure-only posture for this ticket or otherwise correct the routing so it reflects remaining implementation work; current repository evidence does not support closure.
- critic-item-2 [required-po-action] Keep the current delivery contract, but re-handoff the ticket as normal development work once status/routing no longer treats it as closure-ready.
- critic-item-3 [required-po-action] Require landed repository evidence on the named quickstart surfaces before sending this ticket back through PO-critic as closure-ready.
- critic-item-4 [blocking-finding] This cannot be approved as a closure-only ticket: the owner branch is still at the scratch/source commit and contains no landed updates in the quickstart surfaces named by the contract.
- critic-item-5 [blocking-finding] The repository still contradicts the closure claim because the primary quickstart path continues to model the default-only setup instead of the required binary-first recommendation in the root README, getting-started guide, examples README, and runnable SQLite/PostgreSQL quickstarts.
- critic-item-6 [blocking-finding] The required quickstart-path compatibility caveat is not yet visible where the new-project recommendation is introduced; existing storage-contract text exists elsewhere, but the closure contract requires that caveat in the primary quickstart path itself.

Missing examples / edge cases
- There is still no primary-entry quickstart example that clearly distinguishes the recommended new-project binary-first path from the existing-project compatibility path.
- There is still no visible quickstart-path note saying existing HexString-backed databases/configurations remain valid until an intentional migration, reset, or data-move plan is executed.
- The README code-first entry path still lacks a direct example of the named binary-first model-builder profile called out in the contract.

Risky assumptions
- Assuming the ticket can close because the binary-first APIs already exist would be incorrect; the documentation and runnable quickstarts this ticket owns have not been updated to use them.
- Assuming the storage note in `docs/getting-started.md:66-70` is enough would be risky; the contract requires the compatibility caveat to be explicit in the primary quickstart path.
- Assuming no dev work remains because the branch name matches the ticket would be incorrect; the branch head and scratch/source ref are the same SHA and there is no diff for the scoped files.

AC / test suggestions
- Before re-running PO-critic in closure mode, require branch evidence that all scoped quickstart surfaces changed: `README.md`, `docs/getting-started.md`, `examples/README.md`, `examples/DCoding.Data.DVault.SqliteQuickstart/Program.cs`, and `examples/DCoding.Data.DVault.PostgresQuickstart/Program.cs`.
- Closure evidence should demonstrate both halves of the contract: the new-project path recommends the binary-first profile, and the same entry path visibly states that existing databases are not migrated automatically.
- If the root README remains the code-first entry path, closure evidence should include a direct source diff showing the named model-builder binary-first profile in that snippet and the public lowercase-hex hash-key boundary preserved in the surrounding wording.

Implementation watchouts
- Do not let a future closure claim rely only on API existence or release-note prose; this ticket's acceptance is specifically about the quickstart entry surfaces and runnable examples.
- Keep the compatibility caveat equally visible with the recommendation so the quickstart does not imply automatic schema or data migration.
- Use only the already-shipped named binary-first APIs the contract cites; reopening low-level provider capability composition would not match the documented handoff intent.

Non-blocking notes
- From a pre-development clarity standpoint, the contract itself is well bounded: scoped files are named, the required recommendation/caveat is explicit, `## Open Questions` is `none`, and the enabling public APIs are directly present in source.
- Sibling ticket `06FBSC0TMZBXVVECGQGESWPCY4` already owns broader release-note/changelog follow-up, so that broader work does not need to block rerouting this ticket back to development.

Split recommendations
- No additional feature split is needed; the immediate issue is routing accuracy. Reclassify this ticket out of closure-only posture and let the existing bounded quickstart update proceed as normal development work.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment