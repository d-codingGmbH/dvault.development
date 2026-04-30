[gicket-bot] PO refinement contract

Summary
- Refined the epic using ticket state, prior child-ticket reads, current branch evidence, and referenced planning docs. No new tickets, relations, attachments, or planning documents were created; existing child tickets 06EXB6XBV95E08R2W9ZQ1PRDPM, 06EXB6YBXPDBPWZPNV89A9F9AM, and 06EXB6Z3YMAPSRYRB8NQX3ZST4 cover the skeleton, package metadata, and convention-first entry-point slices.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The v1 repository baseline is the current README and project layout: root DVault.slnx, source project src/DCoding.Data.DVault/DCoding.Data.DVault.csproj, and tests/DCoding.Data.DVault.Tests.
- The .NET baseline is net10.0 and the v1 package/root namespace identity is DCoding.Data.DVault.
- The first-use API surface is convention-first: <redacted> registers provider-neutral defaults and requires no DVault options object.
- The finite v1 technical metadata role set is HashKey, HashDiff, LoadTimestamp, and RecordSource with reusable contracts and tests.
- Shared implementation standards, docs/formatting.md, README.md, the optional hooks plan, and the persistence convention policy are accepted planning context for this epic; no new planning document or attachment was needed.
- Older child-story text that names historical src/DVault paths should not reopen the current repository baseline; current branch evidence ratifies src/DCoding.Data.DVault and tests/DCoding.Data.DVault.Tests for v1.

Scope In
- Maintain DVault.slnx as the root .NET entry point and include all current .NET projects in it.
- Maintain the library project, package metadata, and namespace identity for DCoding.Data.DVault targeting net10.0.
- Keep a minimal consumer reference path, including package metadata, README packaging, Apache-2.0 license metadata, repository metadata, symbols, and local pack output as scoped by the existing packaging child story.
- Provide and preserve optionless service registration and provider-neutral default conventions for first-use startup.
- Expose bounded foundation modeling and technical metadata defaults needed by first consumers without provider-specific persistence.
- Keep tests for current foundation contracts and repository validation commands aligned with README.md and shared standards.

Scope Out
- Provider-specific persistence adapters, physical schema mapping, migrations, SQL dialect behavior, or runtime persistence execution.
- Full implementation of optional advanced configuration hooks for naming, hashing, record source, timestamp, or provider behavior.
- CI workflow creation, release publishing, NuGet credential handling, package signing, or registry rollout.
- Expanded Data Vault capabilities such as PIT tables, bridge tables, multi-active satellites, ingestion pipelines, or provider optimizations.
- Runnable examples or benchmarks beyond preserving documented placeholder layout unless separate tickets pull that work forward.

Open questions
- none

Follow-up questions
- Later provider tickets should decide adapter-specific persistence, schema or migration generation, and physical naming constraints.
- Later configuration tickets should implement the full optional advanced hook surface for naming, hashing, record source, timestamp, and provider behavior when needed.
- Later developer-experience or documentation tickets may add runnable examples and benchmarks once the public API shape stabilizes.
- A future hygiene ticket may update historical child descriptions that mention src/DVault so archived contracts match current repository naming; this does not block the epic handoff.
- The first CI or workflow ticket should add tools/check-format.sh and root dotnet validation as blocking steps.

Risks
- Developer environments without the .NET 10 SDK or .slnx-capable tooling cannot validate the build even when the repository is correct.
- The epic can expand into provider or persistence work if downstream implementation ignores the explicit scope boundary.
- Historical child contract path references may confuse implementers; current README and csproj evidence is authoritative for current work.
- Public API names such as AddDVault become durable once consumers adopt them, so later changes require compatibility planning.

Split recommendations
- No new split is recommended; existing child tickets 06EXB6XBV95E08R2W9ZQ1PRDPM, 06EXB6YBXPDBPWZPNV89A9F9AM, and 06EXB6Z3YMAPSRYRB8NQX3ZST4 already cover the foundation slices and were read during this PO run.
- Use future separate tickets for provider adapters, advanced configuration hooks, executable examples or benchmarks, and CI or release automation.

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