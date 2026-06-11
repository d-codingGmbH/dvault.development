# One Member Per File

DVault enforces one top-level C# declaration per source file across the repository. The policy applies to public, internal, private, and file-local top-level `class`, `struct`, `interface`, `record`, `enum`, and `delegate` declarations.

Generated source and `bin` or `obj` output are outside this policy. Nested helper types remain allowed when they belong to the containing type and are not part of the file-level source layout.

## Automated Check

Normal local validation runs the policy through:

```sh
bash tools/check-format.sh
```

To run only this source-layout check:

```sh
bash tools/check-one-member-per-file.sh
```

The check scans tracked and untracked C# files in the repository and fails when a file contains more than one top-level declaration. Failure output includes the repository-relative source path so the declaration can be moved or reviewed directly.

There is no baseline exception list. If a future source file genuinely needs multiple top-level declarations, the policy and automated check should be changed together with a short rationale.
