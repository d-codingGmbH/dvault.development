[gicket-bot] PO-critic review contract

Summary
- Return to PO: the contract resolves open questions, but it still does not define a concrete developer-facing delta beyond query capabilities already present in the repository.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06F2PGPKXWRFXNPFA1JR0X67XC/description.md now says `## Open Questions` = `none`, but its acceptance criteria mostly restate the existing latest/as-of baseline instead of naming a new observable API or behavior change.
- src/DCoding.Data.DVault/DataVaultLatestSatelliteReadRequest.cs already exposes a two-constructor pattern where omitting `asOf` means latest/current and supplying `DateTimeOffset? asOf` means as-of; it also normalizes `AsOf` with `ToUniversalTime()`.
- src/DCoding.Data.DVault/DataVaultRegistryLatestSatelliteReadRequest.cs plus src/DCoding.Data.DVault/DataVaultReadServiceRegistryExtensions.cs already provide registry-backed latest/as-of requests and typed `ReadLatestSatelliteAsync(...)` helpers for `UseDataVaultMetadata()` callers.
- src/DCoding.Data.DVault/DataVaultReadServiceTypedProjectionExtensions.cs already provides caller-owned typed projection helpers over `IDataVaultReadService`, so the non-reflection typed-read path already exists.
- tests/DCoding.Data.DVault.Tests/Integration/ExplicitDataVaultSaveServiceSqliteTests.cs and tests/DCoding.Data.DVault.Tests/Integration/DataVaultTypedSatelliteReadServiceSqliteTests.cs already cover latest reads, as-of reads, registry-backed reads, link-parent reads, multi-active series behavior, missing-parent empty results, and UTC-normalized timestamps.
- tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt already snapshots `DataVaultLatestSatelliteReadRequest`, registry-backed latest read helpers, and typed `ReadLatestSatelliteAsync(...)` as public API surface.
- README.md (latest/as-of read section) plus docs/releases/v0.6.0.md and docs/releases/v0.7.0.md already document typed latest/as-of reads, registry-backed `UseDataVaultMetadata()` reads, and separate PIT-backed historical reads.

Blocking findings
- The ticket does not state what new developer-visible behavior, API shape, or user workflow remains to be implemented beyond the current repository baseline. A developer could reasonably treat the story as a no-op or invent a convenience surface that PO did not actually ask for.
- Historical scope is still ambiguous. The contract says `If the story touches historical multi-satellite reads`, but it never decides whether PIT-backed historical ergonomics are actually in scope for this story or explicitly deferred.

Required PO actions
- Define the concrete delta versus the current repository baseline: name the exact new helper, request shape, signature, or user-facing behavior this story must add, or explicitly mark the story as already satisfied / no-work-required if no delta remains.
- Decide whether any `current`-named convenience surface is in scope here or explicitly deferred. If it is in scope, add at least one concrete caller example for both explicit-metadata and registry-backed usage.
- Decide whether PIT-backed historical ergonomics are in scope for this ticket. If yes, add one concrete target usage example and acceptance criterion for that path; if no, remove the conditional historical language from this story.

Open issues ledger
- critic-item-1 [required-po-action] Define the concrete delta versus the current repository baseline: name the exact new helper, request shape, signature, or user-facing behavior this story must add, or explicitly mark the story as already satisfied / no-work-required if no delta remains.
- critic-item-2 [required-po-action] Decide whether any `current`-named convenience surface is in scope here or explicitly deferred. If it is in scope, add at least one concrete caller example for both explicit-metadata and registry-backed usage.
- critic-item-3 [required-po-action] Decide whether PIT-backed historical ergonomics are in scope for this ticket. If yes, add one concrete target usage example and acceptance criterion for that path; if no, remove the conditional historical language from this story.
- critic-item-4 [blocking-finding] The ticket does not state what new developer-visible behavior, API shape, or user workflow remains to be implemented beyond the current repository baseline. A developer could reasonably treat the story as a no-op or invent a convenience surface that PO did not actually ask for.
- critic-item-5 [blocking-finding] Historical scope is still ambiguous. The contract says `If the story touches historical multi-satellite reads`, but it never decides whether PIT-backed historical ergonomics are actually in scope for this story or explicitly deferred.

Missing examples / edge cases
- A before/after caller example that shows the exact ergonomic gap this ticket is supposed to close on top of today's `ReadLatestSatelliteAsync(...)` surface.
- A registry-backed example that shows the intended improvement beyond the existing `DataVaultRegistryLatestSatelliteReadRequest` path.
- If historical reads are intended, one explicit negative example proving that missing PIT snapshots must not fall back to non-PIT latest/as-of reads.

Risky assumptions
- Assuming that restating existing latest/as-of behavior is enough to guide implementation without naming a new observable outcome.
- Assuming developers will infer the intended `current` ergonomics consistently even though the repository keeps latest/as-of as the stable public vocabulary and the ticket defers alias decisions.
- Assuming PIT/history work is an optional implementation detail instead of a ticket-scope decision that PO must make explicitly.

AC / test suggestions
- Add one acceptance criterion that names the exact new public entry point or usage pattern not already covered by the current latest/as-of API.
- Anchor the story with one explicit-metadata example and one `UseDataVaultMetadata()` example so docs, tests, and API snapshot updates are objectively checkable.
- If PIT-backed historical ergonomics remain in scope, add an acceptance criterion and test expectation for the no-fallback boundary when PIT rows are absent.

Implementation watchouts
- Keep `DataVaultLatestSatelliteReadRequest`, `DataVaultRegistryLatestSatelliteReadRequest`, `ReadLatestSatelliteRowsAsync(...)`, and `ReadLatestSatelliteAsync(...)` compatibility intact; the repository already exposes them as baseline public surface.
- Do not blur latest/as-of satellite reads with PIT-backed historical reads; README.md and docs/releases/v0.7.0.md already document them as separate surfaces.
- Preserve existing UTC normalization, ordinal parent-hash-key deduplication, deterministic ordering, and empty-result behavior if PO later specifies an additive convenience layer.

Non-blocking notes
- none

Split recommendations
- If PO wants both naming/entry-point convenience and PIT/history ergonomic work, split them: one ticket for additive latest/current caller ergonomics over the existing latest-satellite surface, and one ticket for any PIT-backed historical UX refinement.
- If no concrete delta beyond the current baseline can be named, close or re-route this story instead of handing it to development as an open-ended API-improvement task.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment