Goal: prove that provider strategy dispatch chooses optimized implementations only when the provider package registers compatible capabilities.

Acceptance Criteria:
- Tests cover fallback selection, optimized selection, missing capability registration, and unknown provider behavior.
- Tests do not require live external database services.
- Failure messages make it clear which provider capability or registration path is broken.