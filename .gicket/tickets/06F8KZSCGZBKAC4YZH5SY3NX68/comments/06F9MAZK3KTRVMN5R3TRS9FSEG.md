[gicket-bot] PO-critic review contract

Summary
- Contract is sufficiently specific for a bounded docs-only pre-development story; no unresolved PO refinement blocker was found.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06F8KZSCGZBKAC4YZH5SY3NX68/description.md:30-43` defines concrete acceptance criteria and definition-of-done items for a docs-only observability update, and `description.md:52-53` shows `## Open Questions` with `- none`.
- `README.md:265-296` already documents the repository baseline the ticket references: metrics opt-in via `services.AddDVault(); services.AddDVaultTelemetry();`, built-in meter name `DCoding.Data.DVault`, and tracing via `ActivityListener` for source `DCoding.Data.DVault` while `AddDVault()` remains telemetry-free by default.
- `docs/releases/v0.23.0.md:30-34` and `docs/architecture/dvault-v1-activity-tracing-contract.md:19-25` confirm tracing is listener-driven, metrics remain a sibling opt-in surface, and adopter docs should link to the tracing contract instead of duplicating it.
- Live relation files `.gicket/relations/68/18/06F8KZSCGZBKAC4YZH5SY3NX68--06F8KZSYCVZ21MS983501BZG18--blocks.json` and `.gicket/relations/88/68/06F8KZQNH8CCMTJW9P95W1N388--06F8KZSCGZBKAC4YZH5SY3NX68--parentOf.json` confirm this ticket blocks `06F8KZSYCVZ21MS983501BZG18` and is a child of epic `06F8KZQNH8CCMTJW9P95W1N388`; `find .gicket/relations -name '*blocks.json' | rg '06F8KZSCGZBKAC4YZH5SY3NX68'` returned only that outgoing blocks file, matching the no-active-incoming-blocker claim.
- Comment `.gicket/tickets/06F8KZSCGZBKAC4YZH5SY3NX68/comments/06F9M8VRTT5TDKNAWV3K10NBFW.md:10-15` states there are no human comments or attachments adding constraints and records the same live-relation summary; `06F9M956HCDX44MMGRYHPJT8SM.md:13-18` records the PO handoff and ticket-field updates.
- Branch history and diff are still pre-development: `git log --oneline --max-count=6 ticket/06F8KZSCGZBKAC4YZH5SY3NX68-task-add-opentelemetry-examples-for-dvault-activ` shows only PO/po-critic workflow commits after `develop`, and `git diff --name-only develop...ticket/06F8KZSCGZBKAC4YZH5SY3NX68-task-add-opentelemetry-examples-for-dvault-activ` returned only `.gicket/tickets/06F8KZSCGZBKAC4YZH5SY3NX68/**` paths, with no `README.md`, `examples/README.md`, or `src/**` changes yet.
- `examples/README.md:17-23` still shows `0.16.0` package install examples while `README.md:10-16` uses `0.30.0`, matching the persisted risk called out in `description.md:59-62`.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- Assumes a compact docs-only delta, likely centered in `examples/README.md` with links back to the root README/tracing contract, is sufficient for blocked follow-on ticket `06F8KZSYCVZ21MS983501BZG18` as stated in `description.md:43-50`.
- Assumes any OpenTelemetry-style snippet stays explicitly package-agnostic and application-owned; if the delivery drifts into exporter/package/backend setup, it would exceed the current scope rather than clarify it.

AC / test suggestions
- Review the final doc diff directly against `description.md:31-36`: separate metrics from tracing, state that `AddDVault()` is telemetry-free by default, and link to `docs/architecture/dvault-v1-activity-tracing-contract.md`.
- If `examples/README.md` is the delivery surface, verify the snippet stays aligned with the existing metadata-first startup shape (`AddDVault(...)`, provider registration, `UseDataVaultMetadata()`) instead of introducing a new quickstart architecture.
- Add a reviewer check that no example text includes raw connection strings, payload values, SQL text, exporter endpoints, or package-install instructions for OpenTelemetry backends.

Implementation watchouts
- Do not duplicate large tracing tables or redaction prose from `docs/architecture/dvault-v1-activity-tracing-contract.md`; the contract is already authoritative and the ticket explicitly wants link-first wording.
- If `examples/README.md` is edited, do not leave the surrounding `0.16.0` version examples unexamined next to new observability guidance; that file currently diverges from the root `README.md` `0.30.0` baseline.

Non-blocking notes
- The branch currently contains only ticket metadata/history updates under `.gicket/**`; for this role that is acceptable and does not block approving the ticket for developer handoff.

Split recommendations
- none

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment