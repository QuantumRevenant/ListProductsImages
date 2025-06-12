using ListProductsImages.Core;
using QuantumRevenant.Utilities;

namespace ListProductsImages.Pipeline
{
    class ProductImageProcessor
    {
        public void Pipeline(ProgramCriteria criteria)
        {
            var totalTimer = new QuantumTools.StopwatchLogger("Tiempo total");

            //Program classes
            var dataSource = new FileDataSource();
            var adapter = new FileDataAdapter();
            var selector = new FileSelector();
            var sink = new FileDataSink();

            IEnumerable<string>? rawFilePaths = null;
            IEnumerable<AdaptedFileInfo>? adaptedFiles = null;
            IEnumerable<AdaptedFileInfo>? selectedFiles = null;

            QuantumTools.RunWithTimer(() =>
            {
                rawFilePaths = dataSource.GetFilePaths(criteria.ProcessCriteria.RootPath);
            }, "Recolectando datos... ", showDecimals: true);

            QuantumTools.RunWithTimer(() =>
            {
                adaptedFiles = adapter.Adapt(rawFilePaths!, criteria.ProcessCriteria.RootPath);
            }, "Adaptando datos... ", showDecimals: true);

            QuantumTools.RunWithTimer(() =>
            {
                selectedFiles = selector.Select(adaptedFiles!, criteria);
            }, "Seleccionando datos... ", showDecimals: true);

            QuantumTools.RunWithTimer(() =>
            {
                sink.WriteResults(selectedFiles!, criteria.ProcessCriteria.OutputFile);
            }, "Escribiendo datos... ", showDecimals: true);

            totalTimer.StopAndReport(showDecimals: true);
        }
    }
}
