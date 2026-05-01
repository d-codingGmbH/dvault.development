[gicket-bot] PO refinement contract

Summary
- Refined the ticket to choose an explicit DI-registered DVault save service over SaveChanges interception, keep the work as one bounded ticket with no new child artifacts, and align the write entry point with the current explicit SQLite-first API baseline.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- V1 chooses an explicit DVault save service as the default write entry point; SaveChanges interception is not the selected baseline for this ticket.
- The current repository already establishes explicit convention-first surfaces through AddDVault(), UseDataVault(), and ApplyDataVaultMetadata(), and no SaveChanges interceptor surface exists on this branch; this ticket should extend that explicit pattern rather than introduce hidden write behavior.
- The default provider baseline remains the existing SQLite-oriented EF Core 10 path under tests/DCoding.Data.DVault.Tests/Integration.
- Required write metadata follows the documented defaults already present in repository planning documents: load timestamp and record source are mandatory, and stable hashing stays behind the existing IStableHashService DI abstraction.
- The immediate concrete proof for this ticket may stay bounded to representative hub and link writes, while the chosen entry point must remain extensible to later satellite work.
- This ticket defines the write boundary that downstream persistence work, including blocked ticket 06EXB7HEJY18HEB5A5MVTN5KZC, should build on after completed schema-snapshot groundwork from 06EXB7GPRGEJHKFMJ8MVAVF8ZG.
- No child tickets or planning documents were materialized in this refinement because the current evidence supports a single bounded ticket once the entry-point decision is fixed.

Scope In
- Design and implement a public explicit DVault write boundary/service for the v1 EF Core path instead of a SaveChanges interceptor.
- Register the default implementation through AddDVault() with the same optionless first-use ergonomics and normal caller-override behavior already used elsewhere in the package.
- Define the service boundary so caller-visible record source and load timestamp semantics remain explicit and deterministic while stable hashing is consumed through IStableHashService.
- Add unit and SQLite integration coverage proving the explicit service is discoverable, invocable, and functional with minimal configuration in the existing test layout.
- Document the explicit-service decision and the rejection of SaveChanges interception for the v1 default path.

Scope Out
- Registering or relying on EF Core SaveChanges interceptors as the default v1 persistence entry point.
- Broad idempotency, duplicate reuse, concurrency, or mutable-record behavior beyond what is necessary to establish the explicit write boundary; those belong to downstream persistence tickets such as 06EXB7HEJY18HEB5A5MVTN5KZC.
- Complete satellite-specific persistence semantics, hash-diff change rules, or mutable update rules beyond keeping the service boundary extensible to that future work.
- Additional database providers, provider-specific batching, or migration/design-time infrastructure beyond the current SQLite baseline.
- Advanced configuration hooks or public options matrices for naming, hashing, provider behavior, record source resolution, or timestamp sourcing beyond existing defaults.

Open questions
- none

Follow-up questions
- After the explicit service contract and initial persistence semantics stabilize, should DVault add an optional SaveChanges-based convenience wrapper in a separate ticket?
- When satellite persistence and mutable/update behavior are scheduled, should they extend the same explicit service boundary through dedicated collaborators rather than a new public entry point?
- If future providers need batching or bulk-write optimizations, should those be added behind the same explicit service contract as provider-specific follow-up tickets?

Risks
- If the service boundary is defined too narrowly around today's hub and link proof cases, later satellite or mutable-record work may require breaking API changes.
- If record source or load timestamp handling becomes implicit or hidden, deterministic tests and replay or import scenarios will be harder to reason about.
- If implementation quietly reintroduces interceptor-like behavior under the explicit API, the repository will lose the clear explicitness established by its current public surfaces.

Split recommendations
- No immediate split is required if this ticket stays focused on the explicit write boundary plus minimal SQLite-backed proof; keep idempotent hub and link semantics in 06EXB7HEJY18HEB5A5MVTN5KZC.
- If SaveChanges convenience is still desired after the explicit service lands, create a separate follow-up ticket for optional interceptor-based integration.
- Keep provider-specific optimizations or non-SQLite write implementations in separate follow-up tickets rather than widening this ticket.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment