[gicket-bot] PO refinement contract

Summary
- Refined the v0.47 documentation ticket around the current v0.46 baseline: add docs/releases/v0.47.0.md, update CHANGELOG.md, align package-guidance docs to 8.47.0 and 10.47.0, and keep provider-maintenance claims bounded to completed evidence artifacts.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Current repository evidence still stops at the v0.46.0 release-doc baseline: docs/releases has no v0.47.0 note yet, CHANGELOG.md starts with v0.46.0, and docs/performance-profiles.md still declares a v0.46.0 provider-optimization closure baseline with a carried-forward v0.45.0 PIT maintenance overlay.
- The public release label must continue the established dual package-line model: v0.47.0 maps to 8.47.0 for net8.0 and EF Core 8, and 10.47.0 for net10.0 and EF Core 10; there is no consumer-facing 0.47.0 package version.
- Maintenance and performance wording must stay inside the current evidence contract: completed benchmark claims require preserved artifact triplets and run context, while PIT maintenance remains source and test backed unless new completed artifacts exist.
- No bounded write actions were materialized during refinement: no child tickets, relation cleanup, description edits, attachments, or planning documents were added.

Scope In
- Create docs/releases/v0.47.0.md using the existing release-note pattern and the v0.47 package-line mapping.
- Add the matching v0.47.0 summary entry in CHANGELOG.md and link it to the new release note.
- Update docs/performance-profiles.md and docs/plans/provider-optimization-evidence-matrix.md so the v0.47 documentation baseline and claim boundaries stay aligned with the checked-in evidence sources.
- Update the versioned package-guidance docs that track the active release line so they consistently cite 8.47.0 and 10.47.0.

Scope Out
- Running new benchmarks, generating new evidence artifacts, or changing benchmark or manifest contracts.
- Changing product code, provider strategy registrations, or maintenance implementations.
- Promoting provider-maintenance work to performance-win language without completed checked-in artifacts.
- Package publishing, signed release approval, release automation, or non-documentation rollout work.

Open questions
- none

Follow-up questions
- Once the incoming blocker tickets close, does the team want one final wording sweep to confirm the v0.47 docs describe only landed provider-maintenance and evidence behavior?
- If the v0.47 package-guidance sweep grows beyond version alignment into broader README or analyzer-installation rewrites, should that stay in this ticket or move to a separate docs cleanup ticket?

Risks
- Incoming blocker tickets may still shift the exact v0.47 maintenance wording, which can force a final documentation pass even though the refinement boundary is clear.
- The repository currently advertises v0.46.0 across multiple guidance surfaces, so an incomplete sweep can leave mismatched release-line or package-version references.
- The existing evidence contract forbids treating source and test backed PIT maintenance work as completed timing evidence, so careless release-note language can create a documentation regression.

Split recommendations
- If package-guidance edits expand beyond release-line alignment and evidence-boundary consistency, split broader adopter-document rewrites into a separate docs ticket so this ticket stays bounded to the v0.47 release-doc sweep.

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