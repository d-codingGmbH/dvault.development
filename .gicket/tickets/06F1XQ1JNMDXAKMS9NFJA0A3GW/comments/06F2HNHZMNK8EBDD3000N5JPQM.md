[gicket-bot] PO-critic review contract

Summary
- The ticket is now specific enough for developer handoff: the persisted contract resolves the prior diagnostic-id and catalog-ownership blockers, keeps the slice bounded to DMV1901 and DMV1902 plus minimal scaffolding, and has no open questions.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06F1XQ1JNMDXAKMS9NFJA0A3GW/description.md reserves DMV1901-DMV1999, assigns DMV1901 to unsupported selector shape and DMV1902 to duplicate member, and its Open Questions section is '- none'.
- .gicket/tickets/06F1XQ1JNMDXAKMS9NFJA0A3GW/comments/06F2HKR2FWJBMMYHNTBZX5CEG8.md records prior critic items 1 through 5 as answered and marks the PO handoff decision as 'ready_for_po_critic'.
- git log --oneline --decorate -n 12 on branch ticket/06F1XQ1JNMDXAKMS9NFJA0A3GW-task-implement-first-analyzer-rules-and-tests shows only workflow and ticket commits after cc7e30ee2 (develop), including 15b14b356 handoff po->po-critic and b4a543766 lease claim po-critic.
- git diff --name-only develop..HEAD lists only .gicket/tickets/06F1XQ1JNMDXAKMS9NFJA0A3GW/**, so the current branch is ticket-metadata-only and carries no hidden implementation changes.
- docs/plans/fluent-code-first-api-contract.md contains the product contract for repeated direct member selectors; rg found BusinessKey, Payload, and DrivingKey direct-member rules at lines 33-45, 64, 79-80, 120, and 128.
- tests/DCoding.Data.DVault.Tests/Unit/DataVaultCodeFirstMetadataTranslationTests.cs already exercises unsupported selector shapes and duplicate logical member declarations for BusinessKey, Payload, and DrivingKey, giving the analyzer work a concrete runtime baseline.
- Repository inspection shows why the PO clarification matters: src/DCoding.Data.DVault/DataVaultDiagnosticCatalog.cs and DataVaultDiagnosticDefinition.cs are internal, and src/DCoding.Data.DVault/Properties/AssemblyInfo.cs exposes internals only to provider and test assemblies; the refined ticket explicitly avoids depending on those types by allowing analyzer-local mirrored metadata.
- An rg search for Microsoft.CodeAnalysis, DiagnosticDescriptor, DiagnosticAnalyzer, Roslyn, and RunAnalyzers across src/, tests/, and DVault.slnx returns only the three existing RunAnalyzers=false test-project settings, which aligns with the ticket's explicit requirement to add minimal analyzer scaffolding and a dedicated harness.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- The contract does not name a concrete analyzer fixture for a valid direct field selector, even though src/DCoding.Data.DVault/DataVaultCodeFirstHubBuilder.cs and DataVaultCodeFirstSelector.cs both accept FieldInfo.
- The contract does not spell out a boxing or conversion example such as x => (object)x.Member; current runtime helpers are not uniform here because DataVaultCodeFirstHubBuilder.cs unwraps converts while DataVaultCodeFirstSelector.cs does not.
- The ticket says the first rules should stay tightly scoped and generated-code-safe, but it does not name an explicit non-DVault or generated-code false-positive guard fixture.

Risky assumptions
- Developers must treat analyzer-local mirrored metadata as the intended boundary for this ticket and not reopen shared or public catalog extraction from parent story 06F1XQ15J5JEC92T1QCE9TABBM.
- Developers will need a real Roslyn analyzer test harness because all existing test projects currently set RunAnalyzers=false.
- Analyzer semantics should be mirrored from the currently exercised runtime behavior, not from unused or future-facing helper code.

AC / test suggestions
- Add one explicit false-positive guard on a valid field selector if field support is intended to mirror current runtime selector helpers.
- Add one explicit non-DVault noise guard fixture so 'direct DVault Code-First invocations only' is proven by tests.
- Add a dedicated fixture for boxing or conversion selectors if DMV1901 is expected to treat them consistently across BusinessKey, Payload, and DrivingKey.

Implementation watchouts
- Analyzer project and harness scaffolding are net-new: repository search found no existing Microsoft.CodeAnalysis or DiagnosticAnalyzer project entries, only RunAnalyzers=false in the current test projects.
- Do not depend on src/DCoding.Data.DVault/DataVaultDiagnosticCatalog.cs or DataVaultDiagnosticDefinition.cs from the analyzer assembly; the ticket's clarified boundary is analyzer-local mirrored metadata.
- Runtime selector behavior is split across DataVaultCodeFirstHubBuilder.cs and DataVaultCodeFirstSelector.cs, and the repo also contains an unused src/DCoding.Data.DVault/DataVaultCodeFirstMemberSelector.cs; avoid creating a third semantic path.

Non-blocking notes
- DVault.slnx already has established src/ and tests/DCoding.Data.DVault.Tests/Integration, tests/DCoding.Data.DVault.Tests/Modeling, tests/DCoding.Data.DVault.Tests/Shared, and tests/DCoding.Data.DVault.Tests/Unit layout entries, and the current test csproj files use net10.0, nullable, and implicit-usings conventions that the ticket explicitly tells developers to follow.
- Parent story 06F1XQ15J5JEC92T1QCE9TABBM still holds broader analyzer-foundation and packaging concerns, but this child ticket now explicitly scopes that work out instead of leaving the boundary implicit.

Split recommendations
- No immediate split is needed; the two-rule analyzer slice plus minimal scaffolding remains developer-sized.
- If the team later wants shared or public diagnostic metadata across analyzer assemblies, keep that as a follow-up under story 06F1XQ15J5JEC92T1QCE9TABBM instead of expanding this task.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment