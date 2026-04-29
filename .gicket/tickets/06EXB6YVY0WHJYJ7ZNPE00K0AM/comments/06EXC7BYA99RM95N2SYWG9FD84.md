[gicket-bot] PO-critic review contract

Summary
- The delivery contract is sufficiently refined for developer handoff: scope, exclusions, acceptance criteria, validation expectations, and known risks are concrete, and Open Questions is explicitly closed as none.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- The delivery contract states the v1 scope targets src/DVault and validation through tests/DVault.Tests.
- Repository branch state snapshot lists src-roots: [src/DVault] and test-roots: [tests/DVault.Tests].
- The branch state snapshot shows tests/DVault.Tests exists as a directory.
- The contract Scope In covers XML documentation output, deterministic build settings, SourceLink/package metadata, and local package/symbol inspection.
- The contract Scope Out excludes external publishing, public API shape changes solely for documentation warnings, broad documentation content, multi-project packaging strategy, and workflow label/status changes.
- The Acceptance Criteria require XML documentation artifacts, missing-doc reporting consistent with repository warning policy, deterministic build settings, SourceLink where supported, and local package/symbol artifacts.
- The Definition of Done requires exact build/package commands or documented environmental blockers and local verification of generated package and symbol artifacts.
- The Open Questions section contains only 'none'.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- The contract intentionally leaves SourceLink verification conditional on available repository host or remote metadata, which is acceptable but should be documented by implementation if unavailable.

AC / test suggestions
- During implementation validation, capture the exact dotnet build/pack/test commands used and verify XML documentation plus symbol/source metadata in the local artifacts.

Implementation watchouts
- Keep warning behavior aligned with any existing repository warning policy instead of introducing a separate XML documentation warning standard.
- Do not commit generated nupkg, snupkg, bin, or obj artifacts after local inspection.
- Treat src/DVault as the single package output owner and tests/DVault.Tests as validation scope only.

Non-blocking notes
- Follow-up questions about hard-error documentation policy, CI package verification, and broader API documentation standards are explicitly deferred and do not block this task.
- No ticket comments are present in the snapshot.

Split recommendations
- none

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment