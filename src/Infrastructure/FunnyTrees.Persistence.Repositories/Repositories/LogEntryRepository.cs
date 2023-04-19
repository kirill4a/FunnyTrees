using FunnyTrees.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using DbLogEntry = FunnyTrees.Persistence.Entities.Models.LogEntry;
using DomainLogEntry = FunnyTrees.Domain.Aggregates.LogEntry;

namespace FunnyTrees.Persistence.Repositories;
internal class LogEntryRepository : RepositoryBase<DbLogEntry>, ILogEntryRepository
{
    public LogEntryRepository(DbSet<DbLogEntry> set, IUnitOfWork unitOfWork) : base(set, unitOfWork)
    {
    }

    public async Task<int> CreateAsync(DomainLogEntry entity, CancellationToken cancellation)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        var dbEntity = new DbLogEntry
        {
            EventId = entity.EventId,
            Timestamp = entity.Timestamp,
            QueryParams = (entity.QueryParams?.Any() ?? false) ? string.Join('?', entity.QueryParams) : null,
            BodyParams = entity.BodyParam,
            StackTrace = entity.StackTrace,
            Message = entity.Message
        };
        Set.Add(dbEntity);

        await _unitOfWork.CommitAsync(cancellation).ConfigureAwait(false);
        return dbEntity.Id;
    }

    public Task<DomainLogEntry?> GetAsync(int entityId, CancellationToken cancellation)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(DomainLogEntry entity, CancellationToken cancellation)
    {
        throw new NotImplementedException();
    }
}