# Godot Project

## Overview
This project is a game starter template for use with the **Godot Engine** with C# scripting. It leverages the power of **.NET 8** for high-performance game logic and **C# 12** for modern, expressive code.

This project is written in C# however you may use gdscript as well and the systems inp lace will not be affected or interfere.

## Key Features

### Technical Stack
*   **Engine**: Godot 4.5 (via `Godot.NET.Sdk` 4.5.0)
*   **Framework**: .NET 8.0
*   **Language**: C# 12.0

### Template Gameplay Features
*   Menu System:
    * Main Menu
    *  Pause / Quit Menu
    *  GamePlay Scene
    *  Settings Menu
         * Master Volume Slider
         * SFX Volume Slider
         * Music Volume Slider
         * Windowed Mode Toggle (only works in real build)
*  GameEvents singleton - for dispatching game events
*  Random NumberManager singleton 
*  Sound Effects and Music placeholders (8-bit BXSFR style / AI Music)
*  Localization Support for the menu system in 5 languages (English, Spanish, French, German, Japanese)
*  Achievement System
*  Toast style Notification system with cards that fade over time
*  Save Game System - serialize data to JSON in user directory

## Getting Started

### Prerequisites
1.  **Godot Engine 4.5**: Ensure you have the .NET version of Godot installed.
2.  **.NET 8.0 SDK**: Required to build the C# solution.

### Setup
1.  Clone the repository.
2.  Open the project folder in Godot Engine.
3.  Godot should automatically build the C# solution. If not, click **Build** in the top right corner of the Godot editor.
4.  Implement gameplay in the game_play.tscn scene.
5.  If you wish to add additional menus to the game see the Main.tscn scene and file for examples.

### Development
*   **IDE**: The solution is configured for JetBrains Rider. Open the `.sln` file to start coding.
*   **Building**: You can build the project directly from Rider or let Godot handle the compilation on play.

## Project Structure
*   `project.godot`: Main configuration file.
*   `*.cs`: C# scripts containing game logic.
*   `*.tscn`: Godot scene files.

## License
* MIT License