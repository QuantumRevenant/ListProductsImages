# 📄 ListProductsImages

Console application in C# (.NET 8.0) that allows you to explore a directory and its subdirectories, list all found files, filter them using defined conditions (regex, folder names, extensions, etc.), and generate a `.txt` file with the results.

The system uses interactive console menus to select the base path, define filters, and set output options.

## 📜 Table of Contents

- [Requirements](#-requirements)
- [Installation](#-installation)
- [Usage](#-usage)
- [Output Example](#-output-example)
- [Key Features](#-key-features)
- [License](#-license)
- [Contact](#-contact)
- [Contributors](#-contributors)
- [Dependencies](#-dependencies)

---

## 📦 Requirements

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

---

## ⚙️ Installation

Clone this repository:

```bash
git clone https://github.com/your-user/ListProductsImages.git
cd ListProductsImages
```

---

## 🚀 Usage

Build the solution:

```bash
dotnet build ListProductsImages.sln
```

Run the main project:

```bash
dotnet run --project ListProductsImages/ListProductsImages.csproj
```

The application will launch an interactive console interface where you can:

* View the current working directory.
* Change the base directory path.
* Define filtering rules (by folder name or file name regex).
* Select the output path and file name.
* Generate and save the filtered file list as a `.txt`.

> **Note**: Command-line argument support will be added soon.

---

## 🛠️ Output Example

### File System Structure

```bash
/Root/
├── /Approved Folder/
│   ├── Cod001.jpg
│   ├── Cod002 Description.png
│   ├── Cod003-Cod004.jpg
│   ├── Cod005-Cod006 Description.webp
│   ├── /Dissaproved Folder/
│   │   ├── Cod007.jpg
│   │   ├── Cod008 Description.png
│   │   ├── Cod009-Cod010.jpg
│   │   └── Cod011-Cod012 Description.webp
│   └── /Neutral SubFolder/
│       ├── Cod013.jpg
│       ├── Cod014 Description.png
│       ├── Cod015-Cod016.jpg
│       └── Cod017-Cod018 Description.webp
├── /Disapproved Folder/
│   ├── Cod019.jpg
│   ├── Cod020 Description.png
│   ├── Cod021-Cod022.jpg
│   └── Cod023-Cod024 Description.webp
└── /Neutral Folder/
    ├── Cod025.jpg
    ├── Cod026 Description.png
    ├── Cod027-Cod028.jpg
    └── Cod029-Cod030 Description.webp
```

### Console Filtering Rules

```
Reject: "Disapproved" | Match Mode: Contains  
Accept: "Approved"     | Match Mode: Exact
```

### Output File

```bash
Cod001
Cod002
Cod003
Cod004
Cod005
Cod006
Cod013
Cod014
Cod015
Cod016
Cod017
Cod018
```

---

## 📁 Key Features

* Recursive directory exploration.
* Customizable filters (regex for filenames, folder exclusion, etc.).
* Export results to a `.txt` file.
* Interactive console menu for manual configuration (no CLI args required yet).

---

## 📝 License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for more information.

---

## ✉️ Contact

If you have any questions or feedback, feel free to reach out:

[![X (Formerly Twitter)](https://img.shields.io/badge/X%20(formerly%20Twitter)-%23000000.svg?logo=X&logoColor=white)](https://twitter.com/QuantumRevenant)  
[![GitHub](https://img.shields.io/badge/GitHub-%23121011.svg?logo=github&logoColor=white)](https://github.com/YourGitHubUsername)

---

## 🤝 Contributing

See [CONTRIBUTING](CONTRIBUTING.md) for contribution guidelines.

---

## 👥 Contributors

- [QuantumRevenant](https://github.com/QuantumRevenant)

---

## ⚠️ Notice

This codebase and documentation are primarily written in Spanish or a mix of Spanish and English. If you intend to adapt this project to a different programming language (e.g., C++ or Java), make the necessary modifications accordingly.

