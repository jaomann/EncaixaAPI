using EncaixaAPI.Core.Contracts;
using EncaixaAPI.Core.Entities;
using EncaixaAPI.Database;
using Microsoft.EntityFrameworkCore;

namespace EncaixaAPI.Repository
{
    public class BoxRepository : IBoxRepository
    {
        private readonly Context _context;
        public BoxRepository(Context context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Box>> GetAllBoxesAsync() =>
            await _context.Boxes.ToListAsync();

        public async Task<Box?> GetBoxByIdAsync(Guid id) =>
            await _context.Boxes.FirstOrDefaultAsync(b => b.Id == id);
    }
}
