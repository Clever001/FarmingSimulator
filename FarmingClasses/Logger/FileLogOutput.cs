using System;
using System.Collections.Generic;
using System.IO;

namespace FarmingClasses.Logger;
public class FileLogOutput : ILogOutput {
    private readonly int _MAX_BUFFER_SIZE;
    private string _fileName;
    private LinkedList<string> _buffer = new();
    private int _curSize = 0;

    public FileLogOutput(string fileName, int maxBufferSize) {
        ArgumentException.ThrowIfNullOrEmpty(fileName, nameof(fileName));
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(maxBufferSize, 0, nameof(maxBufferSize));

        File.WriteAllText(fileName, string.Empty);
        _fileName = fileName;
        _MAX_BUFFER_SIZE = maxBufferSize;
    }

    public void WriteLog(string message) {
        int sizeOfMessage = sizeof(char) * message.Length;
        if (sizeOfMessage > _MAX_BUFFER_SIZE) throw new Exception("Недостаточный размер буфера.");

        if (_curSize + sizeOfMessage > _MAX_BUFFER_SIZE) {
            Flush();
        }

        _buffer.AddLast(message);
        _curSize += sizeOfMessage;
    }

    public void Flush() {
        if (_curSize > 0) {
            File.AppendAllLines(_fileName, _buffer);
            _buffer.Clear();
            _curSize = 0;
        }
    }

    public void Dispose() => Flush();
}