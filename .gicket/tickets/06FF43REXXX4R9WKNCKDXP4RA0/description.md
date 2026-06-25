<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Parent-child hygiene is now aligned: the archived duplicate child is no longer in the live parentOf set, the parent description now explains the analyzer-coverage mapping, and the ticket can return to PO-critic.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Ratify the current first-run default as the root README plus docs/getting-started SQLite-first binary-first path with explicit AddDVault(...), provider registration, schema creation, IDataVaultSaveService, and IDataVaultReadService usage.
- Treat runnable examples as companion proofs, not the primary onboarding surface: 06FF43SFHY4EWTFQ2PAEKD8J50 covers the minimal SQLite path and 06FF43T2EK3CBYHTR287YWC5NR covers bounded PostgreSQL parity notes.
- Treat analyzer guidance as optional compile-time adoption support, not a runtime prerequisite: 06FF43W243BZM340V86CAXQC00 ratifies the current single net10.0 analyzer asset and .NET 10 SDK host baseline across the 8.47.0 and 10.47.0 lines.
- Treat 06FBSBW6HDT15D1KGVD7XBQXM8 as historical evidence only, not as a formal live dependency of this parent story; the accepted analyzer coverage tracked here remains completed child 06FF43W243BZM340V86CAXQC00.
- Keep privacy coverage separate from first-run adoption; 06FF43K0B0MJF45078STZ3H6DC remains an adjacent optional-extension story, not part of the minimum onboarding contract.
- Do not introduce or imply a bundled dotnet template suite, scaffolding CLI, or custom dotnet ef integration under this story; any such tooling needs separate ownership.

### Scope In
- Define the public adoption path as documentation-first and library-first across README, docs/getting-started, examples guidance, and analyzer README.
- Keep SQLite binary-first onboarding as the shortest default path for a new project.
- Allow concise PostgreSQL parity guidance as an opt-in companion path with developer-managed connection strings.
- Include optional analyzer package guidance that stays aligned with the current coordinated package lines and SDK host baseline.
- Keep the parent tracking set aligned with the three intended bounded child tickets: 06FF43SFHY4EWTFQ2PAEKD8J50, 06FF43T2EK3CBYHTR287YWC5NR, and 06FF43W243BZM340V86CAXQC00.

### Scope Out
- Bundled dotnet new templates, scaffolding CLIs, starter-app suites, or other application-platform packaging.
- Custom dotnet ef shims, DVault-owned design-time services, or migration automation beyond the documented consumer-owned workflow.
- Privacy compliance, key lifecycle, deletion, retention, or provider-native encryption features as part of first-run adoption.
- Provider provisioning, container orchestration, credentials, deployment infrastructure, or background job setup.
- Changing runtime save/read behavior away from explicit caller-driven boundaries.

## Acceptance Criteria
- The primary public onboarding surface identifies the root README and docs/getting-started as the shortest SQLite-first binary-first path and keeps DVault framed as an EF Core library family.
- Runnable examples are positioned as companion proofs: SQLite requires no external infrastructure and PostgreSQL remains opt-in behind DVAULT_TEST_POSTGRES_CONNECTION_STRING.
- Analyzer documentation is explicit that the package is optional, version-aligned with the selected package line, and validated on a .NET 10 SDK host rather than treated as a first-run runtime prerequisite.
- The adoption-path contract preserves explicit AddDVault/provider registration, explicit schema creation or migrations owned by the app, and explicit IDataVaultSaveService/IDataVaultReadService usage.
- The live parentOf set and the parent ticket contract both identify the same bounded child set: 06FF43SFHY4EWTFQ2PAEKD8J50, 06FF43T2EK3CBYHTR287YWC5NR, and 06FF43W243BZM340V86CAXQC00, with archived duplicate 06FF43V3NVWER898D8CKXJ74D8 excluded.
- No bundled template suite, scaffolding CLI, or custom dotnet ef integration is introduced or implied unless a later separately owned ticket approves it.

## Definition of Done
- README, docs/getting-started, examples guidance, and analyzer guidance remain consistent about the library-first onboarding posture, binary-first recommendation for new projects, and explicit save/read boundaries.
- The live parentOf set and parent story contract remain aligned with the completed bounded child work in 06FF43SFHY4EWTFQ2PAEKD8J50, 06FF43T2EK3CBYHTR287YWC5NR, and 06FF43W243BZM340V86CAXQC00, while archived duplicate 06FF43V3NVWER898D8CKXJ74D8 stays outside the tracked child set.
- Referenced architecture documents continue to keep privacy and design-time workflow behind explicit opt-in or consumer-owned boundaries rather than expanding DVault into an application platform.
- Any future tooling expansion beyond docs, examples, or analyzers is routed to a separate ticket or project instead of silently widening this story.

## Implementation Notes
- docs/getting-started.md already establishes the authoritative first-run baseline: binary-first registration, SQLite-first quickstart, explicit save/read services, optional PostgreSQL parity note, and consumer-owned schema lifecycle.
- examples/README.md already positions the SQLite and PostgreSQL quickstarts as runnable companions to the README and docs/getting-started path rather than as a template suite or hidden orchestration platform.
- src/DCoding.Data.DVault.Analyzers/README.md already documents the analyzer package as optional compile-time guidance with the current 8.47.0 and 10.47.0 line alignment and .NET 10 SDK host requirement.
- Parent ticket revision 06FFZ171PZMHZQBQ0Q3MW70KDM now records that archived duplicate 06FF43V3NVWER898D8CKXJ74D8 was retired from the tracked decomposition and that done story 06FBSBW6HDT15D1KGVD7XBQXM8 is historical evidence only while 06FF43W243BZM340V86CAXQC00 remains the accepted tracked analyzer child.
- docs/architecture/dvault-dotnet-ef-design-time-workflow.md explicitly keeps dotnet ef ownership in the consumer project and rejects a DVault-provided CLI shim or design-time service surface.
- docs/architecture/dvault-v1-optional-privacy-extension-boundary.md and docs/production-adoption-checklist.md keep privacy as an optional package seam and production readiness as caller-owned guidance, which reinforces the library-focused adoption boundary.

## Open Questions
- none

## Follow-Up Questions
- If product later wants scaffolding help, what separately owned minimal tool surface should exist first: dotnet new templates, sample repo generation, or something else?
- Should future documentation maintenance collapse repeated package-version and install guidance into one canonical source to reduce drift across README, examples, and analyzer docs?
- Once 06FF43K0B0MJF45078STZ3H6DC is refined, should adopter docs add a tighter cross-link from the main getting-started path to the optional privacy extension story?

## Risks
- Install and version guidance is duplicated across multiple docs, so future package-line bumps can reintroduce drift unless one canonical source remains clearly authoritative.
- Because the repo documents multiple declaration paths, adopters may still confuse the shortest SQLite-first path with richer metadata-first companion examples unless the cross-link hierarchy stays explicit.
- A future attempt to add templates or CLI scaffolding without separate ownership would blur the library-first boundary ratified by this story.

## Split Recommendations
- No additional split recommended; the bounded adoption-path strands are already materialized by 06FF43SFHY4EWTFQ2PAEKD8J50, 06FF43T2EK3CBYHTR287YWC5NR, and 06FF43W243BZM340V86CAXQC00.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Define how DVault improves first-run adoption while staying a focused EF Core library. Acceptance: chooses examples, analyzer guidance, and docs over a bundled CLI template/tool-suite unless a later separate project owns templates.