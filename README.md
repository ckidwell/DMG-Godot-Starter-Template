# Godot Project

## Overview
This project is a game starter template for use with the **Godot Engine** with C# scripting. It leverages the power of **.NET 8** for high-performance game logic and **C# 12** for modern, expressive code.

This project is written in C# however you may use gdscript as well and the systems in place will not be affected or interfere.

## AI Usage

Some sample content in this project was generated using AI - you may replace that content and be AI generation free. I do use 'code assist' during programming but will not use 'generative' AI for this project. At the initial time of this writing only the sample music is AI generated.  Any other content added will be marked as AI but not always noted in this disclaimer.

## Key Features

### Technical Stack
*   **Engine**: Godot 4.7 (.NET build)
*   **Framework**: .NET 8.0 (.NET 9.0 when exporting for Android)
*   **Language**: C# 12.0

### Template Gameplay Features
*   Menu System autoload
    * Main Menu
    * Pause / Quit Menu (Escape, gameplay only)
    * GamePlay Scene
    * Settings Menu
         * Master Volume Slider
         * SFX Volume Slider
         * Music Volume Slider
         * Language selector
         * Windowed Mode Toggle (only works in a real build, not the editor)
    * Achievements Menu
*  GameEvents singleton - for dispatching game events
*  RandomNumberManager singleton - with an optional deterministic seed for reproducible runs
*  Sound Effects (8-bit BFXR style) - polyphonic, so overlapping effects mix instead of cutting each other off
*  Music placeholders (AI Music)
*  Localization Support for the menu system and achievement text in 6 languages (English, Spanish, French, German, Italian, Japanese). The saved language is applied on startup, and changing it in Settings updates every label live
*  Achievement System
*  Toast style Notification system with cards that fade over time
    * **Demo:** pressing **Play** on the main menu fires the "Welcome" achievement toast every time, so you can see what an earned achievement and its notification look like. This is intentional demo behavior — it bypasses the normal one-time unlock in `ProgressionManager.AchievementUnlocked()`. Replace it with your own unlock logic (see `MainMenu.OnPlayButtonPressed`).
*  Save Game System - serialize data to JSON in the user directory (implemented in the `ProgressionManager` singleton). Writes are atomic and debounced, and a save that cannot be read is kept as `.corrupt` next to the new one instead of being discarded
*  Object pooling (`PoolSpawner`) for bullets, enemies and other frequently spawned scenes
*  Camera shake component (`GameCameraShake`) - opt-in, attach to your gameplay Camera2D

## Getting Started

### Prerequisites
1.  **Godot Engine 4.7**: Ensure you have the .NET version of Godot installed.
2.  **.NET 8.0 SDK**: Required to build the C# solution.

### Setup
1.  Clone the repository.
2.  Open the project folder in Godot Engine.
3.  Godot should automatically build the C# solution. If not, click **Build** in the top right corner of the Godot editor.
4.  Implement gameplay in the game_play.tscn scene.
5.  If you wish to add additional menus to the game see the Main.tscn scene and file for examples.

## Conventions 

*   **Button click sounds** come from the button, not the menu. Attach `SoundTextureButton`, `SoundButton` or `SoundCheckButton` to a button.
*   **Adding an achievement** is three steps: add the value to the `Achievements` enum, add a case in `AchievementDescription`, and add its `_TITLE_`, `_DESC_` and `_EARNED_` rows to the localization CSV. Existing saves pick the new achievement up automatically.
*   **Save file enums are stored by name.** You can reorder `Achievements` and `SupportedLanguages`, but never rename or remove a value once a build has shipped.
*   **Adding a language**: add a column to the localization CSV (header is the Godot locale code, e.g. `pt`), add the value to `SupportedLanguages`, and map it in `SupportedLanguagesExtensions`.
*   **Randomness** should come from `RandomNumberManager`, not `GD.RandRange`, so a deterministic seed makes the whole game reproducible.
*   **Cheats** (`CheatCodeManager`) are InputMap actions and only work in debug builds.

## Project Structure
*   `project.godot`: Main configuration file.
*   `Autoload/`: singletons (events, menus, progression/save, RNG, toasts, music).
*   `Code/`: plain C# (save DTOs, pooling, localization, constants).
*   `Scenes/`: scenes and their scripts (UI, audio, achievements, camera, managers).
*   `Localization/`: the translation CSV and the `.translation` files Godot imports from it.

## License
* MIT License
