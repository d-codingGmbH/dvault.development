<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Ratified the shared two-event customer-profile comparison contract and the existing explicit DVault save-service baseline; no split, relation write, or new planning artifact was needed.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- docs/plans/customer-profile-comparison-contract-06EXB7RY-06EXB7S6.md is already the authoritative shared scenario for this ticket and paired plain-EF ticket 06EXB7RYFJ3YQDB1E4QHPP8034.
- The scenario is fixed to customer business key C-100 with event 1 at 2026-04-29T10:15:00Z from crm-import and event 2 at 2026-04-29T11:30:00Z from crm-change.
- Customer identity belongs in one customer hub and descriptive profile history belongs in one customer profile satellite; customer_name and customer_status stay out of the hub per the MVP Data Vault concepts.
- The repository already fixes the v1 write path to the explicit IDataVaultSaveService/DataVaultSaveRequest boundary, including satellite operations and latest-hash-diff historization behavior, so this ticket should reuse that baseline rather than introduce a new persistence path.
- Minimal v1 delivery for this ticket is automated SQLite coverage in tests/DCoding.Data.DVault.Tests using the existing AddDVault()/ApplyDataVaultMetadata pattern, not a separate sample app, new options surface, or planning artifact.
- No child tickets, relation writes, attachment writes, or new planning documents were materialized in this refinement run.

### Scope In
- Implement the DVault-backed customer-profile comparison scenario on the existing SQLite test baseline using the current DVault metadata and explicit save-service path.
- Persist one customer hub identity for business key C-100 and customer profile satellite history for customer_name and customer_status.
- Execute the exact two shared business events from the comparison contract and assert the persisted DVault outcome after both events.
- Add or update automated tests under tests/DCoding.Data.DVault.Tests so the scenario runs in the current solution layout.
- Keep the scenario minimal and comparison-focused by using existing repository conventions, naming policy, and persistence behavior.

### Scope Out
- Re-implementing plain EF baseline behavior already covered by ticket 06EXB7RYFJ3YQDB1E4QHPP8034.
- New SaveChanges interception, alternate write APIs, or hidden parent-resolution behavior beyond the current explicit save-service contract.
- PIT tables, bridge tables, multi-active satellites, provider-specific optimizations, or other deferred post-MVP Data Vault capabilities.
- A broader order or link demo, extra replay or deduplication variants beyond the locked two-event scenario, or a standalone runnable example application.

## Acceptance Criteria
- A DVault-backed customer-profile scenario exists on the existing SQLite automated test baseline and uses the current explicit DVault configuration path without requiring a new options object or separate app.
- Using the shared two-event sequence for customer C-100 results in exactly 1 persisted customer hub row.
- Using the same two-event sequence results in exactly 2 persisted customer profile satellite rows for that hub, ordered by load timestamp ascending.
- Satellite row 1 stores customer_name = Alice Adams, customer_status = prospect, load_timestamp = 2026-04-29T10:15:00Z, and record_source = crm-import.
- Satellite row 2 stores customer_name = Alice Baker, customer_status = active, load_timestamp = 2026-04-29T11:30:00Z, and record_source = crm-change.
- The second event creates a new satellite history row instead of overwriting the first state, and it does not insert an extra customer hub row.
- Automated assertions stay aligned with docs/plans/customer-profile-comparison-contract-06EXB7RY-06EXB7S6.md and the repository's current v1 Data Vault naming and persistence conventions.

## Definition of Done
- The acceptance criteria pass in automated coverage under the existing tests/DCoding.Data.DVault.Tests structure and are intended to run with the normal repository dotnet test flow.
- The implementation uses the repository's current explicit DVault save-service boundary and translated metadata conventions instead of introducing a separate scenario-specific persistence mechanism.
- The customer-profile scenario remains consistent with docs/plans/customer-profile-comparison-contract-06EXB7RY-06EXB7S6.md, docs/architecture/mvp-data-vault-concepts.md, and the shared implementation standards artifact.
- The delivery stays within the current SQLite-focused MVP boundary and does not widen into deferred Data Vault capabilities or a separate sample-app track.

## Implementation Notes
- Reuse the existing SQLite integration style already present in tests/DCoding.Data.DVault.Tests/Integration/ExplicitDataVaultSaveServiceSqliteTests.cs rather than introducing a new host or example project.
- Model the scenario as a customer hub plus a customer profile satellite attached to that hub; descriptive fields customer_name and customer_status belong in the satellite, not the hub.
- Because the current save-service contract requires explicit ParentHashKey on satellite operations, capture or compute the customer hub hash key through the existing explicit save path and reuse it for profile satellite writes; do not expand this ticket into hidden parent-derivation behavior.
- Use deterministic request-level load timestamps and record sources from the shared comparison contract for both events so the persisted outcome remains stable across repeated test runs.
- Use caller-supplied deterministic hash-diff placeholders for the two profile states if the test needs to drive satellite historization; do not widen this ticket into defining a new hash-diff algorithm.
- This ticket does not need to re-prove every unchanged or replay edge case already covered by the existing satellite historization baseline; it needs the exact two-event customer-profile comparison scenario.

## Open Questions
- none

## Follow-Up Questions
- After both comparison tickets are complete, should the customer-profile scenario also be promoted into a runnable examples or documentation sample instead of remaining test-first?
- Once more comparison scenarios exist, should shared fixtures or assertion helpers be introduced so the plain-EF and DVault baselines stay synchronized in code as well as in the planning document?

## Risks
- Comparison value drops if the DVault scenario drifts from the locked two-event contract or introduces extra business events, extra replay behavior, or additional persisted rows beyond the agreed baseline.
- The current v1 save-service contract expects caller-supplied ParentHashKey and HashDiff inputs, so ad hoc test helpers could accidentally expand scope or hide the explicit boundary if they start deriving behavior not required by this ticket.
- If future stakeholders interpret 'example' as a standalone runnable sample rather than the current test-based comparison baseline, scope pressure could grow unless the ticket keeps the v1 example surface explicitly minimal.

## Split Recommendations
- No split recommended; current evidence supports one bounded ticket focused on the automated two-event DVault customer-profile comparison scenario.
- If a standalone runnable example or broader relationship demo is later desired, schedule it as a separate follow-up ticket instead of widening this one.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

## Summary
Create the DVault-backed version of the customer profile scenario.

## Scope
- Demonstrate customer hub and profile satellite behavior.

## Acceptance Criteria
- Profile changes create expected satellite history.
- The example stays minimal to configure.

## Definition of Done
- The work satisfies the acceptance criteria.
- Shared standards from the charter attachment are followed.