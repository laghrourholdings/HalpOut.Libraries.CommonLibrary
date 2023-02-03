namespace CommonLibrary.ClientServices.Core;

public interface ICookies
{
    public Task SetValue(string key, string value, int? days = null);
    public Task<string> GetValue(string key, string def = "");
}