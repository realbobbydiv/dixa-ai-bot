# Dixa AI Bot

Browser automation tool built with .NET and Playwright.

The goal of this project is to automate parts of the customer support workflow inside Dixa, while still keeping a human in control.

## What it does

* Logs into Dixa using Google authentication
* Navigates through conversations in the inbox
* Reads messages before claiming them
* Classifies conversations
* Drafts replies (does NOT send automatically)
* Moves to the next conversation

## Why I built it

I wanted to explore how automation can assist support agents instead of replacing them.

The idea is:

* reduce repetitive work
* speed up response time
* keep final decisions human

## Features

* Browser automation using Playwright
* CLI commands for running different actions
* Modular structure (Core / Infrastructure / CLI)
* Draft-only mode (no auto-send for safety)

## Tech used

* C# (.NET)
* Playwright
* CLI tools
* Google login via saved session

## How it works (high level)

1. The bot opens a browser session
2. Logs into Dixa (Google account)
3. Navigates to the inbox
4. Reads conversations
5. Generates a draft reply
6. Leaves the final decision to the user

## Project structure

* `DixaBot.Core` - core logic and models
* `DixaBot.Infrastructure` - Playwright automation
* `DixaBot.Cli` - CLI interface
* `DixaBot.Tests` - test project

## Usage

```bash
dotnet run -- auth
dotnet run -- run
```

*(commands may vary depending on setup)*

## Notes

* This project is for educational and automation purposes
* It does not interact with any official API
* Requires active browser session

## Future improvements

* Better classification logic
* Smarter reply drafting
* Improved selectors and stability
* Full workflow automation (optional)
