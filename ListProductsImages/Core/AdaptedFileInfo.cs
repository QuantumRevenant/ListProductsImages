namespace ListProductsImages.Core
{
    internal class AdaptedFileInfo
    {
        public IEnumerable<string> Folders { get; private set; } // Todos los Folders a Evaluar
        public string CompletePath { get; private set; } 
        public string ResumedPath { get; private set; }
        public string FileName { get; private set; }
        public string FileNameWithoutExtension { get; private set; }
        public string WritingValue { get; private set; } //Valor final a imprimir (no para el filtro)

        public AdaptedFileInfo(string completePath, string basePath = "")
        {
            CompletePath = completePath;
            FileName = Path.GetFileName(completePath);
            FileNameWithoutExtension = Path.GetFileNameWithoutExtension(completePath);

            if (!string.IsNullOrEmpty(basePath) && completePath.StartsWith(basePath))
                ResumedPath = $"~{completePath[basePath.Length..]}";
            else
                ResumedPath = CompletePath;

            Folders = ResumedPath
                        .TrimStart('~')
                        .Split(Path.DirectorySeparatorChar, StringSplitOptions.RemoveEmptyEntries)
                        .TakeWhile(part => part != FileName);

            var firstToken = FileNameWithoutExtension.Split(' ').FirstOrDefault() ?? "";

            WritingValue = firstToken.Contains('-')
                ? string.Join("\n", firstToken.Split('-'))
                : firstToken;
        }
    }
}