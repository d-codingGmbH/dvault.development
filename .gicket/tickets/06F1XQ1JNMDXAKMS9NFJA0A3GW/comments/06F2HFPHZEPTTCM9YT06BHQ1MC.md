[gicket-bot] PO refinement contract

Summary
- Refined the ticket to a bounded first Roslyn-analyzer slice: two high-confidence Code-First rules, explicit analyzer-test harness expectations, and repository-layout defaults with no blocking PO questions.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Repository evidence shows no existing Roslyn analyzer project or analyzer test harness yet, so this ticket may add only the minimal package/test scaffolding needed to implement and verify the first rules.
- The safest v1 analyzer target is the existing Code-First fluent API under ApplyDataVaultMetadata(vault => ...), because docs/plans/fluent-code-first-api-contract.md and current unit tests already define the invalid selector and duplicate-member behaviors precisely.
- Existing tests in tests/DCoding.Data.DVault.Tests/Unit/DataVaultCodeFirstMetadataTranslationTests.cs and DataVaultCodeFirstLinkTests.cs already provide canonical negative examples that the analyzer rules should mirror instead of inventing new semantics.
- All current test projects set RunAnalyzers=false, so ordinary project compilation is not sufficient proof; this ticket must set up explicit analyzer verification or equivalent programmatic compilation for rule tests.
- The repository already has an established DVault diagnostic contract and catalog conventions around documented ids, categories, titles, and remediation; this ticket should consume that baseline for analyzer diagnostics rather than inventing ad hoc rule metadata.
- Broader package-foundation work such as packing polish, installation guidance, and suppression documentation belongs to story 06F1XQ15J5JEC92T1QCE9TABBM unless a minimal amount is required here to host and test the two rules.

Scope In
- Create the minimal Roslyn analyzer project boundary and analyzer-test infrastructure needed for DVault within the existing src/ and tests/ repository conventions if that scaffolding is not already present.
- Implement one high-confidence rule that reports unsupported direct-member selector misuse in BusinessKey(...), Payload(...), and DrivingKey(...) calls inside DVault Code-First configuration.
- Implement one high-confidence rule that reports duplicate logical member declarations within the same Code-First hub or satellite configuration for BusinessKey(...), Payload(...), or DrivingKey(...).
- Add positive and negative analyzer test samples for each rule, including true-positive diagnostics and false-positive guards on valid direct scalar member declarations.
- Keep analysis limited to user-authored Code-First invocation chains so the first analyzer release stays precise and low-noise.

Scope Out
- Broader analyzer coverage such as missing business keys, link participant ordering, missing hubs, or duplicate metadata names outside the first two bounded rules.
- Model-first JSON validation, migration guardrail diagnostics, or any non-Roslyn validation surface already covered by existing DMV#### or DVM2xxx runtime diagnostics.
- NuGet packaging polish, installation docs, suppression docs, and broader analyzer-package foundation work beyond the minimal scaffolding needed to compile and test these rules.
- Non-trivial code fixes or broad semantic/dataflow analysis that would increase false positives in the first analyzer slice.

Open questions
- none

Follow-up questions
- After these two rules are stable, should the next analyzer slice cover link participant errors such as too few participants, repeated same-hub participants, or participants declared before their hubs?
- Should a later ticket add code fixes for the selector-shape and duplicate-member diagnostics, or should the analyzer package remain diagnostics-only for v0.10.0?
- Does the team want an explicit documented DMV code-band allocation for analyzer diagnostics after this first slice lands, or is per-ticket catalog extension sufficient for now?

Risks
- If the analyzer attempts to infer builder state across locals, helper methods, or complex control flow in v1, false positives and false negatives will rise quickly.
- Because current test projects disable analyzer execution, the ticket can appear complete without real analyzer coverage unless the explicit test harness is added.
- If new analyzer diagnostics do not follow the existing catalog conventions, DVault diagnostic ids and categories may drift across runtime and Roslyn surfaces.
- Expanding this ticket beyond the two high-confidence Code-First rules will likely turn it into a broader analyzer-foundation effort and slow delivery.

Split recommendations
- No additional split is required for PO-critic readiness; this ticket is already well-bounded if it stays on the two high-confidence Code-First rules plus the minimal harness needed to test them.
- If the team wants broader analyzer coverage such as missing business keys or link-participant validation, create a follow-up task after this first low-noise rule pair ships.
- If packaging polish, installation guidance, or suppression documentation is still missing after the minimal scaffolding is in place, keep that work on the parent analyzer-foundation story rather than expanding this task.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 5
- implementation-notes items: 7

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment