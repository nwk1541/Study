---
name: code-simplifier
description: Simplify or review user-specified code or defined code changes for clarity and long-term maintainability while preserving existing externally observable behavior and relevant contracts. Use when explicitly asked to simplify code or review unnecessary abstractions, redundant defensive checks, duplicated state, or avoidable complexity. Do not treat this as general feature development or bug fixing.
---

# Code Simplifier

Simplify code only when there is a clear maintainability benefit. Preserve existing externally observable behavior and relevant contracts; do not infer the developer's intent or silently fix suspected bugs. Prefer fewer concepts and change points over fewer lines. Leaving already-clear code unchanged is a valid result.

## Scope and mode

- **Mode:** Apply justified simplifications when the user requests edits, including requests to review and simplify. For review, analysis, or suggestions without an edit request, report findings without modifying files.
- Prefer user-specified files, symbols, hunks, or commit ranges. Without an explicit target, inspect local Git changes to identify a coherent task; do not assume the entire worktree belongs to one task. If scope remains ambiguous, leave ambiguous areas untouched and request clarification only when necessary to proceed.
- Read neighboring code as needed, but restrict edits to the authorized scope, including within partially edited files. Preserve unrelated staged, unstaged, and untracked user work. Never stage or commit changes unless explicitly requested; never discard or overwrite unrelated user changes.
- Follow applicable repository guidance and existing project conventions. Use local sources by default; do not fetch external documentation or upstream skill definitions unless required for a specific unresolved dependency or explicitly requested.

## Judgment and verification

- Look for unnecessary abstractions, forwarding layers, duplicated state, repeated checks, and speculative generality. Keep abstractions and safeguards that serve real boundaries, invariants, or execution paths; do not mechanically add or remove them.
- Include unnecessary intermediate collections, copies, and immediately invoked lambdas or closures as simplification candidates. Prefer direct iteration when it is clearer and the copy or callback has no required semantic role. Distinguish statically evident allocation removal from measured performance gains.
- Match analysis and verification depth to the change's actual behavioral risk. Investigate relevant contracts, callers, concurrency, ownership, lifecycle, serialization, and performance **when affected**, not as a mandatory checklist for every edit.
- Resolve uncertainty through focused code analysis and proportionate verification. If a meaningful behavior or ownership risk remains unresolved, leave that candidate unchanged, report the uncertainty, and continue with other safe candidates.
- For Unity/C# changes where engine-specific semantics, asset lifetimes, or performance could matter, consult [Unity/C# safeguards](references/unity-csharp.md) before making affected changes.
- Verify modified code with suitable available checks and inspect the final diff for accidental edits. Tests and inspection provide evidence, not proof of complete behavioral equivalence.

## Output

Briefly report meaningful changes and why they help, verification actually performed, and outstanding risks. In review-only mode, identify specific candidates and locations. If no worthwhile simplification is found, say so without forcing edits.
