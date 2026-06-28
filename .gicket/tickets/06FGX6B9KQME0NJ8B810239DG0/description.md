Make the manifest validator available through the existing preflight or diagnostics boundary.

Acceptance:
- Consumers can include a hash-key storage migration manifest in a preflight-style request or equivalent diagnostics path.
- The resulting report clearly separates manifest validation from EF migration operation guardrails.
- Support-bundle output, if extended, includes only structural migration-plan facts and no raw hash-key values.
- Tests cover successful and failing manifest-validation reports.