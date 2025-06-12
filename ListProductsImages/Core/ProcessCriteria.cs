namespace ListProductsImages.Core
{
    internal class ProcessCriteria(string rootPath = "", string outputFile = "", bool inverseSearch = false)
    {
        public string RootPath { get; set; } = rootPath;
        public string OutputFile { get; set; } = outputFile;
        public bool InverseSearch { get; set; } = inverseSearch;
        
    }
}