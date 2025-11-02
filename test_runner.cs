using PbToDotNetCore;

string testCode = """
FUNCTION PBMAIN() AS LONG
    LOCAL hThread AS DWORD
    THREAD CREATE WorkerThread(0) TO hThread
END FUNCTION
""";

string result = PbToCsConverter.GenerateCsCode(testCode);
Console.WriteLine(result);
