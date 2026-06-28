Add a compact, test-backed privacy example that shows how consumers wire a caller-owned key provider and encrypted payload value converter into ordinary EF Core mapped properties.

Acceptance:
- The example demonstrates registration, alias mapping, encryption/decryption flow, and failure behavior when the alias or key provider is missing.
- The text explains that key rotation/destruction is caller-owned and that DVault provides seams rather than compliance automation.
- The sample is runnable or covered by tests in the existing validation lane.
- Provider-specific caveats point back to the boundary matrix instead of duplicating inconsistent claims.