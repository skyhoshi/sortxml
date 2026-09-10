# Code Format

Portable formatting and code-generation rules for projects. The authoritative machine-readable style for a specific repository lives in `.editorconfig` and related local tooling; when this file and local tooling disagree, follow the local tooling and existing nearby code.

## First Principles

- Check the current worktree before editing files.
- Read nearby code before changing style-sensitive files.
- Preserve unrelated user work and avoid broad formatting churn.
- Prefer focused edits that match existing project patterns.
- Use generated code or framework conventions only when they are already part of the project.

## Project-Wide Defaults

Use these defaults unless `.editorconfig`, a formatter, or existing files in the repository clearly say otherwise:

- Indent with 2 spaces.
- Always use braces on control blocks; never use single-line control bodies.
- Keep brace placement consistent with the repository.
- Private fields use `_camelCase`.
- Const fields use `PascalCase`, unless the local project uses another convention.
- Methods, properties, types, and public members use `PascalCase`.
- Local variables and parameters use `camelCase`.
- Avoid hardcoding values that belong in configuration, data files, or named constants.

## C# Style

- Target modern C# when the project supports it.
- Keep nullable annotations enabled when the project enables them; do not weaken null-safety to silence warnings.
- Use `var` only when the type is obvious from the right-hand side.
- Prefer string interpolation and `nameof()`.
- Prefer pattern matching where it improves clarity.
- Prefer null-coalescing and null-conditional operators where they improve clarity.
- Use async/await for asynchronous code.
- Use namespaces that match folder structure.
- Prefer file-scoped namespaces when the project uses them.
- Order `using` directives according to local rules; if no local rule exists, put `System.*` first, then third-party namespaces, then project namespaces.
- Keep method-call spacing consistent with the project. If no local rule exists, use normal C# spacing: `method(arg1, arg2)`.

## Code Generation Best Practices

- Follow patterns in existing code before introducing new abstractions.
- Put new code in the appropriate project, module, namespace, and folder.
- Prefer small, composable types and methods over large procedural blocks.
- Add comments only when they explain non-obvious intent, constraints, or tradeoffs.
- Do not add dependencies, frameworks, or architectural layers unless they are justified by the task.
- Update relevant docs, examples, schemas, fixtures, or config when behavior changes.
- Test generated code with the repository's normal build, test, lint, or formatting workflow when practical.

## Markdown And Docs Style

- Root project docs use uppercase filenames, such as `README.md`, `DEVELOPMENT.md`, `DESIGN.md`, `TODO.md`, `AGENTS.md`, and `CODEFORMAT.md`.
- Focused docs under `docs/` use lower kebab-case filenames.
- Prefer relative Markdown links for repository files.
- Keep durable decisions in root docs or focused docs under `docs/`.
- Keep raw notes, temporary work files, and historical chats under `docs/scratch/`.
- Do not duplicate the same plan in multiple docs; keep one source of truth and link to it.

## Tests, Fixtures, And Generated Output

- Expected fixtures should represent intended behavior, not incidental output from a temporary bug.
- Regenerate expected output only after deciding and documenting the behavior being blessed.
- Preserve fixture readability and review fixture diffs carefully.
- Avoid committing generated churn that is unrelated to the task.

## Formatting Commands

Use the repository's documented formatting command. Common examples:

```powershell
dotnet format
```

```powershell
npm run format
```

If a formatter changes many unrelated files, stop and inspect the diff before continuing.

## Commit Message Guidelines

When committing changes to the repository, follow these guidelines:

- Use the present tense ("Add feature" not "Added feature").
- Use the imperative mood ("Move cursor to..." not "Moves cursor to...").
- Limit the first line to 72 characters or less.
- Separate subject from body with a blank line when a body is needed.
- Use the body to explain what and why, not just how.
- Reference issues and pull requests when relevant.
- Use bullet points and lists when they make the message easier to scan.
- Be concise but descriptive.
- Avoid using "I" or "we" in commit messages.
- Proofread messages for clarity and typos.
