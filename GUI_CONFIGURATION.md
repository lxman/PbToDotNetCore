# GUI Framework Configuration

PbToDotNetCore supports converting PowerBASIC GUI code to different C# GUI frameworks. You can configure which framework to use.

## Available GUI Frameworks

- **None** (default) - Generates helpful TODO comments for manual conversion
- **WinForms** - Converts to Windows Forms (not yet implemented)
- **Wpf** - Converts to Windows Presentation Foundation (not yet implemented)

## How to Configure

### Method 1: Configuration File (Recommended)

Create a file named `pbtodotnetcore.config.json` in one of these locations:
1. Your project's working directory (checked first)
2. The directory containing PbToDotNetCore.dll
3. Your user home directory

**Example configuration file:**

```json
{
  "GuiFramework": "WinForms"
}
```

Valid values for `GuiFramework`:
- `"None"` - No automatic conversion (default)
- `"WinForms"` - Windows Forms conversion
- `"Wpf"` - WPF/XAML conversion

### Method 2: Programmatic Configuration (Advanced)

If you're using PbToDotNetCore as a library, you can set the framework programmatically:

```csharp
using PbToDotNetCore.Gui;

// Set the target framework before conversion
GuiConfiguration.TargetFramework = GuiFramework.WinForms;

// Now convert your code
string csCode = PbToCsConverter.GenerateCsCode(pbCode);
```

### Method 3: Generate Sample Config File

You can programmatically generate a sample configuration file:

```csharp
using PbToDotNetCore.Gui;

// Creates pbtodotnetcore.config.json in current directory
ConfigurationLoader.CreateSampleConfig();

// Or specify a directory
ConfigurationLoader.CreateSampleConfig(@"C:\MyProject");
```

## Configuration File Search Order

PbToDotNetCore searches for the configuration file in this order:

1. **Current working directory** - Where you run the application from
2. **Application base directory** - Where PbToDotNetCore.dll is located
3. **User home directory** - Your user profile folder

The first file found is used. This allows you to have a global default in your home directory and project-specific overrides in project directories.

## Important Notes

- The GUI framework setting is **global** - all GUI code in your project will be converted to the same framework
- Configuration is loaded once when `PbToCSharpConverter` is first instantiated
- To reload configuration after changing the file, call `ConfigurationLoader.ReloadConfiguration()`
- If no configuration file is found, the default (`None`) is used

## Implementation Status

Currently, only the `None` option is fully implemented, which generates helpful TODO comments for manual conversion.

WinForms and WPF converters are stubbed out and will throw `NotImplementedException` until their implementations are completed.
