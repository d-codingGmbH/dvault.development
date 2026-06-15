<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Repository evidence already fixes the product decision: new projects should opt into the binary-first profile, existing HexString stores remain valid until an intentional reviewed migration, and DVault does not auto-migrate persisted hash storage; no ticket-side writes or splits were needed.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Repository evidence already ratifies the bounded v1 decision: README.md and docs/getting-started.md say new projects should choose the binary-first profile explicitly while public hash-key values stay lowercase hexadecimal strings.
- Existing persisted HexString-compatible databases and configurations remain supported until the application owner chooses a separate reviewed migration, reset, or data-move plan; this ticket should not reopen that baseline.
- The current release-note trail already establishes the no-automatic-migration posture: docs/releases/v0.36.0.md introduces it, docs/releases/v0.37.0.md carries it forward, and CHANGELOG.md summarizes the same storage-profile story.
- Relations and attachments were reviewed from the ticket snapshot; no child-ticket split, relation cleanup, description update, planning document, or attachment materialization was necessary for refinement.

### Scope In
- Make the public documentation tell one consistent three-part story: binary-first is the recommendation for new projects, HexString remains the compatibility posture for existing persisted stores, and automatic hash-storage migration is out of scope.
- Align README.md, CHANGELOG.md, and the versioned release-note surfaces relevant to the current baseline so they use the same adoption and non-goal language.
- Preserve the public API and storage boundary statement that logical hash-key values remain lowercase hexadecimal strings even when Binary physical storage is selected.

### Scope Out
- Changing runtime defaults, stable hash algorithm defaults, or physical storage behavior in code.
- Adding automatic rehash, backfill, dual-write, repair, or persisted-key migration tooling.
- Defining provider-specific migration runbooks, operational cutover steps, or new benchmark evidence beyond the checked-in documentation baseline.
- Any product-code, schema, or test changes outside documentation alignment.

## Acceptance Criteria
- README.md explicitly states that new projects should opt into UseBinaryFirstProfile()/UseDataVaultBinaryFirstProfile() while public hash-key values remain lowercase hexadecimal strings.
- CHANGELOG.md and the relevant release notes explicitly state that HexString remains the compatible posture for existing persisted databases and configurations and that staying on HexString is valid until an owner-planned reviewed migration, reset, or data move is executed.
- The touched documentation explicitly states that DVault does not automatically rehash, backfill, dual-write, repair, or migrate persisted hash-key storage when the storage profile or stable hash algorithm changes.
- Documentation wording does not imply that Binary turns public hash-key values into byte arrays or that DVault silently changes existing deployments to binary storage.

## Definition of Done
- README.md, CHANGELOG.md, and the relevant release-note files use consistent language for new-project adoption, existing-store compatibility, and no-automatic-migration non-goals.
- Any examples or narrative that mention binary storage preserve the logical and public hash-key contract as lowercase hexadecimal string values.
- The final wording remains consistent with docs/plans/hash-key-storage-profile-contract.md, hash-key-footprint.md, and the carried-forward v0.36.0/v0.37.0 release-note baseline.
- No documentation text claims automatic migration behavior or a runtime default switch that is not implemented.

## Implementation Notes
- Treat this as a documentation-alignment ticket, not a new architecture decision. The core product decisions are already visible in README.md, docs/getting-started.md, docs/releases/v0.36.0.md, docs/releases/v0.37.0.md, hash-key-footprint.md, and docs/production-adoption-checklist.md.
- Keep the recommendation-versus-default distinction precise: the repository recommends binary-first for new projects, but AddDVault() still keeps HexString as the compatible default storage profile unless the application explicitly opts into the binary-first profile.
- When explaining why there is no automatic migration, anchor the wording in persisted compatibility facts already called out by the repository baseline: algorithm id, digest byte length, digest encoding, hash-key storage profile, provider store type and value format, and conversion behavior.
- Prefer wording that says existing HexString-compatible setups remain valid, rather than wording that frames them as deprecated or unsupported.
- No ticket mutation was applied during refinement: no planning document was written, no attachment was added, and no relation change or child-ticket creation was needed.

## Open Questions
- none

## Follow-Up Questions
- If the product later wants a supported HexString-to-Binary migration playbook, should that be tracked as a separate provider-aware adoption ticket with rollback and operational preconditions rather than folded into this documentation-alignment task?

## Risks
- The biggest documentation risk is collapsing two different ideas into one sentence: binary-first is the recommendation for new projects, but it is not a silent runtime-default change for existing deployments.
- If only the historical v0.36.0 release note carries the strongest adoption language while CHANGELOG.md or the current carried-forward release note stays softer, adopters may still read the public guidance as inconsistent.

## Split Recommendations
- No split recommended; the current repository evidence already bounds this as one documentation-alignment task.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Document when to use the binary-first profile, when to stay with hex-compatible storage, and why DVault does not provide automatic hash-storage migrations. Acceptance: README/CHANGELOG/release notes consistently describe the recommendation for new projects.