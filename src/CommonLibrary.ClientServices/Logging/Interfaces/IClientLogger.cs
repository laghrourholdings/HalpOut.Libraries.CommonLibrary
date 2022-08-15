namespace CommonLibrary.ClientServices.Logging.Interfaces;

public interface IClientLogger
{
    void Debug(string message);
    void Warning(string message);
    void Error(string message);
    void Verbose(string message);
    void Information(string message);
}