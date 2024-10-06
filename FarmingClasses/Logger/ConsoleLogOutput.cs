using System.Diagnostics;

namespace FarmingClasses.Logger;
public class ConsoleLogOutput : ILogOutput {
    public void WriteLog(string message) {
        Debug.WriteLine(message);
    }

    public void Dispose() { }
}
