namespace DCoding.Data.DVault;

internal sealed class SqlServerAlwaysEncryptedDataVaultProviderNativeCryptoSelectionProvider(
    string encryptedPayloadAlias,
    IReadOnlyList<string> callerOwnedPrerequisiteProofNames)
    : IDataVaultProviderNativeCryptoSelectionProvider {
  private const string CapabilityFamily = "always-encrypted";
  private const string ProviderPackageName = "DCoding.Data.DVault.SqlServer";
  private const string SelectionStatusRequested = "provider-native-requested";
  private const string SelectionStatusRejectedIncompatibleProfile = "provider-native-rejected-incompatible-profile";
  private const string SelectionStatusRejectedMissingPrerequisite = "provider-native-rejected-missing-prerequisite";
  private const string SelectionStatusRejectedUnavailable = "provider-native-rejected-unavailable";
  private const string SelectionStatusRejectedUnsupported = "provider-native-rejected-unsupported";

  public string EncryptedPayloadAlias { get; } = encryptedPayloadAlias;

  public IReadOnlyList<DataVaultProviderNativeCryptoSelectionFact> Analyze(
      DataVaultProviderNativeCryptoSelectionContext context) {
    ArgumentNullException.ThrowIfNull(context);
    ArgumentNullException.ThrowIfNull(context.ReviewedCapabilities);

    if (!string.Equals(
        DataVaultProviderCapabilityProfiles.SqlServer.ProfileName,
        context.CapabilityProfileName,
        StringComparison.Ordinal)) {
      return
      [
          CreateRejectedFact(
              context,
              capability: null,
              SelectionStatusRejectedIncompatibleProfile,
              "SQL Server Always Encrypted selection for encrypted-payload alias '" +
              EncryptedPayloadAlias +
              "' requires capability profile '" +
              DataVaultProviderCapabilityProfiles.SqlServer.ProfileName +
              "', but the active profile is '" +
              context.CapabilityProfileName +
              "'."),
      ];
    }

    if (callerOwnedPrerequisiteProofNames.Count == 0) {
      return
      [
          CreateRejectedFact(
              context,
              capability: null,
              SelectionStatusRejectedMissingPrerequisite,
              "SQL Server Always Encrypted selection for encrypted-payload alias '" +
              EncryptedPayloadAlias +
              "' is missing caller-owned prerequisite proof names."),
      ];
    }

    var capability = context.ReviewedCapabilities.SingleOrDefault(candidate =>
        string.Equals(candidate.CapabilityProfileName, DataVaultProviderCapabilityProfiles.SqlServer.ProfileName, StringComparison.Ordinal) &&
        string.Equals(candidate.CapabilityFamily, CapabilityFamily, StringComparison.Ordinal));
    if (capability is null) {
      return
      [
          CreateRejectedFact(
              context,
              capability: null,
              context.CapabilityProfileDefaulted ? SelectionStatusRejectedUnavailable : SelectionStatusRejectedUnsupported,
              "SQL Server Always Encrypted selection for encrypted-payload alias '" +
              EncryptedPayloadAlias +
              "' requested reviewed capability '" +
              CapabilityFamily +
              "', but no static capability fact is available for the active profile."),
      ];
    }

    if (string.Equals(capability.Status, "unsupported", StringComparison.Ordinal)) {
      return
      [
          CreateRejectedFact(
              context,
              capability,
              SelectionStatusRejectedUnsupported,
              "SQL Server Always Encrypted selection for encrypted-payload alias '" +
              EncryptedPayloadAlias +
              "' requested reviewed capability '" +
              capability.CapabilityLabel +
              "', but that capability is marked unsupported."),
      ];
    }

    return
    [
        new DataVaultProviderNativeCryptoSelectionFact(
            context.ProviderName,
            EncryptedPayloadAlias,
            ProviderPackageName,
            DataVaultProviderCapabilityProfiles.SqlServer.ProfileName,
            CapabilityFamily,
            capability.CapabilityLabel,
            capability.CapabilityKind,
            capability.Status,
            SelectionStatusRequested,
            "SQL Server Always Encrypted selection for encrypted-payload alias '" +
            EncryptedPayloadAlias +
            "' names reviewed capability '" +
            capability.CapabilityLabel +
            "' and remains owned by the SQL Server provider package; DVault shared privacy code does not dispatch native runtime behavior."),
    ];
  }

  private DataVaultProviderNativeCryptoSelectionFact CreateRejectedFact(
      DataVaultProviderNativeCryptoSelectionContext context,
      DataVaultProviderCryptoCapabilityFact? capability,
      string selectionStatus,
      string message) {
    return new DataVaultProviderNativeCryptoSelectionFact(
        context.ProviderName,
        EncryptedPayloadAlias,
        ProviderPackageName,
        DataVaultProviderCapabilityProfiles.SqlServer.ProfileName,
        CapabilityFamily,
        capability?.CapabilityLabel,
        capability?.CapabilityKind,
        capability?.Status,
        selectionStatus,
        message);
  }
}
