using System.Linq.Expressions;
using CommonLibrary.Core;
using CommonLibrary.Logging.Models;
using CommonLibrary.Settings;
using Flurl;
using Flurl.Http;

namespace CommonLibrary.ClientServices.Logging.Implementations;

public class LogHandleRepository : IRepository<LogHandle>
{
    public async Task<IEnumerable<LogHandle>?> GetAllAsync()
    {
        return await ServicesSettings.GatewayServiceDevURL
            .AppendPathSegment("admin/logs")
            .GetJsonAsync<IEnumerable<LogHandle>>();
    }

    public Task<IEnumerable<LogHandle>?> GetAllAsync(
        Expression<Func<LogHandle, bool>> filter)
    {
        throw new NotImplementedException();
    }

    public Task<LogHandle?> GetAsync(
        Guid Id)
    {
        throw new NotImplementedException();
    }

    public Task<LogHandle?> GetAsync(
        Expression<Func<LogHandle, bool>> filter)
    {
        throw new NotImplementedException();
    }

    public Task CreateAsync(
        LogHandle entity)
    {
        throw new NotImplementedException();
    }

    public Task RangeAsync(
        IEnumerable<LogHandle> entity)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(
        LogHandle entity)
    {
        throw new NotImplementedException();
    }

    public Task UpdateOrCreateAsync(
        LogHandle entity)
    {
        throw new NotImplementedException();
    }
}