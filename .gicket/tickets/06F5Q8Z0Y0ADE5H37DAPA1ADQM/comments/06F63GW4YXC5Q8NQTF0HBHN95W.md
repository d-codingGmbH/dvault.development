[gicket-bot] PO-critic review contract

Summary
- Ready for developer handoff. The persisted contract is authoritative, has `## Open Questions` set to `none`, matches current repository baselines for additive diagnostics/telemetry/support-bundle work, and the branch history shows only ticket orchestration metadata.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- Ticket comments under `.gicket/tickets/06F5Q8Z0Y0ADE5H37DAPA1ADQM/comments/` are bot orchestration/refinement entries only, such as `06F624B9F4D665BDF8WD15WK20.md`, `06F627NH8HRDJFCKAE5X51CGW4.md`, and `06F63FNSE1MP92WPMQX63RZ240.md`; no human scope-change comment was found.
- Related ticket `.gicket/tickets/06F5Q8YKR31DXGRXVPJ9031BQW/ticket.json` is `done`, matching the contract claim that the staging SPI/transaction contract was already settled.
- Relation files `.gicket/relations/AR/QM/06F5Q8YBVRS2EZVMJK5EATV9AR--06F5Q8Z0Y0ADE5H37DAPA1ADQM--parentOf.json` and `.gicket/relations/QM/EM/06F5Q8Z0Y0ADE5H37DAPA1ADQM--06F5Q90718D21DN1N1Q2AP7YEM--blocks.json` confirm the epic parent and downstream docs blocker; `.gicket/relations/QW/QM/06F5Q8YKR31DXGRXVPJ9031BQW--06F5Q8Z0Y0ADE5H37DAPA1ADQM--blocks.json` still exists but its source ticket is `done` and the persisted contract/comment history already marks it stale.
- `src/DCoding.Data.DVault/DataVaultDiagnostics.cs:338-365` already defines additive save-strategy diagnostics with ordered candidates (`Ordinal`, `StrategyName`, `Priority`, `CanSave`, `FallbackCauses`) and top-level `DataVaultSaveStrategyDiagnostics` fields for `ProviderName`, `SelectedStrategyName`, `SelectedStrategyPriority`, `Candidates`, and `FallbackCauses`.
- `src/DCoding.Data.DVault/DataVaultDiagnostics.cs:<redacted>` shows diagnostics evaluation preserves strategy ordering, returns selected-strategy identity when a candidate can save, and otherwise produces distinct finite fallback causes for provider-neutral fallback.
- `src/DCoding.Data.DVault/DataVaultSaveTelemetrySummary.cs:120-244` already exposes request/hub/link/satellite counts, derived `OperationCount`, `ProviderName`, `SelectedStrategyName`, finite fallback-cause kinds, bounded explanations, and chunked-state diagnostics, which matches the ticket's additive-surface framing.
- `src/DCoding.Data.DVault/DataVaultDesignTimeCommandHost.cs:44-46` and `src/DCoding.Data.DVault/DataVaultSupportBundle.cs:12-41` show support-bundle diagnostics are caller-supplied via `CreateSupportBundleDiagnostics` and serialized under `dvault.support-bundle.v1`, aligning with the ticket's consumer-owned request-bound support-bundle rule.
- `docs/architecture/dvault-v1-explicit-save-service.md:31-42` and `docs/releases/v0.19.0.md:29,33,50,58,94` keep `IDataVaultSaveService` as the public boundary and explicitly state staged provider bulk ingestion remains future/additive work rather than a new save API.
- `git show --stat --summary --name-only --format=medium HEAD~3..HEAD` shows the recent branch commits `448cefcdcd55`, `019057c124d9`, and `f860f2e51c10` only touched `.gicket/tickets/06F5Q8Z0Y0ADE5H37DAPA1ADQM/...` metadata/comment files, and `git diff --name-only f860f2e51c1088c727fa407996ee89b2a6dfd026..ticket/06F5Q8Z0Y0ADE5H37DAPA1ADQM-story-add-actionable-staged-bulk-fallback-diagno` returned no files.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- An explicit example for how diagnostics should look when a staged provider candidate is selected and later falls back or declines mid-lifecycle would reduce implementation interpretation risk, but the current contract is still sufficient for dev handoff.
- A concrete example covering multiple staged candidates with preserved ordering and deduplicated finite fallback causes would strengthen test design, but this is not required to start development.

Risky assumptions
- The ticket assumes one finite staged-provider caveat taxonomy can serve all providers; if a provider needs materially different caveats, the contract already points to provider-specific follow-up tickets.
- The ticket assumes staged fallback/decline details can be added as additive extensions to existing public diagnostics and telemetry types without reopening the public save boundary.
- Automation may still see the stale relation file `.gicket/relations/QW/QM/06F5Q8YKR31DXGRXVPJ9031BQW--06F5Q8Z0Y0ADE5H37DAPA1ADQM--blocks.json`, but the source ticket is `done`, current ticket `isBlocked` is `false`, and the PO contract/comment trail already treats that blocker as obsolete.

AC / test suggestions
- Add tests that prove selected-strategy reporting and candidate ordering for both staged-decline-to-provider-neutral and successful staged-strategy evaluation paths.
- Add a test that verifies staged lifecycle/provider-caveat data flows through request-bound support-bundle diagnostics only when `CreateSupportBundleDiagnostics` supplies representative save diagnostics.
- Add negative redaction tests for raw SQL, credentials, hash keys, payload values, and transient stage-row contents across diagnostics, telemetry, and support-bundle serialization.

Implementation watchouts
- Keep the work additive on the existing public types in `DataVaultDiagnostics.cs` and `DataVaultSaveTelemetrySummary.cs`; do not introduce a new public save entrypoint or separate diagnostics channel.
- Preserve bounded/redacted output semantics already documented in `docs/releases/v0.16.0.md` and `docs/releases/v0.19.0.md`.
- Reuse the consumer-owned support-bundle path instead of inventing representative requests inside the generic design-time command runner.
- Be careful to separate provider-neutral fallback causes already in the product from the new staged-provider decline/fallback vocabulary so telemetry and explain output stay deterministic.

Non-blocking notes
- This is still a pre-development branch: recent commits are ticket orchestration/handoff only, not product code.
- The persisted ticket has no attachments or planning artifacts under `.gicket/tickets/06F5Q8Z0Y0ADE5H37DAPA1ADQM/`, which is consistent with the contract note that no extra materialized artifacts were created in refinement.

Split recommendations
- No split recommended at this gate; the current contract is bounded to additive diagnostics work and already keeps provider implementation, benchmark, and documentation work in separate tickets.
- If implementation evidence later shows materially different provider-specific caveat taxonomies or lifecycle outcomes, split those into provider-specific follow-up tickets rather than widening this shared diagnostics story.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment