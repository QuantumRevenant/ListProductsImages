using ListProductsImages.Core;

namespace ListProductsImages.Pipeline
{
    class ProductImageProcessor
    {
        public void Pipeline(ProgramCriteria criteria)
        {
            //Program classes
            var dataSource = new FileDataSource();
            var adapter = new FileDataAdapter();
            var selector = new FileSelector();
            var sink = new FileDataSink();

            IEnumerable<string> rawFilePaths = dataSource.GetFilePaths(criteria.ProcessCriteria.RootPath);              //Recolectar datos
            IEnumerable<AdaptedFileInfo> adaptedFiles = adapter.Adapt(rawFilePaths, criteria.ProcessCriteria.RootPath); //Adaptar datos
            IEnumerable<AdaptedFileInfo> selectedFiles = selector.Select(adaptedFiles, criteria);       //Seleccionar datos
            sink.WriteResults(selectedFiles, criteria.ProcessCriteria.OutputFile);                                       //Imprimir datos
        }
    }
}
