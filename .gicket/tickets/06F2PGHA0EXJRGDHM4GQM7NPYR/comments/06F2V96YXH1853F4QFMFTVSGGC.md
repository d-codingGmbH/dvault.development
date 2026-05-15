[gicket-bot] PO refinement contract

Summary
- Reframed the ticket as active pre-development documentation work rather than closure-only review, ratified the missing and stale v0.11.0 public-doc surfaces, and made the required five-path update set explicit for the next `po-critic -> dev` handoff.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - This ticket should follow the normal pre-development path. It is not a closure-only review; after PO-critic it should continue through `dev` as one bounded documentation rollout, with no child-ticket split required by the current evidence.
- critic-item-2: `answered` - Closure-only is not intentional, so commit-level closure proof is not the right contract for this pass. The contract now requires the implementation to land on the five named documentation paths and to record documentation-level verification evidence when those edits are completed.
- critic-item-3: `answered` - The contract now names the required path set explicitly: `docs/releases/v0.11.0.md`, `README.md`, `examples/README.md`, `docs/production-adoption-checklist.md`, and `docs/model-first-governance.md`.
- critic-item-4: `answered` - Confirmed. The repository still needs the documentation implementation: `docs/releases/v0.11.0.md` does not exist, and the current public docs still advertise the older `0.10.0` / SQLite-first baseline in places that are supposed to describe the latest release.
- critic-item-5: `answered` - Confirmed. Bounded branch inspection shows no documentation implementation or documentation-verification evidence on the target branch, so the ticket cannot be treated as already completed.
- critic-item-6: `answered` - Confirmed. Approving closure would require assuming unobserved README, examples, adoption, model-first, and release-note updates landed elsewhere, and the bounded repository inspection does not support that. The contract now requires those exact documentation surfaces to be updated in this ticket before closure is considered.

Clarifications
- This is an active implementation ticket, not a closure-only validation ticket; after PO-critic it should continue through the normal developer path.
- No child tickets, relation writes, attachments, or planning documents were materialized in this PO pass because the required work remains one bounded documentation rollout.
- The required doc-edit path set is exactly `docs/releases/v0.11.0.md`, `README.md`, `examples/README.md`, `docs/production-adoption-checklist.md`, and `docs/model-first-governance.md`.
- Use `docs/architecture/dvault-dotnet-ef-design-time-workflow.md` and `src/DCoding.Data.DVault/DataVaultLiveSchemaReader.cs` as repository-backed source-of-truth inputs for command names, design-time boundaries, and built-in provider-reader claims.
- Upstream implementation tickets remain completed inputs for this roll-up; this ticket documents shipped behavior and does not reopen their code scope.

Scope In
- Create `docs/releases/v0.11.0.md` as the authoritative public release summary for v0.11.0.
- Update `README.md` installation guidance, release-summary references, and current-baseline wording from `0.10.0` / older lifecycle language to the v0.11.0 baseline.
- Update `examples/README.md` package versions and current quickstart guidance to the v0.11.0 baseline.
- Update `docs/production-adoption-checklist.md` so the design-time and drift guidance matches the shipped v0.11.0 command surface and current live-schema support.
- Update `docs/model-first-governance.md` so its current-baseline language and linked workflow guidance align with v0.11.0 public documentation.
- Keep the five-path documentation set internally consistent on consumer-owned command-host wording, default artifact-versus-design-time-model drift gating, and opt-in live-schema checks.

Scope Out
- No product code, provider-reader implementation, diagnostics, CLI surface, or CI workflow behavior changes.
- No new runnable provider quickstart projects, secret-management recipes, or container-provisioning guides.
- No rewrite of historical pre-v0.11.0 release notes beyond repointing current public guidance to the new `docs/releases/v0.11.0.md` summary.
- No split into separate tickets unless later implementation evidence shows the five-path documentation rollout is no longer bounded.

Open questions
- none

Follow-up questions
- After v0.11.0 lands, should a separate docs ticket add runnable non-SQLite live-schema examples, or keep those providers documented only as external opt-in validation lanes?
- Should a later documentation pass add provider-specific operational appendices for external live-schema checks instead of keeping shared cross-provider guidance in the root docs?

Risks
- If the five-path update drifts internally, adopters may assume DVault ships a standalone CLI or that `export` is the default CI gate.
- If the docs overstate live-schema automation for PostgreSQL, SQL Server, Oracle, or MySQL, users may confuse built-in reader support with DVault-managed operational infrastructure.
- Until `docs/releases/v0.11.0.md` exists and current docs stop pointing at `0.10.0`, the public release posture remains misleading.

Split recommendations
- No split recommended. The missing release note plus the four named current-doc updates remain one bounded documentation rollout that should proceed through the normal `po-critic -> dev` path.
- If later work wants provider-specific operational tutorials or runnable non-SQLite live-schema walkthroughs, track those as separate follow-up tickets rather than widening this ticket.

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