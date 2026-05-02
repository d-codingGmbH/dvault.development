[gicket-bot] PO refinement contract

Summary
- Ratified the shared two-event customer-profile comparison contract and the existing explicit DVault save-service baseline; no split, relation write, or new planning artifact was needed.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- docs/plans/customer-profile-comparison-contract-06EXB7RY-06EXB7S6.md is already the authoritative shared scenario for this ticket and paired plain-EF ticket 06EXB7RYFJ3YQDB1E4QHPP8034.
- The scenario is fixed to customer business key C-100 with event 1 at <redacted>-29T10:15:00Z from crm-import and event 2 at <redacted>-29T11:30:00Z from crm-change.
- Customer identity belongs in one customer hub and descriptive profile history belongs in one customer profile satellite; customer_name and customer_status stay out of the hub per the MVP Data Vault concepts.
- The repository already fixes the v1 write path to the explicit IDataVaultSaveService/DataVaultSaveRequest boundary, including satellite operations and latest-hash-diff historization behavior, so this ticket should reuse that baseline rather than introduce a new persistence path.
- Minimal v1 delivery for this ticket is automated SQLite coverage in tests/DCoding.Data.DVault.Tests using the existing AddDVault()/ApplyDataVaultMetadata pattern, not a separate sample app, new options surface, or planning artifact.
- No child tickets, relation writes, attachment writes, or new planning documents were materialized in this refinement run.

Scope In
- Implement the DVault-backed customer-profile comparison scenario on the existing SQLite test baseline using the current DVault metadata and explicit save-service path.
- Persist one customer hub identity for business key C-100 and customer profile satellite history for customer_name and customer_status.
- Execute the exact two shared business events from the comparison contract and assert the persisted DVault outcome after both events.
- Add or update automated tests under tests/DCoding.Data.DVault.Tests so the scenario runs in the current solution layout.
- Keep the scenario minimal and comparison-focused by using existing repository conventions, naming policy, and persistence behavior.

Scope Out
- Re-implementing plain EF baseline behavior already covered by ticket 06EXB7RYFJ3YQDB1E4QHPP8034.
- New SaveChanges interception, alternate write APIs, or hidden parent-resolution behavior beyond the current explicit save-service contract.
- PIT tables, bridge tables, multi-active satellites, provider-specific optimizations, or other deferred post-MVP Data Vault capabilities.
- A broader order or link demo, extra replay or deduplication variants beyond the locked two-event scenario, or a standalone runnable example application.

Open questions
- none

Follow-up questions
- After both comparison tickets are complete, should the customer-profile scenario also be promoted into a runnable examples or documentation sample instead of remaining test-first?
- Once more comparison scenarios exist, should shared fixtures or assertion helpers be introduced so the plain-EF and DVault baselines stay synchronized in code as well as in the planning document?

Risks
- Comparison value drops if the DVault scenario drifts from the locked two-event contract or introduces extra business events, extra replay behavior, or additional persisted rows beyond the agreed baseline.
- The current v1 save-service contract expects caller-supplied ParentHashKey and HashDiff inputs, so ad hoc test helpers could accidentally expand scope or hide the explicit boundary if they start deriving behavior not required by this ticket.
- If future stakeholders interpret 'example' as a standalone runnable sample rather than the current test-based comparison baseline, scope pressure could grow unless the ticket keeps the v1 example surface explicitly minimal.

Split recommendations
- No split recommended; current evidence supports one bounded ticket focused on the automated two-event DVault customer-profile comparison scenario.
- If a standalone runnable example or broader relationship demo is later desired, schedule it as a separate follow-up ticket instead of widening this one.

Persisted contract coverage
- acceptance-criteria items: 7
- definition-of-done items: 4
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment