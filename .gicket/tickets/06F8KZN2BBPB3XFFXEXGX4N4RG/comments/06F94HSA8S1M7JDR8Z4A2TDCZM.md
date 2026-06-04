[gicket-bot] PO refinement contract

Summary
- Refined the provider identifier preflight story against the landed guardrail contract and visible repository baselines; no child tickets, relation writes, description writes, attachments, or planning documents were materialized.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The predecessor contract ticket 06F8KZMRXRHRKHV56Y96M4S90G is done, so this story should implement the already-ratified provider identifier and DDL guardrail contract rather than reopen supported-provider or naming-policy decisions.
- The supported-provider baseline for this story is the finite repository set already named in the contract: Sqlite, Oracle, Postgres, SqlServer, and MySql.
- The provider-neutral logical naming baseline remains the existing default naming policy; this story only adds provider-aware preflight validation and deterministic physical-name safety handling where the contract allows it.
- The ticket snapshot shows no recent human comments, and no bounded child-ticket, relation, attachment, or planning-document writes were needed for this refinement pass.
- Broader provider-specific migration guardrail expansion stays in the separate downstream story 06F8KZNBGB8FPW6TK5A8SAJMVC and is not absorbed here.

Scope In
- Preflight validation for generated table, column, index, key, and constraint names before DVault-owned schema generation or migration DDL emits unsafe provider-specific identifiers.
- Consumption of provider capability/profile facts needed for identifier safety, including identifier-length limits where declared, reserved-word handling, duplicate produced-name detection, and post-truncation collision handling.
- Deterministic diagnostics and guardrail reporting that identify the provider profile, logical or produced name, affected artifact kind, and failure class when a generated name is unsafe or unsupported.
- Validation of unsafe naming-policy and provider combinations when the selected logical naming output cannot be projected to a stable provider-safe physical name within the v1 contract.
- Coverage for the finite supported-provider baseline already visible in the repository and contract.

Scope Out
- Changing the provider-neutral logical naming rules in docs/naming/default-naming-policy.md or the v1 persistence token set.
- Automatic repair or execution of consumer-authored migrations, raw SQL, or arbitrary third-party DDL.
- Broader provider-specific migration guardrails for destructive changes, nullable timestamp behavior, included-column policy, or non-identifier migration risks already scoped to the separate migration-guardrail story.
- Adding new provider packages, open-ended vendor keyword research, or a broad new public override surface for custom physical naming.

Open questions
- none

Follow-up questions
- Should a later ticket add a consumer-visible override hook for provider-specific shortening or quoting, or should v1 stay convention-only?
- After this story lands, do we want separate maintenance coverage to detect provider-package drift in reserved-word sets or identifier-limit facts?

Risks
- Provider upgrades can change reserved words or identifier behavior, so the finite contract baseline may need maintenance follow-up when dependencies move.
- If deterministic shortening or collision rules change after implementation, existing generated schemas and migration artifacts could churn.
- Fail-fast validation may expose previously hidden unsafe names in existing models, which is correct but may need rollout communication for consumers.

Split recommendations
- No PO split is required for this story; keep broader migration-risk work in 06F8KZNBGB8FPW6TK5A8SAJMVC rather than widening this ticket.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 5
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment