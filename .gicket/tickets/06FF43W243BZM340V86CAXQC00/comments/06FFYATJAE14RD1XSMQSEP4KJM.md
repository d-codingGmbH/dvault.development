[gicket-bot] PO-critic review contract

Summary
- Ticket contract is clear and repository evidence supports an audit-backed no-work closure that keeps one net10.0 analyzer asset and the .NET 10 SDK host baseline for both visible package lines.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `docs/plans/analyzer-package-compatibility-audit.md` records the v0.47.0 decision to keep one `net10.0` analyzer asset and a `.NET 10 SDK` build-host baseline for both `8.47.0` and `10.47.0`, and it says pure `.NET 8 SDK` analyzer consumption is not proven.
- `src/DCoding.Data.DVault.Analyzers/DCoding.Data.DVault.Analyzers.csproj` targets only `net10.0` and packs the analyzer DLL/XML under `analyzers/dotnet/cs/`.
- `tools/pack-release-packages.sh` packs the analyzer project via `pack_analyzer_line` for `8.47.0` and again for `10.47.0` without changing the analyzer target framework.
- `tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj` multi-targets `net8.0;net10.0` and references the analyzer project with `PrivateAssets=all` and `SetTargetFramework=TargetFramework=net10.0`.
- `README.md`, `docs/manual-nuget-publication.md`, `docs/package-compatibility.md`, and `tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs` all preserve the same `.NET 10 SDK` analyzer-host baseline; `PackageVerifier.cs` also expects analyzer assets at `analyzers/dotnet/cs/`.
- `git log -1 --oneline --decorate` shows HEAD `83731e6cae` on branch `ticket/06FF43W243BZM340V86CAXQC00-task-prototype-analyzer-package-retargeting-if-a`, and `git diff --name-only <redacted>..83731e6cae` lists only `.gicket/tickets/06FF43W243BZM340V86CAXQC00/**`, which is consistent with a pre-dev no-work handoff.
- Archived `.gicket/archive/06FF43V3NVWER898D8CKXJ74D8/ticket.json` is `done`, and repository relations mark it as a duplicate of done story `06FBSBW6HDT15D1KGVD7XBQXM8`, so the earlier audit dependency is closed rather than open.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- Any future doc or release statement that implies the `8.47.0` analyzer package is validated on a pure `.NET 8 SDK` host would exceed the repository-backed proof accepted by this ticket.
- Developers must treat this as a bounded no-work/closure handoff unless they find a concrete mismatch against the documented baseline; otherwise they could accidentally reopen retargeting scope that the audit rejected.

AC / test suggestions
- Before closing, dev/test can reuse the existing proof points named in the contract: analyzer csproj target, pack script line behavior, package-verifier analyzer-path checks, and the integration test reference with `SetTargetFramework=TargetFramework=net10.0`.
- If product later wants pure `.NET 8 SDK` analyzer consumption, create a separate ticket with an explicit build lane that compiles a `net8.0` consumer on a `.NET 8 SDK` host.

Implementation watchouts
- Do not retarget `DCoding.Data.DVault.Analyzers` to `net8.0` or add per-line analyzer assets under this ticket; that would contradict the accepted audit decision.
- Do not broaden README or package-verifier wording into a pure `.NET 8 SDK` analyzer-support claim; the validated baseline is one `net10.0` analyzer asset on a `.NET 10 SDK` host for both package lines.

Non-blocking notes
- The ticket title still reads like a conditional implementation task, but the persisted delivery contract is explicit that the authoritative outcome is audit-backed no-work closure; that mismatch is notable but not a handoff blocker.

Split recommendations
- No split recommended; the current ticket is already bounded to ratifying the existing analyzer baseline, and any pure `.NET 8 SDK` analyzer-host expansion belongs in a separate follow-up ticket.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment