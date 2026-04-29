[gicket-bot] PO refinement contract

Summary
- Refined the story as a product-level minimal-configuration principle using repository and relation evidence. Completed child work already defines the default convention policy (06EXB6QNB799DKQHRAZ5BY38H0) and optional advanced configuration hooks (06EXB6QX6JJX9H7CZT3YAXSAD4), so this ticket can ratify the convention-first baseline for downstream entry-point work.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The v1 product principle is convention-first: <redacted> DVault usage must work without custom configuration, and configuration exists to override or extend defaults only when users need advanced behavior.
- Existing repository evidence already fixes the bounded baseline: AddDVault registers provider-neutral defaults, DataVaultConventions.Default exposes default concepts and logical object names, DataVaultModel.Create accepts optional options, and the planning docs define deterministic naming, persistence, hashing, timestamps, record source, and provider behavior defaults.
- Completed child ticket 06EXB6QNB799DKQHRAZ5BY38H0 produced the default convention policy, and completed child ticket 06EXB6QX6JJX9H7CZT3YAXSAD4 produced the optional advanced configuration hook plan; this story should consolidate those outcomes rather than reopen them.
- Downstream story 06EXB6Z3YMAPSRYRB8NQX3ZST4 should use this principle when refining public entry points, but implementation API shape belongs there rather than in this product-principle ticket.

Scope In
- Define the minimal-configuration principle as a first-class product constraint for DVault v1 developer experience.
- Ratify the zero-configuration default path: users should be able to register DVault defaults and build a simple model without selecting providers, naming policies, hash services, timestamp behavior, or configuration files up front.
- Require defaults to be deterministic across machines, processes, cultures, time zones, providers, and repeated runs unless a later versioned contract explicitly changes them.
- Preserve advanced configuration hooks as optional additive overrides for naming, hashing, record source resolution, timestamp sourcing and formatting, and provider behavior.
- Tie the principle to quickstart expectations: the first-use path should be short, convention-led, and focused on the user's model rather than framework setup.

Scope Out
- Implementing new runtime APIs, persistence providers, schema generation, migrations, hashing code, or provider-specific configuration behavior.
- Renaming established default policy artifacts or reopening completed decisions from the default convention policy, optional hook plan, naming policy, or stable hashing contract.
- Choosing exact method names, parameter names, helper class names, or file layout details for downstream implementation tickets.
- Adding PIT tables, bridge tables, multi-active satellites, provider-specific optimizations, or other deferred Data Vault capabilities to the MVP default path.
- Workflow metadata changes, status transitions, labels, board configuration, or runtime handoff mechanics.

Open questions
- none

Follow-up questions
- When downstream public entry-point story 06EXB6Z3YMAPSRYRB8NQX3ZST4 is refined, what exact quickstart example should become the canonical minimal path?
- After provider priorities are known, which provider-specific options deserve separate tickets without weakening the provider-neutral default path?
- Should a later documentation ticket create a dedicated user-facing quickstart page that demonstrates AddDVault and optionless model creation against the accepted v1 defaults?

Risks
- If downstream API work adds required setup before users can build a basic vault model, it will violate this principle even if the underlying configuration hooks remain technically optional.
- If examples lead with advanced options, users may perceive configuration as required; quickstart material should keep options after the default path.
- The repository currently contains both reserved DCoding.Data.DVault layout references and visible src/DVault implementation files; downstream implementation tickets should follow the current branch evidence while avoiding unrelated layout churn.

Split recommendations
- No new split is recommended for this story. The already-completed child tickets for default conventions and optional advanced hooks cover the major planning subdivisions, and the remaining public entry-point work is already represented by downstream story 06EXB6Z3YMAPSRYRB8NQX3ZST4.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 5
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment