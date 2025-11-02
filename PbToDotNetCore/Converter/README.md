# Converter Architecture

This folder contains the next-generation converter architecture designed to support multi-file PowerBASIC projects.

## Current State (Phase 1)

**Active:**
- `PbToCsConverter` - Facade for single-file conversion
- `PbToCSharpConverter` - ANTLR visitor that generates C# strings directly

**In Development:**
- `PbToModelConverter` - Will convert AST → IR Model (not yet implemented)

## Future Architecture (Phases 2-5)

### Phase 2: IR Model Foundation
```
PowerBASIC AST → IR Model (CodeModel) → C# Code
```

**Goal:** Decouple parsing from code generation

**Status:** IR Model classes created in `/Models`, `CSharpGenerator` created in `/Generator`

### Phase 3: Multi-File Awareness
```
Multiple PB Files → Merged CodeModel → Project Structure Analysis
```

**Goal:** Understand cross-file dependencies, identify shared code

**Components to Build:**
- `CodeAnalyzer` - Analyzes dependencies and relationships
- Cross-reference resolver
- Namespace mapper

### Phase 4: Project Organization
```
CodeModel → ProjectStructure → Solution
```

**Goal:** Organize classes into appropriate projects (UI, Business Logic, etc.)

**Components to Build:**
- `ProjectOrganizer` - Groups related classes
- `Solution` model - Represents .sln + .csproj files
- Heuristics for project separation

### Phase 5: Solution Generation
```
Solution → File System (.sln, .csproj, .cs files)
```

**Goal:** Write complete Visual Studio solution

**Components to Build:**
- `SolutionWriter` - Creates directory structure
- MSBuild project file generator
- Dependency management

## API Evolution

### Current API (Works Today)
```csharp
string csCode = PbToCsConverter.GenerateCsCode(pbSourceCode);
```

### Future API (Target State)
```csharp
// Single file (Phase 2)
var model = PbToModelConverter.ConvertFile(pbCode);
string csCode = CSharpGenerator.Generate(model);

// Multi-file project (Phase 3-5)
var files = new Dictionary<string, string>
{
    ["Program.bas"] = File.ReadAllText("Program.bas"),
    ["Utils.bas"] = File.ReadAllText("Utils.bas"),
    // ...
};

var model = PbToModelConverter.ConvertProject(files);
var structure = ProjectOrganizer.Organize(model);
var solution = SolutionGenerator.Generate(structure);
SolutionWriter.Write(solution, outputPath);
```

## Migration Strategy

1. ✅ **Phase 1 Complete** - Current string-based converter works
2. 🔄 **Phase 2 In Progress** - IR models defined, Generator created
3. ⏳ **Phase 2 Next** - Implement PbToModelConverter (AST → IR)
4. ⏳ **Phase 3** - Add multi-file support
5. ⏳ **Phase 4** - Add project organization
6. ⏳ **Phase 5** - Add solution writing

## Why This Architecture?

| Requirement | Old Approach | New Approach |
|-------------|--------------|--------------|
| Single file | ✅ Works | ✅ Works (via facade) |
| Multi-file project | ❌ Not possible | ✅ Designed for this |
| Cross-file references | ❌ No tracking | ✅ Explicit analysis |
| Project structure | ❌ Not considered | ✅ Core feature |
| Testability | ⚠️ Monolithic | ✅ Each stage isolated |
| Maintainability | ⚠️ 1100+ line visitor | ✅ Separated concerns |

## Files in This Folder

- `PbToModelConverter.cs` - Future AST → IR converter (placeholder)
- `README.md` - This file

## Related Folders

- `/Models` - IR model classes (ClassDeclaration, FunctionDeclaration, etc.)
- `/Generator` - CSharpGenerator (IR → C# strings)
- Root - Legacy PbToCSharpConverter.cs (current working converter)
