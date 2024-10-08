using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace FarmingClasses.Logger {
    public class FileLogOutput : ILogOutput, IDisposable {
        private readonly int _MAX_BUFFER_SIZE;
        private readonly string _fileName;
        private readonly ConcurrentQueue<string> _logQueue;  // Очередь для логов
        private readonly Task _logTask;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private int _curSize = 0;

        public FileLogOutput(string fileName, int maxBufferSize) {
            ArgumentException.ThrowIfNullOrEmpty(fileName, nameof(fileName));
            ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(maxBufferSize, 0, nameof(maxBufferSize));

            File.WriteAllText(fileName, string.Empty);  // Очистка файла при создании
            _fileName = fileName;
            _MAX_BUFFER_SIZE = maxBufferSize;
            _logQueue = new ConcurrentQueue<string>();  // ConcurrentQueue для потокобезопасности
            _cancellationTokenSource = new CancellationTokenSource();

            // Запуск фоновой задачи для обработки логов
            _logTask = Task.Run(() => ProcessLogsAsync(_cancellationTokenSource.Token));
        }

        public void WriteLog(string message) {
            int sizeOfMessage = sizeof(char) * message.Length;
            if (sizeOfMessage > _MAX_BUFFER_SIZE) {
                throw new Exception("Недостаточный размер буфера.");
            }

            // Добавляем сообщение в очередь
            _logQueue.Enqueue(message);
        }

        private async Task ProcessLogsAsync(CancellationToken cancellationToken) {
            while (!cancellationToken.IsCancellationRequested) {
                // Проверяем, есть ли что-то в очереди
                if (_logQueue.TryDequeue(out var logMessage)) {
                    int sizeOfMessage = sizeof(char) * logMessage.Length;

                    // Если буфер заполнен, записываем данные в файл
                    if (_curSize + sizeOfMessage > _MAX_BUFFER_SIZE) {
                        Flush();
                    }

                    // Добавляем размер сообщения в общий размер буфера
                    _curSize += sizeOfMessage;

                    // Записываем сообщение в файл
                    File.AppendAllText(_fileName, logMessage + Environment.NewLine);
                }
                else {
                    // Ждём перед повторной проверкой
                    await Task.Delay(2000);
                }
            }
        }

        // Записываем оставшиеся данные из буфера в файл
        public void Flush() {
            if (_curSize > 0) {
                _curSize = 0;  // Сбрасываем размер буфера, так как лог сразу пишется в файл
            }
        }

        // Останавливаем задачу логирования и записываем оставшиеся логи
        public void Dispose() {
            _cancellationTokenSource.Cancel();
            _logTask.Wait();  // Ожидаем завершения фонового потока
            Flush();  // Записываем всё, что осталось
        }
    }
}
