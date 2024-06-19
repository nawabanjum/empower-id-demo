using EmpowerID.Domain.Repositories;

namespace EmpowerID.Infrastructure.Repositories
{
    internal sealed class UnitOfWork : IUnitOfWork
    {
        private readonly EmpowerIdDbContext _context;

        public UnitOfWork(EmpowerIdDbContext context) => _context = context;

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
