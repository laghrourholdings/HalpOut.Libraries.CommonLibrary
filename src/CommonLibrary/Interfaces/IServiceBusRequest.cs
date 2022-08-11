using System.Net;

namespace CommonLibrary.Interfaces;

public interface IServiceBusRequest<TSubject> : IServiceBusMessage
{
    public TSubject? Subject { get; set; }
    public IEnumerable<TSubject>? Subjects { get; set; }
}