using System.Text.Json;
using ListProductsImages.Core;
using ListProductsImages.Pipeline;
using QuantumKit.UI;
using QuantumKit.Tools.IO;
using QuantumKit.Tools;

namespace ListProductsImages
{
    public class ImageListerConsole
    {
        private FilterContainer _defaultFilterPresets;
        private AppSettings _userSettings;
        private readonly string _appSpecificFolder;
        private readonly string _settingsFilePath;
        private string _currentListSourcePath = "E:\\Dropbox\\03. Proyecto ERP Feng Shui\\Ernesto Rivas\\2. WORKSPACE\\TEST SPACE";
        private string _currentOutputDirectory = "E:\\Dropbox\\03. Proyecto ERP Feng Shui\\Ernesto Rivas\\2. WORKSPACE\\TEST SPACE";
        private ProductImageProcessor _processor;
        private List<MenuItem> _mainMenuItems;
        private List<MenuItem> _customFilterItems;
        private List<MenuItem> _preferencesItems;
        private List<MenuItem> _restorePreferencesItems;

        public ImageListerConsole()
        {
            _appSpecificFolder = AppSettings.AppSpecificFolder;
            Directory.CreateDirectory(_appSpecificFolder);
            _settingsFilePath = AppSettings.settingsFilePath;

            _defaultFilterPresets = new();
            _userSettings = new();

            LoadDefaultPresets();
            LoadUserSettings();
            InitializeCurrentPaths();

            _processor = new ProductImageProcessor();

            _mainMenuItems =
            [
                new MenuItem("Listar Archivos",HandleListingOption),
                new MenuItem("Filtros Personalizados",HandleCustomFiltersOption),
                new MenuItem("Preferencias de Usuario",HandlePreferencesOption)
            ];

            _customFilterItems =
            [
                new MenuItem("Ver Filtros Preestablecidos",ShowPresetFilters),
                new MenuItem("Ver Filtros Personalizados",ShowCustomFilters),
                new MenuItem("Crear Filtro Personalizado",CreateCustomFilter),
                new MenuItem("Borrar Filtro Personalizado",DeleteCustomFilter),
                new MenuItem("Borrar TODOS los Filtros Personalizados",DeleteAllFilters)

            ];

            _preferencesItems = [];
            UpdatePreferencesItemsMenu();

            _restorePreferencesItems =
            [
                new MenuItem("Restablecer Ajustes (conservar filtros personalizados)",RestorePreferences),
                new MenuItem("Borrar Todos los Filtros Personalizados",DeleteAllFilters),
                new MenuItem("Restablecer Ajustes y Filtros (estado de fábrica)",RestoreApp),
                new MenuItem("Borrar Datos de Configuración de Usuario",UninstallApp),
            ];
        }

        private void InitializeCurrentPaths()
        {
            _userSettings ??= new AppSettings();

            _currentListSourcePath = _userSettings.UseExecutionPathAsListSource ? Directory.GetCurrentDirectory()
                                                                                : _userSettings.CustomListSourcePath;

            if (string.IsNullOrEmpty(_currentListSourcePath) && _userSettings.UseExecutionPathAsListSource) { _currentListSourcePath = "."; }

            _currentOutputDirectory = _userSettings.UseExecutionPathAsOutputDirectory ? Directory.GetCurrentDirectory()
                                                                                        : (!string.IsNullOrEmpty(_userSettings.CustomOutputDirectory)
                                                                                        ? _userSettings.CustomOutputDirectory : Directory.GetCurrentDirectory());

            if (string.IsNullOrEmpty(_currentOutputDirectory) && _userSettings.UseExecutionPathAsOutputDirectory) { _currentOutputDirectory = "."; }
        }
        private void UpdatePreferencesItemsMenu()
        {
            bool exPathList = _userSettings.UseExecutionPathAsListSource;
            bool exPathOutput = _userSettings.UseExecutionPathAsOutputDirectory;
            _preferencesItems =
            [
                new MenuItem("Consultar Ajustes Actuales", ShowActualPreferences),
                new MenuItem($"Cambiar Ruta de Listado Actual (Sesión)", ChangeActualListPath),
                new MenuItem($"Cambiar Ruta de Resultados Actual (Sesión)", ChangeActualOutputPath),
                new MenuItem($"Listar desde Ruta de Ejecución (Predeterminado): [{(exPathList ? "ON" : "OFF")}]", ToggleUseExecutionPathAsListSource),
                new MenuItem($"Guardar Resultados en Ruta de Ejecución (Predeterminado): [{(exPathOutput ? "ON" : "OFF")}]", ToggleUseExecutionPathAsOutputDirectory),
                new MenuItem($"Ruta de Listado Personalizada: [{(exPathList ? "Ignorada" : "En Uso")}]", ChangeCustomListSourcePath),
                new MenuItem($"Ruta de Guardado Personalizada: [{(exPathOutput ? "Ignorada" : "En Uso")}]", ChangeCustomOutputDirectory),
                new MenuItem($"Nombre Archivo de Salida", ChangeCustomOutputFileName),
                new MenuItem("Restaurar Configuraciones por Defecto", HandleRestorePreferences)
            ];
        }

        private void LoadDefaultPresets()
        {
            string resourceName = "ListProductsImages.Resources.DefaultFilterPresets.json";
            string json = JsonUtils.JsonFromEmbeddedResource(resourceName);
            _defaultFilterPresets = JsonSerializer.Deserialize<FilterContainer>(json) ?? new();
        }
        private void LoadUserSettings()
        {
            try
            {
                if (File.Exists(_settingsFilePath))
                {
                    string jsonString = JsonUtils.JsonFromFile(_settingsFilePath);
                    _userSettings = JsonSerializer.Deserialize<AppSettings>(jsonString) ?? new();

                    if (_userSettings == null)
                    {
                        _userSettings = new();
                    }
                }
                else
                {
                    Console.WriteLine("Información: No se encontró archivo de configuración. Creando uno con valores por defecto.");
                    _userSettings = new(); // Carga los valores por defecto
                    SaveUserSettings(); // Guarda el nuevo archivo con los valores por defecto inmediatamente
                }
            }
            catch (JsonException jsonEx)
            {
                Console.WriteLine($"Error al leer el archivo de configuración (JSON inválido): {jsonEx.Message}. Usando valores por defecto.");
                _userSettings = new(); // Fallback a los valores por defecto
            }
            catch (Exception ex)
            {
                // Otros errores como problemas de permisos de archivo
                Console.WriteLine($"Error al cargar la configuración: {ex.Message}. Usando valores por defecto.");
                _userSettings = new(); // Fallback
            }
        }

        private void SaveUserSettings()
        {
            try
            {
                if (_userSettings == null) _userSettings = new();

                JsonSerializerOptions options = new JsonSerializerOptions { WriteIndented = true }; // Para que el JSON sea legible
                string jsonString = JsonSerializer.Serialize(_userSettings, options);
                File.WriteAllText(_settingsFilePath, jsonString);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al guardar la configuración: {ex.Message}");
                // Aquí podrías informar al usuario que los cambios podrían no haberse guardado.
            }
        }

        public void Run()
        {
            if (_userSettings.UseExecutionPathAsListSource) _currentListSourcePath = Directory.GetCurrentDirectory();
            else _currentListSourcePath = _userSettings.CustomListSourcePath;

            if (_userSettings.UseExecutionPathAsOutputDirectory) _currentOutputDirectory = Directory.GetCurrentDirectory();
            else _currentOutputDirectory = _userSettings.CustomOutputDirectory;

            do
            {
                ConsoleMenuBuilder.ShowAndProcessMenu(
                    title: "Listado de Imágenes de Productos",
                    subtitle: $"PRECAUCIÓN: La busqueda se está realizando en:\n {_currentListSourcePath}.\n Cambialo en Preferencias.\nSelecciona tu opción:",
                    menuItems: _mainMenuItems,
                    out bool goBack,
                    isSubMenu: false
                );
                if (goBack) return;
            } while (true);// El bucle se rompe con Environment.Exit() llamado desde ConsoleMenu.Exit()
        }

        private void HandleListingOption()
        {
            Console.WriteLine("\n--- Configurar Búsqueda ---");

            List<FilterCriteria> allFilters = _defaultFilterPresets.Filters.Concat(_userSettings.CustomFiltersContainer.Filters).ToList();

            FilterCriteria? selectedFilter = ConsoleMenuBuilder.ShowAndSelectItem("Selecciona el filtro para la Busqueda",
                                                                            $"PRECAUCIÓN: La busqueda se está realizando en:\n {_currentListSourcePath}.\n Cambialo en Preferencias.",
                                                                            allFilters, fc => fc.Name);

            if (selectedFilter == null) return;

            Console.Clear();
            Console.WriteLine($"Se escogió \"{selectedFilter.Name}\"");
            Console.WriteLine($"Descripción: {selectedFilter.Description}");
            Console.WriteLine();
            if (!ConsoleMenuBuilder.Confirm("¿Confirma que desea ese filtro?")) { return; }

            bool inverseSearch = ConsoleMenuBuilder.Confirm("¿Deseas mostrar solo los archivos que coinciden con los criterios, o los que no coinciden?", defaultIfInvalid: false, trueKey: 'n', falseKey: 'c');

            ProcessCriteria processCriteria = new(_currentListSourcePath, Path.Combine(_currentOutputDirectory, _userSettings.OutputFileName), inverseSearch);

            ProgramCriteria criteria = new(selectedFilter, processCriteria);

            _userSettings.LastAnalyzedPath = criteria.ProcessCriteria.RootPath;
            SaveUserSettings(); // Llamarías a esto cuando realmente se guarde la config

            Console.WriteLine($"\nIniciando búsqueda con filtro '{criteria.FilterCriteria?.Name}' en '{criteria.ProcessCriteria.RootPath}'...");

            _processor.Pipeline(criteria);

            ConsoleMenuBuilder.Pause($"Búsqueda completada. Resultados en '{criteria.ProcessCriteria.OutputFile}'. Presiona Enter...\a");
            return;
        }

        private void HandleCustomFiltersOption()
        {
            do
            {
                Console.WriteLine("\n--- Filtros Personalizados ---");

                ConsoleMenuBuilder.ShowAndProcessMenu(
                        title: "Filtros",
                        subtitle: "PRECAUCIÓN: Algunas de estas acciones son irreversibles.",
                        menuItems: _customFilterItems,
                        out bool goBack,
                        isSubMenu: true
                    );
                if (goBack) return;
            } while (true); // El bucle se rompe con goBack True
        }

        private void ShowPresetFilters() { ShowFilters(_defaultFilterPresets.Filters); }
        private void ShowCustomFilters() { ShowFilters(_userSettings.CustomFiltersContainer.Filters); }
        private void ShowFilters(List<FilterCriteria> filterCriterias)
        {
            Console.Clear();
            FilterCriteria? selectedFilter = ConsoleMenuBuilder.ShowAndSelectItem("Selecciona el filtro para la Consultar", "", filterCriterias, fc => fc.Name);
            ShowFilter(selectedFilter);
        }
        private void ShowFilter(FilterCriteria? filter)
        {
            Console.Clear();

            if (filter == null) return;

            Console.WriteLine
            (

            $"Nombre: {filter.Name}\n" +
            $"Descripción:\n{filter.Description}\n\n" +
            $"Folders Rechazados Forzosamente: {string.Join(" - ", filter.FolderRejectForced.Where(f => f != null && f.Match != null).Select(f => f.Match))}\n" +
            $"Folders Aceptados Forzosamente: {string.Join(" - ", filter.FolderAcceptForced.Where(f => f != null && f.Match != null).Select(f => f.Match))}\n\n" +
            $"Folders Rechazados: {string.Join(" - ", filter.FolderRejectIfFound.Where(f => f != null && f.Match != null).Select(f => f.Match))}\n" +
            $"Folders Aceptados: {string.Join(" - ", filter.FolderAcceptIfFound.Where(f => f != null && f.Match != null).Select(f => f.Match))}\n\n" +
            $"Regex Archivos Rechazados: {string.Join(" - ", filter.FileRejectRegex)}\n" +
            $"Regex Archivos Aceptados: {string.Join(" - ", filter.FileAcceptRegex)}\n" +
            $"Sensible a Mayúsculas: {(filter.CaseSensitive ? "Verdadero" : "Falso")}"
            );

            ConsoleMenuBuilder.Pause();
        }

        private void CreateCustomFilter()
        {
            Console.Clear();
            Console.WriteLine("--- Creación de Nuevo Filtro Personalizado ---");
            Console.WriteLine("Siga las instrucciones para definir las propiedades de su nuevo filtro.");
            ConsoleMenuBuilder.Pause("Presiona Enter para comenzar...");

            FilterCriteria newFilter = new FilterCriteria();

            // 1. Nombre del Filtro (Único y Requerido)
            string filterName;
            bool nameExists;
            if (_userSettings.CustomFiltersContainer.Filters == null) _userSettings.CustomFiltersContainer.Filters = new List<FilterCriteria>();

            List<string> existingNames = (_defaultFilterPresets?.Filters ?? Enumerable.Empty<FilterCriteria>())
                                        .Select(f => f.Name)
                                        .Concat(_userSettings.CustomFiltersContainer.Filters.Select(f => f.Name)) // Asume CustomPresets en AppSettings
                                        .ToList();
            do
            {
                filterName = ConsoleMenuBuilder.AskString("\nNombre del nuevo filtro (requerido, debe ser único): ", allowEmpty: false);
                nameExists = existingNames.Any(n => n.Equals(filterName, StringComparison.OrdinalIgnoreCase));
                if (nameExists)
                {
                    Console.WriteLine($"El nombre de filtro '{filterName}' ya existe. Por favor, introduce un nombre diferente.");
                }
            } while (nameExists);
            newFilter.Name = filterName;

            // 2. Descripción (Opcional)
            newFilter.Description = ConsoleMenuBuilder.AskString("\nDescripción del filtro (opcional): ", allowEmpty: true);

            // 3. Sensibilidad a Mayúsculas/Minúsculas (Sí/No)
            newFilter.CaseSensitive = ConsoleMenuBuilder.Confirm("\n¿El filtro debe ser sensible a mayúsculas/minúsculas para patrones y regex?", defaultIfInvalid: false);

            // --- Configuración de Reglas de Carpetas ---
            Console.WriteLine("\n--- Configuración de Reglas de Carpetas ---");
            Console.WriteLine("Para cada tipo de regla de carpeta, se le preguntará si desea configurarla.");
            Console.WriteLine("Si elige sí, podrá añadir uno o más patrones de carpeta.");
            ConsoleMenuBuilder.Pause();

            if (ConsoleMenuBuilder.Confirm("¿Configurar 'Carpetas de Rechazo Forzado'?\n(RECHAZA el archivo si la ruta contiene estas carpetas, ignora otras reglas de aceptación)"))
                newFilter.FolderRejectForced = ConfigureFolderMatchList("Carpetas de Rechazo Forzado",
                    "Archivos en rutas que contengan estas carpetas serán SIEMPRE RECHAZADOS.");
            else newFilter.FolderRejectForced = new List<FolderMatch>();

            if (ConsoleMenuBuilder.Confirm("\n¿Configurar 'Carpetas de Aceptación Forzada'?\n(ACEPTA el archivo si la ruta contiene estas carpetas, anulado solo por 'Rechazo Forzado de Carpeta')"))
                newFilter.FolderAcceptForced = ConfigureFolderMatchList("Carpetas de Aceptación Forzada",
                    "Archivos en rutas que contengan estas carpetas serán ACEPTADOS (a menos que un Rechazo Forzado de Carpeta aplique).");
            else newFilter.FolderAcceptForced = new List<FolderMatch>();

            if (ConsoleMenuBuilder.Confirm("\n¿Configurar 'Carpetas de Rechazo si se Encuentran'?\n(RECHAZA si no hay 'Aceptación Forzada')"))
                newFilter.FolderRejectIfFound = ConfigureFolderMatchList("Carpetas de Rechazo si se Encuentran",
                    "Archivos en rutas que contengan estas carpetas serán RECHAZADOS (a menos que una Aceptación Forzada aplique).");
            else newFilter.FolderRejectIfFound = new List<FolderMatch>();

            if (ConsoleMenuBuilder.Confirm("\n¿Configurar 'Carpetas de Aceptación si se Encuentran'?\n(ACEPTA si no hay alguna regla de rechazo que aplique)"))
                newFilter.FolderAcceptIfFound = ConfigureFolderMatchList("Carpetas de Aceptación si se Encuentran",
                    "Archivos en rutas que contengan estas carpetas serán ACEPTADOS (a menos que una regla de Rechazo aplique).");
            else newFilter.FolderAcceptIfFound = new List<FolderMatch>();

            // --- Configuración de Reglas de Nombres de Archivo (Expresiones Regulares) ---
            Console.WriteLine("\n--- Configuración de Reglas para Nombres de Archivo (usando Expresiones Regulares) ---");
            ConsoleMenuBuilder.Pause();

            if (ConsoleMenuBuilder.Confirm("¿Configurar 'Regex de Rechazo de Archivos'?\n(Nombres de archivo que coincidan serán RECHAZADOS, a menos que haya 'Aceptación Forzada de Archivo')"))
                newFilter.FileRejectRegex = ConfigureStringList("Regex de Rechazo de Archivos",
                    "Los nombres de archivo que coincidan con estas expresiones regulares serán RECHAZADOS.", isRegexList: true);
            else newFilter.FileRejectRegex = new List<string>();

            if (ConsoleMenuBuilder.Confirm("\n¿Configurar 'Regex de Aceptación de Archivos'?\n(Nombres de archivo que coincidan serán ACEPTADOS, a menos que una regla de rechazo aplique)"))
                newFilter.FileAcceptRegex = ConfigureStringList("Regex de Aceptación de Archivos",
                    "Los nombres de archivo que coincidan con estas expresiones regulares serán ACEPTADOS.", isRegexList: true);
            else newFilter.FileAcceptRegex = new List<string>();

            // --- Revisar y Confirmar ---
            Console.Clear();
            Console.WriteLine("--- Resumen del Nuevo Filtro Personalizado Creado ---");
            ShowFilter(newFilter); // Tu método existente para mostrar detalles del filtro

            Console.WriteLine("\n----------------------------------------------------");
            if (ConsoleMenuBuilder.Confirm("¿Desea guardar este nuevo filtro personalizado?"))
            {
                // Asumiendo que AppSettings tiene: public List<FilterCriteria> CustomPresets { get; set; }
                _userSettings.CustomFiltersContainer.Filters.Add(newFilter);
                SaveUserSettings();
                Console.WriteLine($"Filtro '{newFilter.Name}' guardado exitosamente.");
            }
            else
            {
                Console.WriteLine("Creación de filtro cancelada. No se guardaron los cambios.");
            }
            ConsoleMenuBuilder.Pause();
        }

        private List<FolderMatch> ConfigureFolderMatchList(string listFriendlyName, string itemPurposeDescription)
        {
            List<FolderMatch> folderMatches = new List<FolderMatch>();
            Console.Clear();
            Console.WriteLine($"--- Configurar Lista: {listFriendlyName} ---");
            Console.WriteLine(itemPurposeDescription);
            Console.WriteLine("\nCuando se le solicite un 'Patrón de Carpeta', déjelo en blanco y presione Enter para terminar de añadir a esta lista.");
            Console.WriteLine("----------------------------------------------------");

            int itemCount = 1;
            while (true)
            {
                Console.WriteLine($"\nDefiniendo Condición de Carpeta #{itemCount} para '{listFriendlyName}':");
                string matchPattern = ConsoleMenuBuilder.AskString($"  Patrón de Carpeta #{itemCount} (ej: '_baja', 'Final'): ", allowEmpty: true);

                if (string.IsNullOrWhiteSpace(matchPattern)) break; // El usuario ha terminado de añadir a esta lista

                // Selección simplificada para MatchType (Contains o Exact)
                bool isExactMatch = ConsoleMenuBuilder.Confirm(
                    "  ¿El patrón debe coincidir de forma EXACTA con el nombre de la carpeta?\n" +
                    "  (E para Coincidencia Exacta, P para Coincidencia Parcial - el nombre contiene el patrón)",
                    defaultIfInvalid: false, trueKey: 'E', falseKey: 'P'
                );

                string folderMatchType = isExactMatch ? "Exact" : "Contains";

                folderMatches.Add(new FolderMatch { Match = matchPattern, Type = folderMatchType });
                // Muestra el tipo interpretado por la propiedad MatchType de FolderMatch
                Console.WriteLine($"Añadido: Patrón='{matchPattern}', Tipo Interpretado='{folderMatches.Last().MatchType}'");
                itemCount++;
            }

            Console.WriteLine($"\nSe configuraron {folderMatches.Count} condiciones para '{listFriendlyName}'.");
            ConsoleMenuBuilder.Pause();
            return folderMatches;
        }

        private List<string> ConfigureStringList(string listFriendlyName, string itemPurposeDescription, bool isRegexList = false)
        {
            List<string> stringEntries = [];
            Console.Clear();
            Console.WriteLine($"--- Configurar Lista: {listFriendlyName} ---");
            Console.WriteLine(itemPurposeDescription);
            if (isRegexList)
            {
                Console.WriteLine("NOTA: Para expresiones regulares, recuerde escapar caracteres especiales si busca literales (ej. '\\.' para un punto).");
            }
            Console.WriteLine("\nDeje la entrada en blanco y presione Enter para terminar de añadir a esta lista.");
            Console.WriteLine("----------------------------------------------------");

            int itemCount = 1;
            while (true)
            {
                string entry = ConsoleMenuBuilder.AskString($"  {(isRegexList ? "Expresión Regular" : "Entrada")} #{itemCount}: ", allowEmpty: true);
                if (string.IsNullOrWhiteSpace(entry))
                {
                    break;
                }
                stringEntries.Add(entry);
                Console.WriteLine($"Añadido: '{entry}'");
                itemCount++;
            }
            Console.WriteLine($"\nSe configuraron {stringEntries.Count} entradas para '{listFriendlyName}'.");
            ConsoleMenuBuilder.Pause();
            return stringEntries;
        }

        private void DeleteCustomFilter()
        {
            Console.Clear();
            List<FilterCriteria> filters = _userSettings.CustomFiltersContainer.Filters;
            FilterCriteria? selectedFilter = ConsoleMenuBuilder.ShowAndSelectItem("Selecciona el filtro para eliminar", "", filters, fc => fc.Name);
            if (selectedFilter == null) return;

            Console.Clear();
            Console.WriteLine($"Se escogió \"{selectedFilter.Name}\"");
            Console.WriteLine($"Descripción: {selectedFilter.Description}");
            Console.WriteLine();
            if (!ConsoleMenuBuilder.Confirm("¿Confirma que desea eliminarlo? (Esta acción es irreversible)", defaultIfInvalid: false)) return;
            if (!ConsoleMenuBuilder.Confirm("[Doble Confirmación]\n¿Confirma que desea eliminarlo? (Esta acción es irreversible)", defaultIfInvalid: false)) return;
            filters.Remove(selectedFilter);
            _userSettings.CustomFiltersContainer.Filters = filters;
            SaveUserSettings();
        }

        private void DeleteAllFilters()
        {
            if (!ConsoleMenuBuilder.Confirm("¿Confirma que desea eliminar TODOS los filtros personalizados? (Esta acción es irreversible)", defaultIfInvalid: false)) return;
            if (!ConsoleMenuBuilder.Confirm("[Doble Confirmación]\n¿Confirma que desea eliminar TODOS los filtros personalizados? (Esta acción es irreversible)", defaultIfInvalid: false)) return;

            _userSettings.CustomFiltersContainer ??= new();
            _userSettings.CustomFiltersContainer.Filters ??= new();
            _userSettings.CustomFiltersContainer.Filters.Clear();

            SaveUserSettings();

            Console.WriteLine("Todos los filtros personalizados han sido eliminados.");
            ConsoleMenuBuilder.Pause();
        }

        private void HandlePreferencesOption()
        {
            do
            {
                Console.WriteLine("\n--- Preferencias ---");

                ConsoleMenuBuilder.ShowAndProcessMenu(
                        title: "Preferencias",
                        subtitle: "",
                        menuItems: _preferencesItems,
                        out bool goBack,
                        isSubMenu: true
                    );
                if (goBack) return;

                UpdatePreferencesItemsMenu();
            } while (true); // El bucle se rompe con goBack True
        }

        private void ShowActualPreferences()
        {
            Console.Clear();

            if (_userSettings == null) return;

            Console.WriteLine
            (
            $"Ruta Actual de Listado: {_currentListSourcePath}\n" +
            $"Ruta Actual de Salida: {_currentOutputDirectory}\n\n" +

            $"Usar Ruta de Ejecución como Ruta de Listado: {(_userSettings.UseExecutionPathAsListSource ? "Sí" : "No")}\n" +
            $"Ruta de Listado Predeterminada {(!_userSettings.UseExecutionPathAsListSource ? "(Fuera de Uso)" : "")}: {_userSettings.CustomListSourcePath}\n\n" +

            $"Usar Ruta de Ejecución como Ruta de Salida: {(_userSettings.UseExecutionPathAsOutputDirectory ? "Sí" : "No")}\n" +
            $"Ruta de Salida Predeterminada {(!_userSettings.UseExecutionPathAsOutputDirectory ? "(Fuera de Uso)" : "")}: {_userSettings.CustomOutputDirectory}\n\n" +

            $"Nombre del Archivo de Salida: {_userSettings.OutputFileName}\n\n" +
            $"Cantidad de Filtros Personalizados: {_userSettings.CustomFiltersContainer.Filters.Count()}\n\n" +
            $"Ultima Ruta Personalizada: {_userSettings.LastAnalyzedPath}\n"
            );

            ConsoleMenuBuilder.Pause();
        }

        private void ChangeActualListPath()
        {
            Console.WriteLine($"Ruta Actual de Listado: {_currentListSourcePath}");
            string path = ConsoleMenuBuilder.AskPath(requireExistence: true, requireValidPath: false);
            if (path == "") { return; }
            Console.WriteLine($"Se ingresó: \"{path}\"");
            if (!ConsoleMenuBuilder.Confirm("¿El Path ingresado es el correcto?¿Confirma que desea actualizar la Ruta Actual de Listado?")) { return; }
            _currentListSourcePath = path;
            SaveUserSettings();
        }

        private void ChangeActualOutputPath()
        {
            Console.WriteLine($"Ruta Actual de Listado: {_currentOutputDirectory}");
            string path = ConsoleMenuBuilder.AskPath(requireExistence: true, requireValidPath: false);
            if (path == "") { return; }
            Console.WriteLine($"Se ingresó: \"{path}\"");
            if (!ConsoleMenuBuilder.Confirm("¿El Path ingresado es el correcto?¿Confirma que desea actualizar la Ruta Actual de Salida?")) { return; }
            _currentOutputDirectory = path;
            SaveUserSettings();
        }

        private void ToggleUseExecutionPathAsListSource() { CoreUtils.Toggle(ref _userSettings.UseExecutionPathAsListSource); }
        private void ToggleUseExecutionPathAsOutputDirectory() { CoreUtils.Toggle(ref _userSettings.UseExecutionPathAsOutputDirectory); }
        private void ChangeCustomListSourcePath()
        {
            Console.WriteLine($"Ruta de Listado Predeterminada: {_userSettings.CustomListSourcePath}");
            string path = ConsoleMenuBuilder.AskPath(requireExistence: true, requireValidPath: false);
            if (path == "") { return; }
            Console.WriteLine($"Se ingresó: \"{path}\"");
            if (!ConsoleMenuBuilder.Confirm("¿El Path ingresado es el correcto?¿Confirma que desea actualizar la Ruta Predeterminada de Listado?")) { return; }
            _userSettings.CustomListSourcePath = path;
            SaveUserSettings();
        }
        private void ChangeCustomOutputDirectory()
        {
            Console.WriteLine($"Ruta de Listado Predeterminada: {_userSettings.CustomOutputDirectory}");
            string path = ConsoleMenuBuilder.AskPath(requireExistence: true, requireValidPath: false);
            if (path == "") { return; }
            Console.WriteLine($"Se ingresó: \"{path}\"");
            if (!ConsoleMenuBuilder.Confirm("¿El Path ingresado es el correcto?¿Confirma que desea actualizar la Ruta Predeterminada de Salida?")) { return; }
            _userSettings.CustomOutputDirectory = path;
            SaveUserSettings();
        }
        private void ChangeCustomOutputFileName()
        {
            Console.WriteLine($"Nombre de Archivo de Salida: {_userSettings.OutputFileName}");
            string name = ConsoleMenuBuilder.AskFileName(extension: ".txt", requireValidName: false);
            if (name == "") { return; }
            Console.WriteLine($"Se ingresó: \"{name}\"");
            if (!ConsoleMenuBuilder.Confirm("¿El nombre ingresado es el correcto?¿Confirma que desea actualizar el Nombre de Archivo de Salida?")) { return; }
            _userSettings.OutputFileName = name;
            SaveUserSettings();
        }
        private void HandleRestorePreferences()
        {
            do
            {
                Console.WriteLine("\n--- Restaurar Preferencias ---");

                ConsoleMenuBuilder.ShowAndProcessMenu(
                        title: "Restaurar Preferencias",
                        subtitle: "PRECAUCIÓN: Estas acciones son irreversibles.",
                        menuItems: _restorePreferencesItems,
                        out bool goBack,
                        isSubMenu: true
                    );
                if (goBack) return;
            } while (true); // El bucle se rompe con goBack True
        }

        private void RestorePreferences()
        {
            if (!ConsoleMenuBuilder.Confirm("¿Confirma que desea restablecer los ajustes personalizados? (Esta acción es irreversible)", defaultIfInvalid: false)) return;
            if (!ConsoleMenuBuilder.Confirm("[Doble Confirmación]\n¿Confirma que desea restablecer los ajustes personalizados? (Esta acción es irreversible)", defaultIfInvalid: false)) return;

            FilterContainer currentFilterContainer = _userSettings.CustomFiltersContainer ?? new();

            _userSettings = new();
            _userSettings.CustomFiltersContainer = currentFilterContainer;
            SaveUserSettings();
            InitializeCurrentPaths();
            Console.WriteLine("Ajustes restablecidos. Los filtros personalizados han sido conservados.");
            ConsoleMenuBuilder.Pause();
        }

        private void RestoreApp()
        {
            if (!ConsoleMenuBuilder.Confirm("¿Confirma que desea restablecer TODA la aplicación? (Esta acción es irreversible)", defaultIfInvalid: false)) return;
            if (!ConsoleMenuBuilder.Confirm("[Doble Confirmación]\n¿Confirma que desea restablecer TODA la aplicación? (Esta acción es irreversible)", defaultIfInvalid: false)) return;

            _userSettings = new();
            SaveUserSettings();
            InitializeCurrentPaths();
            Console.WriteLine("Toda la configuración de la aplicación ha sido restablecida a sus valores predeterminados.");
            ConsoleMenuBuilder.Pause();
        }

        private void UninstallApp()
        {
            if (!ConsoleMenuBuilder.Confirm("¿Confirma que desea borrar TODOS los datos de configuración guardados de esta aplicación? (Esta acción es irreversible)", defaultIfInvalid: false))
                return;

            if (!ConsoleMenuBuilder.Confirm("[Doble Confirmación]\n¿Confirma que desea borrar TODOS los datos de configuración guardados de esta aplicación? (Esta acción es irreversible)", defaultIfInvalid: false))
                return;

            _userSettings = new();
            SaveUserSettings();

            bool appFolderDeleted = CoreUtils.TryDeleteDirectory(_appSpecificFolder, "la aplicación",true);

            if (appFolderDeleted)
            {
                Console.Clear();
                Console.WriteLine($"Carpeta de datos '{_appSpecificFolder}' eliminada correctamente.");
            }
            else
            {
                ConsoleMenuBuilder.Pause();
                return; // No continúa si no se pudo borrar la carpeta principal
            }

            bool familyFolderDeleted = CoreUtils.TryDeleteDirectory(AppSettings.AppFamilyFolder, "la familia");

            if (familyFolderDeleted)
            {
                Console.WriteLine($"Carpeta de datos '{AppSettings.AppFamilyFolder}' eliminada correctamente.");
            }
            else
            {
                Console.WriteLine($"La carpeta de datos de la familia no se pudo eliminar. Probablemente haya otras apps usándola.");
            }

            Console.WriteLine("El programa se cerrará ahora.");
            ConsoleMenuBuilder.Exit();
        }

    }

}