[gicket-bot] PO-critic review contract

Summary
- The unsupported existing-API assumption was fixed, but the ticket still is not ready for developer handoff because it describes a future guardrail that already exists in the repo and does not identify a concrete standalone deliverable or owning-story anchor.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- tests/DCoding.Data.DVault.Tests/Unit/ApiSurfaceSnapshotTests.cs:12-57 already contains six approval tests for DCoding.Data.DVault, Sqlite, Postgres, SqlServer, Oracle, and MySql.
- docs/quality/api-surface-snapshots.md:5-34 already documents the same gate, the solution-level dotnet test DVault.slnx --nologo path, and the DVAULT_UPDATE_API_SNAPSHOTS=1 approval workflow.
- tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt:7-141 exposes AddDVault(), ApplyDataVaultMetadata(), UseDataVault(), and IDataVaultSaveService; the provider snapshot files at tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.{Sqlite,Postgres,SqlServer,Oracle,MySql}.approved.txt:7-8 expose provider registration extensions only.
- Repository search rg -n PIT|Bridge|Multi|Hook|Deferred tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi returned no matches, so no deferred-capability public surface is currently visible in the approved snapshots.
- docs/plans/deferred-data-vault-capabilities.md:41 says ticket 06EZ0NSQFCD3W4CDCJ44GFSKA0 must use the decision as a guardrail and must not infer concrete PIT, bridge, multi-active, or hook API names.
- .gicket/tickets/06EZ0NSQFCD3W4CDCJ44GFSKA0/description.md:35-45 makes the acceptance criteria conditional on a future owning change introducing a public type/member or remaining internal, and line 60 still asks which owning deferred-capability story will introduce the first public API.
- .gicket/relations/A0/70/06EZ0NSQFCD3W4CDCJ44GFSKA0--06EZ0NSXY2Y1JZ8SSCX177C770--blocks.json:1-11, .gicket/relations/A0/CC/06EZ0NSQFCD3W4CDCJ44GFSKA0--06EZ0NTV4SVAKV98C418T8A3CC--blocks.json:1-11, and .gicket/relations/A0/YG/06EZ0NSQFCD3W4CDCJ44GFSKA0--06EZ0NVN71BN0QWJDCWGVZ2PYG--blocks.json:1-11 show this ticket currently blocks the PIT, bridge, and multi-active stories.
- git show --name-only 7297a0093c4864b01dea8afc3bd475c5c198de65 lists only .gicket ticket files, and git diff --name-only develop..ticket/06EZ0NSQFCD3W4CDCJ44GFSKA0-task-add-api-snapshot-guardrails-for-deferred-ca | rg -v ^[.]gicket/ returned no output.
- Repository search rg -n no public contract was introduced|internal-only outcome|surface remained internal|stayed internal docs README.md tests returned no matches, so the required internal-only note has no named repo artifact or existing convention.

Blocking findings
- The repo already contains the per-package snapshot guardrail and workflow, while the contract also forbids inventing placeholder public APIs and confirms no deferred-capability public type/member is currently visible. As written, the ticket still lacks an independent developer deliverable for this ticket alone.
- Scope ownership is still ambiguous: the contract says snapshot coverage should follow a real exported API introduced by owning work and asks which owning story will introduce the first public API, but this ticket is currently blocking the PIT, bridge, and multi-active owning stories.
- Acceptance Criterion 2 requires an explicit note when work stays internal, but the ticket does not name where that note must live or what artifact reviewers should inspect.

Required PO actions
- Decide whether this ticket should be closed/re-scoped as already-covered snapshot infrastructure or rewritten to point at one concrete repository artifact that must change independently.
- If the intent is to guard a future public API, move or mirror this guardrail requirement into the specific owning deferred-capability story that will introduce that API and realign the blocking relations accordingly.
- Name the auditable artifact for the internal-only case so Acceptance Criterion 2 is objectively checkable.

Open issues ledger
- critic-item-1 [required-po-action] Decide whether this ticket should be closed/re-scoped as already-covered snapshot infrastructure or rewritten to point at one concrete repository artifact that must change independently.
- critic-item-2 [required-po-action] If the intent is to guard a future public API, move or mirror this guardrail requirement into the specific owning deferred-capability story that will introduce that API and realign the blocking relations accordingly.
- critic-item-3 [required-po-action] Name the auditable artifact for the internal-only case so Acceptance Criterion 2 is objectively checkable.
- critic-item-4 [blocking-finding] The repo already contains the per-package snapshot guardrail and workflow, while the contract also forbids inventing placeholder public APIs and confirms no deferred-capability public type/member is currently visible. As written, the ticket still lacks an independent developer deliverable for this ticket alone.
- critic-item-5 [blocking-finding] Scope ownership is still ambiguous: the contract says snapshot coverage should follow a real exported API introduced by owning work and asks which owning story will introduce the first public API, but this ticket is currently blocking the PIT, bridge, and multi-active owning stories.
- critic-item-6 [blocking-finding] Acceptance Criterion 2 requires an explicit note when work stays internal, but the ticket does not name where that note must live or what artifact reviewers should inspect.

Missing examples / edge cases
- No concrete example identifies the first owning story/package that should exercise the per-package snapshot update rule.
- No concrete example shows an internal-only deferred-capability change and where the required no-public-contract note should be recorded.

Risky assumptions
- Assuming a developer can implement this ticket without either inventing placeholder public APIs or piggybacking on another owning story.
- Assuming reviewers will consistently recognize the internal-only outcome without a named evidence location.

AC / test suggestions
- Rewrite the acceptance criteria around one explicit outcome: either a named documentation/process artifact for internal-only cases or a named owning story/public API example that will exercise the snapshot gate.
- Add one verifiable evidence rule for the internal-only path so reviewers know exactly what to inspect when snapshot files stay unchanged.

Implementation watchouts
- Do not let this ticket drive speculative public surface additions; docs/plans/deferred-data-vault-capabilities.md:41 explicitly forbids inferring concrete API names from the decision record.
- Because provider packages share the DCoding.Data.DVault namespace, any future owning story must name the affected package snapshot file, not just the namespace.

Non-blocking notes
- The prior blocker about unsupported inferred existing APIs appears resolved: .gicket/tickets/06EZ0NSQFCD3W4CDCJ44GFSKA0/description.md:11-18 now matches the visible snapshot/test evidence, comment 06EZM8VZBCAZKQAJ2H4HNC8964.md marks critic-item-1/2/3 answered, and ## Open Questions is none.

Split recommendations
- Prefer one of two ticket-level paths: close/re-scope this as a named documentation/process task, or attach the guardrail acceptance criteria directly to the first owning deferred-capability story that introduces a public API.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment