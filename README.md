# ListProductsImages 🖼️

**ListProductsImages** is a console application written in **C# (.NET 8.0)** that allows you to recursively explore a directory, filter image files based on customizable rules, and generate a `.txt` file with the results.

The system offers an interactive menu interface to set paths, filters, and output options in a simple and user-friendly way.

---

## Requirements 📦

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

---

## Installation ⚙️

Clone this repository:

```
git clone https://github.com/your-user/ListProductsImages.git
cd ListProductsImages
```

---

## Usage 🚀

Build the solution:

```
dotnet build ListProductsImages.sln
```

Run the main project:

```
dotnet run --project ListProductsImages/ListProductsImages.csproj
```

Once running, the app will let you:

- View and change the base directory
- Set include/exclude filters (folder names, regex)
- Choose output path and file name
- Export a `.txt` with the filtered results

> **Note**: Command-line argument support is planned for future releases.

---

## Key Features 🧩

- Recursive directory scanning  
- Flexible filtering (by folder, regex, file type)  
- Clean `.txt` export of filtered image codes  
- Interactive console interface with counters  
- Integrated with [QuantumKit](https://github.com/QuantumRevenant/QuantumKit) for utility functions

---

## Third-party dependencies 🔗

This project integrates the following external library:

- [QuantumKit](https://github.com/QuantumRevenant/QuantumKit) – Personal utility toolkit used for file handling, string manipulation, and console enhancements.

---

## License 📝

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for more information.

---

## Contact ✉️

[![X (Twitter)](https://img.shields.io/badge/X_(Twitter)%09--%40QuantumRevenant-%23000000.svg?logo=X&logoColor=white)](https://twitter.com/QuantumRevenant)  
[![GitHub](https://img.shields.io/badge/GitHub%09--%40QuantumRevenant-%23121011.svg?logo=github&logoColor=white)](https://github.com/QuantumRevenant)

---

## Contributing 🤝

See the [CONTRIBUTING](CONTRIBUTING.md) file for contribution guidelines.

---

## Authors 👥

- [QuantumRevenant](https://github.com/QuantumRevenant)

---

## Notice ⚠️

This project was originally written in Spanish or a Spanish-English hybrid. If adapting to another language or platform (e.g., C++, Java), apply the necessary modifications.

---

## Changelog 📘

See [CHANGELOG](CHANGELOG.md) for the list of updates.
