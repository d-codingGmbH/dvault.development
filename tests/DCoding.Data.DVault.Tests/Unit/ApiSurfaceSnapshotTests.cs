using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using DCoding.Data.DVault.Privacy;
using Xunit;

namespace DCoding.Data.DVault.Tests.Unit;

public sealed class ApiSurfaceSnapshotTests {
  private const string UpdateSnapshotsEnvironmentVariable = "DVAULT_UPDATE_API_SNAPSHOTS";

  [Fact]
  public void CorePublicApiMatchesApprovedSnapshot() {
    AssertPublicApiMatchesApprovedSnapshot(
        "DCoding.Data.DVault",
        typeof(DVaultServiceCollectionExtensions).Assembly,
        "DCoding.Data.DVault.approved.txt");
  }

  [Fact]
  public void SqlitePublicApiMatchesApprovedSnapshot() {
    AssertPublicApiMatchesApprovedSnapshot(
        "DCoding.Data.DVault.Sqlite",
        typeof(DVaultSqliteServiceCollectionExtensions).Assembly,
        "DCoding.Data.DVault.Sqlite.approved.txt");
  }

  [Fact]
  public void PostgresPublicApiMatchesApprovedSnapshot() {
    AssertPublicApiMatchesApprovedSnapshot(
        "DCoding.Data.DVault.Postgres",
        typeof(DVaultPostgresServiceCollectionExtensions).Assembly,
        "DCoding.Data.DVault.Postgres.approved.txt");
  }

  [Fact]
  public void SqlServerPublicApiMatchesApprovedSnapshot() {
    AssertPublicApiMatchesApprovedSnapshot(
        "DCoding.Data.DVault.SqlServer",
        typeof(DVaultSqlServerServiceCollectionExtensions).Assembly,
        "DCoding.Data.DVault.SqlServer.approved.txt");
  }

  [Fact]
  public void OraclePublicApiMatchesApprovedSnapshot() {
    AssertPublicApiMatchesApprovedSnapshot(
        "DCoding.Data.DVault.Oracle",
        typeof(DVaultOracleServiceCollectionExtensions).Assembly,
        "DCoding.Data.DVault.Oracle.approved.txt");
  }

  [Fact]
  public void MySqlPublicApiMatchesApprovedSnapshot() {
    AssertPublicApiMatchesApprovedSnapshot(
        "DCoding.Data.DVault.MySql",
        typeof(DVaultMySqlServiceCollectionExtensions).Assembly,
        "DCoding.Data.DVault.MySql.approved.txt");
  }

  [Fact]
  public void Db2PublicApiMatchesApprovedSnapshot() {
    AssertPublicApiMatchesApprovedSnapshot(
        "DCoding.Data.DVault.Db2",
        typeof(DVaultDb2ServiceCollectionExtensions).Assembly,
        "DCoding.Data.DVault.Db2.approved.txt");
  }

  [Fact]
  public void PrivacyPublicApiMatchesApprovedSnapshot() {
    AssertPublicApiMatchesApprovedSnapshot(
        "DCoding.Data.DVault.Privacy",
        typeof(DVaultPrivacyServiceCollectionExtensions).Assembly,
        "DCoding.Data.DVault.Privacy.approved.txt");
  }

  private static void AssertPublicApiMatchesApprovedSnapshot(
      string packageId,
      Assembly assembly,
      string snapshotFileName,
      [CallerFilePath] string sourceFilePath = "") {
    var actual = CreatePublicApiSnapshot(packageId, assembly);
    var snapshotPath = GetSnapshotPath(sourceFilePath, snapshotFileName);

    if (ShouldUpdateSnapshots()) {
      Directory.CreateDirectory(Path.GetDirectoryName(snapshotPath)!);
      File.WriteAllText(snapshotPath, actual);
    }

    if (!File.Exists(snapshotPath)) {
      throw new InvalidOperationException(
          "Missing approved public API snapshot for package '" +
          packageId +
          "' at " +
          snapshotPath +
          ". Run with " +
          UpdateSnapshotsEnvironmentVariable +
          "=1 to approve the current built API.");
    }

    var expected = NormalizeLineEndings(File.ReadAllText(snapshotPath));

    Assert.Equal(expected, actual);
  }

  private static string CreatePublicApiSnapshot(string packageId, Assembly assembly) {
    var builder = new StringBuilder();
    builder.AppendLine("# DVault public API snapshot");
    builder.AppendLine("# Package: " + packageId);
    builder.AppendLine("# Assembly: " + assembly.GetName().Name);
    builder.AppendLine("# Generated from built assembly output.");
    builder.AppendLine("# Update intentionally with: " + UpdateSnapshotsEnvironmentVariable + "=1 dotnet test DVault.slnx --nologo --filter FullyQualifiedName~ApiSurfaceSnapshotTests");

    foreach (var type in assembly.GetExportedTypes().OrderBy(FormatTypeName, StringComparer.Ordinal)) {
      AppendType(builder, type);
    }

    return NormalizeLineEndings(builder.ToString());
  }

  private static void AppendType(StringBuilder builder, Type type) {
    builder.AppendLine();
    builder.AppendLine("type " + FormatTypeDeclaration(type));

    if (type.IsEnum) {
      AppendEnumValues(builder, type);
      return;
    }

    var constructors = type
        .GetConstructors(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
        .Select(FormatConstructor)
        .Order(StringComparer.Ordinal);

    foreach (var constructor in constructors) {
      builder.AppendLine("  ctor " + constructor);
    }

    var fields = type
        .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly)
        .Where(field => !field.IsSpecialName)
        .Select(FormatField)
        .Order(StringComparer.Ordinal);

    foreach (var field in fields) {
      builder.AppendLine("  field " + field);
    }

    var properties = type
        .GetProperties(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly)
        .Select(FormatProperty)
        .Order(StringComparer.Ordinal);

    foreach (var property in properties) {
      builder.AppendLine("  property " + property);
    }

    var events = type
        .GetEvents(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly)
        .Select(FormatEvent)
        .Order(StringComparer.Ordinal);

    foreach (var eventMember in events) {
      builder.AppendLine("  event " + eventMember);
    }

    var accessorMethods = GetPropertyAndEventAccessors(type);
    var methods = type
        .GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly)
        .Where(method => !accessorMethods.Contains(method))
        .Where(method => !IsUnspeakableCompilerMember(method))
        .Select(FormatMethod)
        .Order(StringComparer.Ordinal);

    foreach (var method in methods) {
      builder.AppendLine("  method " + method);
    }
  }

  private static void AppendEnumValues(StringBuilder builder, Type type) {
    var values = Enum
        .GetValues(type)
        .Cast<object>()
        .Select(value => new EnumValue(Enum.GetName(type, value)!, Convert.ToInt64(value, CultureInfo.InvariantCulture)))
        .OrderBy(value => value.Value)
        .ThenBy(value => value.Name, StringComparer.Ordinal);

    foreach (var value in values) {
      builder.AppendLine("  value " + value.Name + " = " + value.Value.ToString(CultureInfo.InvariantCulture));
    }
  }

  private static HashSet<MethodInfo> GetPropertyAndEventAccessors(Type type) {
    var accessors = new HashSet<MethodInfo>();

    foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly)) {
      if (property.GetMethod is not null) {
        accessors.Add(property.GetMethod);
      }

      if (property.SetMethod is not null) {
        accessors.Add(property.SetMethod);
      }
    }

    foreach (var eventMember in type.GetEvents(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly)) {
      if (eventMember.AddMethod is not null) {
        accessors.Add(eventMember.AddMethod);
      }

      if (eventMember.RemoveMethod is not null) {
        accessors.Add(eventMember.RemoveMethod);
      }
    }

    return accessors;
  }

  private static string FormatTypeDeclaration(Type type) {
    var builder = new StringBuilder("public ");

    if (type.IsInterface) {
      builder.Append("interface ");
    }
    else if (type.IsEnum) {
      builder.Append("enum ");
    }
    else if (type.IsValueType) {
      builder.Append("struct ");
    }
    else if (type.IsAbstract && type.IsSealed) {
      builder.Append("static class ");
    }
    else {
      if (type.IsAbstract) {
        builder.Append("abstract ");
      }

      if (type.IsSealed) {
        builder.Append("sealed ");
      }

      builder.Append("class ");
    }

    builder.Append(FormatTypeName(type));

    var inheritedTypes = GetInheritedTypes(type).ToArray();
    if (inheritedTypes.Length > 0) {
      builder.Append(" : ");
      builder.Append(string.Join(", ", inheritedTypes));
    }

    AppendGenericConstraints(builder, type.GetGenericArguments());

    return builder.ToString();
  }

  private static IEnumerable<string> GetInheritedTypes(Type type) {
    if (!type.IsInterface && !type.IsEnum && type.BaseType is not null && type.BaseType != typeof(object) && type.BaseType != typeof(ValueType)) {
      yield return FormatType(type.BaseType);
    }

    if (type.IsEnum) {
      yield break;
    }

    foreach (var interfaceType in type.GetInterfaces().OrderBy(FormatType, StringComparer.Ordinal)) {
      yield return FormatType(interfaceType);
    }
  }

  private static string FormatConstructor(ConstructorInfo constructor) {
    return "public " +
        FormatTypeShortName(constructor.DeclaringType!) +
        "(" +
        FormatParameters(constructor.GetParameters(), extensionMethod: false) +
        ")";
  }

  private static string FormatField(FieldInfo field) {
    var builder = new StringBuilder("public ");

    if (field.IsLiteral && !field.IsInitOnly) {
      builder.Append("const ");
    }
    else if (field.IsStatic) {
      builder.Append("static ");
      if (field.IsInitOnly) {
        builder.Append("readonly ");
      }
    }

    builder.Append(FormatNullableType(field.FieldType, Nullability.Create(field)));
    builder.Append(' ');
    builder.Append(field.Name);

    if (field.IsLiteral) {
      builder.Append(" = ");
      builder.Append(FormatConstantValue(field.GetRawConstantValue(), field.FieldType));
    }

    return builder.ToString();
  }

  private static string FormatProperty(PropertyInfo property) {
    var accessor = property.GetMethod ?? property.SetMethod!;
    var builder = new StringBuilder("public ");

    if (accessor.IsStatic) {
      builder.Append("static ");
    }

    builder.Append(FormatNullableType(property.PropertyType, Nullability.Create(property)));
    builder.Append(' ');
    builder.Append(property.GetIndexParameters().Length == 0 ? property.Name : "Item[" + FormatParameters(property.GetIndexParameters(), extensionMethod: false) + "]");
    builder.Append(" { ");

    if (property.GetMethod?.IsPublic == true) {
      builder.Append("get; ");
    }

    if (property.SetMethod?.IsPublic == true) {
      builder.Append(IsInitOnly(property.SetMethod) ? "init; " : "set; ");
    }

    builder.Append('}');

    return builder.ToString();
  }

  private static string FormatEvent(EventInfo eventMember) {
    var accessor = eventMember.AddMethod ?? eventMember.RemoveMethod!;
    var builder = new StringBuilder("public ");

    if (accessor.IsStatic) {
      builder.Append("static ");
    }

    builder.Append(FormatType(eventMember.EventHandlerType!));
    builder.Append(' ');
    builder.Append(eventMember.Name);

    return builder.ToString();
  }

  private static string FormatMethod(MethodInfo method) {
    var builder = new StringBuilder("public ");

    if (method.IsStatic) {
      builder.Append("static ");
    }
    else if (method.IsAbstract) {
      builder.Append("abstract ");
    }
    else if (method.IsVirtual) {
      builder.Append(method.GetBaseDefinition() == method ? "virtual " : "override ");
    }

    builder.Append(FormatNullableType(method.ReturnType, Nullability.Create(method.ReturnParameter)));
    builder.Append(' ');
    builder.Append(method.Name);

    var genericArguments = method.GetGenericArguments();
    if (genericArguments.Length > 0) {
      builder.Append('<');
      builder.Append(string.Join(", ", genericArguments.Select(argument => argument.Name)));
      builder.Append('>');
    }

    builder.Append('(');
    builder.Append(FormatParameters(method.GetParameters(), method.IsDefined(typeof(ExtensionAttribute), inherit: false)));
    builder.Append(')');
    AppendGenericConstraints(builder, genericArguments);

    return builder.ToString();
  }

  private static string FormatParameters(IReadOnlyList<ParameterInfo> parameters, bool extensionMethod) {
    return string.Join(
        ", ",
        parameters.Select((parameter, index) => FormatParameter(parameter, extensionMethod && index == 0)));
  }

  private static string FormatParameter(ParameterInfo parameter, bool extensionThisParameter) {
    var builder = new StringBuilder();
    var parameterType = parameter.ParameterType;

    if (extensionThisParameter) {
      builder.Append("this ");
    }
    else if (parameter.IsOut) {
      builder.Append("out ");
    }
    else if (parameterType.IsByRef && IsReadOnlyByReferenceParameter(parameter)) {
      builder.Append("in ");
    }
    else if (parameterType.IsByRef) {
      builder.Append("ref ");
    }

    if (parameterType.IsByRef) {
      parameterType = parameterType.GetElementType()!;
    }

    builder.Append(FormatNullableType(parameterType, Nullability.Create(parameter)));
    builder.Append(' ');
    builder.Append(parameter.Name);

    if (parameter.HasDefaultValue) {
      builder.Append(" = ");
      builder.Append(FormatConstantValue(parameter.RawDefaultValue, parameterType));
    }

    return builder.ToString();
  }

  private static void AppendGenericConstraints(StringBuilder builder, IEnumerable<Type> genericArguments) {
    foreach (var genericArgument in genericArguments.Where(argument => argument.IsGenericParameter)) {
      var constraints = new List<string>();
      var attributes = genericArgument.GenericParameterAttributes;

      if ((attributes & GenericParameterAttributes.ReferenceTypeConstraint) != 0) {
        constraints.Add("class");
      }

      if ((attributes & GenericParameterAttributes.NotNullableValueTypeConstraint) != 0) {
        constraints.Add("struct");
      }

      constraints.AddRange(genericArgument.GetGenericParameterConstraints().Select(FormatType));

      if ((attributes & GenericParameterAttributes.DefaultConstructorConstraint) != 0 &&
          (attributes & GenericParameterAttributes.NotNullableValueTypeConstraint) == 0) {
        constraints.Add("new()");
      }

      if (constraints.Count > 0) {
        builder.Append(" where ");
        builder.Append(genericArgument.Name);
        builder.Append(" : ");
        builder.Append(string.Join(", ", constraints.Distinct(StringComparer.Ordinal)));
      }
    }
  }

  private static string FormatNullableType(Type type, NullabilityInfo? nullabilityInfo) {
    if (type == typeof(void)) {
      return "void";
    }

    if (type.IsByRef) {
      type = type.GetElementType()!;
    }

    if (Nullable.GetUnderlyingType(type) is { } underlyingType) {
      return FormatType(underlyingType) + "?";
    }

    if (type.IsArray) {
      return FormatNullableType(type.GetElementType()!, nullabilityInfo?.ElementType) + "[]";
    }

    if (type.IsGenericType) {
      var genericTypeDefinition = type.GetGenericTypeDefinition();
      var genericArguments = type.GetGenericArguments();
      var nullableArguments = nullabilityInfo?.GenericTypeArguments ?? [];
      var formattedArguments = genericArguments
          .Select((argument, index) => FormatNullableType(argument, index < nullableArguments.Length ? nullableArguments[index] : null))
          .ToArray();
      var formattedType = FormatTypeName(genericTypeDefinition) + "<" + string.Join(", ", formattedArguments) + ">";

      return AddNullableSuffix(type, nullabilityInfo, formattedType);
    }

    return AddNullableSuffix(type, nullabilityInfo, FormatType(type));
  }

  private static string AddNullableSuffix(Type type, NullabilityInfo? nullabilityInfo, string formattedType) {
    return !type.IsValueType && nullabilityInfo?.ReadState == NullabilityState.Nullable
        ? formattedType + "?"
        : formattedType;
  }

  private static string FormatType(Type type) {
    if (type == typeof(void)) {
      return "void";
    }

    if (type.IsByRef) {
      return FormatType(type.GetElementType()!);
    }

    if (BuiltInTypeNames.TryGetValue(type, out var builtInName)) {
      return builtInName;
    }

    if (Nullable.GetUnderlyingType(type) is { } underlyingType) {
      return FormatType(underlyingType) + "?";
    }

    if (type.IsArray) {
      return FormatType(type.GetElementType()!) + "[]";
    }

    if (type.IsGenericParameter) {
      return type.Name;
    }

    if (type.IsGenericType) {
      return FormatTypeName(type.GetGenericTypeDefinition()) +
          "<" +
          string.Join(", ", type.GetGenericArguments().Select(FormatType)) +
          ">";
    }

    return FormatTypeName(type);
  }

  private static string FormatTypeName(Type type) {
    return RemoveGenericArity((type.FullName ?? type.Name).Replace('+', '.'));
  }

  private static string FormatTypeShortName(Type type) {
    return RemoveGenericArity(type.Name);
  }

  private static string RemoveGenericArity(string name) {
    var builder = new StringBuilder();

    for (var index = 0; index < name.Length; index++) {
      if (name[index] == '`') {
        index++;
        while (index < name.Length && char.IsDigit(name[index])) {
          index++;
        }

        index--;
        continue;
      }

      builder.Append(name[index]);
    }

    return builder.ToString();
  }

  private static bool IsInitOnly(MethodInfo setMethod) {
    return setMethod.ReturnParameter
        .GetRequiredCustomModifiers()
        .Any(modifier => modifier.FullName == "System.Runtime.CompilerServices.IsExternalInit");
  }

  private static bool IsReadOnlyByReferenceParameter(ParameterInfo parameter) {
    return parameter
        .GetRequiredCustomModifiers()
        .Any(modifier => modifier.FullName == "System.Runtime.CompilerServices.IsReadOnlyAttribute");
  }

  private static bool IsUnspeakableCompilerMember(MethodInfo method) {
    return method.Name.Contains('<', StringComparison.Ordinal) ||
        method.Name.Contains('$', StringComparison.Ordinal);
  }

  private static bool ShouldUpdateSnapshots() {
    return string.Equals(
        Environment.GetEnvironmentVariable(UpdateSnapshotsEnvironmentVariable),
        "1",
        StringComparison.Ordinal);
  }

  private static string GetSnapshotPath(string sourceFilePath, string snapshotFileName) {
    var sourceDirectory = Path.GetDirectoryName(sourceFilePath);
    if (!string.IsNullOrWhiteSpace(sourceDirectory) && Directory.Exists(sourceDirectory)) {
      return Path.Combine(sourceDirectory, "Snapshots", "PublicApi", snapshotFileName);
    }

    return Path.Combine(
        FindRepositoryRoot(),
        "tests",
        "DCoding.Data.DVault.Tests",
        "Unit",
        "Snapshots",
        "PublicApi",
        snapshotFileName);
  }

  private static string FindRepositoryRoot() {
    var directory = new DirectoryInfo(AppContext.BaseDirectory);

    while (directory is not null) {
      if (File.Exists(Path.Combine(directory.FullName, "DVault.slnx"))) {
        return directory.FullName;
      }

      directory = directory.Parent;
    }

    throw new InvalidOperationException("Unable to locate the DVault repository root from the test output directory.");
  }

  private static string NormalizeLineEndings(string value) {
    return value.Replace("\r\n", "\n", StringComparison.Ordinal);
  }

  private static string FormatConstantValue(object? value, Type valueType) {
    if (value is null) {
      return valueType.IsValueType && Nullable.GetUnderlyingType(valueType) is null ? "default" : "null";
    }

    if (value is string stringValue) {
      return "\"" + EscapeString(stringValue) + "\"";
    }

    if (value is char charValue) {
      return "'" + EscapeString(charValue.ToString()) + "'";
    }

    if (value is bool boolValue) {
      return boolValue ? "true" : "false";
    }

    if (value is Enum enumValue) {
      return FormatType(valueType) + "." + enumValue;
    }

    if (value is float floatValue) {
      return floatValue.ToString("R", CultureInfo.InvariantCulture) + "f";
    }

    if (value is double doubleValue) {
      return doubleValue.ToString("R", CultureInfo.InvariantCulture) + "d";
    }

    if (value is decimal decimalValue) {
      return decimalValue.ToString(CultureInfo.InvariantCulture) + "m";
    }

    return Convert.ToString(value, CultureInfo.InvariantCulture) ?? "default";
  }

  private static string EscapeString(string value) {
    return value
        .Replace("\\", "\\\\", StringComparison.Ordinal)
        .Replace("\"", "\\\"", StringComparison.Ordinal)
        .Replace("\r", "\\r", StringComparison.Ordinal)
        .Replace("\n", "\\n", StringComparison.Ordinal)
        .Replace("\t", "\\t", StringComparison.Ordinal);
  }

  private static readonly NullabilityInfoContext Nullability = new();

  private static readonly Dictionary<Type, string> BuiltInTypeNames = new() {
    [typeof(bool)] = "bool",
    [typeof(byte)] = "byte",
    [typeof(char)] = "char",
    [typeof(decimal)] = "decimal",
    [typeof(double)] = "double",
    [typeof(float)] = "float",
    [typeof(int)] = "int",
    [typeof(long)] = "long",
    [typeof(object)] = "object",
    [typeof(sbyte)] = "sbyte",
    [typeof(short)] = "short",
    [typeof(string)] = "string",
    [typeof(uint)] = "uint",
    [typeof(ulong)] = "ulong",
    [typeof(ushort)] = "ushort",
  };

  private sealed record EnumValue(string Name, long Value);
}
