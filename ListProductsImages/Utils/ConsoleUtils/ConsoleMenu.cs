using QuantumRevenant.Utilities;

namespace QuantumRevenant.ConsoleUtils
{
    public static class ConsoleMenu
    {
        #region Constants
        private const double ErrorValue = -1;
        #endregion


        #region Option Menus
        public static void ShowAndProcessMenu(
            string title,
            string subtitle,
            List<MenuItem> menuItems,
            out bool goBack,
            bool isSubMenu = false,
            bool showError = true,
            string? exitOrReturnTextOverride = null)
        {
            goBack = false;
            if (menuItems == null || !menuItems.Any())
            {
                ShowErrorEmptyMenu();
                return;
            }

            string[] displayNames = menuItems.Select(item => item.DisplayName).ToArray();

            int choice = ShowOptions(title, subtitle, displayNames, showError, returnInsteadOfExit: isSubMenu, exitOrReturnTextOverride);

            if (choice == 0)
            {
                if (!isSubMenu) Exit();
                goBack = true;
                return;
            }
            else if (choice > 0 && choice <= menuItems.Count)
            {
                MenuItem selectedItem = menuItems[choice - 1];
                selectedItem.Action.Invoke();
            }
            else ShowErrorBadOption();
        }
        public static T? ShowAndSelectItem<T>(
            string title,
            string subtitle,
            List<T> items,
            Func<T, string> displayNameSelector,
            bool showError = true,
            string? exitOrReturnTextOverride = null)
        {
            if (items == null || items.Count == 0)
            {
                ShowErrorEmptyMenu();
                return default;
            }

            string[] displayNames = items.Select(displayNameSelector).ToArray();

            int choice = ShowOptions(title, subtitle, displayNames, showError, returnInsteadOfExit: true, exitOrReturnTextOverride);

            if (choice == 0) return default;

            if (choice > 0 && choice <= items.Count) return items[choice - 1];

            ShowErrorBadOption();
            return default;
        }
        public static int ShowOptions(string title, string subtitle, string[] options, bool showError = true, bool returnInsteadOfExit = false, string? exitOrReturnTextOverride = null)
        {
            int choice;

            do
            {
                Console.Clear();

                if (!string.IsNullOrWhiteSpace(title))
                    Console.WriteLine($"{title}\n");

                if (!string.IsNullOrWhiteSpace(subtitle))
                    Console.WriteLine($"{subtitle}\n");

                for (int i = 0; i < options.Length; i++)
                    Console.WriteLine($"{i + 1}) {options[i]}");

                Console.WriteLine($"0) {exitOrReturnTextOverride ?? (returnInsteadOfExit ? "Volver" : "Salir")}");

                choice = AskInt("Selecciona una opción: ", (0, options.Length), showError);

            } while (choice == ErrorValue && showError);

            Console.Clear();
            return choice;
        }


        #endregion
        #region Ask Menus
        public static int AskInt(string question, (double min, double max)? range = null, bool showError = true)
        {
            return (int)AskDouble(question, range, showError);
        }
        public static double AskDouble(string question, (double min, double max)? range = null, bool showError = true)
        {
            double result;
            bool valid;
            var (min, max) = range ?? (double.NegativeInfinity, double.PositiveInfinity);

            do
            {
                Console.Write($"{question}");
                var input = Console.ReadLine();

                valid = double.TryParse(input, out result) && result >= min && result <= max;

                if (!valid && showError)
                {
                    string rangeInfo = double.IsInfinity(min) && double.IsInfinity(max)
                        ? ""
                        : $" entre {(!double.IsNegativeInfinity(min) ? min.ToString() : "sin mínimo")} y {(!double.IsPositiveInfinity(max) ? max.ToString() : "sin máximo")}";

                    ShowError("Entrada inválida", $"Debe ser un número válido{rangeInfo}");
                }

            } while (!valid && showError);

            return valid ? result : ErrorValue;
        }
        public static bool Confirm(
            string question,
            bool defaultIfInvalid = true,
            bool defaultFirst = true,
            bool showError = false,
            char trueKey = 'y',
            char falseKey = 'n')
        {
            // Asegura que las teclas sean distintas
            if (char.ToLower(trueKey) == char.ToLower(falseKey))
                throw new ArgumentException("Las teclas de confirmación deben ser diferentes.");

            // Arma las opciones en el orden visual correcto
            string first = defaultIfInvalid && defaultFirst ? trueKey.ToString().ToUpper() : trueKey.ToString().ToLower();
            string second = defaultIfInvalid && defaultFirst ? falseKey.ToString().ToLower() : falseKey.ToString().ToUpper();

            return AskBinaryOption(question:question,
                                    trueLabel:first, falseLabel:second,
                                    showError:showError,
                                    defaultIfInvalid:defaultIfInvalid,
                                    defaultFirst:defaultFirst);
        }
        public static bool AskBinaryOption(
            string question,
            string trueLabel = "Yes",
            string falseLabel = "No",
            bool acceptInitialLetter = true,
            bool showError = false,
            bool defaultIfInvalid = false,
            bool defaultFirst = true)
        {
            string trueKey = acceptInitialLetter ? trueLabel[..1].ToLower() : trueLabel.ToLower();
            string falseKey = acceptInitialLetter ? falseLabel[..1].ToLower() : falseLabel.ToLower();

            // En caso de colisión de iniciales, usar palabras completas
            if (trueKey == falseKey)
            {
                trueKey = trueLabel.ToLower();
                falseKey = falseLabel.ToLower();
            }

            // Orden visual de las opciones
            string firstOption = trueLabel;
            string secondOption = falseLabel;

            if (defaultFirst && !defaultIfInvalid)
                (firstOption, secondOption) = (falseLabel, trueLabel);

            string optionsDisplay =$"({firstOption}/{secondOption})";

            while (true)
            {
                Console.Write($"{question} {optionsDisplay}: ");
                string input = Console.ReadLine()?.Trim().ToLower() ?? string.Empty;

                if (input == trueKey) return true;
                if (input == falseKey) return false;

                if (showError)
                {
                    ShowError(
                        "Entrada inválida",
                        $"Debe ser '{trueLabel}' o '{falseLabel}'{(acceptInitialLetter ? " (o su inicial)" : "")}"
                    );
                }
                else
                {
                    return defaultIfInvalid;
                }
            }
        }
        public static string AskPath(bool requireExistence = false, bool showError = true, bool requireValidPath = true, string message = "Ingrese un path: ")
        {
            while (true)
            {
                Console.Write(message);
                string input = Console.ReadLine()?.Trim() ?? "";

                bool isValid =
                    !string.IsNullOrWhiteSpace(input) &&
                    QuantumTools.IsValidPathFormat(input) &&
                    (!requireExistence || Directory.Exists(input) || File.Exists(input));

                if (isValid)
                    return input;

                if (!showError)
                    return string.Empty;


                if (string.IsNullOrWhiteSpace(input))
                    ShowError("El path no puede estar vacío.");
                else if (!QuantumTools.IsValidPathFormat(input))
                    ShowError("El formato del path no es válido.");
                else if (requireExistence)
                    ShowError("El path no existe.");
                else
                    ShowError("Entrada inválida.");

                if (!requireValidPath)
                    return string.Empty;
            }
        }
        public static string AskFileName(
            string? extension = null,
            bool requireExtension = false,
            bool overwriteExtension = true,
            bool overwriteAnyExtension = false,
            bool showError = true,
            bool requireValidName = true,
            string message = "Ingrese un nombre de archivo: ")
        {
            extension = extension?.Trim().TrimStart('.');

            while (true)
            {
                Console.Write(message);
                string input = Console.ReadLine()?.Trim() ?? "";

                // Validación anticipada si no se requiere nombre válido
                if (!requireValidName && (string.IsNullOrWhiteSpace(input) || !QuantumTools.IsValidFileNameFormat(input)))
                    return string.Empty;

                bool isValid = QuantumTools.IsValidFileNameFormat(input);
                bool endsWithExpectedExt = !string.IsNullOrEmpty(extension)
                    && input.EndsWith("." + extension, StringComparison.OrdinalIgnoreCase);
                bool hasAnyExt = input.Contains('.') && !input.EndsWith(".");
                bool extensionValid = !requireExtension || (
                    string.IsNullOrEmpty(extension) ? hasAnyExt : endsWithExpectedExt
                );

                // Modificar extensión si es necesario
                if (!string.IsNullOrEmpty(extension))
                {
                    if ((overwriteExtension && endsWithExpectedExt) || (overwriteAnyExtension && hasAnyExt))
                    {
                        int lastDot = input.LastIndexOf('.');
                        input = input[..lastDot]; // Remueve última extensión
                    }

                    input += "." + extension;
                }

                if (!string.IsNullOrWhiteSpace(input) && isValid && extensionValid)
                    return input;

                if (!showError)
                    return string.Empty;

                // Mostrar errores relevantes
                if (string.IsNullOrWhiteSpace(input))
                    ShowError("El nombre no puede estar vacío.");
                else if (!isValid)
                    ShowError("El nombre contiene caracteres inválidos.");
                else if (requireExtension && string.IsNullOrEmpty(extension))
                    ShowError("Debe incluir alguna extensión (por ejemplo: '.txt').");
                else if (requireExtension && !endsWithExpectedExt)
                    ShowError($"El nombre debe terminar en '.{extension}'.");
            }
        }
        public static string AskString(
            string message,
            bool allowEmpty = true,
            string? defaultValue = null,
            Func<string, (bool IsValid, string ErrorMessage)>? customValidator = null,
            string fieldNameForError = "la entrada")
        {
            string displayPrompt = message;
            if (defaultValue != null) { displayPrompt += $" [{defaultValue}]"; }

            while (true)
            {
                Console.Write($"{displayPrompt}: ");
                string input = Console.ReadLine() ?? string.Empty;

                if (string.IsNullOrWhiteSpace(input) && defaultValue != null) return defaultValue;

                if (string.IsNullOrWhiteSpace(input))
                {
                    if (allowEmpty) return input;
                    else { ShowError($"Error: {fieldNameForError} no puede estar vacía. Por favor, inténtalo de nuevo.", color: ConsoleColor.Yellow); continue; }
                }

                if (customValidator != null)
                {
                    var validationResult = customValidator(input);
                    if (!validationResult.IsValid)
                    {
                        ShowError(title: $"Error: {validationResult.ErrorMessage ?? $"{fieldNameForError} no es válida."} Por favor, inténtalo de nuevo.", color: ConsoleColor.Yellow);
                        continue;
                    }
                }

                return input;
            }
        }

        #endregion
        #region Pause and Exit
        public static void Pause(string? customMessage = null)
        {
            string message = customMessage ?? "Presiona cualquier tecla para continuar...";
            Console.WriteLine(message);
            Console.ReadKey();
        }
        public static void Exit(string? customMessage = null)
        {
            Pause(customMessage);
            Environment.Exit(0);
        }

        #endregion
        #region Info Menus
        public static void ShowErrorEmptyMenu() { ShowError("Error de Menú", "No se han definido opciones para este menú."); }
        public static void ShowErrorBadOption() { ShowError("Error de Selección", "La opción seleccionada no fue procesada correctamente."); }
        public static void ShowError(string title = "¡Error!", string subtitle = "Se presentó un error", string? errorCode = null, ConsoleColor? color = null)
        {
            Console.Clear();
            Console.ForegroundColor = color ?? Console.ForegroundColor;
            Console.WriteLine(title);
            Console.WriteLine();
            Console.WriteLine(subtitle);
            Console.WriteLine();
            if (errorCode != null) { Console.WriteLine($"Código de Error: {errorCode}"); }
            Pause();
            if (color != null) { Console.ResetColor(); }
            Console.Clear();
        }
        #endregion
    }
    public class MenuItem
    {
        public string DisplayName { get; }
        public Action Action { get; }
        public MenuItem(string displayName, Action action)
        {
            if (string.IsNullOrWhiteSpace(displayName))
                throw new ArgumentException("El nombre para mostrar no puede estar vacío.", nameof(displayName));

            DisplayName = displayName;
            Action = action ?? throw new ArgumentNullException(nameof(action));
        }
    }
}