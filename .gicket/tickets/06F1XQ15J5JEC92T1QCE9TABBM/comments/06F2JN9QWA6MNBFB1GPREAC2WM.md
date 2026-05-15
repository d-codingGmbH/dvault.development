[gicket-bot] PO-critic review contract

Summary
- Return to PO: the implementation scope is mostly well bounded, but the persisted contract contains stale/incorrect ticket-context claims about child/relation evidence that local ticket data contradicts.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06F1XQ15J5JEC92T1QCE9TABBM/description.md has ## Open Questions: none, so the open-question rule does not block approval by itself.
- The persisted PO Summary says no child tickets were created, but .gicket/relations/BM/GW/06F1XQ15J5JEC92T1QCE9TABBM--06F1XQ1JNMDXAKMS9NFJA0A3GW--parentOf.json records a parentOf relation from this story to child 06F1XQ1JNMDXAKMS9NFJA0A3GW.
- git diff --name-status develop..HEAD contains only .gicket/tickets/06F1XQ15J5JEC92T1QCE9TABBM metadata/comment/event changes, not analyzer code changes.
- DVault.slnx includes src/DCoding.Data.DVault.Analyzers/DCoding.Data.DVault.Analyzers.csproj and tests/DCoding.Data.DVault.Tests/Analyzers/DCoding.Data.DVault.Tests.Analyzers.csproj.
- src/DCoding.Data.DVault.Analyzers/DCoding.Data.DVault.Analyzers.csproj targets net10.0, RootNamespace DCoding.Data.DVault.Analyzers, references Microsoft.CodeAnalysis and Microsoft.CodeAnalysis.CSharp, and currently has IsPackable=false.
- src/DCoding.Data.DVault.Analyzers/CodeFirstDiagnosticCatalog.cs defines DMV1901 and DMV1902 with CodeFirst category, warning descriptors, explanation, and remediation text.
- tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultCodeFirstAnalyzerTests.cs asserts SupportedDiagnostics exactly [DMV1901, DMV1902] and covers positive and non-reporting selector/duplicate cases.
- README.md documents Code-First BusinessKey, Payload, and DrivingKey declarations as direct scalar member selectors; src/DCoding.Data.DVault/DataVaultCodeFirstModelBuilder.cs, DataVaultCodeFirstHubBuilder.cs, DataVaultCodeFirstSatelliteBuilder.cs, and DataVaultCodeFirstModelBuilderExtensions.cs provide direct public source evidence for the existing Code-First API/types.

Blocking findings
- none

Required PO actions
- Update the delivery contract to acknowledge the actual relation state: parent epic 06F1XQ0T5WQWN1AES5Z3E0RMSR, done child 06F1XQ1JNMDXAKMS9NFJA0A3GW, and done blockers 06F1XPS7KGKBP5SVMQPJC49J2G and 06F1XPX99KQRB09GRQG50Z75FM.
- Replace the stale no-child-tickets claim with a concise statement that the first analyzer rules/tests slice is already integrated via child 06F1XQ1JNMDXAKMS9NFJA0A3GW and this story's remaining dev work is packaging/build/test/docs readiness for the analyzer package foundation.

Open issues ledger
- critic-item-1 [required-po-action] Update the delivery contract to acknowledge the actual relation state: parent epic 06F1XQ0T5WQWN1AES5Z3E0RMSR, done child 06F1XQ1JNMDXAKMS9NFJA0A3GW, and done blockers 06F1XPS7KGKBP5SVMQPJC49J2G and 06F1XPX99KQRB09GRQG50Z75FM.
- critic-item-2 [required-po-action] Replace the stale no-child-tickets claim with a concise statement that the first analyzer rules/tests slice is already integrated via child 06F1XQ1JNMDXAKMS9NFJA0A3GW and this story's remaining dev work is packaging/build/test/docs readiness for the analyzer package foundation.

Missing examples / edge cases
- The acceptance criteria already cover positive diagnostics plus non-reporting cases for valid direct scalar selectors, separate scopes, and selector variables outside the first direct-lambda slice.
- Consider explicitly naming a non-string reference direct-member selector case as allowed or unsupported, because the analyzer's scalar rule should align with the existing public Code-First selector semantics and README guidance.

Risky assumptions
- Assuming DMV1901 and DMV1902 are the correct next analyzer ids relies on current local search showing no other DMV1901/DMV1902 usage outside analyzer code/tests; the stable diagnostic story currently documents DMV1001-DMV1801 as the seeded baseline.
- Assuming IsPackable=false is acceptable for handoff depends on the dev completing either analyzer asset packability or a documented package-boundary rationale, which is present in the AC but not yet implemented in the current project file.

AC / test suggestions
- Keep the existing AC requiring package metadata suitable for packing or an explicit documented reason for any remaining IsPackable=false switch.
- Keep docs/package guidance AC for normal Roslyn analyzer installation and diagnostic suppression; local rg did not find current analyzer installation/suppression guidance in README.md or docs.
- Keep test expectations for DMV1901/DMV1902 descriptor metadata, unsupported selector diagnostics, duplicate same-scope diagnostics, valid direct scalar selectors, separate satellite scopes, and selector variables outside the first direct lambda slice.

Implementation watchouts
- If IsPackable is enabled, inspect package output so analyzer assemblies land under analyzer assets rather than normal library references.
- Do not introduce runtime DVault/provider behavior or Microsoft.EntityFrameworkCore.Design dependencies while packaging the analyzer.
- Use direct source APIs DataVaultCodeFirstModelBuilder, DataVaultCodeFirstHubBuilder, DataVaultCodeFirstSatelliteBuilder, and ApplyDataVaultMetadata as the compatibility baseline, not only analyzer tests or README prose.
- Align the analyzer's readable scalar definition with the existing public Code-First selector behavior before treating direct reference-type members as unsupported.

Non-blocking notes
- The analyzer project/test project already exist in the solution, and the analyzer/test source evidence supports most of the intended scope.
- I did not run build or tests because this PO-critic execution boundary is read-only and test/build commands may write bin/obj or artifacts.
- git status showed unrelated local modifications in .gicket-bot/.gitignore, .gicket/.gitignore, .gicket/project.json, and .gicket/types.json; I did not inspect or modify them for this ticket-level review.

Split recommendations
- No new implementation split is required if PO corrects the relation/provenance text and keeps remaining work focused on analyzer package packability/docs/readiness.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment