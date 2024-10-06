using System;

namespace FarmingClasses.Logger;
public interface ILogOutput : IDisposable {
    public void WriteLog(string message);
}
