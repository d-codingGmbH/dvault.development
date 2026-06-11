[gicket-bot] PO-critic review contract

Summary
- Approve for developer handoff. The delivery contract is specific, `## Open Questions` is `none`, and local ticket/repository evidence already aligns on the bounded algorithm-aware StableHashDigest scope; the remaining concern is workflow alignment because the owner branch currently differs from `develop` only in `.gicket` metadata.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06F9GF3TRG65G8MTMG7DH4PREC/description.md` contains PO handoff `ready_for_po_critic`, explicit scope/acceptance criteria, and `## Open Questions` -> `- none`.
- `.gicket/tickets/06F9GF3TRG65G8MTMG7DH4PREC/comments/06FB8N72X0B17ATT6DFAAFV5P0.md` says PO refinement processed the ticket and it is ready for handoff to role `po-critic`.
- `docs/plans/stable-hashing-contract.md` directly defines the bounded v1 set `sha256-v1`, `sha1-v1`, `sha256-128-v1`, `sha256-160-v1` and says unknown caller-supplied ids are accepted only for whole-byte lowercase hexadecimal digests.
- `src/DCoding.Data.DVault/StableHashDigest.cs` validates lowercase whole-byte hex, enforces known-id lengths `64/40/32/40`, and derives `DigestByteLength` from `Value.Length / 2` while keeping `AlgorithmId`, `Value`, and `DigestByteLength` public.
- `src/DCoding.Data.DVault/DefaultStableHashService.cs` still returns `AlgorithmId => sha256-v1`, and `src/DCoding.Data.DVault/DVaultServiceCollectionExtensions.cs` registers `IStableHashService` with `DefaultStableHashService.Instance` in `AddDVault()`.
- `tests/DCoding.Data.DVault.Tests/Unit/StableHashServiceTests.cs` covers published SHA-256 vectors, known short digest lengths for `sha1-v1`/`sha256-128-v1`/`sha256-160-v1`, custom whole-byte lower-hex acceptance, wrong-length rejects, invalid uppercase/non-hex/odd-length rejects, and the `AddDVault` override path.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- Approval assumes the ticket is intentionally being sent to `dev` even though the current branch delta versus `develop` is ticket metadata only and the repository source already matches the refined contract; the next role may discover the work is effectively confirmation/closure rather than fresh implementation.
- Approval assumes queued replay of outbox mutation `mutation-ee8323dd972bfc8a` is sufficient for the stale `06F9GF3MZHKQQ6D4SAQ0AMTKJR --blocks--> 06F9GF3TRG65G8MTMG7DH4PREC` relation, because the relation file still exists locally.

AC / test suggestions
- Keep developer verification anchored to the ticket's bounded contract: known-id length enforcement, unknown-id whole-byte lower-hex acceptance, unchanged `sha256-v1` vectors, and preserved `AddDVault` default behavior.

Implementation watchouts
- Do not widen the task into non-default algorithm auto-registration, persistence `content_hash` semantics, or rehash/migration work; those are explicitly scope-out items and the current `DefaultStableHashService`/`AddDVault()` source still preserves `sha256-v1` as the default.
- Because `git diff` versus `develop` is `.gicket`-only, developers should first verify whether any remaining action is real code/test work or only ticket/closure alignment before opening redundant source changes.

Non-blocking notes
- The ticket is operationally ready for handoff: `ticket.json` is not blocked, the PO handoff comment exists, and the delivery contract has no unresolved open questions.
- The historical blocking relation is already treated as obsolete by comment evidence, but local relation views may still show it until queued replay completes.

Split recommendations
- No split recommended; the delivery contract already keeps scope bounded to StableHashDigest validation behavior, preserved `sha256-v1` compatibility, and regression coverage.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment