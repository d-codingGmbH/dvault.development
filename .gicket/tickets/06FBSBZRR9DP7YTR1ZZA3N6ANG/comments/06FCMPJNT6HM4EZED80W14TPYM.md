[gicket-bot] PO-critic review contract

Summary
- Ticket contract requires substantive product-owner changes before development.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06FBSBZRR9DP7YTR1ZZA3N6ANG/description.md contains only the legacy one-line draft and no delivery-contract block describing remaining parent-level work.
- .gicket/tickets/06FBSBZRR9DP7YTR1ZZA3N6ANG/comments/06FCA5BZG2MMGW94PXGM9GVKMM.md says develop removed stale parent-to-child `blocks` edges and marked the parent `tracking/parent` plus `tracking/waiting-on-children`, which does not match the current ticket.json label set.
- Historical events under .gicket/tickets/06FBSBZRR9DP7YTR1ZZA3N6ANG/events/ show this parent was split into child tickets `06FBSBZY1XEJYK1DRV4RV2ZN88`, `06FBSC03KAGDABNFGPK9D95QKR`, `06FBSC08W24BJGFZ87RSFS21WC`, `06FBSC0EJHAY200E7PXNRGV7XR`, `06FBSC0MNH0YAWQ4NY2WSC8KJG`, and `06FBSC0TMZBXVVECGQGESWPCY4` via `parentOf` and historical `blocks` relations.
- Repository surfaces already carry the intended contract outcome: README.md and docs/getting-started.md recommend `UseBinaryFirstProfile()` / `UseDataVaultBinaryFirstProfile()` for new projects with explicit existing-project migration caveats; src/DCoding.Data.DVault/DataVaultOptions.cs and src/DCoding.Data.DVault/DataVaultModelBuilderExtensions.cs expose those APIs; tests/DCoding.Data.DVault.Tests/Unit/StableHashServiceTests.cs and tests/DCoding.Data.DVault.Tests/Unit/DataVaultModelBuilderExtensionsTests.cs assert the `binary-first` behavior.
- git log --oneline shows child work already landed on develop (`0353d7d50` for `06FBSBZY1XEJYK1DRV4RV2ZN88`, `c9404808b` for `06FBSC0EJHAY200E7PXNRGV7XR`) and the current branch head `7413d67d8` is only the PO-critic lease-claim commit for this parent ticket.

Blocking findings
- Ticket state is inconsistent with its own maintenance comment and with current child completion: the parent is still `todo`/`critic-needed`, but the comment says it should have been marked `tracking/waiting-on-children`, and all observed split child tickets are already `done`.

Required PO actions
- Decide whether `06FBSBZRR9DP7YTR1ZZA3N6ANG` is still an executable story or is now a tracking/closure ticket for already-completed child work.
- If the parent remains open, replace the one-line description with an authoritative delivery contract that names the exact remaining parent-level outcome, the authoritative repository artifact(s), and how child-ticket completion rolls up into parent acceptance.
- If the parent is closure-only, move it out of the developer handoff path and update its ticket state accordingly instead of sending it to a developer queue.

Open issues ledger
- critic-item-1 [required-po-action] Decide whether `06FBSBZRR9DP7YTR1ZZA3N6ANG` is still an executable story or is now a tracking/closure ticket for already-completed child work.
- critic-item-2 [required-po-action] If the parent remains open, replace the one-line description with an authoritative delivery contract that names the exact remaining parent-level outcome, the authoritative repository artifact(s), and how child-ticket completion rolls up into parent acceptance.
- critic-item-3 [required-po-action] If the parent is closure-only, move it out of the developer handoff path and update its ticket state accordingly instead of sending it to a developer queue.
- critic-item-4 [blocking-finding] Ticket state is inconsistent with its own maintenance comment and with current child completion: the parent is still `todo`/`critic-needed`, but the comment says it should have been marked `tracking/waiting-on-children`, and all observed split child tickets are already `done`.

Missing examples / edge cases
- If this parent stays open as a contract ticket, add one explicit parent-level example of the intended new-project entry points (`AddDVault(...UseBinaryFirstProfile())` and `modelBuilder.UseDataVaultBinaryFirstProfile()`) and one explicit non-example showing that existing-project default compatibility remains on `HexString` unless callers opt in.
- Clarify at the parent level whether manual low-level provider shaping (`WithHashKeyStorageProfile(...)`) is merely an advanced implementation path or part of the same product contract as the named binary-first profile.

Risky assumptions
- Assuming the parent can still be treated as fresh developer work even though the observed split child tickets are already `done`.
- Assuming the maintenance comment's `tracking/waiting-on-children` state is already reflected in the current persisted ticket state when the current ticket.json does not show that label.
- Assuming the repository's landed API/docs/tests are sufficient to close the parent without first stating, at the parent ticket level, whether any parent-specific acceptance remains.

AC / test suggestions
- If the parent remains open, its acceptance should name the authoritative evidence surfaces already in the repo: README.md, docs/getting-started.md, docs/plans/hash-key-storage-profile-contract.md, docs/releases/v0.36.0.md, src/DCoding.Data.DVault/DataVaultOptions.cs, src/DCoding.Data.DVault/DataVaultModelBuilderExtensions.cs, and the binary-first unit/integration tests.
- Parent-level acceptance should explicitly state whether success is evidence aggregation only or requires a separate parent artifact beyond the already-landed child deliverables.

Implementation watchouts
- Do not reopen the runtime default: repository evidence still keeps `AddDVault()` / `UseDataVault()` on `sha256-v1` plus `HexString`; the binary-first posture is a recommendation for new projects via explicit opt-in APIs.
- Do not let parent wording imply automatic migration, backfill, dual-write, or a public `byte[]` hash-key boundary; the existing docs and contracts explicitly reject those interpretations.
- Do not send this parent to dev until the parent ticket makes clear whether any work remains beyond the already-landed child tickets.
- Non-blocking tracking-parent audit note: The parent ticket does not define any authoritative remaining parent-level deliverable. Its description is still a one-line draft while the real scope was decomposed into child tickets and those child tickets carry the actual delivery contracts.
- Non-blocking tracking-parent audit note: Because the repository already contains the shipped API, docs, diagnostics coverage, and benchmark evidence via child tickets, routing this parent to `dev` would be ambiguous: it is unclear whether this ticket is now tracking-only, closure-only, or still expected to produce a separate parent artifact.

Non-blocking notes
- The underlying product contract appears well evidenced in the repository already; the blocker is ticket clarity and routing, not lack of technical direction.
- No unresolved `## Open Questions` block was found on the parent ticket, but that is because the parent lacks the richer delivery-contract structure that its child tickets already use.
- Tracking-parent closure findings were downgraded because the ticket contract resolves as direct implementation work, so missing parentOf child evidence is not a PO blocker.

Split recommendations
- No further split is needed. The split already exists and the observed child tickets are done; the needed action is parent-ticket reconciliation, not new child creation.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment