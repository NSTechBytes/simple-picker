# Simple Picker

A lightweight Windows Forms application for picking colors from your screen, with magnifier, color utilities, and global hotkey support.

## Features

- **Color Picker:** Select any color from your screen.
- **Magnifier:** Zoom in on screen areas for precise color selection.
- **Global Hotkey:** Quickly activate the picker from anywhere.
- **Color Utilities:** Convert and copy color values in various formats.
- **Settings:** Customize hotkeys, formats, and behavior.
- **Installer:** Easy setup with NSIS installer.

## Getting Started

### Prerequisites

- Windows 10/11
- [.NET 8.0 Runtime](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)

### Installation

1. Download the latest release.
2. Run the installer and follow the instructions.

### Usage

- Launch the app from the Start Menu or desktop shortcut.
- Use the global hotkey (configurable in settings) to activate the picker.
- Select a color and copy its value to the clipboard.

## Project Structure

- `FormsFunctionality/` — Core forms and utilities (ColorPicker, Magnifier, Settings, etc.)
- `Installer/` — NSIS installer scripts and resources.
- `resources/` — Application icons and images.
- `Program.cs` — Application entry point.

## Building

1. Clone the repository:
   ```sh
   git clone https://github.com/NSTechBytes/simple-picker.git
   ```
2. Open `simple-picker.sln` in Visual Studio.
3. Build the solution (`Release` or `Debug`).

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

## Credits

Developed by NSTechBytes.
