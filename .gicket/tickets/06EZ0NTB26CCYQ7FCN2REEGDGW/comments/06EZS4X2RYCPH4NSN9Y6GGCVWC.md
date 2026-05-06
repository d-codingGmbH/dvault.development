[gicket-bot] PO-critic review contract

Summary
- Contract is concrete enough for developer handoff: this ticket now carries the minimum consumed PIT contract, the sibling dependency is machine-readable via a live blocks relation, and no Open Questions remain.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06EZ0NTB26CCYQ7FCN2REEGDGW/description.md:11-15 defines the consumed PIT contract, the worked baseline Customer + [Profile, Status], the ownership boundary with sibling 06EZ0NT4FDPC7XTQH40PQS942M, and the live blocks relation.
- .gicket/tickets/06EZ0NTB26CCYQ7FCN2REEGDGW/description.md:29-40 gives concrete acceptance criteria and DoD, including exact PIT entity/key/column names, deterministic rejection cases, SQLite baseline proof, and same-delivery API snapshot updates.
- .gicket/tickets/06EZ0NTB26CCYQ7FCN2REEGDGW/description.md:49-50 records Open Questions as '- none', so the explicit open-question gate is satisfied.
- .gicket/relations/2M/GW/06EZ0NT4FDPC7XTQH40PQS942M--06EZ0NTB26CCYQ7FCN2REEGDGW--blocks.json:1-10 records a live blocks relation from sibling ticket 06EZ0NT4FDPC7XTQH40PQS942M to this ticket.
- src/DCoding.Data.DVault/Modeling/DataVaultMetadataModel.cs:13-19,25-35 and src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs:29-40 currently expose and translate only hubs, links, and satellites; the ticket explicitly treats PIT as additive output-side work against that baseline.
- src/DCoding.Data.DVault/DataVaultAnnotationNames.cs:42-95, src/DCoding.Data.DVault/Modeling/DataVaultModel.cs:447-462, and tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt:439-442 show there is no PIT property role or table kind yet, matching the ticket's requirement to add any needed output-side public surface and snapshot updates in lockstep.
- docs/plans/deferred-data-vault-capabilities.md:20-25 and 58-65 keep PIT opt-in and outside ordinary hub/link/satellite setup; the ticket's scope-in/scope-out matches that repository architecture boundary.
- Branch history shows the substantive PO refinement in commit 0f9dfa62ff87, whose git show --stat updated .gicket/tickets/06EZ0NTB26CCYQ7FCN2REEGDGW/description.md and added .gicket/relations/2M/GW/06EZ0NT4FDPC7XTQH40PQS942M--06EZ0NTB26CCYQ7FCN2REEGDGW--blocks.json; later commits 46a22cdf and ed4e6c7b are workflow residual/claim writes only.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- Null-handling or carry-forward behavior when one included satellite has no row at a PIT instant remains explicitly deferred in .gicket/tickets/06EZ0NTB26CCYQ7FCN2REEGDGW/description.md:52-55.
- Link-based PIT and multi-active satellite PIT examples are intentionally absent and remain out of scope per .gicket/tickets/06EZ0NTB26CCYQ7FCN2REEGDGW/description.md:23-27 and 54-55.

Risky assumptions
- The future producer-side PIT API could still diverge from the copied consumer contract; .gicket/tickets/06EZ0NTB26CCYQ7FCN2REEGDGW/description.md:57-59 already flags that such divergence would require a PO re-check.
- Because current public surface has no PIT entity kind, PIT property role, or PIT logical property kind yet, delivery likely spans coordinated enum, annotation, provider-mapping, and API-snapshot changes.

AC / test suggestions
- Keep one explicit baseline fixture for Customer + [Profile, Status] that asserts entity name, property order, primary-key order, and stable repeated output, matching .gicket/tickets/06EZ0NTB26CCYQ7FCN2REEGDGW/description.md:30-40.
- Keep one negative test per named invalid shape rather than a pooled failure case so contract drift is obvious: empty satellites, duplicates, unattached satellites, link-based PIT, multi-active PIT, and other out-of-baseline shapes.
- Treat approved API snapshot updates as required acceptance evidence whenever PIT-facing public members are introduced, per .gicket/tickets/06EZ0NTB26CCYQ7FCN2REEGDGW/description.md:33 and 40 plus docs/quality/api-surface-snapshots.md:3-28.

Implementation watchouts
- Do not let this ticket absorb PIT input-side modeling or builder API work; that stays with sibling 06EZ0NT4FDPC7XTQH40PQS942M per .gicket/tickets/06EZ0NTB26CCYQ7FCN2REEGDGW/description.md:14 and 23-25.
- Do not add provider-name branching or SQLite-specific SQL to core PIT translation; the contract requires provider-neutral core logic with SQLite only as baseline proof in .gicket/tickets/06EZ0NTB26CCYQ7FCN2REEGDGW/description.md:32.
- Current translation and test baseline assumes only hub/link/satellite outputs; tests/DCoding.Data.DVault.Tests/Unit/DataVaultEfMetadataTranslationTests.cs:62-69 and src/DCoding.Data.DVault/Modeling/DataVaultModel.cs:447-462 will need intentional expansion rather than silent fixture drift.

Non-blocking notes
- Sibling 06EZ0NT4FDPC7XTQH40PQS942M remains unrefined on its own ticket, but this ticket now carries the minimum consumer-side PIT contract and the dependency is explicitly modeled as a blocks relation, so that is a sequencing concern rather than a PO-contract gap on this ticket.

Split recommendations
- No additional split is needed; retain the current story split across producer-side API ticket 06EZ0NT4FDPC7XTQH40PQS942M, this EF mapping ticket 06EZ0NTB26CCYQ7FCN2REEGDGW, and docs/examples ticket 06EZ0NTJZEMVA5RPR01V0KNVMR.
- Keep the existing blocks relation 06EZ0NT4FDPC7XTQH40PQS942M -> 06EZ0NTB26CCYQ7FCN2REEGDGW as the sequencing mechanism.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment