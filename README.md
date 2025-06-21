# Emoji Memory

A simple memory matching game built with Blazor WebAssembly. The project is intended as a playful demonstration of .NET's UI capabilities while following the ideas laid out in [Emoji_Memory_Game_Design.md](Emoji_Memory_Game_Design.md).

## Getting Started

1. Ensure the .NET SDK is installed.
2. Navigate to the UI project:
   ```bash
   cd UI/EmojiMemory.UI
   ```
3. Run the application:
   ```bash
   dotnet run
   ```
4. Open the printed local address in your browser to play.

For further design notes and game flow details, see [Emoji_Memory_Game_Design.md](Emoji_Memory_Game_Design.md).

## Architecture

The solution is organized using a lightweight layered approach:

* **UI** – A Blazor WebAssembly project found under `UI/EmojiMemory.UI` which contains pages and Razor components.
* **Application** – Services, repositories and contracts located in `UI/EmojiMemory.UI.Application`.
* **Domain** – Entities, value objects and enums in `UI/EmojiMemory.UI.Domain`.
* **Infrastructure** – Implementations for storage and sound playback under `EmojiMemory.UI.Infrastructure`.

Unit tests for the game engine are located in `Test/EmojiMemory.Tests.UnitTests`.

## Generated with ChatGPT Codex

All of the code, except some cleanup, was produced using ChatGPT Codex.

## Sound Effects

The sound effects included in `wwwroot/sounds` are taken from [Mixkit](https://mixkit.co/free-sound-effects/).
