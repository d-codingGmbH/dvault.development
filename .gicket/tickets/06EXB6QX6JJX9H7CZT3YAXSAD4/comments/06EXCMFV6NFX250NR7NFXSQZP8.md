## Developer delivery: optional advanced configuration hook plan

### Decision
DVault v1 should keep a convention-first configuration model. Typical users do not provide custom configuration. Advanced users may opt into one coherent advanced configuration surface that can override specific hook categories while leaving every other category on the DVault defaults.

This plan is architecture-level only. It intentionally does not define class names, method names, parameter names, file locations, package layout, or provider-specific option matrices before the repository has a source and test layout.

### Configuration grouping
Use one conceptual advanced configuration surface with these grouped hook categories:

- Naming conventions
- Hashing behavior
- Record source resolution
- Timestamp sourcing and formatting
- Provider behavior

Every hook is optional. Unset hooks inherit the DVault default. Users should be able to customize one category without restating the defaults for the others.

### Default path
The default path is zero-configuration:

- DVault applies its standard naming, hashing, record source, timestamp, and provider behavior conventions.
- No custom hook registration is required for a vault to work.
- Defaults are owned by DVault, documented with the implementation that introduces them, and treated as the stable baseline for normal users.
- Advanced hooks wrap or replace only the category they are registered for.

### Naming hook
Default behavior: DVault derives deterministic vault and record names from the standard DVault identity conventions. Names should be normalized into a stable, safe representation suitable for the target persistence layer. The default should avoid provider-specific naming rules unless a provider requires a bounded adaptation.

Optional customization: advanced users may override naming when they need existing system compatibility, tenant-specific prefixes, legacy path conventions, or provider-specific constraints. Custom naming must remain deterministic for equivalent inputs and must fail clearly if it produces invalid, empty, ambiguous, or unsafe names.

### Hashing hook
Default behavior: DVault computes a stable content hash from the canonical record representation used by DVault. The default hash is DVault-owned rather than provider-owned, deterministic for equivalent canonical content, and suitable for change detection and integrity checks. The exact algorithm and canonical byte representation should be bound by the implementation ticket that creates the source layout.

Optional customization: advanced users may override hashing for compatibility with an existing archive, migration from another system, or stricter integrity requirements. Custom hashing must declare enough behavior for DVault to validate output shape and must not silently downgrade integrity. Invalid, missing, or non-deterministic hash output should fail clearly.

### Record source hook
Default behavior: DVault resolves record source from the standard ingestion or provider context. When a provider supplies source metadata, DVault uses that metadata through the common source model. When source cannot be resolved safely, DVault should report a clear configuration or ingestion error instead of inventing an ambiguous source.

Optional customization: advanced users may derive source from record metadata, external routing, tenant context, or legacy identifiers. Custom source resolution must return a stable source identity and should make ambiguity explicit. It should not silently collapse distinct sources into one identity.

### Timestamp hook
Default behavior: DVault prefers authoritative record or provider timestamps when available, normalizes stored timestamps to UTC, and writes them using the canonical timestamp representation selected by the implementation. If no record timestamp is available, DVault may use its standard processing time source as the fallback, with that fallback documented by the implementation ticket.

Optional customization: advanced users may supply a different time source, normalize provider-specific timestamp formats, pin deterministic time for tests, or preserve legacy timestamp semantics. Custom timestamp hooks must produce valid, normalized timestamps and must fail clearly on unparseable or out-of-range values.

### Provider behavior hook
Default behavior: DVault uses common provider behavior with minimal provider assumptions. Provider defaults should cover standard persistence, capability detection, error reporting, and common safe behavior without requiring users to choose provider-specific options.

Optional customization: advanced users may tune bounded provider behavior such as capability flags, provider-level normalization, retry boundaries, or compatibility settings when a provider requires it. The v1 hook should remain generic. Concrete provider-specific option matrices should wait until provider requirements are visible in later tickets.

### Validation expectations
Advanced hook configuration should fail clearly and early when a custom hook is invalid. Validation should catch missing required custom behavior, invalid output shape, ambiguous source resolution, unsafe names, timestamp normalization failures, and hashing behavior that cannot produce stable output. DVault should not silently continue with surprising fallback behavior after a custom hook has been accepted.

### Current v1 decisions
- Defaults require no user action.
- All advanced hooks are opt-in.
- A single conceptual advanced configuration surface should group the hook categories.
- Naming, hashing, record source, timestamps, and provider behavior are the planned hook categories.
- Provider customization remains generic and bounded for now.
- Runtime implementation and concrete API naming are deferred until source and test layout exist.

### Non-blocking future questions
- Whether the advanced configuration surface becomes stable public API immediately or starts as internal or experimental.
- Which provider ecosystems need provider-specific options first.
- Whether timestamp behavior should expose separate documented modes for deterministic test-time injection and wall-clock fallback.