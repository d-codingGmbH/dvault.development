[gicket-bot] PO-critic review contract

Summary
- PO refinement closed the prior contract gaps; the ticket now pins the typed read surface, diagnostics, and reserved-name rules on top of the existing latest/as-of read path, so it is ready for developer handoff.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06F0MECPFAVBFBNC5XMVDZRQ6M/description.md:32-56 now defines the typed helper signatures, DataVaultSatelliteProjectionRow accessor behavior, deterministic failure prefix/tokens, reserved-name rejection, and ## Open Questions = none.
- Previous PO-critic blockers are recorded in .gicket/tickets/06F0MECPFAVBFBNC5XMVDZRQ6M/comments/06F13BER4EV06Y6Z3YQV47FK9R.md:18-26; the PO refinement comment .gicket/tickets/06F0MECPFAVBFBNC5XMVDZRQ6M/comments/06F13F7DC8N3D2D18BN92TTVNW.md:10-16 marks critic-item-1 through critic-item-6 answered.
- git log on the ticket path shows <redacted> [06F0MECPFAVBFBNC5XMVDZRQ6M] handoff po-critic->po followed by 960a5dd0f [06F0MECPFAVBFBNC5XMVDZRQ6M] handoff po->po-critic, confirming this review is after a refinement pass.
- The existing public read path is already additive and provider-neutral: src/DCoding.Data.DVault/IDataVaultReadService.cs:8-19, src/DCoding.Data.DVault/DataVaultLatestSatelliteReadRequest.cs:14-35, src/DCoding.Data.DVault/DataVaultRegistryLatestSatelliteReadRequest.cs:15-40, and src/DCoding.Data.DVault/DataVaultReadServiceRegistryExtensions.cs:26-44 show explicit and registry-backed latest/as-of requests on one pipeline.
- src/DCoding.Data.DVault/DefaultDataVaultReadService.cs:21-49,83-148 shows the current raw reader batches keys, applies as-of cutoff/series selection, and silently skips malformed rows; description.md:43-44,51-53,64-67 now explicitly guards typed work against that behavior.
- Existing raw-read integration coverage already proves explicit and registry-backed latest/as-of behavior in tests/DCoding.Data.DVault.Tests/Integration/ExplicitDataVaultSaveServiceSqliteTests.cs:862-952 and :<redacted>.
- src/DCoding.Data.DVault/Modeling/DataVaultMetadata.cs:754-760,820-845 currently rejects only payload/driving-key overlap, so the new typed reserved-name rule in description.md:14-16,38,44,52 closes a real contract gap instead of duplicating an existing validator.
- Upstream blockers are already complete in .gicket/tickets/06F0MEC7FEXAD069AJNYZW0DRM/ticket.json:7-15 and .gicket/tickets/06F0MEB634X6CTBZ00W108G3FG/ticket.json:7-15; downstream docs remain intentionally separate in .gicket/tickets/06F0MEDJC732GDD77H60R259P0/ticket.json:7-16.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- No blocking gap remains, but the persisted contract still lacks a concrete link-parent multi-active typed projection example even though description.md:45 requires link-parent parity tests.
- The contract pins failureKind tokens and the message prefix, but it does not spell out one full example message for each of missing-name, null-value, and invalid-value.

Risky assumptions
- Implementation must project from a pre-silent-drop row shape rather than only from DataVaultSatelliteReadRecord, or the required/null diagnostics in description.md:36-37,43-44,51 cannot be met.
- Explicit and registry-backed typed overloads must share one projection pipeline as required by description.md:13,34,43,65, or parity and diagnostic wording can drift.
- Reserved-name validation must happen before query execution as required by description.md:16,38,44; relying on existing metadata validation alone is insufficient.

AC / test suggestions
- Keep one typed parity test each for hub-parent and link-parent multi-active satellites so the generic request surface is proven across both parent kinds.
- Assert all three failureKind tokens (missing-name, null-value, invalid-value) against both explicit and registry-backed typed overloads.
- Assert typed LoadTimestamp normalization on the typed path itself across provider-default, ISO 8601 UTC text, and UTC-ticks storage, not only on raw reads.

Implementation watchouts
- Do not widen IDataVaultReadService; the current public shape keeps registry reads as companion extensions in src/DCoding.Data.DVault/DataVaultReadServiceRegistryExtensions.cs:8-45 and tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt:194-195,322-323.
- Reuse the existing batching, as-of cutoff, multi-active grouping, and ordinal ordering semantics from src/DCoding.Data.DVault/DefaultDataVaultReadService.cs:21-49.
- Preserve exact StringComparer.Ordinal name matching and the current timestamp normalization behavior visible in src/DCoding.Data.DVault/DataVaultLoadTimestampValueConverter.cs:67-99.
- Mirror current registry semantics by resolving metadata once and then delegating to the explicit request path, consistent with src/DCoding.Data.DVault/DataVaultReadServiceRegistryExtensions.cs:35-44.

Non-blocking notes
- Approval is valid under the stated rule because .gicket/tickets/06F0MECPFAVBFBNC5XMVDZRQ6M/description.md:55-56 shows ## Open Questions followed by - none.
- README and release-note updates remain intentionally out of scope here and are split to ticket 06F0MEDJC732GDD77H60R259P0.

Split recommendations
- No split recommended; the repository and persisted contract still bound this to one additive typed-read helper layer plus diagnostics and tests.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment