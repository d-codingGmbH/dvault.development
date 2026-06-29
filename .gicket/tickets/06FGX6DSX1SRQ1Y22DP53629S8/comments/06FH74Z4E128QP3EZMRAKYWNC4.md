[gicket-bot] PO refinement contract

Summary
- Fresh repository inspection shows `docs/releases/v0.50.0.md` is missing, `CHANGELOG.md` still starts at v0.49.0, several current-baseline docs still point to v0.49.0 until a v0.50.0 note exists, shared implementation standards still describe the active compatibility contract as v0.49.0, and the stale-version package-verifier guardrails for `8.49.0` / `10.49.0` are already present. No child-ticket split or relation cleanup is justified; this remains one bounded documentation-and-verification alignment task.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The visible package-line baseline is already fixed in repository evidence as `8.50.0` for `net8.0` / EF Core 8 and `10.50.0` for `net10.0` / EF Core 10; this ticket should ratify that baseline instead of reopening version selection.
- `docs/releases/v0.50.0.md` is currently absent, while `README.md`, `docs/package-compatibility.md`, and `docs/manual-nuget-publication.md` explicitly say they are waiting for the v0.50.0 release-note/changelog update.
- `docs/local-validation.md` already matches the v0.50.0 package-line baseline and should be treated as verified unless a consistency edit becomes necessary during the release-note alignment pass.
- `tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs` and the visible package-verifier tests already reject stale `8.49.0` / `10.49.0` install and version fragments, so the verifier baseline is a preserve-and-align requirement, not an open design question.

Scope In
- Create `docs/releases/v0.50.0.md` as the current release-note artifact for the v0.50.0 documentation baseline.
- Update `CHANGELOG.md` so v0.50.0 becomes the current top release entry and links to the new release notes.
- Replace temporary v0.49.0 current-release cross-references in `README.md`, `docs/package-compatibility.md`, and `docs/manual-nuget-publication.md` once the v0.50.0 release-note/changelog pair exists.
- Update `docs/plans/shared-implementation-standards.md` so the current package-compatibility contract names v0.50.0 and forbids consumer-facing `0.50.0`, not `0.49.0`.
- Keep the documented nine-package family, analyzer `.NET 10 SDK` host guidance, `8.50.0` / `10.50.0` package lines, and stale-version verification aligned with the current baseline.

Scope Out
- NuGet publication, signing, push approvals, or package artifact generation.
- Changing the consumer package versions away from `8.50.0` / `10.50.0` or altering the target-specific dependency matrix.
- Expanding analyzer compatibility to pure `.NET 8 SDK` consumption.
- Adding provider-performance claims, rerunning benchmarks, or carrying any provider-performance placeholder into the v0.50.0 release notes.
- Product-code or package-shape changes outside documentation alignment and existing verifier guardrails.

Open questions
- none

Follow-up questions
- Should a separate cleanup ticket normalize `docs/production-adoption-checklist.md` and any other ancillary docs that still cite v0.49.0 as the current release-note baseline after this ticket lands?
- Should `src/DCoding.Data.DVault.Analyzers/README.md` be pulled into the same release-note cross-link cleanup standard, or remain package-local guidance outside this ticket's acceptance surface?

Risks
- If ancillary docs outside the explicit acceptance surface keep v0.49.0 as the current baseline, consumers may still see inconsistent current-release guidance after the main v0.50.0 note/changelog work is done.
- Because the visible verifier evidence targets packaged README/install fragments, drift in non-packaged planning or adoption documents can escape automated package verification unless those docs are manually reviewed or separately covered.

Split recommendations
- none

Persisted contract coverage
- acceptance-criteria items: 7
- definition-of-done items: 5
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment