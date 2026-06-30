#!/usr/bin/env bash
set -euo pipefail

if [ "$#" -ne 1 ]; then
  echo "Usage: $0 <sdk-major: 8|10>" >&2
  exit 2
fi

sdk_major="$1"
case "$sdk_major" in
  8)
    target_framework="net8.0"
    package_version="8.50.0"
    ;;
  10)
    target_framework="net10.0"
    package_version="10.50.0"
    ;;
  *)
    echo "Unsupported SDK major '$sdk_major'. Expected 8 or 10." >&2
    exit 2
    ;;
esac

script_dir="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
repo_root="$(cd "$script_dir/.." && pwd)"
package_dir="${DVAULT_PACKAGE_DIR:-$repo_root/artifacts/packages}"

runtime_package="$package_dir/DCoding.Data.DVault.$package_version.nupkg"
analyzer_package="$package_dir/DCoding.Data.DVault.Analyzers.$package_version.nupkg"

if [ ! -f "$runtime_package" ]; then
  echo "Missing runtime package '$runtime_package'. Run bash tools/pack-release-packages.sh first." >&2
  exit 1
fi

if [ ! -f "$analyzer_package" ]; then
  echo "Missing analyzer package '$analyzer_package'. Run bash tools/pack-release-packages.sh first." >&2
  exit 1
fi

sdk_version="$(
  dotnet --list-sdks |
    awk -v prefix="$sdk_major." '$1 ~ ("^" prefix) { version = $1 } END { print version }'
)"

if [ -z "$sdk_version" ]; then
  echo "No installed .NET $sdk_major SDK was found. Installed SDKs:" >&2
  dotnet --list-sdks >&2
  exit 1
fi

work_dir="$(mktemp -d "${TMPDIR:-/tmp}/dvault-analyzer-smoke.XXXXXX")"
cleanup() {
  rm -rf "$work_dir"
}
trap cleanup EXIT

cat > "$work_dir/global.json" <<JSON
{
  "sdk": {
    "version": "$sdk_version",
    "rollForward": "disable"
  }
}
JSON

cat > "$work_dir/NuGet.config" <<XML
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <packageSources>
    <clear />
    <add key="dvault-local-packages" value="$package_dir" />
  </packageSources>
</configuration>
XML

cat > "$work_dir/AnalyzerPackageSmoke.csproj" <<XML
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>$target_framework</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
    <NuGetAudit>false</NuGetAudit>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="DCoding.Data.DVault" Version="$package_version" />
    <PackageReference Include="DCoding.Data.DVault.Analyzers" Version="$package_version" PrivateAssets="all" />
  </ItemGroup>
</Project>
XML

cat > "$work_dir/Program.cs" <<'CS'
using DCoding.Data.DVault;

var mapper = AnalyzerSmokeCustomerSourceDataVaultHubMapping.CreateMapper();
var operation = mapper.Map(new AnalyzerSmokeCustomerSource("C-SDK-HOST", "DE"));

if (operation.HubName != "Customer" ||
    operation.BusinessKeyValues["Customer Id"] != "C-SDK-HOST" ||
    operation.BusinessKeyValues["Region Code"] != "DE") {
  throw new InvalidOperationException("Generated analyzer mapper output did not match the expected values.");
}

Console.WriteLine("DVault analyzer package smoke passed.");

[DataVaultHubMapping("Customer")]
[DataVaultBusinessKeyBinding(0, "Customer Id", nameof(CustomerId))]
[DataVaultBusinessKeyBinding(1, "Region Code", nameof(RegionCode))]
internal sealed record AnalyzerSmokeCustomerSource(string CustomerId, string RegionCode);
CS

echo "Running DVault analyzer package smoke for $target_framework on .NET SDK $sdk_version"
(
  cd "$work_dir"
  dotnet restore AnalyzerPackageSmoke.csproj --nologo
  dotnet build AnalyzerPackageSmoke.csproj --configuration Release --no-restore --nologo
  dotnet run --project AnalyzerPackageSmoke.csproj --configuration Release --no-build --no-restore --nologo
)
