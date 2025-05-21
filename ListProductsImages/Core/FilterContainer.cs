namespace ListProductsImages.Core
{
    public class FilterContainer
    {
        public List<FilterCriteria> Filters { get; set; }
        public FilterContainer()
        {
            Filters = [];
        }
        
    }
}