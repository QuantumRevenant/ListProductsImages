using QuantumKit.Tools;
using QuantumKit.Tools.IO;

namespace ListProductsImages.Core
{
    internal class AppSettings
    {
        private static readonly string AppName = "ListProductsImages";
        private static readonly string settingsFileName = "settings.json";
        private static readonly string AppDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        public static readonly string AppFamilyFolder = Path.Combine(AppDataFolder, AppFolders.CompanyName);
        public static readonly string AppSpecificFolder = Path.Combine(AppFamilyFolder, AppName);
        public static readonly string settingsFilePath = Path.Combine(AppSpecificFolder, settingsFileName);
        // Rutas para la fuente de la lista de archivos
        public bool UseExecutionPathAsListSource;
        public string CustomListSourcePath { get; set; } // Se usa si UseExecutionPathAsListSource es false

        // Rutas para el directorio de salida del archivo de resultados
        public bool UseExecutionPathAsOutputDirectory;
        public string CustomOutputDirectory { get; set; } // Se usa si UseExecutionPathAsOutputDirectory es false

        public string OutputFileName { get; set; }

        public FilterContainer CustomFiltersContainer { get; set; } = new();

        public string LastAnalyzedPath { get; set; }


        public AppSettings()
        {
            UseExecutionPathAsListSource = true;
            CustomListSourcePath = string.Empty;

            UseExecutionPathAsOutputDirectory = true;
            CustomOutputDirectory = string.Empty;

            OutputFileName = "Ω Lista de archivos.txt";

            LastAnalyzedPath = string.Empty;
        }
    }
}