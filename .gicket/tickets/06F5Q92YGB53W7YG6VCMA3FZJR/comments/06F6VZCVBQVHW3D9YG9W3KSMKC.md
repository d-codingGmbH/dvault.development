[gicket-bot] PO-critic review contract

Summary
- Re-baselined contract now defines a distinct residual diagnostics-only follow-up with no open questions and clear ownership boundaries, so the story is ready for developer handoff.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06F5Q92YGB53W7YG6VCMA3FZJR/description.md now scopes the story to DMV1963, DMV1964, DMV1967, DMV1968, and DMV1969 diagnostics plus tests, explicitly keeps README/analyzer/model-first/release-note docs on 06F5Q93H60W6X8FJ88PWTR6NG4, excludes new typed-read code fixes, and shows `## Open Questions` -> `none`.
- Comment .gicket/tickets/06F5Q92YGB53W7YG6VCMA3FZJR/comments/06F6VWYBPNA72A1BJR6E7VMTBC.md answers all six earlier PO-critic items and states `decision: ready_for_po_critic`.
- .gicket/relations/JR/G4/06F5Q92YGB53W7YG6VCMA3FZJR--06F5Q93H60W6X8FJ88PWTR6NG4--blocks.json shows this story blocks docs task 06F5Q93H60W6X8FJ88PWTR6NG4, and .gicket/tickets/06F5Q93H60W6X8FJ88PWTR6NG4/description.md sets that task's goal to `Update docs for typed read model generation and hash governance.`
- src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelDiagnosticCatalog.cs defines DMV1963-DMV1969, and src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelSourceGenerator.cs already uses one authoritative `dvault.support-bundle.v1` input and keeps `LegacyExpectedFingerprintProperty`, matching the residual-scope contract.
- Residual developer work is real but not a PO blocker: `rg -n 'DMV1963|DMV1964|DMV1967|DMV1968|DMV1969' tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests.cs` returned no matches.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- The developer will classify ambiguous non-satellite cases with the shared contract in docs/plans/typed-read-model-generator-contract.md rather than inventing provider-specific behavior, especially at the DMV1967 versus DMV1969 boundary.
- Legacy fingerprint compatibility is assumed to remain in place during this ticket because DataVaultTypedReadModelSourceGenerator.cs still defines `LegacyExpectedFingerprintProperty` and the ticket defers any deprecation to a later follow-up.

AC / test suggestions
- Keep one regression assertion per residual diagnostic id plus a no-helper-emission assertion, because the current generator tests file has no direct DMV1963, DMV1964, DMV1967, DMV1968, or DMV1969 coverage.
- Keep one satellite non-regression assertion around DMV1960, DMV1961, DMV1962, DMV1965, and DMV1966 so the residual follow-up does not reopen the landed satellite baseline.

Implementation watchouts
- Current generator behavior still has silent early-return paths in src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelSourceGenerator.cs; the residual work must surface DMV1963, DMV1964, DMV1967, DMV1968, and DMV1969 instead of dropping unsupported/skipped shapes with no helper.
- Do not pull docs or new code-fix scope back into this story; the current contract explicitly keeps documentation on 06F5Q93H60W6X8FJ88PWTR6NG4 and says zero new typed-read code fixes is acceptable.

Non-blocking notes
- Pre-dev state is normal here: no source or docs files changed on the ticket branch beyond ticket metadata, which is expected before developer handoff.
- README.md and src/DCoding.Data.DVault.Analyzers/README.md already advertise the DMV1960-DMV1969 family and optional analyzer usage, so the separate docs task should reconcile public wording after this residual diagnostic work lands.

Split recommendations
- No further split is needed if this ticket stays limited to residual diagnostics and tests.
- Keep satellite work on 06F5Q92AHG0ZCTVQGC6NAYVP9C, PIT/bridge helper generation on 06F5Q92R02HB7FCE1AWKXPTMRW, and documentation rollup on 06F5Q93H60W6X8FJ88PWTR6NG4.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment