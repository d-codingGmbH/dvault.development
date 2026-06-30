[gicket-bot] manual PO refinement

Summary
- Replaced the raw draft with a structured delivery contract for the provider-native crypto usage proof.
- Chose SQL Server Always Encrypted as the bounded first provider-owned proof path because the upstream configuration ticket introduced the provider-owned selection surface there.
- Kept shared native runtime dispatch, provider-name auto-routing, encrypted DDL, SQL crypto calls, key-store integration, and default live probing out of scope.
- Required deterministic local tests for provider-owned selection, fail-closed fallback behavior, custom encrypted-payload preservation, and redacted diagnostics.

Handoff
- decision: `ready_for_po_critic`
- next-role: `po-critic`
- reason: PO contract is now specific enough for critique before dev implementation.