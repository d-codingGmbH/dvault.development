Evaluate whether the Oracle high-volume satellite save threshold should be changed, refined, or documented as an intentional safety boundary.

Observed seed evidence:
- `customer-profile-scale-10000x10` reported `OracleMaximumSatelliteOperationThreshold`.
- The optimized Oracle row remained close to provider-neutral fallback and much slower than the conventional EF comparison row, so the high-volume boundary deserves a focused review.

Scope:
- Review Oracle direct batching and any staged/native bulk boundary for large satellite batches.
- Consider threshold, chunking, or diagnostics changes only when benchmark evidence proves a stable win without semantic drift.
- Do not introduce automatic stored procedure deployment, consumer schema ownership, or a runtime artifact invocation path.

Podman test environment:
- Use the existing `oracle` Podman container for opt-in integration checks and benchmark before/after evidence.
- Keep the run under the v0.32.0 artifact/evidence path and include fallback causes in the report.

Acceptance criteria:
- The ticket produces Oracle before/after benchmark artifacts or a documented no-change decision with measured rationale.
- Large satellite saves preserve transaction behavior, rollback on provider failure, cancellation boundaries, ordering, hash key/hash diff, load timestamp, record source, and idempotency.
- Diagnostics make the high-volume decline reason actionable.
- `dotnet test DVault.slnx --nologo` and `bash tools/check-format.sh` pass.