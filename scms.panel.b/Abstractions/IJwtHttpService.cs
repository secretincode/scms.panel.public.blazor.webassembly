namespace scms.panel.b.Abstractions;

public interface IJwtHttpService
{
    Task<T> Get<T>(string uri);
    Task<T> Post<T>(string uri, object? value = null);
}
