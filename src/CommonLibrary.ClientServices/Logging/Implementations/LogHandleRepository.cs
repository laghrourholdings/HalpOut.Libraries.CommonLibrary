using System.Linq.Expressions;
using CommonLibrary.ClientServices.Core;
using CommonLibrary.Core;
using CommonLibrary.Logging.Models.Dtos;
using Flurl;
using Flurl.Http;

namespace CommonLibrary.ClientServices.Logging.Implementations;

public class LogHandleRepository : IRepository<LogHandleDto>
{
    public async Task<List<LogHandleDto>?> GetAllAsync()
    {
        return await Api.LogService
            .AppendPathSegment("logs/handles")
            .GetJsonAsync<List<LogHandleDto>>();
    }

    public Task<List<LogHandleDto>?> GetAllAsync(
        Expression<Func<LogHandleDto, bool>> filter)
    {
        throw new NotImplementedException();
    }

    public Task<LogHandleDto?> GetAsync(
        Guid Id)
    {
        throw new NotImplementedException();
    }

    public Task<LogHandleDto?> GetAsync(
        Expression<Func<LogHandleDto, bool>> filter)
    {
        throw new NotImplementedException();
    }

    public Task CreateAsync(
        LogHandleDto entity)
    {
        throw new NotImplementedException();
    }

    public Task RangeAsync(
        IEnumerable<LogHandleDto> entity)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(
        LogHandleDto entity)
    {
        throw new NotImplementedException();
    }

    public Task UpdateOrCreateAsync(
        LogHandleDto entity)
    {
        throw new NotImplementedException();
    }
}