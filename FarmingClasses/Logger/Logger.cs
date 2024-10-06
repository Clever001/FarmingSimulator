using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmingClasses.Logger;
public class Logger {
    public event Action<string>? LogEvent;

    public void Log<T>(T message) {
        ArgumentNullException.ThrowIfNull(message, nameof(message));
#pragma warning disable CS8604 
        LogEvent?.Invoke($"{DateTime.Now}: {message}\n");
#pragma warning restore CS8604
    }
}
