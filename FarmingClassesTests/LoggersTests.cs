using FarmingClasses.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmingClassesTests;


public class LoggersTests {
    [Fact]
    public void Constructor_ShouldThrow_WhenStreamIsNull() {
        Assert.Throws<ArgumentNullException>(() => new StreamLogOutput(null!, 1024));
    }

    [Fact]
    public void Constructor_ShouldThrow_WhenBufferSizeIsInvalid() {
        MemoryStream memoryStream = new MemoryStream();
        Assert.Throws<ArgumentOutOfRangeException>(() => new StreamLogOutput(memoryStream, -1));
        Assert.Throws<ArgumentOutOfRangeException>(() => new StreamLogOutput(memoryStream, 0));
    }

    [Fact]
    public void Constructor_ShouldThrow_WhenStreamCannotWrite() {
        using var nonWritableStream = new MemoryStream(new byte[0], writable: false);
        Assert.Throws<ArgumentException>(() => new StreamLogOutput(nonWritableStream, 1024));
    }

    [Fact]
    public void WriteLog_ShouldQueueMessage() {
        using MemoryStream memoryStream = new MemoryStream();
        StreamLogOutput logOutput = new StreamLogOutput(memoryStream, 1024);
        logOutput.WriteLog("Test message");

        // Закрываем и завершаем задачу, чтобы вызвать Flush
        logOutput.Dispose();

        // Проверка, что лог был записан
        memoryStream.Seek(0, SeekOrigin.Begin);
        var logContent = Encoding.UTF8.GetString(memoryStream.ToArray());
        Assert.Contains("Test message", logContent);
    }

    [Fact]
    public async Task WriteLog_ShouldFlush_WhenBufferExceedsMaxSize() {
        using MemoryStream memoryStream = new MemoryStream();
        using StreamLogOutput logOutput = new StreamLogOutput(memoryStream, 1024);

        var largeMessage = new string('a', 512); // Сообщение размером 1024 байт
        logOutput.WriteLog(largeMessage);

        // Ожидание, чтобы задача обработки могла выполнить Flush
        await Task.Delay(1100);

        memoryStream.Seek(0, SeekOrigin.Begin);
        var logContent = Encoding.UTF8.GetString(memoryStream.ToArray());
        Assert.Contains(largeMessage, logContent);
    }

    [Fact]
    public void Dispose_ShouldFlushRemainingLogs() {
        using MemoryStream memoryStream = new MemoryStream();
        StreamLogOutput logOutput = new StreamLogOutput(memoryStream, 1024);

        logOutput.WriteLog("Final log");

        logOutput.Dispose(); // При завершении должно произойти Flush

        memoryStream.Seek(0, SeekOrigin.Begin);
        var logContent = Encoding.UTF8.GetString(memoryStream.ToArray());
        Assert.Contains("Final log", logContent);
    }

    [Theory]
    [InlineData(10)]
    [InlineData(100)]
    [InlineData(10000)]
    public void TestMainLogger_LogsShouldEqual(int cnt) {
        Queue<string> ll = new();
        Logger logger = new();
        logger.LogEvent += ll.Enqueue;

        for (int i = 0; i != cnt; i++) { logger.Log($"Добавлена строка с номером {i}"); }
        Assert.Equal(cnt, ll.Count);

        for (int i = 0; i != cnt; i++) { Assert.Contains($"Добавлена строка с номером {i}", ll.Dequeue()); }
    }

    [Fact]
    public void TestMainLogger_ShouldThrow_WhenMessageIsNull() {
        Queue<string> ll = new();
        Logger logger = new();
        logger.LogEvent += ll.Enqueue;

        string nullString = null!;
        Assert.Throws<ArgumentNullException>(() => logger.Log(nullString));
    }
}
