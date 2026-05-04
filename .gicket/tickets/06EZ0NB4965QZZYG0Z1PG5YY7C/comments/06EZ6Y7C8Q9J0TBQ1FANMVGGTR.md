[gicket-bot] PO refinement contract

Summary
- Refined the Oracle save-strategy story to a bounded v1 Oracle package scope: explicit `oracle-v1` capability registration, `AddDVaultOracle()` startup wiring, Oracle-only hub/link insert optimization with provider-neutral fallback, and documented opt-in Oracle validation. Existing split remains materialized through `parentOf` links to child tickets `06EZ0NBAP31G489S3YXXYY54WM` and `06EZ0NBH3YWJPF05AQWC0E6GV4`.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The safe v1 Oracle optimization boundary is already bounded in repository architecture and code: `AddDVaultOracle()` is the opt-in registration path, while the provider-neutral `AddDVault()` caller contract remains unchanged.
- The optimized Oracle path is intentionally limited to clean `Oracle.EntityFrameworkCore` DbContexts with hub/link-only request batches and no satellite operations; unsupported shapes must fall back through the core `IDataVaultSaveService` writer.
- The Oracle capability baseline is the visible `oracle-v1` profile: hash keys, hash diff, and participant references use `VARCHAR2(64 CHAR)`; business keys and record source use `VARCHAR2(255 CHAR)`; payload text uses `CLOB`; load timestamps use `TIMESTAMP WITH TIME ZONE` with native `DateTimeOffset` mapping.
- Oracle validation is opt-in, not part of the default local developer path; the documented smoke path uses `DVAULT_TEST_ORACLE_CONNECTION_STRING` and the existing Oracle provider integration filter.
- This story already has two materialized child tickets linked via `parentOf`: `06EZ0NBAP31G489S3YXXYY54WM` and `06EZ0NBH3YWJPF05AQWC0E6GV4`.

Scope In
- Explicit Oracle provider capability registration and Oracle package startup wiring through `DataVaultProviderCapabilityProfiles.Oracle` and `AddDVaultOracle()`.
- Oracle-compatible optimized save behavior for the first safe batch shape: clean-context hub and link inserts that preserve deterministic hash-key reuse semantics.
- Provider-strategy selection rules that gate Oracle optimization to the Oracle EF provider and fall back when the context or request shape is incompatible.
- Unit, local integration, and opt-in external Oracle smoke coverage for capability registration, strategy selection, fallback behavior, and documented validation.
- Documentation updates that describe Oracle setup, validation commands, and current Oracle limitations.

Scope Out
- Satellite-optimized Oracle saves.
- Optimized handling for dirty DbContexts that already track added, modified, or deleted entities.
- Provider-neutral concurrency signals, merge or upsert guarantees, retry semantics, or multi-writer conflict behavior.
- Automatic Oracle environment provisioning, CI-hosted Oracle infrastructure, or making Oracle part of the default local validation baseline.
- Changing the public write boundary from explicit `IDataVaultSaveService` calls to `SaveChanges` interception.

Open questions
- none

Follow-up questions
- Should a later story add Oracle-optimized satellite persistence and latest-state checks, or keep all satellite scenarios on the provider-neutral fallback longer?
- Should a later story broaden Oracle provider detection beyond the exact `Oracle.EntityFrameworkCore` provider name if additional Oracle EF providers become supported?
- Should the project later add repeatable CI or containerized Oracle validation, or keep Oracle verification as a developer-managed opt-in smoke path?
- Do the existing child tickets need narrower acceptance criteria aligned to the now-ratified hub/link-only v1 Oracle optimization boundary?

Risks
- Oracle is not part of the default local developer environment, so live validation depends on a manually managed Oracle database, credentials, and table-create or table-drop permissions.
- The optimized Oracle path is intentionally narrow; callers using satellites or dirty DbContexts will still rely on the slower provider-neutral fallback and may perceive uneven performance coverage.
- Provider selection is gated by exact provider identity, so unexpected Oracle provider naming or configuration changes could silently route requests to fallback instead of the optimized path.
- The story currently has two incoming `blocks` relations, so delivery timing still depends on upstream tickets even though the PO refinement scope is clear.

Split recommendations
- No additional split is recommended in PO refinement. The story already has two materialized child tickets linked through `parentOf`: `06EZ0NBAP31G489S3YXXYY54WM` and `06EZ0NBH3YWJPF05AQWC0E6GV4`.

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