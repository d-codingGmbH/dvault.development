[gicket-bot] PO refinement contract

Summary
- Repository and ticket evidence already ratify the deferred-capability architecture boundary, so this story can advance to PO critic without new splits or relation changes.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- `docs/plans/deferred-data-vault-capabilities.md` is already the governing decision record for this story's architecture stance, and child task `06EZ0NSHJVC9SD2KS6PWWNHPJM` is already done.
- Current source and docs fix the preserved v0.5 default baseline: hubs, links, satellites, deterministic naming and hashing, required record source lineage, UTC load-timestamp defaults, optionless `AddDVault()`, convention-first `UseDataVault()` and `ApplyDataVaultMetadata()`, explicit `IDataVaultSaveService`, and SQLite as the default capability profile.
- Deferred PIT, bridge, multi-active, and advanced-hook work is already decomposed into existing downstream tickets; no additional child tickets, relation writes, attachments, or planning documents were created in this refinement pass.
- Provider-specific optimization and storage behavior stay in provider packages and provider strategy or capability-profile contracts rather than in this core deferred-capability architecture story.

Scope In
- Ratify the architecture boundary that deferred capabilities extend around the current hub, link, and satellite baseline instead of changing default DVault setup.
- Define the release-level posture for PIT tables, bridge tables, multi-active satellites, and advanced hooks as opt-in capability families with deterministic inherited defaults when hooks are unset.
- Set public-surface guardrails: preserve the current visible public baseline and require any new deferred-capability or hook surface to pass API snapshot review or carry an explicit compatibility note before it is treated as stable.
- Identify which provider-specific behavior belongs in provider packages or provider save strategies rather than the core package.

Scope Out
- Implementing PIT, bridge, multi-active, or hook runtime behavior in product code.
- Finalizing concrete class names, method names, option object shapes, or complete public APIs for deferred capabilities in this story.
- Changing ordinary `AddDVault()`, `UseDataVault()`, `ApplyDataVaultMetadata()`, or `IDataVaultSaveService` baseline behavior to accommodate deferred features.
- Provider-specific DDL, native SQL, migrations, performance tuning, or option matrices beyond the existing provider-boundary documentation.

Open questions
- none

Follow-up questions
- When deferred hooks start implementation, which first-pass hook surfaces should stay internal or experimental until the API snapshot task confirms a stable public shape?
- Should repository discoverability later add a README link to the deferred-capability decision record, or is the current docs/plan placement sufficient for v0.5?
- After downstream stories land, does the older advanced-hooks planning note need to remain as a separate detailed reference or get narrowed to point primarily at the governing decision record?

Risks
- If downstream work treats the architecture story as approval for concrete PIT, bridge, multi-active, or hook APIs, the API snapshot boundary could be bypassed and compatibility expectations could drift.
- If provider-specific concerns leak back into the core architecture contract, provider packages may inherit commitments that the current repository evidence does not justify.
- If a future hook overrides more than its own category, deterministic defaults for naming, hashing, record source, or timestamp behavior could be destabilized across the baseline path.

Split recommendations
- No additional split is recommended. The existing decision-record child task, API snapshot child task, and PIT, bridge, multi-active, and hooks downstream stories already cover the bounded decomposition.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 4

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment