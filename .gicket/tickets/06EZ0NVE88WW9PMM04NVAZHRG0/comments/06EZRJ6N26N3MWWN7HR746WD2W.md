[gicket-bot] PO-critic review contract

Summary
- Ticket contract requires substantive product-owner changes before development.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06EZ0NVE88WW9PMM04NVAZHRG0/description.md:27-30 says this docs child is blocked until parent story 06EZ0NTV4SVAKV98C418T8A3CC leaves `needs-po` and its ticket contract becomes authoritative.
- .gicket/tickets/06EZ0NTV4SVAKV98C418T8A3CC/description.md:1-11 is still only Goal/Scope/Acceptance Criteria prose, not a refined delivery-contract block for bridge naming/shape.
- .gicket/tickets/06EZ0NVE88WW9PMM04NVAZHRG0/ticket.json:7-18 still carries `blocked/dev`, `blocked/test`, and `critic-needed`.
- .gicket/relations/CC/G0/06EZ0NTV4SVAKV98C418T8A3CC--06EZ0NVE88WW9PMM04NVAZHRG0--parentOf.json confirms the parent-child dependency.
- docs/plans/deferred-data-vault-capabilities.md:37,65,90 names 06EZ0NTV4SVAKV98C418T8A3CC as the downstream bridge owner and lists generated bridge tables as unsupported in the current baseline.
- src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs:29-40 only enumerates hubs, links, and satellites, and src/DCoding.Data.DVault/DataVaultAnnotationNames.cs:6-70 defines no bridge-specific annotation names.
- Comment .gicket/tickets/06EZ0NVE88WW9PMM04NVAZHRG0/comments/06EZRG30XQ7HQX5NWA5RMWWZMG.md says the child is not dev-ready until the parent contract becomes authoritative.
- On branch `ticket/06EZ0NVE88WW9PMM04NVAZHRG0-task-add-bridge-documentation-and-example-scenar`, `git diff --stat 80225174e425e0b7ac625c708848150faa0319fa..HEAD` returned no output, so no newer branch evidence resolved the upstream block.

Blocking findings
- The child's own acceptance criteria make parent refinement a prerequisite for dev handoff, but parent story 06EZ0NTV4SVAKV98C418T8A3CC is still `needs-po`; this ticket is not ready for developer handoff.
- There is still no authoritative persisted or source-backed bridge surface to document; starting now would force the docs example to guess bridge names or shapes.

Required PO actions
- After the parent contract is refined, re-check this child against that contract and refresh the child if the parent introduces concrete bridge naming or shape details before resubmitting to PO-critic.

Open issues ledger
- critic-item-1 [required-po-action] After the parent contract is refined, re-check this child against that contract and refresh the child if the parent introduces concrete bridge naming or shape details before resubmitting to PO-critic.
- critic-item-2 [blocking-finding] The child's own acceptance criteria make parent refinement a prerequisite for dev handoff, but parent story 06EZ0NTV4SVAKV98C418T8A3CC is still `needs-po`; this ticket is not ready for developer handoff.
- critic-item-3 [blocking-finding] There is still no authoritative persisted or source-backed bridge surface to document; starting now would force the docs example to guess bridge names or shapes.

Missing examples / edge cases
- No extra worked example is missing inside the current bounded scope; hierarchy traversal remains intentionally deferred and should stay out of this child until a follow-up ticket exists.
- The eventual many-to-many example still lacks exact bridge surface names because the parent contract has not defined them yet.

Risky assumptions
- Assuming parent story 06EZ0NTV4SVAKV98C418T8A3CC can become authoritative without requiring a follow-up sync on this child, even though the child risk section says one more sync pass may be needed before dev handoff.

AC / test suggestions
- When parent 06EZ0NTV4SVAKV98C418T8A3CC is refined, add a ticket-level check that the many-to-many example cites the exact bridge names/shape from that parent contract and still excludes hierarchy semantics.
- Keep an explicit acceptance check that bridge docs stay provider-neutral and mark hierarchy, PIT, and multi-active implications as deferred.

Implementation watchouts
- Do not invent bridge API, type, annotation, or table names from architecture prose; current repo evidence only proves hub/link/satellite surfaces.
- Keep the docs vocabulary anchored to `AddDVault()`, `UseDataVault()`, `ApplyDataVaultMetadata()`, and `IDataVaultSaveService` until the parent contract publishes concrete bridge surface details.

Non-blocking notes
- The latest PO refinement did resolve the earlier ticket-level ambiguity: `## Open Questions` is `- none`, the authoritative artifact is explicitly the parent ticket contract, and the worked example is narrowed to many-to-many only.
- No split is needed right now if the child stays blocked on the parent bridge contract.

Split recommendations
- No split recommended while this remains a bounded docs child blocked on parent story 06EZ0NTV4SVAKV98C418T8A3CC.
- If hierarchy-style traversal later needs its own worked example, create a separate follow-up docs ticket after the parent bridge surface is defined.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment