[gicket-bot] PO refinement contract

Summary
- Refined the PIT EF mapping ticket by pinning the sibling input-vs-projection ownership split, adding a worked one-hub/two-satellite baseline, and documenting the current hub/link/satellite-only repository surfaces the implementation must extend.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - The contract now names sibling ticket 06EZ0NT4FDPC7XTQH40PQS942M revision 06EZ0Y4A07HWMD2X0AWTC704EM as the consumed PIT input contract and splits ownership explicitly: the sibling owns PIT input/modeling surfaces and first-pass validation, while this ticket owns EF translation and any PIT output-side public snapshot changes required by translation.
- critic-item-2: `answered` - The contract now includes a worked baseline fixture for hub Customer plus satellites Profile and Status: expected PIT table PitCustomerProfileStatus, ordered columns CustomerHashKey, LoadTimestamp, ProfileLoadTimestamp, StatusLoadTimestamp, primary key PkPitCustomerProfileStatusCustomerHashKeyLoadTimestamp over (CustomerHashKey, LoadTimestamp), and deterministic failure cases for empty satellite sets, duplicate satellite references, satellites not attached to the hub, link-based PIT, and multi-active satellite shapes.
- critic-item-3: `answered` - The dependency blocker is now made explicit instead of implicit invention: this ticket is sequenced behind sibling ticket 06EZ0NT4FDPC7XTQH40PQS942M for PIT input/public-model work and owns only translation/output work. The sibling still being needs-po remains a delivery risk, but the current ticket contract no longer asks the developer to invent cross-ticket PIT metadata, naming, or key semantics.
- critic-item-4: `answered` - Because the repository currently exposes only hub/link/satellite PIT-adjacent surfaces, the contract now pins the missing public-surface split: sibling 06EZ0NT4FDPC7XTQH40PQS942M owns new PIT input/modeling and naming APIs, while this ticket owns any translation-facing output additions such as a PIT DataVaultTableKind member, PIT property/logical kinds, annotations, and provider type mappings if they are required to project PIT entities and update snapshots.

Clarifications
- This ticket now consumes sibling task 06EZ0NT4FDPC7XTQH40PQS942M revision 06EZ0Y4A07HWMD2X0AWTC704EM only for the PIT input contract: one hub, one or more attached satellites, deterministic names/key fields, and deterministic rejection of missing hub references, empty satellite sets, and duplicate satellite references.
- Sibling task 06EZ0NT4FDPC7XTQH40PQS942M owns PIT input-side public surfaces such as new modeling types, DataVaultMetadataModel aggregate changes, and any IDataVaultNamingPolicy or naming-context additions needed to express PIT metadata.
- This ticket owns PIT output-side translation surfaces: DataVaultEfMetadataTranslator and ApplyDataVaultMetadata() behavior, PIT EF entity projection, SQLite/queryability proof, and any public produced-model or snapshot changes needed to expose translated PIT entities.
- The worked baseline fixture for this ticket is hub Customer plus satellites Profile and Status, with satellite snapshot reference columns emitted in the same declaration order as the sibling PIT metadata.
- Live relation evidence shows the sibling API task, this mapping task, and docs task all sit under PIT story 06EZ0NSXY2Y1JZ8SSCX177C770 via existing parentOf relations; no child tickets, planning documents, attachments, or relation writes were materialized in this pass.

Scope In
- Translate the sibling-approved PIT metadata for one hub and one or more attached satellites into one provider-neutral shared-type EF entity through ApplyDataVaultMetadata().
- Emit deterministic PIT table, property, key, annotation, and SQLite-baseline schema/queryability behavior for the baseline one-hub plus attached-satellite shape.
- Add or update PIT output-side public surfaces required by translation, such as a PIT entity kind, PIT property/logical kinds, annotations, and provider type mappings, with snapshot coverage in the same delivery if those surfaces become public.
- Add positive and negative PIT translation coverage that proves deterministic ordering and deterministic rejection of out-of-contract shapes.

Scope Out
- Defining or revising PIT input-side modeling types, builder API entry points, naming-policy override APIs, or first-pass validation semantics beyond the sibling contract owned by 06EZ0NT4FDPC7XTQH40PQS942M.
- PIT refresh or population orchestration, PIT query helpers, migrations, provider-specific SQL, or runtime optimization work.
- README or docs/example authoring, which remains in sibling ticket 06EZ0NTJZEMVA5RPR01V0KNVMR.
- Link-based PIT tables, multi-active satellite PIT behavior, and broader deferred-capability expansion beyond the one-hub plus attached-satellite baseline.

Open questions
- none

Follow-up questions
- Should a later PIT population/read-optimization ticket define null-handling or carry-forward semantics when one included satellite has no row at a PIT instant, or is that entirely deferred until PIT materialization work begins?
- If later PIT work needs link-based PIT tables or multi-active satellite snapshots, should those land as separate follow-up tickets instead of widening this baseline contract?
- If the team wants machine-enforced ticket sequencing in live relations, should a bounded blocks relation from 06EZ0NT4FDPC7XTQH40PQS942M to 06EZ0NTB26CCYQ7FCN2REEGDGW be materialized after the sibling contract is stabilized?

Risks
- PIT output projection may require public enum, annotation, logical-property-kind, or provider type-mapping additions that ripple through multiple existing translator and snapshot tests.
- PIT scope can sprawl into refresh/materialization behavior or provider-specific optimization unless the one-hub plus attached-satellite projection boundary stays enforced.

Split recommendations
- No new functional split is recommended; keep the current PIT story split of metadata API, EF mapping, and docs/example work.
- If workflow clarity needs stronger live dependency signaling, add a bounded blocks relation from 06EZ0NT4FDPC7XTQH40PQS942M to 06EZ0NTB26CCYQ7FCN2REEGDGW in a later ticket-operation pass.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 5
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment