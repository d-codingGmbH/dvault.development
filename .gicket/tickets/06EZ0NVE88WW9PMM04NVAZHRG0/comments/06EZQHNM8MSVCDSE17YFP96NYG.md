[gicket-bot] PO-critic review contract

Summary
- Return to PO: the child docs task is bounded, but it depends on an upstream bridge surface that is not yet PO-ready or concretely defined in source, so developer handoff would force the assignee to guess the bridge example shape.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06EZ0NVE88WW9PMM04NVAZHRG0/description.md:47-48` sets `## Open Questions` to `- none`, so the block is not unresolved open questions on this ticket.
- `.gicket/relations/CC/G0/06EZ0NTV4SVAKV98C418T8A3CC--06EZ0NVE88WW9PMM04NVAZHRG0--parentOf.json` confirms this ticket is a child of that parent bridge story.
- `.gicket/tickets/06EZ0NVE88WW9PMM04NVAZHRG0/description.md:14,30,37,55` requires the docs example to align with generated bridge metadata and table shapes, while also acknowledging the parent bridge implementation shape may still change.
- `docs/plans/deferred-data-vault-capabilities.md:41,65,101-111` says deferred-feature API names must not be inferred, generated bridge tables/public APIs are unsupported in the current baseline, and current source evidence still projects only hubs, links, and satellites.
- `src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs:29-40` only creates hub, link, and satellite entities in the current EF metadata translator.
- `rg -n Bridge|bridge src/DCoding.Data.DVault tests/DCoding.Data.DVault.Tests README.md docs/architecture` returned no matches, so there is no current source or architecture-level bridge API/example contract outside planning docs.
- `README.md:32-66,139-165` documents only hub/link quickstart shapes and says `examples/` is reserved for future runnable DVault examples.
- `git log --oneline --decorate -n 5` on branch `ticket/06EZ0NVE88WW9PMM04NVAZHRG0-task-add-bridge-documentation-and-example-scenar` shows only lease/handoff commits after `develop`, and `git show --stat 2f737c20` changed only `.gicket` ticket files.

Blocking findings
- The ticket is a documentation child of parent story `06EZ0NTV4SVAKV98C418T8A3CC`, but that parent still carries `needs-po`; approving the child for dev would hand off work that depends on an upstream scope the repository still marks as not PO-ready.
- The acceptance criteria require a minimal example aligned to generated bridge metadata/table shapes, but the current repo exposes no concrete bridge source/API/type contract and the governing decision record explicitly forbids inferring deferred-feature API names. Without an authoritative bridge-shape reference, the doc example would require guessing.

Required PO actions
- Refine parent story `06EZ0NTV4SVAKV98C418T8A3CC` out of `needs-po`, or explicitly state on this child ticket that documentation work is blocked until that parent establishes the authoritative bridge surface.
- Update this ticket contract to point to the exact authoritative bridge artifact the docs must follow once available: the parent ticket contract, a concrete source path/type, or a follow-up relation.
- Add an explicit blocking relation, label, or equivalent ticket-level sequencing signal so this child does not route to dev before the bridge surface exists.
- Specify which single minimal example scenario is required: the many-to-many traversal case or the hierarchy-style traversal case.

Open issues ledger
- critic-item-1 [required-po-action] Refine parent story `06EZ0NTV4SVAKV98C418T8A3CC` out of `needs-po`, or explicitly state on this child ticket that documentation work is blocked until that parent establishes the authoritative bridge surface.
- critic-item-2 [required-po-action] Update this ticket contract to point to the exact authoritative bridge artifact the docs must follow once available: the parent ticket contract, a concrete source path/type, or a follow-up relation.
- critic-item-3 [required-po-action] Add an explicit blocking relation, label, or equivalent ticket-level sequencing signal so this child does not route to dev before the bridge surface exists.
- critic-item-4 [required-po-action] Specify which single minimal example scenario is required: the many-to-many traversal case or the hierarchy-style traversal case.
- critic-item-5 [blocking-finding] The ticket is a documentation child of parent story `06EZ0NTV4SVAKV98C418T8A3CC`, but that parent still carries `needs-po`; approving the child for dev would hand off work that depends on an upstream scope the repository still marks as not PO-ready.
- critic-item-6 [blocking-finding] The acceptance criteria require a minimal example aligned to generated bridge metadata/table shapes, but the current repo exposes no concrete bridge source/API/type contract and the governing decision record explicitly forbids inferring deferred-feature API names. Without an authoritative bridge-shape reference, the doc example would require guessing.

Missing examples / edge cases
- The contract says to cover both many-to-many and hierarchy-style use cases, but it does not choose which one the single minimal example must demonstrate.
- The unsupported-pattern guidance does not name a minimum set such as recursive hierarchy depth, provider-specific maintenance/optimization, or workload-specific traversal semantics.
- The contract does not say how to resolve the example if the eventual bridge implementation uses names or technical columns that differ from current assumptions.

Risky assumptions
- Assuming the future bridge surface will reuse obvious hub/link naming without a direct source contract.
- Assuming the docs task can start independently even though the parent bridge story is still `needs-po`.
- Assuming the example can be authored now and only lightly adjusted later, despite the ticket's own risk note that the generated bridge shape may still change.

AC / test suggestions
- Add an acceptance criterion that the example must cite the authoritative bridge artifact it matches once the parent implementation exists.
- State explicitly whether the one minimal example is a many-to-many traversal or a hierarchy-style traversal example.
- Require the unsupported-pattern section to name at least recursive/deep hierarchy semantics, provider-specific behavior, and PIT/multi-active non-prerequisites.

Implementation watchouts
- Do not invent new bridge setup APIs; current repository vocabulary is `AddDVault()`, `UseDataVault()`, `ApplyDataVaultMetadata()`, and the explicit `IDataVaultSaveService` boundary.
- Do not imply bridge support is part of the default hub/link/satellite path or that a runnable `examples/` app is required.
- Keep any eventual example consistent with current deterministic naming conventions like `HubCustomer`, `LinkCustomerOrder`, `LoadTimestamp`, and `RecordSource`, but only after the bridge surface itself is concretely defined.

Non-blocking notes
- The child ticket itself is otherwise bounded: `## Open Questions` is `none`, scope-out excludes implementation/provider work, and README already reserves `examples/` for future runnable samples.
- The current branch snapshot is a ticket-refinement handoff only; no bridge documentation or source implementation has been introduced yet.

Split recommendations
- No split required.
- Sequence this task behind parent story `06EZ0NTV4SVAKV98C418T8A3CC` with an explicit blocking dependency instead of treating it as immediately developer-ready.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment