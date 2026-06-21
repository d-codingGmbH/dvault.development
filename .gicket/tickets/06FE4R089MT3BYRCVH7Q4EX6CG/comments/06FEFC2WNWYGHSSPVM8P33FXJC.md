[gicket-bot] PO refinement contract

Summary
- Verified that the v0.43 ticket is already a bounded tracking story over eight downstream tasks; binary-first adoption, analyzer ergonomics, and allocation work now have a finite repository-backed contract with no remaining PO blockers.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Repository evidence dated 2026-06-15 and 2026-06-20 already fixes the baseline: UseBinaryFirstProfile and UseDataVaultBinaryFirstProfile are the recommended new-project path, while AddDVault and UseDataVault keep the compatible HexString default for existing persisted models.
- Public DVault hash-key values remain canonical lowercase hexadecimal strings even when binary physical storage is selected; changing storage profile, algorithm id, digest length, or truncation after persistence is caller-owned compatibility work with no automatic rehash, backfill, dual-write, repair, or migration.
- Analyzer ergonomics are bounded to optional local tooling: DCoding.Data.DVault.Analyzers stays PrivateAssets=all, ships one net10.0 analyzer asset for both coordinated package lines, and the repository does not claim pure .NET 8 SDK analyzer-host compatibility.
- Runtime efficiency claims are evidence-bound, not promise-bound: timing or allocation promotion must cite preserved benchmark artifacts and the provider evidence matrix, and targeted allocation work should improve or hold the targeted metric instead of introducing unmeasured provider-wide claims.
- The live split is already materialized as four outgoing blocks links and eight outgoing relates links across eight downstream tasks; no new child tickets, relation changes, attachments, or planning documents were created in this refinement. The downstream tickets are 06FE4R0H98K42XJY1NEDQX8KB4, 06FE4R0TBG8JP5WA2SHXKH438M, 06FE4R13DS6S2ZTGYTHA458HGM, 06FE4R1C96NBSNMM7AFDTHJ7A4, 06FE4R1N2ADN77NDFDP4GR7020, 06FE4R1XJVQZTQ8S9WN2YE3ZKW, 06FE4R261S2FSQ786S4F4JE90R, and 06FE4R2EGQ444EGPKZBRZCDEV8.
- The only incoming blocks relation comes from done ticket 06FE4QRMXVGJVA65ZR5MZ817K8; per policy it is historical upstream context, not an active PO blocker.

Scope In
- Ratify the v0.43 story-level contract that new projects should adopt the binary-first physical storage profile while the public hash-key boundary remains lowercase hexadecimal strings.
- Define bounded analyzer guidance for binary-first recommendation versus legacy-compatible HexString usage, including the optional and local analyzer-package posture.
- Define the evidence-first runtime efficiency boundary for hash canonicalization, hash diff, save preparation, and provider binary-vs-hex measurement work.
- Coordinate the already-created downstream guide, dry-run tooling, ergonomics, benchmark, hotspot, implementation, and documentation tasks as the delivery path for this story.

Scope Out
- Automatic database migration execution, automatic rehash or backfill, dual-write or repair behavior, or silent conversion of existing persisted hash-key storage.
- Changing the public DVault hash-key value type from lowercase hexadecimal string to byte array or introducing provider-side SQL hashing.
- Unmeasured cross-provider timing or allocation claims, performance SLAs, or broad runtime promises beyond preserved benchmark evidence.
- Analyzer or runtime behavior that treats existing HexString projects as invalid solely because binary-first is recommended for new projects.
- Broader provider tuning, package-line compatibility, or release-publication work outside the existing downstream tasks.

Open questions
- none

Follow-up questions
- Should the live story-to-task relation set be normalized later from mixed blocks and relates links into explicit parentOf links for reporting clarity, or is the current dependency graph the intended long-term representation?
- After the provider binary-vs-hex matrix lands, do any providers need separate follow-up tickets if the evidence supports materially different adoption guidance instead of one shared v0.43 recommendation?
- If hotspot profiling shows the dominant allocation cost is outside canonicalization, hash diff, or save preparation, should any broader optimization work move to a fresh post-v0.43 ticket instead of widening this story?

Risks
- Without disciplined artifact citation, downstream docs could overstate binary-vs-hex wins or allocation reductions beyond the providers and scenarios actually benchmarked.
- Because the live parent story is currently represented through mixed blocks and relates links rather than explicit parentOf links, reporting or workflow automation may interpret the split less clearly until relations are normalized.
- Analyzer guidance can regress adoption clarity if it treats legacy-compatible HexString storage as a product error instead of a supported compatibility posture.

Split recommendations
- No new split is needed; the migration and adoption lane is already materialized as 06FE4R0H98K42XJY1NEDQX8KB4 and 06FE4R0TBG8JP5WA2SHXKH438M.
- No new split is needed; the analyzer and ergonomics lane is already materialized as 06FE4R13DS6S2ZTGYTHA458HGM and 06FE4R1C96NBSNMM7AFDTHJ7A4.
- No new split is needed; the evidence, optimization, and docs lane is already materialized as 06FE4R1N2ADN77NDFDP4GR7020, 06FE4R1XJVQZTQ8S9WN2YE3ZKW, 06FE4R261S2FSQ786S4F4JE90R, and 06FE4R2EGQ444EGPKZBRZCDEV8.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 5
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment