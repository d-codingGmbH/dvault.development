[gicket-bot] PO-critic review contract

Summary
- Return to PO: the ticket now scopes PIT output work better, but the sibling PIT input contract it cites is still not durably refined in persisted ticket evidence, so developer handoff would still rely on an unresolved cross-ticket dependency.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06EZ0NTB26CCYQ7FCN2REEGDGW/description.md now says this ticket consumes sibling 06EZ0NT4FDPC7XTQH40PQS942M revision 06EZ0Y4A07HWMD2X0AWTC704EM for the PIT input contract and its own Open Questions section is 'none'.
- .gicket/tickets/06EZ0NT4FDPC7XTQH40PQS942M/description.md at revision 06EZ0Y4A07HWMD2X0AWTC704EM still contains only a short goal plus three generic acceptance bullets; it does not contain the durable refinement contract that the mapping ticket says it consumes.
- Repository source still exposes only hub/link/satellite PIT-adjacent surfaces: src/DCoding.Data.DVault/Modeling/DataVaultMetadataModel.cs only has Hubs, Links, and Satellites; src/DCoding.Data.DVault/Modeling/IDataVaultNamingPolicy.cs only has hub/link/satellite naming contexts; src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs CreateEntities only iterates hubs, links, and satellites.
- Produced-model and provider surfaces are still hub/link/satellite only: src/DCoding.Data.DVault/Modeling/DataVaultModel.cs DataVaultTableKind only has Hub, Link, Satellite; src/DCoding.Data.DVault/DataVaultAnnotationNames.cs has no PIT annotation names; src/DCoding.Data.DVault/DataVaultProviderCapabilities.cs DataVaultLogicalPropertyKind has no PIT logical kind.
- bash -lc 'if rg -n "Pit|PIT" /mnt/c/Projects/DVault/src/DCoding.Data.DVault /mnt/c/Projects/DVault/tests/DCoding.Data.DVault.Tests -g "!**/bin/**" -g "!**/obj/**"; then :; else echo NO_MATCH; fi' returned NO_MATCH, and tests/DCoding.Data.DVault.Tests/Unit/DataVaultEfMetadataTranslationTests.cs still asserts exactly 3 translated entity types.
- tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt contains DataVaultMetadataModel, IDataVaultNamingPolicy, DataVaultLogicalPropertyKind, DataVaultPropertyRole, and DataVaultTableKind entries but no PIT-facing public API yet.
- .gicket/relations contains parentOf relations from story 06EZ0NSXY2Y1JZ8SSCX177C770 to sibling 06EZ0NT4FDPC7XTQH40PQS942M, this ticket 06EZ0NTB26CCYQ7FCN2REEGDGW, and docs ticket 06EZ0NTJZEMVA5RPR01V0KNVMR, but no direct blocks relation between the sibling API ticket and this mapping ticket.

Blocking findings
- Direct repository evidence still shows zero PIT-facing input/output public surface in source or approved API. Without a durably refined sibling contract, this ticket still asks the developer to bridge that gap by assumption.

Required PO actions
- Refine sibling ticket 06EZ0NT4FDPC7XTQH40PQS942M into a durable delivery contract first, or copy the minimum consumed PIT input contract into this ticket so the dependency is concrete in persisted ticket evidence.
- If workflow depends on machine-readable sequencing, add the bounded dependency relation already mentioned in the current ticket follow-up instead of relying only on prose.

Open issues ledger
- critic-item-1 [required-po-action] Refine sibling ticket 06EZ0NT4FDPC7XTQH40PQS942M into a durable delivery contract first, or copy the minimum consumed PIT input contract into this ticket so the dependency is concrete in persisted ticket evidence.
- critic-item-2 [required-po-action] If workflow depends on machine-readable sequencing, add the bounded dependency relation already mentioned in the current ticket follow-up instead of relying only on prose.
- critic-item-3 [blocking-finding] Direct repository evidence still shows zero PIT-facing input/output public surface in source or approved API. Without a durably refined sibling contract, this ticket still asks the developer to bridge that gap by assumption.

Missing examples / edge cases
- Persisted ticket evidence still lacks one concrete example of the exact sibling-produced PIT metadata shape that this translator ticket consumes; the worked output baseline exists, but the consumed input contract is still only implied.
- Null-handling or carry-forward behavior for a PIT instant where one included satellite has no row is explicitly deferred, but there is still no persisted example showing how that deferral should constrain this ticket's baseline.

Risky assumptions
- That sibling revision 06EZ0Y4A07HWMD2X0AWTC704EM will not change PIT names or key semantics after this ticket is handed to dev.
- That translator-facing public API additions can be designed cleanly without a durably refined sibling input model.
- That prose-only sequencing is enough even though no direct blocks relation exists between sibling 06EZ0NT4FDPC7XTQH40PQS942M and this ticket in .gicket/relations.

AC / test suggestions
- After the sibling contract is durably refined, point this ticket's acceptance criteria at that exact persisted revision and keep Customer/Profile/Status as the canonical translator fixture.
- Retain the explicit acceptance criterion that ordinary hub/link/satellite translation stays unchanged when PIT metadata is absent; that is a strong regression guard.
- Keep the negative cases enumerated one-for-one in the ticket once the dependency is settled so reviewer and developer are testing the same out-of-contract shapes.

Implementation watchouts
- Do not let this ticket absorb PIT input/modeling API ownership that the contract assigns to sibling 06EZ0NT4FDPC7XTQH40PQS942M.
- Do not let developer handoff imply provider-specific SQL or provider-name branching is acceptable; the contract and current architecture both keep PIT translation provider-neutral.
- Any PIT-facing public surface addition still implies same-delivery API snapshot churn, which the current ticket already identifies as a risk.

Non-blocking notes
- The current ticket's own Open Questions section is none, so the block is dependency clarity rather than an unresolved question inside this ticket.
- Story-level parent relations already exist for the sibling API ticket, this mapping ticket, and the docs ticket under PIT story 06EZ0NSXY2Y1JZ8SSCX177C770.
- The latest PO refinement did resolve the earlier missing worked baseline and ownership split; the remaining issue is that the consumed sibling contract is still not durable.

Split recommendations
- No new functional split is needed.
- Keep the existing PIT story split, but do not move this ticket to dev before the sibling input-contract ticket is refined and unblocked.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment