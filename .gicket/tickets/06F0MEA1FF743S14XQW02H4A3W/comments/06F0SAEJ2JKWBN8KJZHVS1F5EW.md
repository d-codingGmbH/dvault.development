[gicket-bot] PO refinement contract

Summary
- Refined the fluent link child ticket to the parent contract: it now covers only link-specific code-first API, ordered participant resolution, projection through the existing metadata translator, and focused link tests, with no blocking PO questions left.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The parent contract and the fluent-link child boundary are authoritative; this child owns only fluent link and relationship projection.
- The fluent surface is additive in DCoding.Data.DVault and must project into DataVaultMetadataModel before reusing the existing ApplyDataVaultMetadata(ModelBuilder, DataVaultMetadataModel, ...) translator path.
- Links are declared from hub CLR types that have already been configured in the same code-first model; participant declaration order is the canonical order for default naming, metadata projection, and generated relationship index columns.
- This child is responsible for explicit relationship names and the derived default when no name is supplied, but not for hub-name overrides, link-parent satellites, save helpers, or provider-specific SQL behavior.
- Covered v1 verification for this child is link-focused: one explicit-name two-participant case, one derived-name multi-participant case, and clear failure paths for missing, ambiguous, or unsupported participant resolution.

Scope In
- Add the minimum fluent code-first link API surface needed to declare links from previously configured hub CLR types, with both explicit-name and derived-name entry points.
- Capture ordered Participant<T>() declarations and resolve them to configured hubs in the same code-first model.
- Project fluent link declarations into DataVaultLinkMetadata and then through the existing provider-aware EF shared-type translation pipeline.
- Add link-specific unit and schema/parity-style tests for covered two-participant and multi-participant shapes plus failure cases.

Scope Out
- Hub business-key capture and hub-parent satellite payload or DrivingKey capture, which remain on ticket 06F0ME9PM8KXH3VP59TQR0ETA8.
- Parity breadth beyond the link-specific scenarios needed here, which remains on ticket 06F0MEAD1BAA5QEVM3F9QJA38G.
- Link-parent satellites, typed save or read helpers, save-service behavior, or SaveChanges interception.
- Provider-specific SQL, migrations, foreign keys, navigations, or new translator branches outside the existing metadata-first path.

Open questions
- none

Follow-up questions
- If a later code-first consumer needs recursive or same-hub self-links, should a future fluent expansion add explicit participant-role or alias support instead of relying only on repeated Participant<T>() calls?

Risks
- This child and the hub or satellite sibling both touch the shared code-first entry surface, so parallel delivery can create API or merge drift unless shared scaffolding stays minimal.
- Any loss of participant declaration order or drift from current naming normalization will change produced link table, key, and index names and break metadata-first equivalence.
- Repeated same-hub participants can produce duplicate participant hash-key names under the current link naming path if the code-first layer does not reject unsupported shapes early.

Split recommendations
- none

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