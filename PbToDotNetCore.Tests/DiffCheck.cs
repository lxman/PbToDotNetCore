using DiffPlex;
using DiffPlex.Chunkers;
using DiffPlex.Model;

namespace PbToDotNetCore.Tests;

public static class DiffCheck
{
    private static readonly IDiffer Differ = new Differ();
    
    public static void Verify(string expected, string actual)
    {
        // Trim trailing whitespace/newlines to ignore differences at end of file
        expected = expected.TrimEnd();
        actual = actual.TrimEnd();

        DiffResult result = Differ.CreateDiffs(expected, actual, true, false, new LineChunker());

        // Check if there are any differences
        bool hasDifferences = result.DiffBlocks.Count > 0;

        if (!hasDifferences) return;
        // Build a detailed error message showing the differences
        var errorMessage = new System.Text.StringBuilder();
        errorMessage.AppendLine("Generated C# code does not match expected output.");
        errorMessage.AppendLine();
        errorMessage.AppendLine("Differences:");

        foreach (DiffBlock? diffBlock in result.DiffBlocks)
        {
            errorMessage.AppendLine($"  Lines {diffBlock.DeleteStartA + 1}-{diffBlock.DeleteStartA + diffBlock.DeleteCountA}:");

            // Show deleted lines (from expected)
            for (var i = 0; i < diffBlock.DeleteCountA; i++)
            {
                int lineIndex = diffBlock.DeleteStartA + i;
                errorMessage.AppendLine($"    - {result.PiecesOld[lineIndex]}");
            }

            // Show inserted lines (from actual)
            for (var i = 0; i < diffBlock.InsertCountB; i++)
            {
                int lineIndex = diffBlock.InsertStartB + i;
                errorMessage.AppendLine($"    + {result.PiecesNew[lineIndex]}");
            }

            errorMessage.AppendLine();
        }

        Assert.Fail(errorMessage.ToString());
    }
}