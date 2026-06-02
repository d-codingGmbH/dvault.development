## Developer Delivery

Summary
- Added the authoritative stored-procedure and provider-specific SQL artifact gate to `docs/performance-profiles.md`.
- The gate states these artifacts are not DVault's default save/read path, require explicit consumer opt-in, are design-time outputs only, and remain consumer-owned for deployment, invocation, versioning, rollback, cleanup, transactions, credentials, environment selection, and observability.
- The gate forbids DVault auto-created runtime dispatch, automatic execution, procedure dispatchers, EF interceptor expansion, migration hooks, deployment automation, default provider routing, and automatic synchronization with migrations, live schema, metadata changes, model-first import/export, or support-bundle refreshes.
- Future implementation tickets must reuse the staged provider ingestion evidence posture: representative request-bound diagnostics, benchmark artifact triplet plus run context, visible skipped/unsupported rows, exact provider/workload evidence, and parity with explicit DVault semantics.

Verification
- `bash tools/check-format.sh` passed.
- `timeout 20s git diff --check -- docs/performance-profiles.md` passed.
- `rg -n "Stored-Procedure And Provider-Specific SQL Artifact Gate|not DVault's default|design-time outputs|automatically synchronize|staged provider ingestion profile" docs/performance-profiles.md` found the required gate language.

No product clarification is needed.