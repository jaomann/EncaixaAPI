using EncaixaAPI.Core.Entities;

namespace EncaixaAPI.Core.Contracts
{
    public interface IBoxRepository
    {
        public Task<IEnumerable<Box>> GetAllBoxesAsync();
        public Task<Box?> GetBoxByIdAsync(Guid id);
    }
}
