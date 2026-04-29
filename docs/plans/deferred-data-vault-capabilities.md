# Deferred Data Vault Capabilities

## Purpose

This note records Data Vault capabilities that are intentionally outside the MVP package. The MVP documentation remains focused on the core concepts needed for the first package: hubs, links, satellites, hash keys, hash diffs, load timestamps, and record sources.

The capabilities below are post-MVP expansion areas. They are not required for the MVP release and must not block the first package.

## MVP Boundary

The first package should keep the Data Vault baseline small and portable. It can describe the core modeling concepts and Sqlite-oriented examples without committing to advanced generation behavior, provider tuning, or complete automation for every Data Vault pattern.

Future work may promote any deferred capability into an epic or smaller capability story after the MVP documentation baseline is accepted. Those follow-up items should make their own product and provider decisions when they are scheduled.

## Deferred Capabilities

| Deferred capability | Planning value | Why deferred from MVP | Future epic hook |
| --- | --- | --- | --- |
| PIT table generation | Point-in-time tables can simplify historical joins and improve query ergonomics across multiple satellites. | PIT automation depends on query patterns, refresh strategy, and temporal grain decisions that are not needed to explain the MVP hub, link, and satellite baseline. | Define PIT scope, refresh expectations, and supported temporal patterns before committing generation behavior. |
| Bridge table generation | Bridge tables can make many-to-many relationships and hierarchy traversal easier for downstream consumers. | Bridge design depends on relationship semantics, hierarchy depth, business rules, and consuming workload needs that should not be assumed in the first package. | Decide which relationship and hierarchy scenarios deserve generated bridge support, then specify validation and maintenance expectations. |
| Multi-active satellites | Multi-active satellites support multiple simultaneous descriptive records for the same business key and load window. | Multi-active modeling needs clear rules for driving keys, uniqueness, conflict handling, and examples beyond the MVP satellite concept. | Create a capability story that defines accepted multi-active modeling patterns and documentation examples before implementation planning. |
| Provider-specific optimizations | Provider-specific work can improve DDL, indexing, hashing, bulk loading, and query behavior for individual database engines. | The MVP should stay aligned with the Sqlite-oriented baseline and avoid promising adapter-specific behavior before provider priorities are known. | Plan provider or adapter epics that identify target engines and optimization responsibilities independently from the core Data Vault concepts. |

## Planning Guardrails

- Do not treat PIT table generation, bridge table generation, multi-active satellites, or provider-specific optimizations as MVP requirements.
- Do not introduce current API, generator, adapter, or provider capability commitments from this note.
- Keep future scope open enough for separate epics or stories to decide automation depth, supported providers, and acceptance criteria.
- Keep MVP concept documentation separate from future expansion areas so the first package can ship without advanced Data Vault patterns.