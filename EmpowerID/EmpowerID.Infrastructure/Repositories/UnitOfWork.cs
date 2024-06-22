using EmpowerID.Domain.Repositories;

namespace EmpowerID.Infrastructure.Repositories
{
    internal sealed class UnitOfWork(EmpowerIdDbContext context) : IUnitOfWork
    {
        private readonly EmpowerIdDbContext _context = context;

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
