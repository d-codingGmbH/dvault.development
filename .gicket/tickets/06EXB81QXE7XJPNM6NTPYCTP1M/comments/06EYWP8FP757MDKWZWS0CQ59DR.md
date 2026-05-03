[gicket-bot] PO refinement contract

Summary
- Verified ticket, relation, and repository evidence for one-member-per-file enforcement; scope is fixed to the six packable packages, existing core multi-declaration files are explicitly in scope, and no planning artifacts were created.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Automation comments added on 2026-05-03 are lease/follow-up notes only; there is no human scope change to absorb.
- Relation context is already coherent: parent story `06EXB80ZNQTTGT6VN2DKEDGB0M` tracks public API quality, upstream API snapshot ticket `06EXB81FSWAA6N1HMYQ0CM4S8G` is `done`, and this ticket still blocks packaging task `06EXB828EAG5QE3WDR503GTBY8`.
- No child tickets, relation writes, attachments, or planning documents were created in this refinement pass.
- Repository evidence fixes the project boundary: `src/DCoding.Data.DVault` and provider packages `MySql`, `Oracle`, `Postgres`, `Sqlite`, and `SqlServer` are in scope; non-packable `src/DCoding.Data` is out of scope.

Scope In
- A repository-enforced one-public/protected-top-level-declaration-per-file rule for the six packable source projects.
- Remediation or explicitly documented exceptions for the current core-package multi-declaration files before the rule is treated as passing.
- Actionable diagnostics that report violating source file paths and do not scan `obj`, `bin`, or other generated/build artifacts.
- Provider package source inclusion under the same rule, even though those packages currently mostly expose one registration extension file each.

Scope Out
- Non-packable `src/DCoding.Data`, test projects, benchmarks, and build/generated output as enforcement targets.
- XML-doc enforcement and package-aware API snapshot design, which are already handled by done sibling tickets `06EXB817Q8RAXCQH5QQR5RFY34` and `06EXB81FSWAA6N1HMYQ0CM4S8G`.
- NuGet publication policy, package content verification behavior, or broader release governance beyond enabling this source-level rule.
- A broader repository-wide rule for internal/private-only declarations.

Open questions
- none

Follow-up questions
- If future packable provider packages are added, should the enforcement mechanism auto-discover packable `src/DCoding.Data.DVault.*` projects or require an explicit allowlist update?
- After the public/protected baseline is stable, does the team want the same rule extended to internal top-level declarations or to remain limited to release-surface code?

Risks
- Enabling the rule without first addressing the existing core multi-declaration files will create an immediate failing baseline.
- A path-only scan that is not project-aware could accidentally include `obj` output or the non-packable `src/DCoding.Data` anchor and create noisy failures.
- Over-broad exception handling for partial types or provider registration files could weaken the rule enough that future regressions slip through.

Split recommendations
- No additional planning split is recommended; this ticket is already the dedicated downstream work item for one-member-per-file enforcement under story `06EXB80ZNQTTGT6VN2DKEDGB0M`, while XML-doc and API-snapshot quality work is already separated into done sibling tickets.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment