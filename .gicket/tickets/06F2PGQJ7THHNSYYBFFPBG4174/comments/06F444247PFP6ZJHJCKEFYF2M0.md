[gicket-bot] PO-critic review contract

Summary
- Ticket contract is specific, has no open questions, and is grounded in existing diagnostics/design-time surfaces; it is ready for developer handoff.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06F2PGQJ7THHNSYYBFFPBG4174/description.md` contains `## Open Questions` with `- none`, plus bounded scope, acceptance criteria, and Definition of Done items for the support-bundle export.
- `git log --oneline --decorate -n 4` on `ticket/06F2PGQJ7THHNSYYBFFPBG4174-story-add-diagnostics-support-bundle-export` shows only PO workflow commits `e57e8985c`, `d921d52e4`, `7e718d888`, and `6422959a0`.
- `git diff --name-only develop..HEAD` lists only `.gicket/tickets/06F2PGQJ7THHNSYYBFFPBG4174/...`; the filtered non-`.gicket` diff against `0a462e934` returned no paths, so this handoff branch has no repo code/doc changes yet.
- `src/DCoding.Data.DVault/DataVaultDesignTimeCommand.cs` `WriteUsage` currently exposes only `validate`, `export`, `drift`, and `guardrail`, and `docs/architecture/dvault-dotnet-ef-design-time-workflow.md` documents the same consumer-owned command-host pattern.
- `src/DCoding.Data.DVault/DataVaultDiagnostics.cs` defines public `DataVaultExplainDiagnostics`, `DataVaultSaveStrategyDiagnostics`, `DataVaultReadStrategyDiagnostics`, and `DataVaultDiagnosticsResult`; `DataVaultLiveSchemaReadResult.cs` and `DataVaultModelDriftReport.cs` expose the live-schema/drift result types named by the contract.
- A repository search for `support-bundle`, `support bundle`, `bundle export`, and `bundle` returned no matches in `src/DCoding.Data.DVault`, `tests/DCoding.Data.DVault.Tests`, `docs/architecture`, or `README.md`, consistent with the contract's claim that this is a new surface.
- `.gicket/tickets/06F2PGQ6T5TGNWCBQBX3700D84/ticket.json` shows the prerequisite strategy-explanation story is `done`; relation files `.gicket/relations/84/74/...--blocks.json` and `.gicket/relations/74/CM/...--blocks.json` confirm this story is blocked by that done story and itself blocks docs task `06F2PGQQJB5FJGDB16M2G7CPCM`.
- `.gicket/tickets/06F2PGP7HM8F39K3J0H5JHB3B4/ticket.json` shows the older inbound blocker epic is `done`, matching the contract's historical-context note.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- Redaction examples should cover connection strings, passwords or tokens, usernames, and nested provider exception messages in both top-level and inner-exception text.
- The contract should be exercised for bundles produced from plain design-time validation where `SaveStrategy` and `ReadStrategy` remain `NotEvaluated`.
- Opt-in live-schema or drift sections should be tested for `UnsupportedProvider` and `Unavailable` outcomes so classified status survives without leaking secret-bearing free text.
- Deterministic output and ordering should be checked for both stdout emission and explicit output-path writes.

Risky assumptions
- The developer will need an explicit consumer-owned way to supply request-bound save or read diagnostics into the support-bundle flow because the current `DataVaultDesignTimeCommandHost` only carries diagnostics service, DbContext factory, export source, migration resolver, and optional live-schema reader.
- Redaction can be implemented provider-neutrally enough to protect secrets without stripping provider names, diagnostic codes, profile names, or other troubleshooting-relevant identifiers.

AC / test suggestions
- Lock the exported JSON contract with snapshot-style tests that cover the minimal design-time-only bundle, a bundle with save-strategy diagnostics, and a bundle with read-strategy diagnostics.
- Add deterministic redaction tests with provider-flavored failure text that includes credential-like substrings and ensure the masked output is still stable.
- Add command-surface tests analogous to `DataVaultDesignTimeCommandTests` to prove stdout-by-default and `--output` behavior for the new bundle path.
- Add tests proving opt-in live-schema or drift sections serialize existing classified results rather than inventing new parallel status vocabularies.

Implementation watchouts
- Keep the bundle surface inside the existing consumer-owned design-time architecture; `DataVaultDesignTimeCommand` usage currently documents only `validate`, `export`, `drift`, and `guardrail`.
- Mirror current export ergonomics from `DataVaultDesignTimeCommand.cs`: one deterministic JSON document, stdout by default, and an explicit output path when requested.
- Reuse `DataVaultDiagnosticsResult`, `DataVaultLiveSchemaReadResult`, and `DataVaultModelDriftReport` directly; avoid a second troubleshooting or explain model that can drift from the source diagnostics.
- Keep v0.16 coordinated release-note work downstream: `docs/releases/v0.16.0.md` is still absent, and the contract already routes that broader documentation wrap-up to ticket `06F2PGQQJB5FJGDB16M2G7CPCM`.

Non-blocking notes
- The branch currently contains ticket metadata only, which is expected at the pre-development PO gate and is not a blocker under the stated review policy.
- The parent epic `06F2PGQ27NWVZ1B1R651S7SM4M` is still `todo`, but this ticket's own dependency chain is explicitly bounded and documented.

Split recommendations
- No split recommended; the live relation set already separates prerequisite strategy diagnostics, this support-bundle story, sibling telemetry work, and downstream v0.16 documentation.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment