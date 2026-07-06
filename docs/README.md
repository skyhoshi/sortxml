# Project Docs

This folder contains focused sortxml docs. Files directly under `docs/` are source of truth for their topic; rough notes and historical chats live under `scratch/`.

## Focused Docs

- [cli-behavior.md](cli-behavior.md): command-line behavior, XML sorting, formatting, and output modes.
- [test-fixtures.md](test-fixtures.md): XML fixture test layout, expected file policy, and regeneration workflow.

## Scratch

- `scratch/`: rough notes, temporary work files, and historical AI/user chats. These files are kept for reference only and are not source of truth.

## Policy

Keep current decisions in focused docs or root docs. Do not append long assistant transcripts to focused docs; put raw discussion in `scratch/`, summarize durable decisions in the relevant focused doc, and move implementation work to `../TODO.md`.

Root docs are mapped from [../README.md](../README.md).