[gicket-bot] PO-critic review contract

Summary
- Contract is repository-backed, Open Questions is explicitly empty, and the story is already split into bounded downstream work, so this is ready for developer handoff.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06FE4R089MT3BYRCVH7Q4EX6CG/description.md contains PO handoff decision `ready_for_po_critic` and `## Open Questions` -> `- none`.
- src/DCoding.Data.DVault/DataVaultOptions.cs defines `UseBinaryFirstProfile()` to switch to `DataVaultHashKeyStorageProfile.Binary`; src/DCoding.Data.DVault/DataVaultModelBuilderExtensions.cs defines `UseDataVaultBinaryFirstProfile()`; src/DCoding.Data.DVault/Modeling/DataVaultConventions.cs keeps `DataVaultConventions.Default` on `DataVaultHashKeyStorageProfile.HexString` with profile name `default`.
- docs/releases/v0.38.0.md states new projects should use `AddDVault(options => options.UseBinaryFirstProfile())` and `modelBuilder.UseDataVaultBinaryFirstProfile()`, while `AddDVault()` and `UseDataVault()` remain the compatible defaults for existing `HexString` models.
- docs/plans/hash-key-storage-profile-contract.md and hash-key-footprint.md both keep public and logical hash-key values as lowercase hex strings and state that storage-profile or algorithm changes after persistence are caller-owned migration work with no automatic migration, rehash, backfill, dual-write, or repair.
- src/DCoding.Data.DVault.Analyzers/README.md keeps analyzer usage local with `PrivateAssets=all`, one `net10.0` analyzer asset, and a `.NET 10 SDK` build-host baseline; it explicitly says pure `.NET 8 SDK` analyzer consumption is not validated.
- docs/plans/performance-evidence-benchmark-artifact-contract.md requires the benchmark triplet `benchmark-summary.md`, `benchmark-summary.csv`, and `benchmark-summary.json` plus allocation metrics on completed rows, and docs/plans/provider-optimization-evidence-matrix.md limits measured claims to `completed-timing` rows with preserved run context.
- .gicket/tickets/06FE4QRMXVGJVA65ZR5MZ817K8/ticket.json shows the only incoming blocker ticket is `done`, matching the parent-ticket clarification that it is historical context only.
- Parent-ticket event files `06FE4R376JDDC1NGJMRT9X6YSW.json`, `06FE4R39GJZJYHG27KJP6HEW34.json`, `06FE4R3BX546WMWZKGDV3B4BHG.json`, and `06FE4R3E5W0Y62BBB8MJ3KF0R8.json` show four outgoing `blocks` links; `06FE4VC696KQFAXGGB15W7SRXG.json` through `06FE4VD221FAM6DGC6S62PFDZR.json` show eight `relates` additions, with matching `parentOf` removals through `06FE4VC8PC53BZ3RAY7ZHMKTB0.json` to `06FE4VD3X7HQNJSVXV7J2AQPMW.json`.
- `git diff --stat 8008a148036bb834c0315065ce9c9828280870fe..HEAD` returned no diff, and `git show --name-only d843bc18d`, `6c227fc9e`, and `8008a1480` show only `.gicket/tickets/06FE4R089MT3BYRCVH7Q4EX6CG/...` metadata writes, confirming this is still a pre-development ticket-quality branch.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- If the provider binary-vs-hex matrix ends up supporting materially different guidance per provider, the downstream docs task should add provider-specific adoption examples instead of collapsing everything into one shared recommendation.
- If hotspot profiling shows the dominant allocation cost is outside canonicalization, hash diff, or save preparation, that work should move to a fresh post-v0.43 ticket rather than widen this story.

Risky assumptions
- Downstream benchmark and docs work must keep citing exact artifact triplets and provider-matrix row identities instead of promoting SQLite-local storage-footprint evidence as cross-provider performance proof.
- Downstream analyzer and ergonomics work must preserve `HexString` as a supported compatibility posture and must not convert the binary-first recommendation into an error posture for existing installations.
- The current mixed `blocks` and `relates` graph is assumed to be good enough for workflow and reporting until any later normalization; the parent ticket already notes automation and reporting ambiguity as a risk.

AC / test suggestions
- Require the benchmark-matrix lane to cite the exact artifact label and matrix row identity `scenario`, `provider`, `baseline`, and `posture` before any docs or release note promotes a win.
- Require the docs-update lane to restate the explicit non-goals: no automatic migration, rehash, backfill, dual-write, or repair.
- Require analyzer and ergonomics work to prove one non-diagnostic path for existing `HexString` configurations while retaining `PrivateAssets=all` and `.NET 10 SDK` build-host guidance.

Implementation watchouts
- Do not rename or semantically widen the existing public APIs `UseBinaryFirstProfile()` and `UseDataVaultBinaryFirstProfile()`; the contract is already anchored on those names.
- Do not imply `AddDVault()` or `UseDataVault()` changed defaults; `DataVaultConventions.Default` still uses `HexString` with profile name `default`.
- Do not promote SQLite-local `hash-key-footprint.md` data as cross-provider timing evidence; the provider matrix keeps `storage-footprint` and `completed-timing` evidence separate.
- Allocation work should follow hotspot evidence first; the benchmark artifact contract requires completed rows to carry allocation fields and the targeted metric to improve or hold.

Non-blocking notes
- The contract's `Follow-Up Questions` section contains downstream steering questions, but they are not persisted under `## Open Questions`, so they do not block `approve_for_dev` under the stated rule.

Split recommendations
- No new split needed; migration and adoption is already covered by 06FE4R0H98K42XJY1NEDQX8KB4 and 06FE4R0TBG8JP5WA2SHXKH438M.
- No new split needed; analyzer and ergonomics is already covered by 06FE4R13DS6S2ZTGYTHA458HGM and 06FE4R1C96NBSNMM7AFDTHJ7A4.
- No new split needed; evidence, optimization, and docs is already covered by 06FE4R1N2ADN77NDFDP4GR7020, 06FE4R1XJVQZTQ8S9WN2YE3ZKW, 06FE4R261S2FSQ786S4F4JE90R, and 06FE4R2EGQ444EGPKZBRZCDEV8.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment