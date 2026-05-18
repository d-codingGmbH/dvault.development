[gicket-bot] PO-critic review contract

Summary
- Tracking-only epic closure audit found blocking readiness gaps.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06F2PGK4QJ0YGXK5479W83Z2J0/description.md:49-50 has `## Open Questions` followed by `- none`, so the epic itself has no unresolved current open-question block.
- `git log --oneline --decorate -n 12` on /mnt/c/Projects/DVault shows the four child AUTO-INTEGRATION commits already on develop: `1f37aac56` (link-parent satellites), `6b8268087` (effectivity ratification), `6e833b1a7` (same-hub role story), and `2b701a9ac` (docs).
- `git diff --name-only 2b701a9ac..HEAD` lists only .gicket/tickets/06F2PGK4QJ0YGXK5479W83Z2J0/* paths, so after those child integrations the epic branch adds only ticket metadata/comments, not parent-owned repository implementation or docs.
- src/DCoding.Data.DVault/DataVaultCodeFirstLinkBuilder.cs:31,47 exposes `Participant<TEntity>(string role)` and `Satellite<TSatellite>(...)`; src/DCoding.Data.DVault/DataVaultCodeFirstModelBuilder.cs:167,176 enforces explicit relationship names and distinct non-blank roles for repeated same-hub participants.
- tests/DCoding.Data.DVault.Tests/Unit/DataVaultCodeFirstLinkTests.cs:42,123 and tests/DCoding.Data.DVault.Tests/Integration/ExplicitDataVaultSaveServiceSqliteTests.cs:80,137,141-142,<redacted> cover role-bearing same-hub links, explicit save behavior, and link-parent satellite metadata/save usage.
- README.md:123,502-506,516, docs/releases/v0.13.0.md:22-26,62,81-83,107-122, and docs/model-first-governance.md:9,242 document the shipped v0.13 surface, including role-bearing same-hub links, link-parent satellites, effectivity as caller-owned generic link-parent satellite state, and explicit scope-out of dependent child keys / typed mapper parity.
- parentOf child 06F2PGKAQVVF8GEZVVC8SHFASG status done: Story: Add Code-First link-parent satellites
- parentOf child 06F2PGKV9AFAMKGJEKKZ3AXHGC status done: Story: Add Code-First effectivity satellite support
- parentOf child 06F2PGM1HQ5W1M2H8T50MZ3EEC status done: Story: Add same-as link and dependent child key modeling
- parentOf child 06F2PGM9038RXVJH0RJFYEJEV0 status done: Task: Update v0.13.0 documentation and release notes

Blocking findings
- The persisted delivery contract does not explicitly mark this tracking-only epic as closure/tracking with no parent-owned implementation slice.

Required PO actions
- Resolve the tracking-epic closure audit findings before this parent ticket can be closed.

Open issues ledger
- critic-item-1 [required-po-action] Resolve the tracking-epic closure audit findings before this parent ticket can be closed.
- critic-item-2 [blocking-finding] The persisted delivery contract does not explicitly mark this tracking-only epic as closure/tracking with no parent-owned implementation slice.

Missing examples / edge cases
- No blocking example gap was found at the epic level; runnable same-as/effectivity samples are already identified as separate follow-on work rather than part of this closure audit.

Risky assumptions
- Reviewers and downstream automation will infer 'tracking-only / no parent-owned work' from the current prose even though the contract never says that explicitly.
- Readers will not overread child ticket `06F2PGM1HQ5W1M2H8T50MZ3EEC` and its broader title as shipping dependent child key modeling, despite the epic and docs scoping that capability out.
- The existing `blocks` links to `06F2PGMFWSEC95ATBCGZ6HYT5W` and its v0.14 child tickets will continue to be treated as release-ordering context rather than reopened work on this epic.

AC / test suggestions
- Add one epic acceptance/closure statement that names the four authoritative child ticket IDs and states that no additional parent-owned code, tests, or docs are expected on the epic branch.
- Use the existing develop evidence as the closure proof set in the contract, for example the child AUTO-INTEGRATION commits `1f37aac56`, `6b8268087`, `6e833b1a7`, and `2b701a9ac` plus the cited repo/docs paths.

Implementation watchouts
- Do not let downstream work reopen this epic for dependent child key modeling, same-hub typed mapper/source-generator parity, or effectivity-specific APIs; the current repository and docs keep those outside the v0.13 public claim set.
- Keep the explicit save boundary and metadata-first/model-first alternatives intact; the current docs intentionally avoid implying a public Code-First-to-registry bridge.

Non-blocking notes
- The epic ticket folder has only `comments` and `events` subdirectories under .gicket/tickets/06F2PGK4QJ0YGXK5479W83Z2J0; no direct ticket attachment surface was observed there during this review.

Split recommendations
- No additional split is needed for the v0.13 parity epic itself once the closure-only/tracking posture is made explicit.
- If product still wants dependent child key modeling, create a separate follow-on ticket rather than widening this epic or reinterpreting child ticket `06F2PGM1HQ5W1M2H8T50MZ3EEC`.
- Keep same-hub typed mapper/source-generator parity and runnable same-as/effectivity examples as separate follow-on work.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment