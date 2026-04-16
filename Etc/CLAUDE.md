# CLAUDE.md

All responses must be in Korean (한국어).

## Coding Guidelines

**Tradeoff:** These guidelines bias toward accuracy, performance, and caution over speed. For trivial tasks, use your judgment.

### 1. Think Before Coding
**Don't assume. Don't hide confusion. Surface tradeoffs.**
- State your assumptions explicitly. If uncertain about the codebase or request, ask for clarification.
- If multiple interpretations or architectural choices exist, present them with comparison—don't pick silently.
- Even when the user specifies particular files or a modification direction, if a structurally better alternative exists, always propose it.
- If a simpler approach exists, say so. Push back when a request seems overly complex.
- If something is unclear, stop. Name what's confusing and ask.

### 2. Simplicity First
**Minimum code that solves the problem. Nothing speculative.**
- No features beyond what was explicitly asked.
- No abstractions, interfaces, or generic classes for single-use code.
- No "flexibility" or "configurability" that wasn't requested.
- No error handling for logically impossible scenarios.
- If you write 200 lines and it could be 50, rewrite it.
- Ask yourself: "Would a senior engineer say this is overcomplicated?" If yes, simplify.

### 3. Surgical Changes
**Touch only what you must. Clean up only your own mess.**
- When editing existing code, don't "improve" adjacent code, comments, or formatting simply because you are there.
- Don't refactor things that aren't broken.
- Strictly match the existing style and conventions, even if you'd do it differently.
- If you notice unrelated dead code or bugs, mention them—don't fix or delete them unprompted.
- When your changes create orphans: Remove imports/variables/functions that YOUR changes made unused. Don't remove pre-existing dead code unless asked.
- The test: Every changed line should trace directly to the user's specific request.

### 4. Goal-Driven Execution & Explicit Planning
**Define success criteria. Outline the plan in Markdown before coding. Loop until verified.**
- Markdown Plan First: Once a specific implementation approach is established, you MUST output a clear, step-by-step plan using Markdown before writing any actual code.
- Transform tasks into verifiable goals:
  - "Add validation" → "Write tests for invalid inputs, then make them pass."
  - "Fix the bug" → "Define how to reproduce it, then write the fix."
  - "Refactor X" → "List what functionality must remain identical before and after."
- For multi-step tasks, outline a brief plan before coding:
  1. [Step] → verify: [check]
  2. [Step] → verify: [check]
  3. [Step] → verify: [check]

### 5. Impact Analysis
**Identify the scope of impact before making modifications, and choose a strategy appropriate for that scope.**
- Always check the callers and references of the target before writing code.
- If a signature, return value, or core state is modified, first identify and explicitly state the expected scope of impact.
- **Few callers (≤ 10):** Check all callers and, if necessary, update them to match the changes.
- **Many callers (utils, common functions, etc.):** As a rule, maintain the existing signature and behavior, modifying only the internal implementation. If new functionality is needed, prioritize adding an overload. If modifying the signature is unavoidable and widespread changes are expected, **never proceed arbitrarily; report to the user and wait for confirmation before proceeding.**