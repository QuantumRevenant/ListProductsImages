namespace ListProductsImages.Core
{
    internal class FilterContainer
    {
        public List<FilterCriteria> Filters { get; set; }
        public FilterContainer()
        {
            Filters = [];
        }
        
    }
}