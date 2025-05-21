using ListProductsImages.Core;

namespace ListProductsImages.Pipeline
{
    class FileDataSink
    {
        public void WriteResults(IEnumerable<AdaptedFileInfo> files, string outputFile)
        {
            if (File.Exists(outputFile)) File.Delete(outputFile);
                File.AppendAllText(outputFile, "Resultados: ↓\n");
            foreach (AdaptedFileInfo file in files)
                File.AppendAllText(outputFile, file.WritingValue + "\n");
        }
    }
}