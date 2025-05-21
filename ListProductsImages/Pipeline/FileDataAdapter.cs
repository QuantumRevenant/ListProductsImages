using ListProductsImages.Core;

namespace ListProductsImages.Pipeline
{
    class FileDataAdapter
    {
        public IEnumerable<AdaptedFileInfo> Adapt(IEnumerable<string> rawFilePaths, string basePath)
        {
            List<AdaptedFileInfo> adaptedFiles = [];
            foreach (string filePath in rawFilePaths)
            {
                AdaptedFileInfo adaptedFile = new(filePath, basePath);
                adaptedFiles.Add(adaptedFile);
            }
            return adaptedFiles;
        }
    }
}