[gicket-bot] PO refinement contract

Summary
- Reconciled the ticket with current repository reality by making this task explicitly own the missing `personalData` transport into runtime diagnostics instead of assuming that transport already exists.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - The ticket is no longer framed as diagnostics over an already-existing runtime representation. It now owns the minimal prerequisite transport work as part of the same task: import valid model-first `satellite.personalData[]` field-plus-alias data into runtime metadata, add an equivalent metadata-first runtime carrier for marked payload fields, and then run the missing-alias diagnostics over that shared carrier.
- critic-item-2: `answered` - `personalData` transport is not being deferred to another ticket, so there is no external prerequisite ticket to name and no new prerequisite relation to add. This ticket itself is the authoritative transport-plus-diagnostics task for the missing carrier work.
- critic-item-3: `answered` - Metadata-first input must present a marked field as additive per-satellite payload metadata keyed by the exact logical payload field name plus one stable `encryptedPayloadAlias`. The diagnostic path must consume that runtime carrier directly, and model-first import must project `satellite.personalData[]` into the same carrier so both declaration paths converge before diagnostics run.
- critic-item-4: `answered` - The acceptance criteria and implementation notes are corrected so they no longer assume that model-first parser support or metadata-first/runtime `personalData` carriage already exists. The ticket now explicitly requires the minimal parser/import and runtime metadata-carrier work needed before the advisory versus fail-closed diagnostic behavior can execute.

Clarifications
- This ticket now owns the smallest complete path needed to make the diagnostic contract real: model-first import of `personalData` markers, metadata-first runtime carriage of the same marked-field evidence, and diagnostics over that shared runtime representation.
- The runtime representation is additive over existing satellite payload metadata and stays keyed by exact logical payload field name plus one stable `encryptedPayloadAlias`, matching the documented schema contract rather than introducing store-specific or provider-specific identifiers.
- The existing opt-in privacy boundary is unchanged: this work adds coverage transport and diagnostics only, not automatic encryption, implicit privacy activation, or key lifecycle behavior.

Scope In
- Add the minimal model-first parser or import projection needed to carry valid `satellite.personalData[]` field-plus-alias declarations onto the runtime diagnostic path.
- Add a metadata-first runtime carrier for marked satellite payload fields so `DataVaultMetadataModel`-based input can express the same field-plus-alias evidence without inventing a separate privacy execution feature.
- Run missing-alias or unusable-coverage diagnostics over that shared runtime carrier and preserve the advisory versus fail-closed split based on the existing privacy opt-in mode and converter coverage.

Scope Out
- Automatic encryption, automatic redaction, implicit `SaveChanges` behavior, or any default runtime privacy activation in the core DVault package.
- Key storage, key rotation, deletion workflow, retention workflow, compliance claims, provider-native encryption behavior, or other privacy-governance ownership outside the documented optional extension boundary.
- A new code-first `personalData` authoring surface; this ticket is limited to model-first import, metadata-first carriage, and diagnostics.

Open questions
- none

Follow-up questions
- After this core transport-plus-diagnostics work lands, decide whether the follow-on documentation should show paired advisory and fail-closed examples for the same marked-field scenario.
- Consider a later additive ticket for a code-first `personalData` authoring surface if teams need parity with model-first and metadata-first declarations.

Risks
- Because the ticket now includes minimal transport work in addition to diagnostics, implementation scope must stay tightly bounded to field-plus-alias carriage and must not sprawl into unrelated model-first or metadata-first feature expansion.
- If the metadata-first carrier is not aligned exactly to existing payload-field naming, diagnostics may drift between imported model-first metadata and directly constructed metadata-first models.
- Advisory-mode wording must stay precise so the optional privacy boundary remains opt-in and the ticket does not accidentally imply automatic encryption or compliance guarantees.

Split recommendations
- No additional split is recommended at refinement time. The missing transport and the diagnostics that consume it are a single bounded implementation slice, and separating them now would introduce an avoidable implicit prerequisite.

Persisted contract coverage
- acceptance-criteria items: 7
- definition-of-done items: 4
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment