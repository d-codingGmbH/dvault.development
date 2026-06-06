Investigate and tune SQL Server save strategy thresholds where the all-provider benchmark shows optimized rows falling back.

Observed seed evidence:
- `customer-profile-scale-10x1` reports `SqlServerMinimumOperationThreshold`.
- Larger SQL Server optimized rows report `SqlServerMaximumSatelliteOperationThreshold`.
- Some rows include both optimized-path wording and fallback diagnostics; reconcile the diagnostics so users can tell whether the provider strategy really executed or declined.

Scope:
- Review SQL Server provider save strategy threshold constants, eligibility gates, and diagnostics detail.
- Tune large-batch eligibility only where the staged/native path is measurably better than provider-neutral fallback and still preserves semantics.
- If the current maximum threshold is intentionally protective, keep it and improve diagnostics/guidance instead of forcing an unsafe provider path.

Podman test environment:
- Use the existing `sqlserver` Podman container for opt-in integration checks and benchmark before/after evidence.
- Run the benchmark harness through the same v0.32.0 evidence path created by the baseline task.

Acceptance criteria:
- SQL Server before/after artifacts show the effect of any changed threshold or document why no threshold change is safe.
- Diagnostics clearly distinguish selected provider strategy, provider strategy decline, and provider-neutral fallback.
- Transaction participation, cancellation behavior, idempotency, row ordering, hash key/hash diff, load timestamp, and record source behavior remain covered by tests.
- `dotnet test DVault.slnx --nologo` and `bash tools/check-format.sh` pass.