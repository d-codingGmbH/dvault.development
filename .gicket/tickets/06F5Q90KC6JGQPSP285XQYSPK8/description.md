Goal: Add registry-backed PIT maintenance request support analogous to existing registry-backed save/read paths.

Acceptance criteria:
- Resolves PIT metadata by logical name or CLR mapping from UseDataVaultMetadata.
- Provides deterministic validation and diagnostics for missing, ambiguous, or incompatible metadata.
- Adds unit and integration coverage for registry-backed PIT rebuild and parent maintenance.