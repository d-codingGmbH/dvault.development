[gicket-bot] PO-critic review contract

Summary
- Return to PO: the ticket depends on PIT metadata/key semantics that are not yet concretely locked in a refined sibling contract or visible source surface.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- Current ticket contract at `.gicket/tickets/06EZ0NTB26CCYQ7FCN2REEGDGW/description.md:13-19,39-46` says this task must consume 'validated PIT metadata' from sibling ticket `06EZ0NT4FDPC7XTQH40PQS942M` and align PIT naming/key semantics with that sibling rather than define its own contract.
- `src/DCoding.Data.DVault/Modeling/DataVaultMetadataModel.cs:13-19,25-35` and public API snapshot `tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt:354-359` expose translation inputs only as hubs, links, and satellites.
- `src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs:29-40` only enumerates hubs, links, and satellites; `tests/DCoding.Data.DVault.Tests/Unit/DataVaultEfMetadataTranslationTests.cs:62-69` currently asserts exactly 3 entity types (hub/link/satellite).
- `src/DCoding.Data.DVault/Modeling/DataVaultModel.cs:447-462` and snapshot `tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt:439-442` show `DataVaultTableKind` currently has only `Hub`, `Link`, and `Satellite`, while this ticket DoD expects any required public API/snapshot changes to land in the same delivery.
- `docs/plans/deferred-data-vault-capabilities.md:24-25,41,58-65` marks PIT as opt-in and explicitly says the architecture record should not infer concrete PIT API names, so this ticket still needs a concrete sibling API contract.
- Branch history on `ticket/06EZ0NTB26CCYQ7FCN2REEGDGW-task-generate-provider-neutral-pit-ef-model-mapp`: `git log --oneline --decorate --grep='PIT|06EZ0NTB26CCYQ7FCN2REEGDGW' -n 20` showed only workflow commits `fd66ee8e`, `a9e9fa71`, `ae2cad2c`, and `f33cd09a`; `git show --stat a9e9fa71` touched only `.gicket/tickets/06EZ0NTB26CCYQ7FCN2REEGDGW/*`.
- `gicket-read-ticket-comments` returned 9 comments consisting of claim/lease/handover bot events plus the PO refinement contract; no comment added the missing PIT API/key-semantics contract or dependency resolution details.

Blocking findings
- The ticket is not developer-ready because it explicitly depends on sibling task `06EZ0NT4FDPC7XTQH40PQS942M` for the PIT metadata model, naming, and key semantics, but that sibling is still `needs-po`. Approving now would force the developer to invent the input contract across ticket boundaries.
- Direct repository evidence shows no PIT input surface or PIT table-kind surface exists yet (`DataVaultMetadataModel`, `DataVaultEfMetadataTranslator`, `DataVaultTableKind`, and the public API snapshot are all hub/link/satellite only). Because this ticket's DoD includes any required public API/snapshot changes, PO still needs to pin which PIT-facing surface is expected here versus in the sibling API ticket.

Required PO actions
- Update `.gicket/tickets/06EZ0NTB26CCYQ7FCN2REEGDGW/description.md` to reference the sibling contract revision or concrete repo surface it consumes, and state which PIT-facing public API/snapshot changes are owned here versus by `06EZ0NT4FDPC7XTQH40PQS942M`.
- Add one worked baseline example for one hub plus two satellites that spells out expected ordered PIT columns, key columns, and explicit failure examples for out-of-contract shapes.

Open issues ledger
- critic-item-1 [required-po-action] Update `.gicket/tickets/06EZ0NTB26CCYQ7FCN2REEGDGW/description.md` to reference the sibling contract revision or concrete repo surface it consumes, and state which PIT-facing public API/snapshot changes are owned here versus by `06EZ0NT4FDPC7XTQH40PQS942M`.
- critic-item-2 [required-po-action] Add one worked baseline example for one hub plus two satellites that spells out expected ordered PIT columns, key columns, and explicit failure examples for out-of-contract shapes.
- critic-item-3 [blocking-finding] The ticket is not developer-ready because it explicitly depends on sibling task `06EZ0NT4FDPC7XTQH40PQS942M` for the PIT metadata model, naming, and key semantics, but that sibling is still `needs-po`. Approving now would force the developer to invent the input contract across ticket boundaries.
- critic-item-4 [blocking-finding] Direct repository evidence shows no PIT input surface or PIT table-kind surface exists yet (`DataVaultMetadataModel`, `DataVaultEfMetadataTranslator`, `DataVaultTableKind`, and the public API snapshot are all hub/link/satellite only). Because this ticket's DoD includes any required public API/snapshot changes, PO still needs to pin which PIT-facing surface is expected here versus in the sibling API ticket.

Missing examples / edge cases
- A concrete one-hub plus multiple-satellites example that fixes deterministic ordering of per-satellite snapshot reference columns.
- Explicit invalid-shape examples for empty satellite sets, duplicate satellite references, satellites not attached to the declared hub, link-based PIT, and multi-active satellite references.
- An exact PIT key-shape example; the ticket asks for key metadata coverage but does not itself spell out the baseline composition.

Risky assumptions
- That sibling ticket `06EZ0NT4FDPC7XTQH40PQS942M` will settle PIT names and key fields early enough that this mapping ticket can start without rework.
- That PIT projection can fit the existing provider-neutral property/annotation surface without needing additional public enums, annotations, or ownership clarification.
- That SQLite queryability proof can be specified cleanly before the PIT metadata/type surface is frozen.

AC / test suggestions
- Use a concrete sibling PIT metadata example as an acceptance fixture and assert produced entity name, ordered columns, primary key columns, and annotations from that exact input.
- Add translator-level negative tests for every unsupported PIT shape the contract expects to reject deterministically.
- Add public API snapshot expectations for any PIT-facing additions to `DataVaultMetadataModel`, `DataVaultTableKind`, or annotation names once ticket ownership is clarified.

Implementation watchouts
- Keep PIT opt-in so hub/link/satellite output remains unchanged when PIT metadata is absent.
- Do not move SQLite-specific SQL or provider-name branching into the core translator; current source and architecture both enforce provider-neutral projection.
- Any new PIT-facing enum, annotation, or metadata type is public-surface work and must stay synchronized with API snapshot coverage.

Non-blocking notes
- The current ticket's own `## Open Questions` section says `none`.
- `docs/plans/deferred-data-vault-capabilities.md` already legitimizes PIT as a bounded opt-in capability; the issue is contract sequencing, not story legitimacy.
- No additional feature split is required if the metadata-model dependency is clarified first.

Split recommendations
- No new functional split is needed, but sequencing should be explicit: finish/refine `06EZ0NT4FDPC7XTQH40PQS942M` before developer handoff for `06EZ0NTB26CCYQ7FCN2REEGDGW`.
- If one engineer must define the PIT public surface and translator projection together, collapse that sequencing explicitly in the ticket plan instead of leaving ownership ambiguous between sibling tasks.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment