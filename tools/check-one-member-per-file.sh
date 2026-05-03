#!/usr/bin/env bash
set -uo pipefail

script_dir=$(CDPATH= cd -- "$(dirname -- "$0")" && pwd -P)
script_repo_root=$(CDPATH= cd -- "$script_dir/.." && pwd -P)
repo_root=$(git -C "$script_repo_root" rev-parse --show-toplevel 2>/dev/null)
if [ -z "${repo_root:-}" ]; then
  repo_root=$script_repo_root
fi

cd "$repo_root" || exit 2

status=0
exception_file="docs/quality/one-member-per-file-exceptions.txt"
scanned_files=0

packable_project_roots=(
  "src/DCoding.Data.DVault"
  "src/DCoding.Data.DVault.MySql"
  "src/DCoding.Data.DVault.Oracle"
  "src/DCoding.Data.DVault.Postgres"
  "src/DCoding.Data.DVault.Sqlite"
  "src/DCoding.Data.DVault.SqlServer"
)

report() {
  printf 'one-member-per-file violation: %s: %s\n' "$1" "$2" >&2
  status=1
}

is_csharp_source_excluded() {
  case "$1" in
    */bin/*|*/obj/*)
      return 0
      ;;
    */generated/*|*.generated.*|*.g.cs|*.designer.cs)
      return 0
      ;;
  esac

  return 1
}

load_exceptions() {
  if [ ! -f "$exception_file" ]; then
    report "$exception_file" "required documented exception list is missing"
    return
  fi

  while IFS= read -r exception_path || [ -n "$exception_path" ]; do
    case "$exception_path" in
      ''|\#*)
        continue
        ;;
    esac

    exceptions["$exception_path"]=1
  done < "$exception_file"
}

is_packable_project_source() {
  path=$1

  for root in "${packable_project_roots[@]}"; do
    case "$path" in
      "$root"/*.cs|"$root"/*/*.cs|"$root"/*/*/*.cs|"$root"/*/*/*/*.cs)
        return 0
        ;;
    esac
  done

  return 1
}

count_public_or_protected_top_level_declarations() {
  awk '
    function strip_csharp(line, result, i, ch, next_ch, quote) {
      result = ""

      for (i = 1; i <= length(line); i++) {
        ch = substr(line, i, 1)
        next_ch = substr(line, i + 1, 1)

        if (in_block_comment) {
          if (ch == "*" && next_ch == "/") {
            in_block_comment = 0
            i++
          }

          continue
        }

        if (ch == "/" && next_ch == "*") {
          in_block_comment = 1
          i++
          continue
        }

        if (ch == "/" && next_ch == "/") {
          break
        }

        if (ch == "\"" || ch == "\047") {
          quote = ch
          result = result " "
          i++

          while (i <= length(line)) {
            ch = substr(line, i, 1)
            next_ch = substr(line, i + 1, 1)

            if (ch == "\\" && quote == "\"") {
              i += 2
              continue
            }

            if (ch == quote) {
              if (quote == "\"" && next_ch == "\"") {
                i += 2
                continue
              }

              break
            }

            i++
          }

          continue
        }

        result = result ch
      }

      return result
    }

    BEGIN {
      brace_depth = 0
      top_level_depth = 0
      count = 0
      in_block_comment = 0
    }

    {
      code = strip_csharp($0)

      if (brace_depth == top_level_depth &&
          code ~ /^[[:space:]]*(public|protected)([[:space:]]+[A-Za-z_][A-Za-z0-9_]*)*[[:space:]]+(class|struct|interface|enum|delegate|record)([[:space:]]+(class|struct))?[[:space:]]+[A-Za-z_][A-Za-z0-9_]*/) {
        count++
      }

      if (brace_depth == 0 && code ~ /^[[:space:]]*namespace[[:space:]]+[^;{]+[{]/) {
        top_level_depth = 1
      }

      open_code = code
      close_code = code
      open_count = gsub(/\{/, "{", open_code)
      close_count = gsub(/\}/, "}", close_code)
      brace_depth += open_count - close_count

      if (brace_depth < 0) {
        brace_depth = 0
      }
    }

    END {
      print count
    }
  ' "$1"
}

check_exception_contract() {
  for exception_path in "${!exceptions[@]}"; do
    if ! is_packable_project_source "$exception_path"; then
      report "$exception_path" "documented exception is outside the six packable DVault source projects"
      continue
    fi

    if [ ! -f "$exception_path" ]; then
      report "$exception_path" "documented exception path does not exist"
      continue
    fi

    declaration_count=$(count_public_or_protected_top_level_declarations "$exception_path")
    if [ "$declaration_count" -le 1 ]; then
      report "$exception_path" "documented exception is stale because it now contains $declaration_count public/protected top-level declarations"
    fi
  done
}

check_source_file() {
  path=$1
  declaration_count=$(count_public_or_protected_top_level_declarations "$path")

  if [ "$declaration_count" -le 1 ]; then
    return
  fi

  if [ "${exceptions[$path]+set}" = "set" ]; then
    return
  fi

  report "$path" "contains $declaration_count public/protected top-level declarations; move each declaration to its own file or document a practical exception in $exception_file"
}

if [ "${BASH_VERSINFO[0]:-0}" -lt 4 ]; then
  echo "one-member-per-file check error: bash 4 or newer is required" >&2
  exit 2
fi

declare -A exceptions
load_exceptions

for project_root in "${packable_project_roots[@]}"; do
  if [ ! -d "$project_root" ]; then
    report "$project_root" "configured packable project source root is missing"
    continue
  fi

  while IFS= read -r -d '' path; do
    case "$path" in
      *.cs)
        ;;
      *)
        continue
        ;;
    esac

    is_csharp_source_excluded "$path" && continue

    scanned_files=$((scanned_files + 1))
    check_source_file "$path"
  done < <(git ls-files -z --cached --others --exclude-standard -- "$project_root")
done

check_exception_contract

if [ "$status" -eq 0 ]; then
  echo "One-member-per-file check passed for $scanned_files packable source files."
fi

exit "$status"
