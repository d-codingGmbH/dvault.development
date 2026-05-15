[gicket-bot] PO refinement contract

Summary
- Refined the story around completing the adopter-facing documentation path that connects the current v0.8 lifecycle guardrails and v0.9 adoption examples/checklist. Existing repository context already establishes the six-package NuGet family, provider-neutral plus provider-specific setup, model declaration paths, migration and drift boundaries, and the production checklist baseline, so no PO-blocking questions remain.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Use the current coordinated package family as the documentation baseline: DCoding.Data.DVault plus DCoding.Data.DVault.MySql, DCoding.Data.DVault.Oracle, DCoding.Data.DVault.Postgres, DCoding.Data.DVault.Sqlite, and DCoding.Data.DVault.SqlServer.
- Do not present src/DCoding.Data as an installable consumer package; it is documented as a non-packable source-root build anchor.
- Keep README and checklist guidance NuGet-based for released consumer setup, with project/source references reserved only for repository development or unpublished local work.
- The supported migration guardrail flow is consumer-owned and preflight-driven; DVault does not ship a dotnet ef shim, intercept EF CLI commands, auto-run migrations, or apply schema repairs.
- Live-schema drift evidence is SQLite-first in the current v1 boundary; other providers should be described as unsupported or external opt-in evidence unless the repository adds first-class readers.
- Analyzer and Testcontainers guidance should appear only if corresponding packages, examples, or test assets are actually present in the repository.
- Examples should stay intentionally small and either build as-is or state exact prerequisites and commands.

Scope In
- Refresh README adoption guidance so Code-First, metadata-first, and model-first paths are presented as compatible choices for different ownership needs.
- Refresh examples documentation for runnable SQLite and PostgreSQL quickstarts, package installation, provider selection, service registration, migrations, diagnostics, drift checks, read helpers, save boundaries, and interceptors where currently supported.
- Maintain or update the production checklist so it distinguishes required production readiness steps from optional evidence or advanced features.
- Tie v0.8 lifecycle guardrails to the v0.9 adoption story, including design-time diagnostics, migration guardrail validation, model-first drift reports, and documented live-schema drift limits.
- Keep package names, version examples/placeholders, provider extension names, and documented commands consistent across README, examples, and the checklist.
- Keep known limitations visible, especially around provider live-schema support, EF CLI ownership, and non-promised automation.

Scope Out
- No marketing landing page or product positioning rewrite.
- No new product behavior, provider implementation, analyzer package, Testcontainers package, or release automation work.
- No undocumented feature promises or forward-looking API guarantees.
- No replacement for API reference documentation.
- No new subtickets created by this refinement; larger future documentation expansions should be documented as recommendations only.

Open questions
- none

Follow-up questions
- Should future documentation add a separate deep-dive guide for provider-specific production operations after the general checklist is complete?
- Should a later story add or expand Testcontainers-backed integration examples if the project decides to publish and support that path?
- Should a later release introduce dedicated analyzer package documentation if analyzer packages become part of the coordinated package family?

Risks
- Documentation can become misleading if it names packages or helper APIs not present in the current repository baseline.
- Provider-specific live drift guidance may overpromise support unless SQLite-first limits are kept explicit.
- A broad adoption document could grow into API reference duplication unless examples stay small and link to detailed source documents.

Split recommendations
- No split is required for this story. If implementation grows too large, keep this ticket focused on README/examples/checklist alignment and move future provider-specific deep dives, Testcontainers examples, or analyzer documentation into separate follow-up work.

Persisted contract coverage
- acceptance-criteria items: 7
- definition-of-done items: 6
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment