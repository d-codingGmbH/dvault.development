[gicket-bot] PO refinement contract

Summary
- Ticket requires additional substantive product clarification before continuing.

PO handoff
- decision: `needs_po_clarification`
- meaning: ticket needs more clarification before PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - The PO handoff evidence is reconciled by no longer claiming label cleanup was completed. The latest persisted ticket read shows blocked/dev and blocked/test still present alongside needs-po and automation/bot-ready, so the correct product-owner contract is that the AddDVault smoke-test scope remains refined but the metadata cleanup claim is not true in the live ticket state.

Clarifications
- The AddDVault-only delivery contract remains the intended implementation scope, but the previous PO Summary statement that label cleanup had completed is not supported by the latest persisted ticket read.
- The v1 startup surface for this ticket remains AddDVault(IServiceCollection) in src/DVault/DVaultServiceCollectionExtensions.cs.
- The smoke test must verify the optionless AddDVault path and must not require UseDataVault, EF provider integration, a consuming DbContext, or DbContext-specific startup behavior.
- tests/DVault.Tests is the existing DVault test area for this ticket, with DVault.Tests.csproj plus Modeling, Unit, Integration, and Shared test areas visible in branch evidence.
- No child tickets, outgoing relations, attachments, or planning documents were created in this pass.

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
- Keep labels unchanged.
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment