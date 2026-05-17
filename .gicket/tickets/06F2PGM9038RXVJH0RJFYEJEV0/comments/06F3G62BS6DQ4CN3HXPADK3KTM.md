[gicket-bot] PO refinement contract

Summary
- Refined the v0.13.0 documentation ticket into a bounded release-closure doc sweep: create `docs/releases/v0.13.0.md`, align current-baseline docs to `0.13.0`, and document the shipped Code-First parity surface without claiming deferred dependent-child or effectivity-specific APIs.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Repository evidence shows `docs/releases` currently stops at `v0.12.0`, while live release `06F2PH9C2PY0EBJBJNQA9338XC` is active as `v0.13.0 - Code-First Parity Expansion`; this ticket owns the missing coordinated `docs/releases/v0.13.0.md` closure.
- Live `.gicket` state keeps this task under epic `06F2PGK4QJ0YGXK5479W83Z2J0`; incoming `blocks` relations from done tickets `06F2PGM1HQ5W1M2H8T50MZ3EEC`, `06F2PGKAQVVF8GEZVVC8SHFASG`, `06F2PGKV9AFAMKGJEKKZ3AXHGC`, and `06F2PGHJAFMH80TZAMANQWH9PW` are satisfied historical context rather than PO blockers.
- Current ticket comments contain only automation lease comments; no human follow-up comment changes the scope.
- Current repository code and public API already expose Code-First repeated same-hub roles via `DataVaultCodeFirstLinkBuilder.Participant<TEntity>(string role)` and link-parent satellites via `DataVaultCodeFirstLinkBuilder.Satellite<TSatellite>(...)`.
- Tests already ratify the shipped surface: repeated same-hub role-bearing links project stable role-based column names, link-parent satellites project `Parent.Kind = Link`, and model-artifact export/import includes link-parent satellites.
- Current docs understate the shipped surface: `README.md`, `examples/README.md`, `docs/model-first-governance.md`, and `docs/production-adoption-checklist.md` still describe Code-First as hub-parent or ordered-link only, and current-version snippets still point at `0.12.0`.
- Done effectivity-ticket evidence ratifies effectivity as a modeling pattern on top of generic link-parent satellites, not as a separate effectivity-specific entity family or fluent API.
- No visible shipped public API baseline exists for dependent child key modeling in the current repository, so v0.13 docs should treat that capability as deferred rather than documented release scope.
- No child tickets, relation writes, attachments, or planning documents were materialized in this refinement run.

Scope In
- Create `docs/releases/v0.13.0.md` using the established release-note structure and the live v0.13 release context.
- Update root `README.md` so installation snippets, current-release references, and Code-First guidance align to `0.13.0` and describe the shipped parity surface: explicit or derived links, repeated same-hub links with distinct roles, hub-parent satellites, and link-parent satellites.
- Document that repeated same-hub Code-First links require an explicit link name plus distinct `Participant<TEntity>(string role)` roles, and add one canonical same-as or self-link example that shows that pattern.
- Document effectivity in v0.13 as caller-owned link-parent satellite usage through `Link(...).Satellite<TSatellite>(...)` with `Payload(...)` and optional `DrivingKey(...)`, not as a separate effectivity-specific builder or metadata kind.
- Version-align and minimally refresh other current-baseline docs that still present the old `0.12.0` or hub-parent-only narrative, including `examples/README.md`, `docs/model-first-governance.md`, `docs/production-adoption-checklist.md`, and any touched package-local install guidance such as `src/DCoding.Data.DVault.Analyzers/README.md`.
- Keep the existing architecture boundaries explicit in touched docs: metadata-first and model-first remain authoritative alternatives for other ownership needs, and there is still no public Code-First-to-registry bridge.

Scope Out
- Any new runtime, modeling, analyzer, persistence, or package-shape implementation; this ticket documents shipped behavior only.
- Dependent child key modeling, including any new link-key metadata, hashing contract, or public API claim for that unfinished capability.
- A new effectivity-specific fluent API, metadata kind, annotation set, or technical column family.
- Typed link-mapper or source-generator parity claims for same-hub role-bearing links when current repository evidence keeps those surfaces separate.
- Retrofitting the runnable quickstarts away from their current metadata-first posture or adding a new end-to-end sample project unless a short consistency note is sufficient.
- Relation cleanup, child-ticket creation, or broader planning-graph reshaping.

Open questions
- none

Follow-up questions
- Should a later examples ticket add a runnable end-to-end Code-First sample for same-as links or link-parent/effectivity satellites instead of keeping v0.13 at README and release-note level?
- If future work delivers dependent child key modeling, should that ship with a separate release-note and documentation sweep rather than being backfilled into the v0.13 story?
- If product later wants first-class effectivity-specific sugar or validation, should that arrive as a separate additive API instead of broadening the generic link-parent satellite contract?

Risks
- If the docs sweep only bumps versions without correcting the surface boundary, public guidance will remain inconsistent and understate shipped Code-First behavior.
- If v0.13 docs overstate the release by claiming dependent child keys or effectivity-specific APIs, the release history will be misleading.
- If touched docs omit the explicit-name-plus-role pattern for repeated same-hub links, adopters may infer that derived names work or that same-hub links remain unsupported.
- If touched docs blur metadata-first, model-first, and Code-First responsibilities, adopters may infer a new metadata authority or save boundary that the repository does not provide.

Split recommendations
- No additional split is recommended; done implementation tickets already isolate same-hub role support, link-parent satellites, and effectivity ratification, and this ticket is the bounded v0.13 documentation closure.
- If product later wants runnable same-as/effectivity samples or dependent child key documentation, track those as separate follow-on tickets instead of widening this release-closure task.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 4
- implementation-notes items: 7

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment