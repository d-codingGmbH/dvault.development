[gicket-bot] PO refinement contract

Summary
- Repository evidence and existing done child tickets already define a library-first adoption path centered on the README/getting-started baseline, runnable companion examples, and optional analyzer guidance, with no bundled template or CLI suite in v1.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Ratify the current first-run default as the root README plus docs/getting-started SQLite-first binary-first path with explicit AddDVault(...), provider registration, schema creation, IDataVaultSaveService, and IDataVaultReadService usage.
- Treat runnable examples as companion proofs, not the primary onboarding surface: 06FF43SFHY4EWTFQ2PAEKD8J50 covers the minimal SQLite path and 06FF43T2EK3CBYHTR287YWC5NR covers bounded PostgreSQL parity notes.
- Treat analyzer guidance as optional compile-time adoption support, not a runtime prerequisite: 06FF43W243BZM340V86CAXQC00 ratifies the current single net10.0 analyzer asset and .NET 10 SDK host baseline across the 8.47.0 and 10.47.0 lines.
- Keep privacy coverage separate from first-run adoption; 06FF43K0B0MJF45078STZ3H6DC remains an adjacent optional-extension story, not part of the minimum onboarding contract.
- Do not introduce or imply a bundled dotnet template suite, scaffolding CLI, or custom dotnet ef integration under this story; any such tooling needs separate ownership.

Scope In
- Define the public adoption path as documentation-first and library-first across README, docs/getting-started, examples guidance, and analyzer README.
- Keep SQLite binary-first onboarding as the shortest default path for a new project.
- Allow concise PostgreSQL parity guidance as an opt-in companion path with developer-managed connection strings.
- Include optional analyzer package guidance that stays aligned with the current coordinated package lines and SDK host baseline.
- Preserve explicit save/read, provider registration, and consumer-owned schema and deployment responsibilities.

Scope Out
- Bundled dotnet new templates, scaffolding CLIs, starter-app suites, or other application-platform packaging.
- Custom dotnet ef shims, DVault-owned design-time services, or migration automation beyond the documented consumer-owned workflow.
- Privacy compliance, key lifecycle, deletion, retention, or provider-native encryption features as part of first-run adoption.
- Provider provisioning, container orchestration, credentials, deployment infrastructure, or background job setup.
- Changing runtime save/read behavior away from explicit caller-driven boundaries.

Open questions
- none

Follow-up questions
- If product later wants scaffolding help, what separately owned minimal tool surface should exist first: <redacted> new templates, sample repo generation, or something else?
- Should future documentation maintenance collapse repeated package-version and install guidance into one canonical source to reduce drift across README, examples, and analyzer docs?
- Once 06FF43K0B0MJF45078STZ3H6DC is refined, should adopter docs add a tighter cross-link from the main getting-started path to the optional privacy extension story?

Risks
- Install and version guidance is duplicated across multiple docs, so future package-line bumps can reintroduce drift unless one canonical source remains clearly authoritative.
- Because the repo documents multiple declaration paths, adopters may still confuse the shortest SQLite-first path with richer metadata-first companion examples unless the cross-link hierarchy stays explicit.
- A future attempt to add templates or CLI scaffolding without separate ownership would blur the library-first boundary ratified by this story.

Split recommendations
- No additional split recommended; the bounded adoption-path strands are already materialized by 06FF43SFHY4EWTFQ2PAEKD8J50, 06FF43T2EK3CBYHTR287YWC5NR, and 06FF43W243BZM340V86CAXQC00.

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