[gicket-bot] PO refinement contract

Summary
- Refined the ticket around a caller-owned design-time preflight dry-run artifact for HexString-to-Binary hash-key storage migrations, ratified the required manifest facts from existing repository contracts, and kept the work as one bounded task. No child tickets, relation edits, description updates, attachments, or planning documents were materialized in this refinement.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The dry-run belongs in the caller-owned design-time preflight lane documented in docs/architecture/dvault-dotnet-ef-design-time-workflow.md; this ticket does not add a DVault-owned dotnet ef shim, IDesignTimeServices surface, or automatic migration runner.
- The manifest/report is for adopter-owned compatibility review of an existing persisted HexString-to-Binary storage-profile change; public and EF-boundary hash-key values remain canonical lowercase hexadecimal strings.
- The visible v1 stable-hash baseline is finite and already documented in repository evidence: sha256-v1, sha1-v1, sha256-128-v1, and sha256-160-v1.
- No bounded ticket writes were applied; live relations remain an incoming blocks link from 06FE4R0H98K42XJY1NEDQX8KB4, an outgoing blocks link to 06FE4R2EGQ444EGPKZBRZCDEV8, and an incoming relates link from 06FE4R089MT3BYRCVH7Q4EX6CG.

Scope In
- Add a dry-run artifact on the caller-owned design-time preflight path that reports planned DVault hash-key storage changes for an existing model moving from HexString to Binary.
- Enumerate all DVault-owned HashKey and ParticipantReference columns in the selected model boundary across generated hubs, links, satellites, PITs, and bridges.
- Surface the compatibility facts already defined by the storage-profile contract: storage profile, provider store type, provider value format, EF CLR model type, conversion behavior, algorithmId, digestByteLength, and digest encoding.
- Fail closed when the comparison detects drift outside the intended storage-profile change.
- Produce deterministic output suitable for CI review without writing to the database.

Scope Out
- Automatic backfill, dual-write, repair, reconcile, or rehash behavior.
- Changing stable-hash algorithmId, digest length, truncation, or caller-facing hash-key representation.
- Adding a DVault-owned dotnet ef shim, IDesignTimeServices package surface, or automatic migration application.
- Broader provider-expansion or live-schema capture workflows beyond the bounded preflight evidence needed for this dry-run.

Open questions
- none

Follow-up questions
- After this lands, should a later ticket attach the dry-run artifact to support-bundle style diagnostics, or is a standalone preflight output sufficient for v1?
- When the downstream blocked ticket 06FE4R2EGQ444EGPKZBRZCDEV8 resumes, does it need any additional manifest field beyond the v1 storage-profile contract facts defined here?
- Should later work add richer provider-specific live-schema evidence for providers whose catalog readers are currently outside the main contract baseline?

Risks
- If the artifact silently omits any DVault-owned HashKey or ParticipantReference column, application owners could approve an incomplete migration plan; implementation should fail closed on incomplete evidence.
- Cross-provider CI stability depends on explicit normalization and ordering; otherwise equivalent dry-runs may churn review diffs.
- Because this ticket sits between an incoming blocks relation from 06FE4R0H98K42XJY1NEDQX8KB4 and an outgoing blocks relation to 06FE4R2EGQ444EGPKZBRZCDEV8, scope drift on adjacent tickets could force a contract adjustment if they redefine the same preflight artifact boundary.

Split recommendations
- No split recommended; current repository evidence already bounds this as one preflight-artifact task.

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