namespace ListProductsImages.Core
{
    public class FilterCriteria
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public IEnumerable<FolderMatch> FolderRejectForced { get; set; }
        public IEnumerable<FolderMatch> FolderAcceptForced { get; set; }
        public IEnumerable<FolderMatch> FolderRejectIfFound { get; set; }
        public IEnumerable<FolderMatch> FolderAcceptIfFound { get; set; }
        public IEnumerable<string> FileRejectRegex { get; set; }
        public IEnumerable<string> FileAcceptRegex { get; set; }
        public bool CaseSensitive { get; set; }
        public FilterCriteria()
        {
            Name = Description = "";
            FolderRejectForced = FolderAcceptForced = FolderRejectIfFound = FolderAcceptIfFound = [];
            FileRejectRegex = FileAcceptRegex = [];
        }
    }
}