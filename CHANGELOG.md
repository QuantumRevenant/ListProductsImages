# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/)
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.1.1] - 2025-06-12
### Fixed
- 🐛 Fixed crash on startup caused by failure to load embedded resource (`DefaultFilterPresets.json`) from the main executable.
- Now passes the correct `Assembly` instance to `QuantumKit.Tools.IO.JsonFromEmbeddedResource`.

## [1.1.0] - 2025-06-12
### Added
- Countdown timer in the interactive interface to enhance user feedback during long operations.
- Integration of the `QuantumKit` submodule as a replacement for the previous `Utils` folder.

### Changed
- Project structure refactored for compatibility with **Visual Studio 2022**.
- Minor internal refactoring to accommodate new modular layout.

## [1.0.0] - 2025-05-21
### Added
- Functional initial version of the project.
- Modular structure with `Core`, `Pipeline`, and `Utils`.
- Interactive console menu.
- Default filter configuration file (`DefaultFilterPresets.json`).
