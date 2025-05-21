namespace ListProductsImages.Core
{
    public class FolderMatch
    {
        public string Match { get; set; } = "";

        // Campo original opcional
        public string Type { get; set; } = "";

        // Propiedad con valor por defecto
        public string MatchType => string.IsNullOrWhiteSpace(Type) ? "Contains" : Type;
    }
}