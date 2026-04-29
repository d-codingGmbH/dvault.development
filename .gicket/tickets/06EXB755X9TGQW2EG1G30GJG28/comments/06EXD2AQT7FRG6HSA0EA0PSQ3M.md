[gicket-bot] PO refinement contract

Summary
- Refined the PO contract against the critic ledger by removing the hidden source/test scaffold assumption, declaring a foundation-order dependency, and pinning objective v1 default effective column names.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - The corrected contract declares that this ticket depends on the separate foundation work that creates the solution, main DVault source project, and DVault test project. It does not include creating .slnx/.csproj/src/test scaffolding in this ticket; implementation should proceed only after that scaffold exists.
- critic-item-2: `answered` - The v1 default effective column names are now explicit: hash key = HashKey, hash diff = HashDiff, load timestamp = LoadTimestamp, and record source = RecordSource. Developers may choose API/member names, but the observable contract defaults must match these values unless explicitly overridden.
- critic-item-3: `answered` - Developer routing is corrected: this ticket is ready for PO-critic, but implementation is ordered after the foundation source/test scaffold exists. Acceptance checks now require explicit defaults and override behavior; automated tests are required only when the test project exists, otherwise an approved planning/documentation artifact may carry the contract until scaffold completion.
- critic-item-4: `answered` - The hidden scaffolding scope is removed. The contract must no longer say src/DVault and tests/DVault.Tests are an existing layout. They are the intended eventual implementation locations after foundation setup, and creating that structure is out of scope for this ticket unless a foundation ticket has already provided it.
- critic-item-5: `answered` - The default-name ambiguity is resolved by making the four physical/effective default names part of this ticket's acceptance contract: HashKey, HashDiff, LoadTimestamp, and RecordSource. Later naming-policy work may change casing or target-specific physical rendering, but it does not block this ticket's v1 defaults.

Clarifications
- This ticket defines reusable Data Vault technical metadata column contracts for exactly four v1 roles: hash key, hash diff, load timestamp, and record source.
- The explicit v1 default effective column names are HashKey, HashDiff, LoadTimestamp, and RecordSource.
- Names are overrideable per contract instance, but overriding a name must not change the metadata role identity.
- Implementation is ordered after the foundation/project scaffolding work that creates the solution, src/DVault library project, and tests/DVault.Tests test project. This ticket does not create those scaffold files as hidden scope.
- If the source/test scaffold is not present when this ticket reaches development, the deliverable may be limited to an approved planning/documentation artifact that preserves the contract until the foundation scaffold is available.

Scope In
- Define a consistent contract shape for hash key, hash diff, load timestamp, and record source metadata columns.
- Encode or document each role's semantic purpose, requiredness expectation, default effective column name, and override behavior.
- Use one shared representation suitable for reuse by hubs, links, and satellites without creating parallel role definitions per structure.
- When the foundation test project exists, add focused tests covering the default contract set and one explicit override for each role.

Scope Out
- Creating the solution file, project files, src/DVault scaffold, or tests/DVault.Tests scaffold before the foundation setup ticket provides them.
- Generating physical database DDL or migration scripts.
- Implementing complete hub, link, satellite, PIT, or bridge modeling behavior beyond what is needed to consume the metadata contracts.
- Defining organization-wide naming policy, SQL-provider casing, or target-specific physical naming beyond the four v1 defaults.
- Changing workflow columns, ticket metadata, automation labels, or other runtime orchestration state.

Open questions
- none

Follow-up questions
- Which foundation/project setup ticket should carry the explicit dependency relation for creating the solution, src/DVault, and tests/DVault.Tests if relation materialization is required by the board?
- Should future tickets add technical metadata roles such as end date, current flag, deleted flag, tenant id, or run id?
- Should a later convention-policy ticket formalize provider-specific physical casing for SQL targets?
- Should downstream modeling tickets expose structure-specific aliases for hash keys, such as hub or link key naming patterns?

Risks
- Development remains blocked until the foundation source/test scaffold exists or the workflow explicitly accepts a documentation-only artifact for this ticket.
- A later convention-policy ticket may introduce provider-specific casing rules, but it should adapt from these logical v1 defaults rather than block the current contract.
- If downstream vault-structure tickets assume different metadata roles or defaults, they will need to align with this shared baseline instead of creating parallel definitions.

Split recommendations
- No child split is needed for the four-role contract itself. Keep project/test scaffolding in the separate foundation setup work and relate this ticket to that dependency when the exact ticket id is available.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 4
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment