# Changelog

This changelog summarizes the public release-note trail. The detailed release records remain under [docs/releases/](docs/releases/); those files are the source of truth for scope, evidence, non-goals, and validation notes.

## v0.36.0 - Binary Hash-Key Storage Adoption Guidance

- Defines the current coordinated package baseline for `8.36.0` / `net8.0` / EF Core 8 and `10.36.0` / `net10.0` / EF Core 10.
- Documents `HexString` as the compatible default hash-key storage profile and `Binary` as explicit opt-in physical storage for generated hash-key columns.
- Keeps public hash-key values as canonical lowercase hexadecimal strings across save, read, diagnostics, and support-bundle boundaries.
- Carries forward stable hash algorithm-selection guidance and records that algorithm or storage-profile changes are caller-owned compatibility work.
- Keeps package publication separate from repository package creation and verification.

See [DVault v0.36.0 Release Notes](docs/releases/v0.36.0.md).

## Recent Releases

| Release | Focus |
| --- | --- |
| [v0.35.0](docs/releases/v0.35.0.md) | Stable hash algorithm-selection guidance and dual package-line continuation. |
| [v0.34.0](docs/releases/v0.34.0.md) | DB2 provider package baseline with optimized save and PIT/bridge read strategy evidence. |
| [v0.33.0](docs/releases/v0.33.0.md) | Parallel `net8.0` and `net10.0` consumer package-version lines. |
| [v0.32.0](docs/releases/v0.32.0.md) | Benchmark-driven provider threshold evidence and review-only SQL artifact manifest lane. |
| [v0.31.0](docs/releases/v0.31.0.md) | Performance decision-tree and observability guidance. |
| [v0.30.0](docs/releases/v0.30.0.md) | Typed helper support-bundle freshness baseline. |
| [v0.29.0](docs/releases/v0.29.0.md) | Provider schema guardrails. |
| [v0.28.0](docs/releases/v0.28.0.md) | Provider read optimization evidence boundary. |
| [v0.27.0](docs/releases/v0.27.0.md) | EF lifecycle analyzer guardrails. |
| [v0.26.0](docs/releases/v0.26.0.md) | Provider-tuning diagnostics and benchmark verifier evidence. |
| [v0.25.0](docs/releases/v0.25.0.md) | ReadShape and typed helper boundary. |
| [v0.24.0](docs/releases/v0.24.0.md) | Async streaming and EF safety boundary. |
| [v0.23.0](docs/releases/v0.23.0.md) | Earlier provider/read documentation baseline. |
| [v0.22.0](docs/releases/v0.22.0.md) | Earlier production adoption and documentation baseline. |
| [v0.21.0](docs/releases/v0.21.0.md) | PIT/bridge maintenance boundary. |

Older release notes are kept in [docs/releases/](docs/releases/) for audit context.
