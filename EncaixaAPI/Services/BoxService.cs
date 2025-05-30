using EncaixaAPI.Core.Contracts;
using EncaixaAPI.Core.Entities;

namespace EncaixaAPI.Services
{
    public class BoxService : IBoxService
    {
        private readonly IBoxRepository _repository;
        public BoxService(IBoxRepository repository)
        {
            _repository = repository;
        }
        public async Task<IEnumerable<Box>> GetAllBoxesAsync() => await _repository.GetAllBoxesAsync();
        public Task<Box?> GetBoxByIdAsync(Guid id) => _repository.GetBoxByIdAsync(id);
    }
}
