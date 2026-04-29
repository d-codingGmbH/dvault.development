<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the PO contract against the critic ledger by removing the hidden source/test scaffold assumption, declaring a foundation-order dependency, and pinning objective v1 default effective column names.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- This ticket defines reusable Data Vault technical metadata column contracts for exactly four v1 roles: hash key, hash diff, load timestamp, and record source.
- The explicit v1 default effective column names are HashKey, HashDiff, LoadTimestamp, and RecordSource.
- Names are overrideable per contract instance, but overriding a name must not change the metadata role identity.
- Implementation is ordered after the foundation/project scaffolding work that creates the solution, src/DVault library project, and tests/DVault.Tests test project. This ticket does not create those scaffold files as hidden scope.
- If the source/test scaffold is not present when this ticket reaches development, the deliverable may be limited to an approved planning/documentation artifact that preserves the contract until the foundation scaffold is available.

### Scope In
- Define a consistent contract shape for hash key, hash diff, load timestamp, and record source metadata columns.
- Encode or document each role's semantic purpose, requiredness expectation, default effective column name, and override behavior.
- Use one shared representation suitable for reuse by hubs, links, and satellites without creating parallel role definitions per structure.
- When the foundation test project exists, add focused tests covering the default contract set and one explicit override for each role.

### Scope Out
- Creating the solution file, project files, src/DVault scaffold, or tests/DVault.Tests scaffold before the foundation setup ticket provides them.
- Generating physical database DDL or migration scripts.
- Implementing complete hub, link, satellite, PIT, or bridge modeling behavior beyond what is needed to consume the metadata contracts.
- Defining organization-wide naming policy, SQL-provider casing, or target-specific physical naming beyond the four v1 defaults.
- Changing workflow columns, ticket metadata, automation labels, or other runtime orchestration state.

## Acceptance Criteria
- Hash key, hash diff, load timestamp, and record source metadata columns are represented through a consistent reusable contract shape.
- Each contract exposes the metadata role, the role's default effective column name, and the current effective column name after optional override.
- The default effective column names are exactly HashKey for hash key, HashDiff for hash diff, LoadTimestamp for load timestamp, and RecordSource for record source.
- Override behavior preserves the metadata role and default name while changing only the effective column name used by consumers.
- The contract can be reused by downstream hub, link, and satellite modeling work without duplicating incompatible metadata definitions.
- When tests/DVault.Tests exists, automated tests verify the default contract set and at least one explicit override for each metadata role; before that scaffold exists, the planning/documentation artifact must state these same verifiable cases.

## Definition of Done
- The shared technical metadata column contract is implemented in the DVault source project after foundation scaffolding exists, or documented as a bounded planning artifact if implementation is blocked by missing project scaffolding.
- Verification covers role identity, explicit v1 defaults, and override behavior through automated tests when the test project exists, or through equivalent documented acceptance cases while awaiting the scaffold.
- No solution/project scaffold, unrelated modeling feature, broad naming-policy rewrite, or database-specific DDL behavior is included in this ticket.
- The handoff material clearly states the foundation-order dependency and the four explicit default effective column names.

## Implementation Notes
- Use namespace DCoding.Data.DVault when implementation files exist, matching the referenced extension-shape planning document.
- Expected eventual locations are src/DVault for source and tests/DVault.Tests for tests, but these are dependency targets rather than evidence of an existing layout on this branch.
- Do not create .slnx/.csproj/source/test scaffold in this ticket unless the foundation setup work has already landed before development starts.
- Prefer a small role model plus naming metadata over separate ad hoc classes per vault structure.
- The role set is closed for v1: HashKey, HashDiff, LoadTimestamp, and RecordSource. API/member naming may follow local C# conventions, but observable default effective column names must be HashKey, HashDiff, LoadTimestamp, and RecordSource.
- Later convention-policy tickets may map these logical/effective names to provider-specific physical casing, but that is downstream work and does not reopen this ticket's v1 defaults.

## Open Questions
- none

## Follow-Up Questions
- Which foundation/project setup ticket should carry the explicit dependency relation for creating the solution, src/DVault, and tests/DVault.Tests if relation materialization is required by the board?
- Should future tickets add technical metadata roles such as end date, current flag, deleted flag, tenant id, or run id?
- Should a later convention-policy ticket formalize provider-specific physical casing for SQL targets?
- Should downstream modeling tickets expose structure-specific aliases for hash keys, such as hub or link key naming patterns?

## Risks
- Development remains blocked until the foundation source/test scaffold exists or the workflow explicitly accepts a documentation-only artifact for this ticket.
- A later convention-policy ticket may introduce provider-specific casing rules, but it should adapt from these logical v1 defaults rather than block the current contract.
- If downstream vault-structure tickets assume different metadata roles or defaults, they will need to align with this shared baseline instead of creating parallel definitions.

## Split Recommendations
- No child split is needed for the four-role contract itself. Keep project/test scaffolding in the separate foundation setup work and relate this ticket to that dependency when the exact ticket id is available.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

## Summary
Define contracts for hash keys, hash diffs, load timestamps, and record source.

## Scope
- Keep default column names conventional and overrideable.

## Acceptance Criteria
- Technical columns are represented consistently for all vault structures.
- Defaults align with the convention policy.

## Definition of Done
- The work satisfies the acceptance criteria.
- Shared standards from the charter attachment are followed.