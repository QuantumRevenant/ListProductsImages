using ListProductsImages.Core;
using QuantumKit.Tools.TextUtils;

namespace ListProductsImages.Pipeline
{
    internal class FileSelector
    {
        public IEnumerable<AdaptedFileInfo> Select(IEnumerable<AdaptedFileInfo> unselectedFiles, ProgramCriteria criteria)
        {
            var rejectForced = criteria.FilterCriteria.FolderRejectForced;
            var acceptForced = criteria.FilterCriteria.FolderAcceptForced;
            var rejectIfFound = criteria.FilterCriteria.FolderRejectIfFound;
            var acceptIfFound = criteria.FilterCriteria.FolderAcceptIfFound;
            var fileRejectRegex = criteria.FilterCriteria.FileRejectRegex;
            var fileAcceptRegex = criteria.FilterCriteria.FileAcceptRegex;
            bool caseSensitive = criteria.FilterCriteria.CaseSensitive;

            foreach (var file in unselectedFiles)
            {
                // Rechazo forzado
                if (rejectForced?.Any() == true &&
                    FindMatch(file.Folders, rejectForced, caseSensitive))
                    continue;

                // Aceptación forzada
                if (acceptForced?.Any() == true &&
                    FindMatch(file.Folders, acceptForced, caseSensitive))
                {
                    yield return file;
                    continue;
                }

                // Rechazo condicional
                if (rejectIfFound?.Any() == true &&
                    FindMatch(file.Folders, rejectIfFound, caseSensitive))
                    continue;

                // Aceptación condicional
                if (acceptIfFound?.Any() == true &&
                    !FindMatch(file.Folders, acceptIfFound, caseSensitive))
                    continue;

                // Rechazo por regex
                if (fileRejectRegex?.Any() == true &&
                    TextUtils.MatchAnyRegex(file.FileName, fileRejectRegex, caseSensitive))
                    continue;

                // Aceptación por regex
                if (fileAcceptRegex?.Any() == true &&
                    !TextUtils.MatchAnyRegex(file.FileName, fileAcceptRegex, caseSensitive))
                    continue;

                // Si pasó todos los filtros
                yield return file;
            }
        }

        private bool FindMatch(IEnumerable<string> searchedValues, IEnumerable<FolderMatch> matchValues, bool caseSensitive)
        {
            var cmp = caseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;

            foreach (var match in matchValues)
            {
                Func<string, string, bool> comparator = match.MatchType.Equals("Exact")
                    ? (a, b) => a.Equals(b, cmp)
                    : (a, b) => a.Contains(b, cmp);

                if (!match.Match.Contains('/'))
                {
                    if (searchedValues.Any(folder => comparator(folder, match.Match)))
                        return true;
                }
                else
                {
                    var submatches = match.Match.Split("/", StringSplitOptions.RemoveEmptyEntries);

                    if (submatches.All(sub =>
                        searchedValues.Any(folder => comparator(folder, sub))))
                        return true;
                }
            }

            return false;
        }
    }
}