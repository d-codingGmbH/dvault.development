## Handover to Test

- template: `handover-test`
- transaction-point: `TP3`
- write-group: `wg-handover-test`

### Implementation Summary
- Completed the provider-neutral binary hash-key conversion in `DataVaultEfMetadataTranslator` for `HashKeyStorageProfile.Binary`.
- The EF model/public boundary remains canonical lowercase hex `string`; persistence conversion now uses the active stable-hash digest byte length.
- The converter rejects wrong hex lengths, uppercase/non-hex payloads, and provider byte arrays with mismatched digest length deterministically.
- HashKey and ParticipantReference properties receive the binary converter through the provider-neutral metadata projection.

### Verification Evidence
- `dotnet test tests\DCoding.Data.DVault.Tests\Unit\DCoding.Data.DVault.Tests.Unit.csproj --nologo --filter FullyQualifiedName~DataVaultEfMetadataTranslationTests` passed for `net8.0` and `net10.0`.
- `dotnet build DVault.slnx --nologo` passed.
- `bash tools/check-format.sh` passed.
- `dotnet test DVault.slnx --nologo` passed; external provider tests without configured connection strings were skipped as expected.

### Notes for Test
- The previous bot stop came from a `no_repository_change_required` outcome, but the ticket contract did require persisted implementation evidence. This branch now contains the implementation and tests.