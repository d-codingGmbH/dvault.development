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
scanned_files=0

report() {
  printf 'one-member-per-file violation: %s: %s\n' "$1" "$2" >&2
  status=1
}

is_csharp_source_excluded() {
  case "$1" in
    .git|.git/*|.gicket|.gicket/*|.gicket-bot|.gicket-bot/*)
      return 0
      ;;
    */bin/*|*/obj/*)
      return 0
      ;;
    */generated/*|*.generated.*|*.g.cs|*.designer.cs)
      return 0
      ;;
  esac

  return 1
}

count_top_level_declarations() {
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
          code ~ /^[[:space:]]*((public|internal|file|protected|private|sealed|static|abstract|partial|readonly|ref|unsafe)[[:space:]]+)*(class|struct|interface|enum|delegate|record)([[:space:]]+(class|struct))?[[:space:]]+[A-Za-z_][A-Za-z0-9_]*/) {
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

list_csharp_source_files() {
  if git rev-parse --is-inside-work-tree >/dev/null 2>&1; then
    git ls-files -z --cached --others --exclude-standard -- '*.cs'
    return
  fi

  find . \
    \( \
      -path './.git' -o \
      -path './.gicket' -o \
      -path './.gicket-bot' -o \
      -path './bin' -o \
      -path './*/bin' -o \
      -path './obj' -o \
      -path './*/obj' \
    \) -prune -o \
    -type f -name '*.cs' -print0
}

check_source_file() {
  path=$1
  declaration_count=$(count_top_level_declarations "$path")

  if [ "$declaration_count" -le 1 ]; then
    return
  fi

  report "$path" "contains $declaration_count top-level type declarations; move each declaration to its own file"
}

if [ "${BASH_VERSINFO[0]:-0}" -lt 4 ]; then
  echo "one-member-per-file check error: bash 4 or newer is required" >&2
  exit 2
fi

source_file_list=$(mktemp "${TMPDIR:-/tmp}/dvault-one-member-files.XXXXXX") || {
  echo "one-member-per-file check error: unable to create a temporary source list" >&2
  exit 2
}

if ! list_csharp_source_files >"$source_file_list"; then
  rm -f "$source_file_list"
  echo "one-member-per-file check error: unable to list C# source files" >&2
  exit 2
fi

while IFS= read -r -d '' path; do
  path=${path#./}
  [ -f "$path" ] || continue
  is_csharp_source_excluded "$path" && continue

  scanned_files=$((scanned_files + 1))
  check_source_file "$path"
done < "$source_file_list"

rm -f "$source_file_list"

if [ "$status" -eq 0 ]; then
  echo "One-member-per-file check passed for $scanned_files C# files."
fi

exit "$status"
