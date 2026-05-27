# DVault Dotnet EF Design-Time Workflow

Status: relocated documentation entrypoint

The authoritative design-time workflow note lives at [docs/architecture/dvault-dotnet-ef-design-time-workflow.md](docs/architecture/dvault-dotnet-ef-design-time-workflow.md).

This root entrypoint is retained for ticket and release validation surfaces that reference `dvault-dotnet-ef-design-time-workflow.md` directly. Use the architecture note for migration guardrails, reviewed `dvault.model.v1` artifact drift, optional live-schema drift, aggregate preflight, and support-bundle guidance.

For the v0.21.0 PIT and bridge documentation boundary, this file is adjacent migration and drift context only. It does not add PIT/bridge-specific automation, automatic maintenance, delete-aware bridge maintenance, registry-backed PIT as-of reads, or provider optimization claims beyond the documented SQLite read optimization and provider-neutral fallback boundary.
