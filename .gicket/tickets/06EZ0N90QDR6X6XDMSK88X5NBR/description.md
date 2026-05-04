Goal: create a concise provider capability matrix for the v0.5 optimization work.

Acceptance Criteria:
- The matrix covers SQLite, PostgreSQL, SQL Server, Oracle, and MySQL.
- Each provider entry states whether optimized insert-only save behavior, set-based existence checks, integration coverage, and benchmark coverage are required in this release.
- The document distinguishes required local validation from opt-in external database validation.
- The document names the provider-neutral fallback as the compatibility baseline.