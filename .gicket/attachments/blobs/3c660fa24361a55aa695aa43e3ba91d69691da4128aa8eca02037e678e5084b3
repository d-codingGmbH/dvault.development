# Bridge Metadata V1 Contract

Status: v1 planning contract
Primary ticket: 06EZ0NV0Y81AE1Z1Q3223TX2S4
Dependent ticket: 06EZ0NV7KG94MTMNXMGVRYVW9C
Related ticket: 06EZ0NVE88WW9PMM04NVAZHRG0

## Purpose

This document defines the durable v1 contract for baseline bridge metadata so the mapping and documentation siblings can implement against one authoritative source. It keeps bridge work additive to the existing hub, link, satellite, shared-type, and SQLite-oriented translator baseline.

## Scope

- Baseline bridge kinds are limited to many-to-many traversal and hierarchy traversal.
- Bridge metadata is modeled in `DCoding.Data.DVault.Modeling` beside hubs, links, and satellites and extends `DataVaultMetadataModel` with a bridge collection.
- Bridge tables remain provider-neutral EF shared-type projections with no EF foreign keys, navigations, save-service changes, migrations, or provider-specific DDL behavior in v1.
- Baseline bridge tables do not introduce new load timestamp, record source, or hash diff families. Many-to-many bridges project only endpoint hash-key columns. Hierarchy bridges add only one integer `TraversalDepth` column.

## Authoritative metadata contract

- A bridge declaration MUST carry a stable bridge name, a bridge kind token, the source metadata it traverses, and explicit endpoint bindings whose declared order becomes the produced column order.
- Many-to-many bridge declarations MUST bind exactly one existing link and exactly two distinct hub endpoints named `from` and `to`.
- Hierarchy bridge declarations MUST bind exactly one recursive link and MUST explicitly identify `ancestor` and `descendant` participant roles over one hub type.
- The modeling surface MUST add the minimum public API needed for bridge projection: bridge declaration types in `src/DCoding.Data.DVault/Modeling`, `Bridge` in `DataVaultTableKind`, and one bridge-depth provider-logical property kind plus one distinct property-role or annotation value for hierarchy depth instead of overloading satellite payload semantics.

## Validation ownership

Metadata validation owned by 06EZ0NV0Y81AE1Z1Q3223TX2S4:
- missing hub or link references
- wrong reference kind
- many-to-many declarations whose source link does not contain the declared `from` and `to` hub endpoints exactly once
- hierarchy declarations whose source link is not a two-participant self-link over one hub type
- ambiguous recursive role binding, duplicate endpoint bindings, and cycle rules for unsupported hierarchy definitions

Translator-time failure owned by 06EZ0NV7KG94MTMNXMGVRYVW9C:
- otherwise valid bridge metadata that asks for columns or behaviors outside the bounded provider-neutral EF projection baseline, such as effectivity windows, path payload columns, closure maintenance state, or EF relationship graph generation

## Worked examples

### Many-to-many traversal

Source metadata:
- Bridge name: `CustomerOrder`
- Kind: `many-to-many`
- Source link: `CustomerOrder`
- Endpoint bindings: `from=Customer`, `to=Order`

Expected EF projection:
- Entity produced name: `BridgeCustomerOrder`
- Entity kind annotation: `Bridge`
- Metadata name annotation: `CustomerOrder`
- Ordered columns: `CustomerHashKey`, `OrderHashKey`
- Primary key: `PkBridgeCustomerOrderCustomerHashKeyOrderHashKey`
- Secondary index: `IxBridgeCustomerOrderTraversalOrderHashKeyCustomerHashKey`
- Property annotations: both columns use participant-reference role, keep `ProducedName` equal to the column name, and keep metadata names `Customer` and `Order`
- No implicit EF foreign keys or navigations

### Hierarchy traversal

Source metadata:
- Bridge name: `SalesRegionHierarchy`
- Kind: `hierarchy`
- Source link: `SalesRegionParentChild`
- Endpoint bindings: `ancestor=ParentRegion`, `descendant=ChildRegion`

Expected EF projection:
- Entity produced name: `BridgeSalesRegionHierarchy`
- Entity kind annotation: `Bridge`
- Metadata name annotation: `SalesRegionHierarchy`
- Ordered columns: `AncestorSalesRegionHashKey`, `DescendantSalesRegionHashKey`, `TraversalDepth`
- Primary key: `PkBridgeSalesRegionHierarchyAncestorSalesRegionHashKeyDescendantSalesRegionHashKey`
- Secondary indexes: `IxBridgeSalesRegionHierarchyTraversalAncestorSalesRegionHashKeyTraversalDepth` and `IxBridgeSalesRegionHierarchyTraversalDescendantSalesRegionHashKeyAncestorSalesRegionHashKey`
- Property annotations: `AncestorSalesRegionHashKey` and `DescendantSalesRegionHashKey` use participant-reference role, and `TraversalDepth` uses the new integer bridge-depth logical kind
- No implicit EF foreign keys or navigations

## Mapping handoff

- Ticket 06EZ0NV7KG94MTMNXMGVRYVW9C consumes this bridge contract and owns only provider-neutral EF projection, produced-name annotations, provider capability mappings, and translation-boundary not-supported diagnostics.
- Ticket 06EZ0NVE88WW9PMM04NVAZHRG0 owns user-facing documentation and examples derived from this contract.
- The persisted relation `06EZ0NV0Y81AE1Z1Q3223TX2S4 blocks 06EZ0NV7KG94MTMNXMGVRYVW9C` remains the required execution order until the metadata ticket is implemented.
