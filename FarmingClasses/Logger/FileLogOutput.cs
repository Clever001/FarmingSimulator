﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FarmingClasses.Logger;

public sealed class FileLogOutput : ILogOutput, IDisposable {
    private readonly int _MAX_BUFFER_SIZE;
    private readonly string _fileName;
    private readonly Queue<string> _logQueue;
    private readonly Task _logTask;
    private readonly CancellationTokenSource _cancellationTokenSource;
    private int _curSize = 0;
    private readonly object _sizeLock = new();

    public FileLogOutput(string fileName, int maxBufferSize) {
        ArgumentException.ThrowIfNullOrEmpty(fileName, nameof(fileName));
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(maxBufferSize, 0, nameof(maxBufferSize));

        File.WriteAllText(fileName, string.Empty);
        _fileName = fileName;
        _MAX_BUFFER_SIZE = maxBufferSize;
        _logQueue = new Queue<string>();
        _cancellationTokenSource = new CancellationTokenSource();

        _logTask = Task.Run(() => ProcessLogsAsync(_cancellationTokenSource.Token));
    }

    public void WriteLog(string message) {
        int sizeOfMessage = sizeof(char) * message.Length;
        
        lock (_sizeLock) {
            _curSize += sizeOfMessage;
            _logQueue.Enqueue(message);
        }
    }

    private async Task ProcessLogsAsync(CancellationToken cancellationToken) {
        StringBuilder logBuilder = new();
        while (!cancellationToken.IsCancellationRequested) {
            lock (_sizeLock) {
                if (_curSize > _MAX_BUFFER_SIZE) {
                    while (_logQueue.TryDequeue(out string? logMessage)) {
                        if (logMessage is not null) logBuilder.AppendLine(logMessage);
                    }
                    _curSize = 0;
                }
            }
            if (logBuilder.Length > 0) {
                File.AppendAllText(_fileName, logBuilder.ToString());
                logBuilder.Clear();
            }
            await Task.Delay(1000);
        }
    }

    private void Flush() {
        if (_curSize > 0) {
            StringBuilder logBuilder = new();
            while (_logQueue.TryDequeue(out string? logMessage)) {
                if (logMessage is not null) logBuilder.AppendLine(logMessage);
            }

            _curSize = 0;
            File.AppendAllText(_fileName, logBuilder.ToString());
        }
    }

    public void Dispose() {
        _cancellationTokenSource.Cancel();
        _logTask.Wait();
        lock (_sizeLock) {
            Flush();
        }
    }
}
