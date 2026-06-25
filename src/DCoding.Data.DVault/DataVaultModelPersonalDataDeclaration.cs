namespace DCoding.Data.DVault;

internal sealed record DataVaultModelPersonalDataDeclaration(
    string Field,
    string EncryptedPayloadAlias,
    string Path);
