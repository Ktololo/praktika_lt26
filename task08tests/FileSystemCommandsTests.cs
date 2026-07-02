using System;
using System.IO;
using Xunit;

public class FileSystemCommandsTests
{
    [Fact]
    public void DirectorySizeCommand_ShouldCalculateSize()
    {
        var testDir = Path.Combine(Path.GetTempPath(), "TestDir");
        Directory.CreateDirectory(testDir);
        File.WriteAllText(Path.Combine(testDir, "test1.txt"), "Hello");
        File.WriteAllText(Path.Combine(testDir, "test2.txt"), "World");

        var command = new DirectorySizeCommand(testDir);
        var exception = Record.Exception(() => command.Execute());
        Assert.Null(exception);

        Directory.Delete(testDir, true);
    }
    [Fact]
    public void DirectorySizeCommand_WithNonExistentDirectory_ShouldNotThrow()
    {
        var command = new DirectorySizeCommand("C:\\NonExistentDir123456");
        var exception = Record.Exception(() => command.Execute());
        Assert.Null(exception);
    }
    [Fact]
    public void FindFilesCommand_ShouldFindMatchingFiles()
    {
        var testDir = Path.Combine(Path.GetTempPath(), "TestDir");
        Directory.CreateDirectory(testDir);
        File.WriteAllText(Path.Combine(testDir, "file1.txt"), "Text");
        File.WriteAllText(Path.Combine(testDir, "file2.log"), "Log");
        var command = new FindFilesCommand(testDir, "*.txt");
        var exception = Record.Exception(() => command.Execute());
        Assert.Null(exception);
        Directory.Delete(testDir, true);
    }
    [Fact]
    public void FindFilesCommand_WithNoMatchingFiles_ShouldNotThrow()
    {
        var testDir = Path.Combine(Path.GetTempPath(), "TestDir");
        Directory.CreateDirectory(testDir);
        File.WriteAllText(Path.Combine(testDir, "file1.txt"), "Text");
        var command = new FindFilesCommand(testDir, "*.log");
        var exception = Record.Exception(() => command.Execute());
        Assert.Null(exception);
        Directory.Delete(testDir, true);
    }
}