namespace ListProductsImages.Core
{
    class ProgramCriteria
    {
        public FilterCriteria FilterCriteria { get; set; }
        public ProcessCriteria ProcessCriteria { get; set; }

        public ProgramCriteria(FilterCriteria filterCriteria, ProcessCriteria processCriteria)
        {
            FilterCriteria = filterCriteria;
            ProcessCriteria = processCriteria;
        }
    }
}