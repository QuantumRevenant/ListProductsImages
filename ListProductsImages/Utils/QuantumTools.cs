using System.Reflection;
using System.Text.RegularExpressions;

namespace QuantumRevenant.Utilities
{
    public static class QuantumTools
    {
        public static string QuantumRevenantFolderPath = "QuantumRevenant";

        #region JsonMethods
        public static string JsonFromEmbeddedResource(string embeddedPath)
        {
            using Stream? stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(embeddedPath);
            if (stream == null)
                throw new InvalidOperationException($"No se encontró el recurso embebido: {embeddedPath}");

            using StreamReader reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }

        public static string JsonFromFile(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"No se encontró el archivo: {filePath}");

            return File.ReadAllText(filePath);
        }
        #endregion

        public static bool MatchAnyRegex(string input, IEnumerable<string> regexList, bool caseSensitive)
        {
            var options = caseSensitive ? RegexOptions.None : RegexOptions.IgnoreCase;

            foreach (var pattern in regexList)
            {
                if (string.IsNullOrWhiteSpace(pattern)) continue;

                if (Regex.IsMatch(input, pattern, options))
                    return true;
            }

            return false;
        }

        public static int GetLastDigits(int number, int count = 1)
        {
            if (count <= 0) return 0;

            var str = number.ToString();
            var len = str.Length;

            var span = str.AsSpan(len >= count ? len - count : 0);
            return int.Parse(span);
        }

        public static int SumDigits(int number)
        {
            number = Math.Abs(number);
            int sum = 0;

            for (; number > 0; number /= 10)
                sum += number % 10;

            return sum;
        }

        public static string RepeatPatternElement(string[] pattern, int step, int groupSize = 1)
        {
            if (pattern == null || pattern.Length == 0 || groupSize <= 0)
                return string.Empty;

            int groupIndex = step / groupSize;
            int index = ((groupIndex % pattern.Length) + pattern.Length) % pattern.Length;
            return pattern[index];
        }
        #region FormatValidation
        public static bool IsValidPathFormat(string path)
        {
            try
            {
                var fullPath = Path.GetFullPath(path);
                var root = Path.GetPathRoot(path);
                return !string.IsNullOrEmpty(root);
            }
            catch
            {
                return false;
            }
        }

        public static bool IsValidFileNameFormat(string fileName)
        {
            char[] invalidChars = Path.GetInvalidFileNameChars();
            return fileName.All(c => !invalidChars.Contains(c));
        }
        #endregion
        public static void Toggle(ref bool value)
        {
            value = !value;
        }

        public static bool TryDeleteDirectory(string path, string label,bool recursive=false)
        {
            try
            {
                if (Directory.Exists(path))
                {
                    Directory.Delete(path, recursive);
                    return true;
                }
                else
                {
                    Console.WriteLine($"La carpeta de {label} '{path}' no fue encontrada.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR al eliminar la carpeta de {label} '{path}': {ex.Message}");
                return false;
            }
        }

    }
}