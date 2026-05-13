[gicket-bot] PO-critic review contract

Summary
- Ticket contract requires substantive product-owner changes before development.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06F1XPRY3ZDB6W1WQ9ABRRJ2V4/description.md:27-47 says the epic stays open until follow-up 06F23Z08K0W49K5JMEHP60WZC0 is done or intentionally superseded, requires a lifecycle-guardrails release summary, and has Open Questions = none.
- .gicket/tickets/06F1XPRY3ZDB6W1WQ9ABRRJ2V4/comments/06F2406F3AQ7ZV5J67Q1E5QPSM.md:10-20 explicitly answers the prior critic checklist, states repository evidence still stops at docs/releases/v0.7.0.md, and says the missing docs work is tracked in 06F23Z08K0W49K5JMEHP60WZC0 instead of this closure-only branch.
- .gicket/tickets/06F23Z08K0W49K5JMEHP60WZC0/description.md:7-24 scopes the remaining work as adding docs/releases/v0.8.0.md and aligns it to docs/architecture/dvault-dotnet-ef-design-time-workflow.md and docs/model-first-governance.md.
- Repository inspection of docs/releases returns only v0.5.0.md, v0.6.0.md, and v0.7.0.md; docs/releases/v0.8.0.md is not present.
- git diff --name-only 80dcde7bf..HEAD lists only .gicket/** files, so this parent branch did not add the missing repository release-summary artifact.
- Current repository boundaries still match the parent contract: docs/architecture/dvault-dotnet-ef-design-time-workflow.md:126-189 keeps preflight consumer-owned and rejects DVault-owned IDesignTimeServices/CLI shim; docs/model-first-governance.md:136-169 keeps drift comparison metadata-only with SQLite-first live-schema evidence; src/DCoding.Data.DVault/DataVaultDiagnostics.cs:383-423, DataVaultMigrationOperationDiagnostics.cs:10-113, and DataVaultModelDriftReporter.cs:13-103 expose the cited diagnostics, guardrail, and drift APIs directly in source.

Blocking findings
- The parent epic no longer owns direct developer work: its contract and latest PO comment both say the only unmet deliverable is the follow-up docs ticket 06F23Z08K0W49K5JMEHP60WZC0. Approving this parent epic for dev would hand off a tracking ticket instead of the actionable docs task.

Required PO actions
- Keep the parent epic as a tracking ticket and do not hand it off directly to dev while the remaining repository artifact is owned by 06F23Z08K0W49K5JMEHP60WZC0.
- Move ticket 06F23Z08K0W49K5JMEHP60WZC0 through PO refinement and PO-critic so the developer handoff lands on the ticket that actually owns docs/releases/v0.8.0.md.
- After the follow-up is routed, keep the parent epic blocked on that ticket rather than implying direct implementation remains on this branch.

Open issues ledger
- critic-item-1 [required-po-action] Keep the parent epic as a tracking ticket and do not hand it off directly to dev while the remaining repository artifact is owned by 06F23Z08K0W49K5JMEHP60WZC0.
- critic-item-2 [required-po-action] Move ticket 06F23Z08K0W49K5JMEHP60WZC0 through PO refinement and PO-critic so the developer handoff lands on the ticket that actually owns docs/releases/v0.8.0.md.
- critic-item-3 [required-po-action] After the follow-up is routed, keep the parent epic blocked on that ticket rather than implying direct implementation remains on this branch.
- critic-item-4 [blocking-finding] The parent epic no longer owns direct developer work: its contract and latest PO comment both say the only unmet deliverable is the follow-up docs ticket 06F23Z08K0W49K5JMEHP60WZC0. Approving this parent epic for dev would hand off a tracking ticket instead of the actionable docs task.

Missing examples / edge cases
- When 06F23Z08K0W49K5JMEHP60WZC0 is refined for execution, the release-summary contract should include one explicit example that migration preflight is a consumer-owned step outside dotnet ef itself and one explicit example that live-schema evidence is optional and SQLite-first.

Risky assumptions
- none

AC / test suggestions
- For 06F23Z08K0W49K5JMEHP60WZC0, keep acceptance text explicitly tied to docs/releases/v0.8.0.md, DataVaultModelFirstDesignTimeWorkflowTests, and the SQLite live-schema lane already cited in docs/model-first-governance.md:159-169.
- Require the follow-up release summary to preserve the exact guarded order documented in docs/architecture/dvault-dotnet-ef-design-time-workflow.md:177-183: diagnostics, migration guardrails, then optional drift evidence.

Implementation watchouts
- Do not let the release summary imply DVault-owned IDesignTimeServices, a custom dotnet ef shim, or automatic guardrail output inside EF CLI commands; docs/architecture/dvault-dotnet-ef-design-time-workflow.md:173-189 explicitly rejects those claims.
- Do not blur metadata-only ModelSnapshot/EF metadata drift comparison with optional live-schema checks; docs/model-first-governance.md:136-169 and docs/releases/v0.7.0.md:49-49 keep the live-schema lane SQLite-first and separately bounded.

Non-blocking notes
- Local ticket comment history is not empty despite the prompt snapshot saying Recent comments <none>; the newest PO-relevant comment is 06F2406F3AQ7ZV5J67Q1E5QPSM.md and it materially changes the review context.

Split recommendations
- Keep the current docs-only follow-up split 06F23Z08K0W49K5JMEHP60WZC0.
- Do not reopen the four done implementation stories on this epic branch.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment