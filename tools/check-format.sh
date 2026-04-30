#!/usr/bin/env bash
set -uo pipefail

script_dir=$(CDPATH= cd -- "$(dirname -- "$0")" && pwd -P)
script_repo_root=$(CDPATH= cd -- "$script_dir/.." && pwd -P)
repo_root=$(git -C "$script_repo_root" rev-parse --show-toplevel 2>/dev/null)
if [ -z "${repo_root:-}" ]; then
  repo_root=$script_repo_root
fi

cd "$repo_root" || exit 2

if ! command -v iconv >/dev/null 2>&1; then
  echo "format check error: iconv is required to verify UTF-8 text" >&2
  exit 2
fi

status=0

report() {
  printf 'format violation: %s: %s\n' "$1" "$2" >&2
  status=1
}

require_file_line() {
  file=$1
  expected=$2

  if [ ! -f "$file" ]; then
    report "$file" "required formatting policy source is missing"
    return
  fi

  if ! LC_ALL=C grep -Fx -- "$expected" "$file" >/dev/null; then
    report "$file" "must contain formatting rule: $expected"
  fi
}

check_policy_sources() {
  require_file_line ".editorconfig" "indent_style = space"
  require_file_line ".editorconfig" "indent_size = 2"
  require_file_line ".editorconfig" "end_of_line = lf"
  require_file_line ".editorconfig" "charset = utf-8"
  require_file_line ".editorconfig" "insert_final_newline = true"
  require_file_line ".editorconfig" "trim_trailing_whitespace = true"
  require_file_line ".editorconfig" "csharp_new_line_before_open_brace = none"
  require_file_line ".editorconfig" "dotnet_diagnostic.IDE0055.severity = error"
  require_file_line ".editorconfig" "brace_style = 1tbs"
  require_file_line ".gitattributes" "* text=auto eol=lf"
}

check_dotnet_format() {
  if [ ! -f "DVault.slnx" ]; then
    return
  fi

  if ! command -v dotnet >/dev/null 2>&1; then
    report "DVault.slnx" "dotnet is required to verify C# formatting for this repository"
    return
  fi

  if ! dotnet format DVault.slnx --verify-no-changes --no-restore --verbosity minimal; then
    report "DVault.slnx" "dotnet format must pass without rewriting C# files"
  fi
}

is_excluded() {
  case "$1" in
    .git/*|.gicket/*|.gicket-bot/*)
      return 0
      ;;
    vendor/*|*/vendor/*|third_party/*|*/third_party/*|node_modules/*|*/node_modules/*)
      return 0
      ;;
    dist/*|*/dist/*|build/*|*/build/*|out/*|*/out/*|coverage/*|*/coverage/*|bin/*|*/bin/*|obj/*|*/obj/*)
      return 0
      ;;
    generated/*|*/generated/*|*.generated.*|*.g.cs|*.designer.cs)
      return 0
      ;;
    *.lock|package-lock.json|npm-shrinkwrap.json|pnpm-lock.yaml|yarn.lock|Pipfile.lock|poetry.lock|Cargo.lock|composer.lock|Gemfile.lock)
      return 0
      ;;
    *.png|*.jpg|*.jpeg|*.gif|*.webp|*.ico|*.bmp|*.tif|*.tiff|*.pdf|*.zip|*.gz|*.tgz|*.xz|*.7z|*.rar|*.tar)
      return 0
      ;;
    *.jar|*.war|*.dll|*.exe|*.so|*.dylib|*.a|*.lib|*.pdb|*.woff|*.woff2|*.ttf|*.eot|*.otf)
      return 0
      ;;
  esac

  return 1
}

is_tab_exception() {
  case "$1" in
    Makefile|makefile|*.mk|*/Makefile|*/makefile|*/*.mk)
      return 0
      ;;
  esac

  return 1
}

check_no_tabs() {
  path=$1

  if is_tab_exception "$path"; then
    return
  fi

  if LC_ALL=C grep -n $'\t' "$path" >/dev/null; then
    report "$path" "tabs are rejected in governed text files; only Makefile and *.mk are documented tab exceptions"
  fi
}

list_governed_files() {
  if git rev-parse --is-inside-work-tree >/dev/null 2>&1; then
    git ls-files -z --cached --others --exclude-standard -- .
    return
  fi

  find . -type f -print0
}

check_policy_sources

while IFS= read -r -d '' path; do
  path=${path#./}
  [ -f "$path" ] || continue
  is_excluded "$path" && continue

  if ! LC_ALL=C grep -Iq '' "$path"; then
    continue
  fi

  if ! iconv -f UTF-8 -t UTF-8 "$path" >/dev/null 2>&1; then
    report "$path" "must be valid UTF-8"
  fi

  if [ "$(head -c 3 "$path" | od -An -t x1 | tr -d ' \n')" = "efbbbf" ]; then
    report "$path" "must be UTF-8 without BOM"
  fi

  if LC_ALL=C grep -n $'\r' "$path" >/dev/null; then
    report "$path" "must use LF line endings"
  fi

  if LC_ALL=C grep -nE '[[:blank:]]$' "$path" >/dev/null; then
    report "$path" "must not contain trailing whitespace"
  fi

  check_no_tabs "$path"

  if [ -s "$path" ] && [ "$(tail -c 1 "$path" | od -An -t x1 | tr -d ' \n')" != "0a" ]; then
    report "$path" "must end with a final newline"
  fi
done < <(list_governed_files)

check_dotnet_format

if [ "$status" -eq 0 ]; then
  echo "Formatting check passed."
fi

exit "$status"
