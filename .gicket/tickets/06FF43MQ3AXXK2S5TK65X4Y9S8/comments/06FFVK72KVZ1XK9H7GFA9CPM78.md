[gicket-bot] PO refinement contract

Summary
- Refined the task against the opt-in privacy boundary, the existing `personalData[].encryptedPayloadAlias` schema contract, and the current parent/blocked ticket relations; no bounded ticket or planning writes were needed.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The repository already fixes the v1 baseline: personal-data metadata is additive satellite metadata keyed by exact payload field name plus one stable `encryptedPayloadAlias`, and privacy behavior remains an explicit opt-in extension rather than a default core runtime change.
- This ticket covers diagnostics for both model-first and metadata-first inputs that carry personal-data markers; it does not reopen the alias shape, package boundary, or caller-owned key-provider decision already documented in the repository.
- Existing relations already express the intended sequence: this task is a child of story `06FF43K0B0MJF45078STZ3H6DC` and currently blocks the follow-on test and documentation tasks `06FF43NAAR3WXH759TVG2RS2M4`, `06FF43NJES6S8NBZVWR4FGHWGW`, and `06FF43QFBQ185N3WPRFD544H00`.

Scope In
- Add coverage diagnostics for model-first and metadata-first personal-data markers whose `encryptedPayloadAlias` is not backed by the current privacy configuration for the same model boundary.
- Differentiate advisory versus fail-closed outcomes based on whether the application has opted into the existing privacy proof and whether marked-field alias/converter coverage is usable.
- Report the issue in provider-neutral terms using logical payload-field and alias names, without changing the existing explicit privacy activation model.

Scope Out
- Automatic encryption, automatic redaction, implicit `SaveChanges` privacy behavior, or any default runtime behavior change in the core DVault package.
- Key lifecycle, key storage, rotation, destruction, compliance claims, retention workflow, or provider-native encryption behavior.
- Documentation expansion and broad regression-matrix work already tracked by the blocked sibling tickets.

Open questions
- none

Follow-up questions
- After the core diagnostic behavior lands, confirm whether the sibling documentation tickets should show separate advisory and fail-closed examples for the same marked-field scenario.
- Consider later whether a future code-first personal-data marker surface should participate in the same coverage diagnostics, but do not expand this ticket beyond model-first and metadata-first inputs.

Risks
- If the implementation cannot reliably detect explicit converter coverage from the existing seams, it may produce either false positives or missed gaps; the check should stay narrowly aligned to the current alias-registration and converter proof.
- Overly strong wording in advisory mode could make the optional privacy boundary look mandatory, which would conflict with the documented opt-in posture.
- This ticket currently blocks downstream test and documentation tickets, so unstable diagnostic identifiers or message semantics will create follow-on churn.

Split recommendations
- No additional split is recommended; the current relation set already separates core diagnostic behavior from the follow-on test and documentation work.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 4

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment