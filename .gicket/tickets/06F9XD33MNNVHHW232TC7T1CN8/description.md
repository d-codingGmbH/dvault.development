Tune PostgreSQL and MySQL small-batch provider save eligibility where the optimized path has more overhead than provider-neutral fallback.

Observed seed evidence:
- PostgreSQL `customer-profile-scale-10x1` optimized was slower than provider-neutral fallback despite no fallback cause.
- MySQL small rows, including `customer-profile-scale-10x1` and `customer-profile-scale-10x10`, showed provider-path overhead or minimum-threshold fallback behavior.

Scope:
- Review PostgreSQL and MySQL small-batch boundaries, staged-provider caveats, and diagnostics.
- Prefer provider-neutral fallback for tiny workloads when benchmark evidence shows the provider path is consistently slower.
- Keep larger-batch optimized wins intact, especially PostgreSQL medium/large rows and MySQL larger history rows.

Podman test environment:
- Use existing `postgres` and `mysql` Podman containers.
- PostgreSQL must be reached through the Podman network when the benchmark runs inside an SDK container.
- Preserve before/after benchmark artifacts under the v0.32.0 evidence path.

Acceptance criteria:
- Before/after artifacts show small-batch PostgreSQL/MySQL behavior and confirm that medium/large optimized rows do not regress materially.
- Diagnostics identify whether small batches declined because of threshold, unsupported shape, or deliberate provider-neutral fallback.
- Tests cover the threshold decision and strategy diagnostic status for the adjusted providers.
- `dotnet test DVault.slnx --nologo` and `bash tools/check-format.sh` pass.