[gicket-bot] PO refinement contract

Summary
- Refined the epic around the existing four-child split, ratified the explicit-save and provider-fallback repository baselines, and tightened acceptance around read ergonomics, compiled-query proof, opt-in interceptors, and optional bulk hooks; no ticket, relation, or planning-document writes were needed.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Live relation state already materializes four child tickets via `parentOf`: `06F1XPXJW79K94G4WG86AG2X6M`, `06F1XPYA9MD0T9C4651ND8KX0W`, `06F1XPZAJBSSNN6HY1CHAQPH74`, and `06F1XQ03MADSPQD0AJN6R50D44`.
- The epic currently has seven outgoing `blocks` relations and one incoming `blocks` relation from `06F1XPRY3ZDB6W1WQ9ABRRJ2V4`; this refinement leaves live relation state unchanged.
- Ticket attachment metadata shows one persisted plan artifact, `v0.9.0-read-runtime-performance-plan.md`; no new attachment or planning document was written in this pass.
- Repository evidence fixes the v1 default write boundary as explicit `IDataVaultSaveService`, with interceptor work remaining opt-in and provider-specific optimization flowing through existing provider strategy dispatch and fallback rather than a new default persistence path.
- Released docs already establish `IDataVaultReadService` as the narrow latest/as-of baseline; this epic extends that baseline into ergonomic current, point-in-time, and bridge scenarios instead of reopening direct EF support.

Scope In
- Ergonomic EF-friendly APIs for current/latest satellite reads, as-of point-in-time reads, and bridge traversal scenarios.
- Proof of compiled-query and compiled-model compatibility for the supported read-helper surface.
- Opt-in load-metadata defaulting and interceptor ergonomics that preserve the explicit-save default.
- Optional provider bulk insert strategy hooks aligned to the existing provider optimization boundary.

Scope Out
- Replacing EF Core or removing direct EF querying and persistence as a supported path.
- Making SaveChanges interception or other convenience plumbing the default write path.
- Mandating any third-party bulk package.
- Provider-specific optimizations or capabilities without provider-neutral fallback or explicit unsupported diagnostics.
- Broader new read-model feature families outside the bounded current/latest, point-in-time, and bridge scenarios of this epic.

Open questions
- none

Follow-up questions
- After the bounded v0.9.0 scope lands, should a later release add richer bridge and point-in-time typed projection helpers beyond endpoint hash keys and `TraversalDepth`?
- Should a later release publish a broader cross-provider benchmark matrix once the bounded provider-neutral and provider-hook baselines are stable?

Risks
- Performance evidence can mislead if it covers only one provider or only happy-path data volumes; the epic should keep claims scoped to the measured scenarios.
- Interceptor convenience can blur the explicit-save guidance unless docs clearly preserve explicit `IDataVaultSaveService` as the default path.
- Provider bulk hooks may create inconsistent expectations across providers unless unsupported and fallback behavior is explicit and tested.

Split recommendations
- No additional split is recommended now; execution should continue through the four existing child tickets already linked by `parentOf`.
- If compiled-query and compiled-model proof grows into provider-by-provider certification instead of one supported baseline with explicit exceptions, split that certification work into a separate follow-up rather than expanding this epic.

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