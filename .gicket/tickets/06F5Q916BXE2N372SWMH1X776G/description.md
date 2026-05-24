Goal: Make bridge maintenance safe for destructive source-link topology changes.

Acceptance criteria:
- Defines explicit delete-aware maintenance separate from append-only MaintainBridgeAsync behavior.
- Handles hierarchy topology shrink, path removal, and increased TraversalDepth deterministically.
- Adds tests for many-to-many removal, hierarchy edge removal, shorter-path replacement, and longer-path correction.