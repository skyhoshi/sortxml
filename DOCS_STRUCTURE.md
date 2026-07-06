# Documentation Structure Guide

Use this guide to organize sortxml's Markdown files into a clear, durable documentation system. It is written so an AI assistant can apply it to this repository without scattering current decisions across scratch notes, README sections, and task lists.

## Goal

Create a documentation layout where each file has one job:

- Root docs are high-visibility project control files.
- `docs/` contains focused current source-of-truth topic docs.
- `docs/scratch/` contains rough notes, temporary work files, and historical chats that are not source of truth.
- Backlog items live in `TODO.md`, not buried in prose.
- AI/code-agent instructions live in `AGENTS.md`.
- Formatting conventions live in `CODEFORMAT.md`.

When restructuring this project, preserve useful information, remove stale duplication, update links, and keep historical discussion available only when it has future value.

## Naming Conventions

Use uppercase root-level Markdown files for important project-wide documents:

```text
README.md
AGENTS.md
CODEFORMAT.md
DEVELOPMENT.md
DESIGN.md
TODO.md
DOCS_STRUCTURE.md
```

Use lower kebab-case for topic files inside folders:

```text
docs/cli-behavior.md
docs/xml-formatting.md
docs/sorting-rules.md
docs/test-fixtures.md
docs/release-packaging.md
docs/argument-binding.md
docs/scratch/test-failure-notes.md
```

Rationale:

- Uppercase root files stand out as repo-level control panels.
- Lowercase kebab-case folder contents are easier to scan when there are many files.
- `README.md` is a long-standing convention and is rendered automatically by common Git hosts.
- `scratch/` is intentionally plain: it signals rough, reference-only material without creating a second documentation system.

## Recommended Layout

```text
README.md
AGENTS.md
CODEFORMAT.md
DEVELOPMENT.md
DESIGN.md
TODO.md
DOCS_STRUCTURE.md

Optional root docs, when useful:
SECURITY.md
CONTRIBUTING.md
CHANGELOG.md
ARCHITECTURE.md
TESTING.md
DEPLOYMENT.md

docs/
  README.md
  cli-behavior.md
  test-fixtures.md
  <focused-topic>.md
  scratch/
    README.md
    <scratch-topic>.md
```

Only add optional root files when the content is large or important enough to deserve a top-level entry.

## File Responsibilities

### `README.md`

The project front door.

Include:

- What sortxml is.
- High-level repo layout.
- Main capabilities.
- Quick start or a link to setup instructions.
- A `Core Docs` section linking to repo-level docs people should find first.
- A link to `docs/README.md`, not a duplicated list of every focused doc.

Avoid:

- Long raw notes.
- Long implementation history.
- Detailed local setup if `DEVELOPMENT.md` exists.
- Detailed topic writeups that belong in `docs/<topic>.md`.
- Large speculative plans that belong in `docs/scratch/` until they become stable.

### `docs/README.md`

The index for focused project docs.

Include:

- A short explanation of what belongs in `docs/`.
- Links to every focused doc directly under `docs/`.
- A short `Scratch` section explaining `docs/scratch/`.
- A reminder that root docs are mapped from the root `README.md`.
- A reminder to summarize durable decisions in curated docs and move action items to `TODO.md`.

Avoid:

- Duplicating the full root docs map.
- Raw transcripts.
- Current task lists that belong in `TODO.md`.

### `DESIGN.md`

Durable cross-system design decisions.

Include:

- CLI behavior principles.
- XML sorting and formatting rules that cut across options.
- Argument-binding rules that affect all CLI options.
- Test fixture and compatibility decisions future implementation should respect.

Avoid:

- Local run commands.
- Raw brainstorming.
- One-off implementation tasks.

### `DEVELOPMENT.md`

How to work on, run, test, configure, and publish the project.

Include:

- Local setup.
- Required tools.
- Build commands.
- Test commands.
- Fixture regeneration instructions.
- Publish/release commands.
- Local troubleshooting.

Avoid:

- Product brainstorming.
- Feature specs, unless they are directly tied to development workflow.

### `TODO.md`

The actionable backlog.

Include:

- Concrete tasks.
- Checklists.
- Grouped implementation work.
- Follow-ups discovered during development.

Use checkboxes:

```markdown
- [ ] Fix default option presence tracking in the argument binder.
- [x] Add a test project reference to the CLI project.
```

Avoid:

- Long explanations that should be summarized in `docs/`.
- Raw conversation.
- Duplicate copies of feature specs.

When a task needs context, link to the relevant doc:

```markdown
- [ ] Decide whether expected XML fixtures should preserve source indentation. See `docs/test-fixtures.md`.
```

### `AGENTS.md`

Instructions for AI coding agents working in the repo.

Include:

- Code style rules that agents must follow.
- Editing constraints.
- Testing expectations.
- Framework-specific conventions.
- Project-specific pitfalls.

Keep this file operational and concise. Put broader design reasoning in `DESIGN.md` or `docs/`.

### `CODEFORMAT.md`

Formatting and style conventions.

Include:

- Language formatting rules.
- Naming conventions.
- Linting/formatting commands.
- Generated code rules.

If a rule is critical for AI agents, summarize or link it from `AGENTS.md`.

### `docs/<focused-topic>.md`

Curated source-of-truth docs for specific systems or plans.

Examples:

```text
docs/cli-behavior.md
docs/xml-formatting.md
docs/sorting-rules.md
docs/test-fixtures.md
docs/release-packaging.md
docs/argument-binding.md
```

Include:

- Current decisions.
- XML behavior notes.
- CLI/API behavior.
- Implementation constraints.
- Links to related tasks in `TODO.md` when useful.

Avoid:

- Full chat transcripts.
- Repeated copies of backlog checklists.
- Stale alternatives unless they explain an important decision.

### `docs/scratch/`

Reference-only rough material.

Use for:

- Early behavior brainstorming.
- Temporary research.
- Work-in-progress notes.
- Long AI/user conversations.
- Raw notes that explain why a decision emerged but are not current source of truth.

Scratch files should not be treated as authoritative unless verified against current docs and code. When scratch material becomes stable, promote or summarize it into `docs/<topic>.md`, `DESIGN.md`, `DEVELOPMENT.md`, or `TODO.md`, then delete the scratch file when it no longer has reference value.

Historical chat files should start with a note like:

```markdown
# XML Formatting Discussion Archive

This is historical discussion. It is not the current source of truth.
See `../cli-behavior.md` for the current plan.
```

## Working Notes Policy

Prefer focused docs over long-lived scratch files.

When starting new work:

- Create or update `docs/<topic>.md` if the material is intended to become the current plan.
- Use `docs/scratch/<topic>.md` only for messy exploration, raw transcripts, copied context, or notes that may be deleted later.
- Move actionable work into `TODO.md` as soon as it is clear.
- Move durable cross-system decisions into `DESIGN.md`.
- Move run/setup/publish details into `DEVELOPMENT.md`.
- Keep scratch files labeled as reference-only.

## Restructuring Process For An AI Assistant

When asked to apply this structure to sortxml:

1. Inventory Markdown files.
   - List all root `.md` files.
   - List all `docs/**/*.md` files.
   - Identify files with stale names, duplicate content, copied content from another project, or old scratch folders.

2. Classify each file.
   - Project overview.
   - Development/setup.
   - Durable design decision.
   - Focused topic/spec.
   - Backlog/tasks.
   - Agent instructions.
   - Formatting conventions.
   - Scratch note.
   - Historical chat.

3. Propose moves before editing if changes are broad.
   - Keep root docs uppercase.
   - Move stable focused plans into `docs/`.
   - Move raw discussions and rough notes into `docs/scratch/`.
   - Split long root README topic sections into `docs/<topic>.md` files.

4. Preserve useful content.
   - Summarize durable decisions into curated docs.
   - Do not delete historical context if it may be useful; move it to `docs/scratch/`.
   - Remove or clearly label stale content.

5. Update links.
   - Root `README.md` should include a `Core Docs` section for root docs and link to `docs/README.md`.
   - `docs/README.md` should link to every focused doc under `docs/`.
   - `docs/README.md` should explain `docs/scratch/` without listing every temporary file unless useful.
   - `TODO.md` tasks should link to relevant focused docs when useful.
   - Remove links to moved or deleted paths.

6. De-duplicate.
   - Do not keep the same plan in `README.md`, root docs, and `docs/`.
   - Keep one source of truth and link to it.
   - Replace duplicated prose with a short pointer.

7. Leave a clear final summary.
   - Files created.
   - Files moved.
   - Files updated.
   - Any docs intentionally left as scratch.
   - Any links or stale references that still need review.

## Source-Of-Truth Rules

Use these rules when deciding where content belongs:

```text
Is it the first thing a newcomer should read?
  -> README.md

Is it about running, building, testing, publishing, or configuring?
  -> DEVELOPMENT.md

Is it a durable cross-system decision?
  -> DESIGN.md

Is it a focused current plan/spec for one system?
  -> docs/<topic>.md

Is it an actionable task?
  -> TODO.md

Is it an instruction for AI/code agents?
  -> AGENTS.md

Is it a formatting/style rule?
  -> CODEFORMAT.md, with critical agent-facing rules summarized in AGENTS.md

Is it temporary, raw, messy, or historical reference material?
  -> docs/scratch/<topic>.md
```

## Link Hygiene

After moving docs:

- Search for old filenames and old paths.
- Update all Markdown links.
- Update references in `README.md`, `docs/README.md`, `TODO.md`, and focused docs.
- Prefer relative links in Markdown.
- Use exact filenames and casing.

Common checks:

```powershell
rg -n "old-file-name|Old Heading|old-folder|docs/old-topic" -g "*.md"
```

## Scratch Policy

Move rough material instead of deleting when:

- The content records useful reasoning.
- The content is a long AI/user conversation.
- The content may help explain future decisions.

Delete or replace when:

- The content is copied from another project and irrelevant.
- The content is duplicated exactly elsewhere.
- The content is misleading and has no historical value.
- The durable parts have already been promoted and the scratch copy no longer helps.

## Root README Template

```markdown
# sortxml

Simple utility that sorts and prettifies XML files.

## Project Layout

- `Program.cs`: CLI entry point and XML sorting/writing flow.
- `AppOptions.cs`: command-line options.
- `PowerCode/`: embedded CLI helper utilities.
- `SortXML.Tests/`: xUnit tests and XML fixtures.
- `Properties/PublishProfiles/`: publish profiles.
- `docs/`: focused documentation.

## Core Docs

- `docs/README.md`: focused documentation index.
- `DEVELOPMENT.md`: local setup, build, test, publish.
- `DESIGN.md`: durable CLI/XML behavior decisions.
- `TODO.md`: actionable backlog.
- `AGENTS.md`: AI/code-agent instructions.
- `CODEFORMAT.md`: formatting and style conventions.
- `DOCS_STRUCTURE.md`: reusable docs organization guide.

## Development

See `DEVELOPMENT.md`.
```

## `docs/README.md` Template

```markdown
# Project Docs

This folder contains focused project docs. Files directly under `docs/` are source of truth for their topic; rough notes and historical chats live under `scratch/`.

## Focused Docs

- `cli-behavior.md`: command-line behavior, XML sorting, formatting, and output modes.
- `test-fixtures.md`: XML fixture test layout, expected file policy, and regeneration workflow.

## Scratch

- `scratch/`: rough notes, temporary work files, and historical AI/user chats. These files are kept for reference only and are not source of truth.

## Policy

Keep current decisions in focused docs or root docs. Do not append long assistant transcripts to focused docs; put raw discussion in `scratch/`, summarize durable decisions in the relevant focused doc, and move implementation work to `../TODO.md`.
```

## Quality Bar

A good documentation restructure should leave sortxml in this state:

- A newcomer can start at `README.md`.
- A maintainer can find focused docs through `docs/README.md`.
- An AI assistant can find repo instructions in `AGENTS.md`.
- Formatting rules are easy to find in `CODEFORMAT.md`.
- Setup and publishing are in `DEVELOPMENT.md`.
- Durable decisions are in `DESIGN.md` or focused `docs/`.
- Tasks are in `TODO.md`.
- Raw or temporary material is under `docs/scratch/`.
- No important current plan exists only in `docs/scratch/`.
- No stale copied docs from other projects remain in curated docs.