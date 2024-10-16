using Core.Entities;
using MongoDB.Bson;

namespace Core.Interfaces.Repositories
{
    public interface ITreatmentRepository
    {
        Task AddAsync(Treatment treatment);
        Task UpdateAsync(Treatment treatment);
        Task DeleteAsync(ObjectId id);
        Task<IEnumerable<Treatment>> GetAllAsync();
        Task<Treatment> GetByIdAsync(ObjectId id);
    }
}
