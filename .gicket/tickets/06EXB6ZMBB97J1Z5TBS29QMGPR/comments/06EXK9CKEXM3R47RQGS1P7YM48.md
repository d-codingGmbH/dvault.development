[gicket-bot] PO refinement contract

Summary
- Verified the current ticket, comments, relations, attachments, and branch evidence. The latest PO-critic item is addressed by preserving the AddDVault-only delivery contract and re-handing off after label cleanup; no child tickets, relations, attachments, or planning documents were created.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - The required PO action is satisfied: the refined contract remains unchanged and AddDVault-only. The current ticket evidence shows status todo, automation/bot-ready present, and runtime-managed handoff labels should not be treated as blocking PO scope. The prior dev/test blocker concern is label cleanup only and does not change product scope.

Clarifications
- The AddDVault-only delivery contract is unchanged for this PO pass.
- The v1 startup surface for this ticket remains the source-backed AddDVault(IServiceCollection) extension in src/DVault/DVaultServiceCollectionExtensions.cs.
- The smoke test must verify the optionless AddDVault path and must not require or imply UseDataVault, EF provider integration, a consuming DbContext, or DbContext-specific startup behavior.
- The legacy draft line about creating a small consuming DbContext remains superseded by the delivery contract.
- tests/DVault.Tests is the existing DVault test area for this ticket, with DVault.Tests.csproj plus Modeling, Unit, Integration, and Shared test areas visible in branch evidence.
- No child tickets, outgoing relations, attachments, or planning documents were created in this pass. Existing relation context remains one incoming parentOf relation from 06EXB6Z3YMAPSRYRB8NQX3ZST4.

Scope In
- Add one self-contained smoke test under the existing tests/DVault.Tests structure for the default optionless AddDVault startup path.
- Use the branch's existing .NET test style to create a minimal ServiceCollection or equivalent startup path and call AddDVault with no DVault-specific configuration.
- Assert observable public startup success: AddDVault returns the same IServiceCollection, the service provider can be built, and provider-neutral defaults registered by AddDVault can be resolved.
- Keep the test deterministic and free of external databases, network services, and machine-specific infrastructure.

Scope Out
- Creating or renaming solution files, source projects, package metadata, repository build configuration, or test scaffolding.
- Adding a new public startup API as part of this ticket.
- Creating, requiring, or testing UseDataVault, EF provider integration, a consuming DbContext, or DbContext-specific startup behavior.
- Provider-matrix coverage, broad configuration regression testing, SQLite integration behavior, and external database integration tests.

Open questions
- none

Follow-up questions
- Decide separately whether DVault needs a public UseDataVault, EF provider integration, or DbContext-specific startup API and create a dedicated implementation ticket if so.
- After the AddDVault smoke test lands, decide separately whether provider-specific startup coverage or broader configuration regression tests are needed.

Risks
- A test that asserts private DI registration mechanics instead of public startup success may become brittle.
- Because the repository contains multiple DVault test layouts, the developer must place the smoke test where the branch's normal test command will actually execute it.

Split recommendations
- No prerequisite setup split is required for the current narrowed AddDVault smoke-test scope because the branch contains the source project, source-backed AddDVault entry point, and DVault test structure.
- Create a separate follow-up ticket only if UseDataVault, EF-specific startup wiring, a consuming DbContext path, provider integration, or a new public startup surface becomes required.

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