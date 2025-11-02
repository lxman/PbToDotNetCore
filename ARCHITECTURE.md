# PowerBASIC to C# Converter - Architecture

## Overview

This project converts PowerBASIC code to C#. It's designed with a phased architecture that starts with single-file conversion and evolves to support entire multi-file applications.

## Current Architecture (Phase 1) ✅

### Single-File Conversion
```
PowerBASIC Source → ANTLR Parser → AST → String-based Visitor → C# Code
```

**Entry Point:**
```csharp
string csCode = PbToCsConverter.GenerateCsCode(pbSourceCode);
```

**Components:**
- `PbToCsConverter.cs` - Facade providing simple API
- `PbToCSharpConverter.cs` - ANTLR visitor that generates C# strings directly
- ANTLR-generated parser in `/Parser`

**Status:** ✅ **Working** - All 10 tests passing

---

## Future Architecture (Phases 2-5)

### Phase 2: IR Model Foundation 🔄 (In Progress)

**Goal:** Decouple parsing from code generation

```
PowerBASIC AST → IR Model → C# Generator → C# Code
```

**New Components Created:**
- `/Models` - Intermediate Representation classes
  - `CodeModel.cs` - Top-level container
  - `ClassDeclaration.cs`, `InterfaceDeclaration.cs`
  - `MethodDeclaration.cs`, `FunctionDeclaration.cs`
  - `Statement.cs`, `Expression.cs` - Statement/expression trees
  - Other supporting types
- `/Generator/CSharpGenerator.cs` - Generates C# from IR model
- `/Converter/PbToModelConverter.cs` - (Placeholder) Will convert AST → IR

**Status:** 🔄 **Foundation Ready** - IR models and generator created, converter not yet implemented

---

### Phase 3: Multi-File Awareness ⏳ (Planned)

**Goal:** Parse multiple files and understand their relationships

```
Multiple .BAS Files → Individual CodeModels → Merge & Analyze → Unified CodeModel
```

**Components to Build:**
```csharp
public class CodeAnalyzer
{
    // Identify cross-file dependencies
    public DependencyGraph AnalyzeDependencies(List<CodeModel> models);

    // Merge related code
    public CodeModel Merge(List<CodeModel> models);
}

public class CrossReferenceResolver
{
    // Resolve DECLARE statements
    // Match FUNCTION definitions to declarations
    // Track shared types and constants
}
```

**Status:** ⏳ **Not Started**

---

### Phase 4: Project Organization ⏳ (Planned)

**Goal:** Organize code into logical C# projects

```
Unified CodeModel → Analyze Structure → Organize into Projects → Solution Model
```

**Components to Build:**
```csharp
public class ProjectOrganizer
{
    public Solution OrganizeIntoProjects(CodeModel model);
}

public class Solution
{
    public string Name { get; set; }
    public List<Project> Projects { get; set; }
}

public class Project
{
    public string Name { get; set; }
    public ProjectType Type { get; set; }  // UI, BusinessLogic, Data, etc.
    public List<SourceFile> Files { get; set; }
    public List<string> References { get; set; }
}
```

**Heuristics for Project Separation:**
- GUI code → UI project
- Database code → Data project
- Shared types → Common/Core project
- Entry point (PBMAIN) → Main executable project

**Status:** ⏳ **Not Started**

---

### Phase 5: Solution Generation ⏳ (Planned)

**Goal:** Write complete Visual Studio solution to disk

```
Solution Model → Generate .sln → Generate .csproj → Generate .cs → File System
```

**Components to Build:**
```csharp
public class SolutionWriter
{
    public void WriteSolution(Solution solution, string outputPath);
}

public class ProjectFileGenerator
{
    public string GenerateCsproj(Project project);
}
```

**Outputs:**
```
MySolution/
├── MySolution.sln
├── MySolution.UI/
│   ├── MySolution.UI.csproj
│   ├── MainForm.cs
│   └── ...
├── MySolution.BusinessLogic/
│   ├── MySolution.BusinessLogic.csproj
│   └── ...
└── MySolution.Data/
    ├── MySolution.Data.csproj
    └── ...
```

**Status:** ⏳ **Not Started**

---

## Project Structure

```
PbToDotNetCore/
├── Models/                     ✅ IR model classes
│   ├── CodeModel.cs
│   ├── ClassDeclaration.cs
│   ├── InterfaceDeclaration.cs
│   ├── MethodDeclaration.cs
│   ├── FunctionDeclaration.cs
│   ├── Statement.cs
│   ├── Expression.cs
│   └── ...
├── Generator/                  ✅ Code generators
│   └── CSharpGenerator.cs
├── Converter/                  🔄 Converters
│   ├── PbToModelConverter.cs   (placeholder)
│   └── README.md
├── Parser/                     ✅ ANTLR-generated
│   ├── PowerBasicLexer.cs
│   ├── PowerBasicParser.cs
│   └── ...
├── PbToCsConverter.cs          ✅ Public API facade
├── PbToCSharpConverter.cs      ✅ Current working visitor
└── ARCHITECTURE.md             ✅ This file
```

---

## API Evolution

### Current API (Works Today) ✅
```csharp
string csCode = PbToCsConverter.GenerateCsCode(pbSourceCode);
```

### Future API - Single File (Phase 2) 🔄
```csharp
var model = PbToModelConverter.ConvertFile(pbCode);
string csCode = CSharpGenerator.Generate(model);
```

### Future API - Multi-File Project (Phases 3-5) ⏳
```csharp
// Read all PowerBASIC files
var files = new Dictionary<string, string>
{
    ["Program.bas"] = File.ReadAllText("Program.bas"),
    ["Database.bas"] = File.ReadAllText("Database.bas"),
    ["Utils.bas"] = File.ReadAllText("Utils.bas"),
    // ... more files
};

// Convert to IR model
var models = files.Select(f => PbToModelConverter.ConvertFile(f.Value, f.Key));

// Merge and analyze
var unified = CodeAnalyzer.Merge(models);
var analyzed = CodeAnalyzer.Analyze(unified);

// Organize into projects
var solution = ProjectOrganizer.OrganizeIntoProjects(analyzed);

// Write to disk
SolutionWriter.WriteSolution(solution, "C:/Output/MySolution");
```

---

## Testing

All tests continue to pass with the new architecture:
```bash
dotnet test
# Passed:    10
# Failed:     0
```

The new IR infrastructure exists alongside the current working converter without disrupting it.

---

## Next Steps

1. **Phase 2 Completion:** Implement `PbToModelConverter` to convert ANTLR AST → IR Model
2. **Test Phase 2:** Ensure IR-based path produces same output as string-based path
3. **Migrate Gradually:** Move features from string-based to IR-based converter
4. **Phase 3:** Add multi-file support
5. **Phase 4:** Add project organization
6. **Phase 5:** Add solution writing

---

## Benefits of This Architecture

| Aspect | Old Approach | New Approach |
|--------|--------------|--------------|
| **Single File** | ✅ Works | ✅ Works (backward compatible) |
| **Multi-File** | ❌ Not possible | ✅ Designed for this |
| **Dependencies** | ❌ No tracking | ✅ Explicit analysis |
| **Projects** | ❌ Single .cs file | ✅ Organized solution |
| **Testability** | ⚠️ Monolithic | ✅ Each stage isolated |
| **Maintainability** | ⚠️ 1100+ line visitor | ✅ Separated concerns |
| **Extensibility** | ⚠️ Hard to add features | ✅ Plug new stages |

---

## Vision

The ultimate goal is to drop a complete PowerBASIC application (dozens of .BAS files) into this converter and get a complete, organized C# solution with:

- Proper project structure (UI, Business Logic, Data, etc.)
- All dependencies resolved
- All cross-file references working
- Ready to build in Visual Studio

We're building toward this vision one phase at a time, keeping the current functionality working throughout.
