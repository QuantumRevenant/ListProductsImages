namespace ListProductsImages.Pipeline
{
    class FileDataSource
    {
        public IEnumerable<string> GetFilePaths(string path)
        {
            List<string> rawFilePaths = [.. Directory.GetFiles(path,"*",SearchOption.AllDirectories)];

            return rawFilePaths;
        }
    }
}
