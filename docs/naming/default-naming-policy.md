# Default table and column naming policy

DVault uses the v1 default naming policy when a model does not supply naming configuration. The policy is provider-neutral and emits deterministic PascalCase identifiers without quoting, snake_case conversion, or provider-specific reserved-word catalogs.

## Table names

Table names use Data Vault prefixes and normalized object names.

| Model concept | Format | Example |
| --- | --- | --- |
| Hub | `Hub{Entity}` | `Customer` -> `HubCustomer` |
| Link | `Link{ParticipantOrRelationshipName}` | `Customer`, `Order` -> `LinkCustomerOrder` |
| Satellite | `Sat{Parent}{SatelliteDescriptor}` | `Customer`, `Contact` -> `SatCustomerContact` |

Links use an explicit relationship name when one is provided. Without one, the policy concatenates normalized participant role or entity names in model declaration order so repeated calls for the same model return the same name.

## Column names

Business-key and payload columns use the same property-column rule: normalize the property token to PascalCase, keep the semantic token when it is safe, and apply the collision rules below when it is not.

| Column concept | Format | Example |
| --- | --- | --- |
| Business key or payload property | `{Property}` | `customer id` -> `CustomerId` |
| Hash key | `{Base}HashKey` | `Customer` -> `CustomerHashKey` |
| Hash diff | `HashDiff` | `HashDiff` |
| Load timestamp | `LoadTimestamp` | `LoadTimestamp` |
| Record source | `RecordSource` | `RecordSource` |

Technical column names are reserved in the same column scope. A user property named `hash diff`, `load_timestamp`, `record-source`, or a scoped hash key such as `customer hash key` is therefore renamed to `HashDiffValue`, `LoadTimestampValue`, `RecordSourceValue`, or `CustomerHashKeyValue`.

## Normalization

The normalizer trims input, splits whitespace, punctuation, snake_case, kebab-case, and PascalCase boundaries, removes non-ASCII identifier characters, and emits PascalCase. Leading digits are discarded until a letter token is found, while digits after a letter are preserved.

Examples:

| Input | Normalized object | Normalized column |
| --- | --- | --- |
| ` customer account ` | `CustomerAccount` | `CustomerAccount` |
| `customer_account` | `CustomerAccount` | `CustomerAccount` |
| `customer-account` | `CustomerAccount` | `CustomerAccount` |
| `CustomerAccount` | `CustomerAccount` | `CustomerAccount` |
| `@@@` | `Entity` | `Value` |

Object names for hubs, links, satellites, roles, and hash-key bases also apply finite v1 singularization. Column names do not singularize property tokens.

## Singular and plural object tokens

The finite v1 singularization rules are deterministic and intentionally small:

- Convert a trailing consonant plus `ies` to `y`, such as `Companies` -> `Company`.
- Strip `es` from common sibilant plurals ending in `ches`, `shes`, `sses`, `xes`, `zes`, or `ses`, such as `Boxes` -> `Box` and `Addresses` -> `Address`.
- Strip a trailing `s` except `ss`, such as `Customers` -> `Customer`.
- Leave names unchanged when no rule applies, such as `Business` -> `Business`.

These rules make `Customer` and `Customers` resolve to the same object base name and therefore to the same hub table name, `HubCustomer`.

## Reserved words and collisions

The v1 provider-neutral reserved sets are finite. Object base names treat `As`, `By`, `Column`, `Constraint`, `Database`, `From`, `Group`, `Index`, `Join`, `On`, `Schema`, `Select`, `Table`, `User`, and `Where` as unsafe. Column names use the same set and also treat `Order` as unsafe because it is a common property name that would otherwise collide with SQL-style ordering language.

Unsafe object base tokens append `Entity`. For example, an entity named `Select` becomes `SelectEntity`, so the hub table is `HubSelectEntity`.

Unsafe property tokens append `Value`. For example, a property named `Order` becomes `OrderValue`, and a property that collides with `HashDiff` becomes `HashDiffValue`.

Duplicate column identifiers in the same scope receive deterministic numeric suffixes starting at 2. For example, `customer id`, `customer-id`, and `CustomerId` become `CustomerId`, `CustomerId2`, and `CustomerId3`.

## Public API

The default policy is exposed by `DCoding.Data.DVault.Modeling.DefaultNamingPolicy`. The shared stateless instance is `DefaultNamingPolicy.Instance`.

The public methods are:

- `GetHubTableName(entityName)`
- `GetLinkTableName(relationshipName, participantNames)`
- `GetSatelliteTableName(parentName, satelliteDescriptor)`
- `GetHashKeyColumnName(baseName)`
- `GetHashDiffColumnName()`
- `GetLoadTimestampColumnName()`
- `GetRecordSourceColumnName()`
- `NormalizeObjectName(value)`
- `NormalizeColumnName(value)`
- `GetColumnName(propertyName, additionalUnsafeColumnNames)`
- `GetColumnNames(propertyNames, additionalUnsafeColumnNames)`
