## Developer Delivery

Summary
- Updated `docs/performance-profiles.md` to make the v0.39.0 provider-evidence baseline explicit, link the evidence matrix and gap matrix, and separate completed timing evidence from follow-up recommendations.
- Added `docs/releases/v0.39.0.md` as a docs-only provider-evidence release note with caveats, posture semantics, and DB2 boundaries.
- Added a matching `CHANGELOG.md` entry that points to the new release note without documenting new consumer package-version lines.

Verification
- `bash tools/check-format.sh` passed.
- `git diff --check -- docs/performance-profiles.md docs/releases/v0.39.0.md CHANGELOG.md` passed.
- A changed-file search for bare `0.39.0`, `8.39.0`, and `10.39.0` consumer package-version claims returned no matches; the changed files only use the `v0.39.0` release label.

Notes
- No benchmarks were rerun and no benchmark schemas, provider implementation, package compatibility, publication, or release automation surfaces were changed.
- `dotnet build` and `dotnet test` were not run because this is a documentation-only change; the repository format check passed.