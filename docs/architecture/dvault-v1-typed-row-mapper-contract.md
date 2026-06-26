# DVault V1 Typed Row Mapper Contract

Status: v1 implementation note
Ticket: 06F0MEC7FEXAD069AJNYZW0DRM

## Decision

DVault v1 exposes three thin typed row-mapper contracts in `DCoding.Data.DVault`:

- `IDataVaultHubMapper<TSource>`
- `IDataVaultLinkMapper<TSource>`
- `IDataVaultSatelliteMapper<TSource>`

Each mapper converts one non-null source value into one registry-backed save operation:

- hubs return `DataVaultRegistryHubSaveOperation`
- links return `DataVaultRegistryLinkSaveOperation`
- satellites return `DataVaultRegistrySatelliteSaveOperation`

The mapper output is identified by exact logical metadata names. Hub business keys, link participant hash keys, satellite driving keys, and satellite payload values are keyed by the exact names declared in the authoritative metadata registry. The registry and save-service pipeline keep canonical declaration ordering; caller enumeration order does not define persistence order.

## Request Boundary

Typed row mappers do not carry load timestamp or record source. Callers or later typed helper APIs must still supply those values explicitly when assembling `DataVaultRegistrySaveRequest` or `DataVaultRegistryBulkSaveRequest`.

```csharp
var hubOperation = customerHubMapper.Map(customer);
var profileOperation = customerProfileMapper.Map(profile);

var request = new DataVaultRegistrySaveRequest(
    loadTimestamp,
    recordSource,
    [hubOperation],
    [],
    [profileOperation]);
```

## Link Boundary

`IDataVaultLinkMapper<TSource>` v1 supports links whose produced participant names are unique by `StringComparer.Ordinal`. For ordinary distinct-hub links, the produced participant name is the participant hub metadata name. For repeated same-hub links, the produced participant name is the explicit role-bearing participant name, such as `SourceCustomer` or `MatchedCustomer`.

Generated link mapper declarations use `DataVaultLinkParticipantBindingAttribute` values in declaration order to bind those produced participant names to source hash-key members. Repeated same-hub generated mappings are supported when every participant binding supplies a non-blank, explicit, unique produced participant name. Ambiguous repeated same-hub declarations that reuse a hub name such as `Customer` for both endpoints remain invalid because `DataVaultRegistryLinkSaveOperation` is keyed by produced participant name.

## Validation Boundary

Mapper implementations should reject null source values immediately, and the existing registry-backed operation constructors reject null mapped values, blank names, and duplicate mapped names. Multi-active satellite driving-key sets are validated when the registry-backed satellite output is resolved to the existing `DataVaultSatelliteSaveOperation`.

Missing required hub business-key names, link produced participant names, and satellite payload names remain owned by the existing `IDataVaultSaveService` pipeline during save-plan creation after registry resolution and before `DbContext.SaveChangesAsync`. This contract does not add a new public validator, factory, hidden hashing helper, source generator, or mapper-discovery surface.
