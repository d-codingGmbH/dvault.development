using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace DCoding.Data.DVault;

internal enum DataVaultProviderIdentifierKind {
  Table,
  Column,
  PrimaryKey,
  Index,
  Constraint,
}
