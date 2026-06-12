#!/usr/bin/env bash
set -euo pipefail

script_dir="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
repo_root="$(cd "$script_dir/.." && pwd)"
package_dir="$repo_root/artifacts/packages"

runtime_projects=(
  "src/DCoding.Data.DVault/DCoding.Data.DVault.csproj"
  "src/DCoding.Data.DVault.Db2/DCoding.Data.DVault.Db2.csproj"
  "src/DCoding.Data.DVault.MySql/DCoding.Data.DVault.MySql.csproj"
  "src/DCoding.Data.DVault.Oracle/DCoding.Data.DVault.Oracle.csproj"
  "src/DCoding.Data.DVault.Postgres/DCoding.Data.DVault.Postgres.csproj"
  "src/DCoding.Data.DVault.Sqlite/DCoding.Data.DVault.Sqlite.csproj"
  "src/DCoding.Data.DVault.SqlServer/DCoding.Data.DVault.SqlServer.csproj"
)

analyzer_project="src/DCoding.Data.DVault.Analyzers/DCoding.Data.DVault.Analyzers.csproj"

mkdir -p "$package_dir"
find "$package_dir" -maxdepth 1 -type f \( -name "*.nupkg" -o -name "*.snupkg" \) -delete

pack_runtime_line() {
  local version="$1"
  local target_framework="$2"

  for project in "${runtime_projects[@]}"; do
    dotnet pack "$repo_root/$project" \
      --configuration Release \
      --nologo \
      -p:TargetFramework="$target_framework" \
      -p:TargetFrameworks="$target_framework" \
      -p:MinVerVersionOverride="$version" \
      -p:PackageOutputPath="$package_dir"
  done
}

pack_analyzer_line() {
  local version="$1"

  dotnet pack "$repo_root/$analyzer_project" \
    --configuration Release \
    --nologo \
    -p:MinVerVersionOverride="$version" \
    -p:PackageOutputPath="$package_dir"
}

pack_line() {
  local version="$1"
  local target_framework="$2"

  echo "Packing DVault package line $version for $target_framework"
  pack_runtime_line "$version" "$target_framework"
  pack_analyzer_line "$version"
}

pack_line "8.36.0" "net8.0"
pack_line "10.36.0" "net10.0"
