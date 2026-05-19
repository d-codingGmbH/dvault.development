# Fluent Code-First Hub, Satellite, and Link Contract

Status: v1 planning contract
Superseding shipped note: v0.13.0 extends the original bounded planning baseline with repeated same-hub participant roles and link-parent satellites. Use the current README and latest release record, currently `docs/releases/v0.15.0.md`, for current shipped public behavior; use `docs/releases/v0.13.0.md` for the Code-First parity introduction.
Ticket: 06F0ME976PM5455JK04S6GPNNW
Parent story: 06F0ME8NFJX6CD20MEA10J761R
Implementation children: 06F0ME9PM8KXH3VP59TQR0ETA8, 06F0MEA1FF743S14XQW02H4A3W, 06F0MEAD1BAA5QEVM3F9QJA38G

## Purpose

Define the additive EF Core Code-First contract for hubs, ordinary satellites, multi-active opt-in, and links before implementation. The contract must stay small, map cleanly to the existing metadata-first model, and preserve the current explicit save boundary.

## Entry Point And Placement

- The EF-specific fluent surface lives in `DCoding.Data.DVault`, beside the existing `UseDataVault()` and `ApplyDataVaultMetadata()` extensions.
- The entry point is an additive overload:

```csharp
modelBuilder.ApplyDataVaultMetadata(vault => {
  // hub and link declarations
});
```

- That overload accepts `Action<DataVaultCodeFirstModelBuilder>`.
- Supporting public builder types live in `DCoding.Data.DVault` and use the `DataVaultCodeFirst*Builder` naming family to avoid colliding with the existing string-based `DCoding.Data.DVault.Modeling` builders.
- The fluent overload projects into `DataVaultMetadataModel` and then uses the existing provider-aware `ApplyDataVaultMetadata(ModelBuilder, DataVaultMetadataModel, ...)` path.
- The current metadata-first overloads and the existing `DCoding.Data.DVault.Modeling.DataVaultModelBuilder` surface remain supported unchanged.

## Hub Contract

- Hubs are declared by CLR entity type.
- The default provider-neutral hub name is the CLR type name.
- V1 does not add a separate hub-name override; callers that need a different logical hub name can stay on the metadata-first path for now.
- Business keys are captured with repeated direct member-selector calls in canonical order:

```csharp
modelBuilder.ApplyDataVaultMetadata(vault => {
  vault.Hub<Customer>(hub => {
    hub.BusinessKey(x => x.CustomerId);
    hub.BusinessKey(x => x.RegionCode);
  });
});
```

- Each `BusinessKey(...)` call captures exactly one logical member.
- Composite business keys use repeated `BusinessKey(...)` calls instead of anonymous-object selectors.
- The fluent hub contract does not ask callers to surface `HashKey`, `LoadTimestamp`, or `RecordSource` on the domain entity.

## Satellite Contract

- Ordinary satellites are nested under a configured hub and keep explicit satellite names:

```csharp
modelBuilder.ApplyDataVaultMetadata(vault => {
  vault.Hub<Customer>(hub => {
    hub.BusinessKey(x => x.CustomerId);
    hub.Satellite("Contact", satellite => {
      satellite.Payload(x => x.EmailAddress);
      satellite.Payload(x => x.PhoneNumber);
    });
  });
});
```

- Payload members are captured with repeated direct member-selector calls in canonical order.
- Multi-active opt-in reuses the existing `DrivingKey` verb:

```csharp
modelBuilder.ApplyDataVaultMetadata(vault => {
  vault.Hub<Customer>(hub => {
    hub.BusinessKey(x => x.CustomerId);
    hub.Satellite("ContactByType", satellite => {
      satellite.DrivingKey(x => x.ContactType);
      satellite.Payload(x => x.EmailAddress);
    });
  });
});
```

- `DrivingKey(...)` is the only fluent opt-in for multi-active satellite behavior. There is no separate boolean flag or options object.
- Repeated `DrivingKey(...)` calls define the canonical driving-key order and align with the existing multi-active driving-key contract.
- The original bounded v1 Code-First implementation baseline for this planning story was hub-parent satellites only. v0.13 later added link-parent satellites through `DataVaultCodeFirstLinkBuilder.Satellite<TSatellite>(...)`; this note preserves the historical contract while deferring current shipped behavior to the release notes and README.

## Link Contract

- Links are declared from previously configured hub entity types.
- The fluent surface offers an overload with an explicit relationship name and an overload that derives the relationship name from participant order:

```csharp
modelBuilder.ApplyDataVaultMetadata(vault => {
  vault.Hub<Customer>(hub => hub.BusinessKey(x => x.CustomerId));
  vault.Hub<Order>(hub => hub.BusinessKey(x => x.OrderId));

  vault.Link("CustomerOrder", link => {
    link.Participant<Customer>();
    link.Participant<Order>();
  });
});
```

```csharp
modelBuilder.ApplyDataVaultMetadata(vault => {
  vault.Hub<Customer>(hub => hub.BusinessKey(x => x.CustomerId));
  vault.Hub<Order>(hub => hub.BusinessKey(x => x.OrderId));
  vault.Hub<SalesRegion>(hub => hub.BusinessKey(x => x.RegionCode));

  vault.Link(link => {
    link.Participant<Customer>();
    link.Participant<Order>();
    link.Participant<SalesRegion>();
  });
});
```

- Links require at least two participants.
- Participant order is declaration order and becomes the canonical order used for default naming and metadata projection.
- Each participant type must resolve to exactly one fluent hub declaration in the same Code-First model.

## Selector And Validation Rules

- `BusinessKey(...)`, `Payload(...)`, and `DrivingKey(...)` accept only direct readable scalar member selectors rooted at the lambda parameter, for example `x => x.CustomerId`.
- Unsupported selector shapes fail fast with `ArgumentException` and name the API being used.
- Unsupported selector shapes include:
  - anonymous-object selectors such as `x => new { x.CustomerId, x.RegionCode }`
  - method calls
  - constants
  - collection navigations
  - nested navigation traversal
- The validation message tells callers to use repeated single-member calls instead.
- Duplicate business-key, payload, or driving-key members are rejected by logical member name using ordinal comparison.
- Link configuration fails clearly when:
  - fewer than two participants are declared
  - a participant type has not been configured as a hub
  - a participant type resolves ambiguously instead of to one hub declaration

## Compatibility Notes

- The fluent surface is additive over the current metadata-first path; it does not replace `DataVaultMetadataModel` or the existing metadata constructors.
- Generated hub, link, and satellite metadata must stay equivalent to the metadata-first baseline for covered scenarios so the parity child can compare table, column, key, and index shapes directly.
- The contract keeps `LoadTimestamp` and `RecordSource` out of domain entities by default and leaves them on the explicit save-request boundary.
- The contract does not promise `SaveChanges` interception.
- The contract does not introduce PIT, bridge, model-first, registry export/import, or typed save/read helper APIs.

## Full Representative Example

```csharp
modelBuilder.ApplyDataVaultMetadata(vault => {
  vault.Hub<Customer>(hub => {
    hub.BusinessKey(x => x.CustomerId);
    hub.Satellite("Contact", satellite => {
      satellite.Payload(x => x.EmailAddress);
      satellite.Payload(x => x.PhoneNumber);
    });
    hub.Satellite("ContactByType", satellite => {
      satellite.DrivingKey(x => x.ContactType);
      satellite.Payload(x => x.EmailAddress);
    });
  });

  vault.Hub<Order>(hub => {
    hub.BusinessKey(x => x.OrderId);
  });

  vault.Link("CustomerOrder", link => {
    link.Participant<Customer>();
    link.Participant<Order>();
  });
});
```
