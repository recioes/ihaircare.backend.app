using Core.Entities;
using Core.Interfaces.Repositories;
using Infrastructure.Factories.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Infrastructure.Repositories
{
    public class TreatmentRepository : ITreatmentRepository
    {
        private readonly IMongoCollection<Treatment> _treatments;

        public TreatmentRepository(IMongoCollectionFactory<Treatment> factory)
        {
            _treatments = factory.GetCollection();
        }

        public async Task AddAsync(Treatment treatment)
        {
            await _treatments.InsertOneAsync(treatment);
        }

        public async Task UpdateAsync(Treatment treatment)
        {
            var filter = Builders<Treatment>.Filter.Eq(t => t.Id, treatment.Id);
            await _treatments.ReplaceOneAsync(filter, treatment);
        }

        public async Task DeleteAsync(ObjectId id)
        {
            var filter = Builders<Treatment>.Filter.Eq(t => t.Id, id);
            await _treatments.DeleteOneAsync(filter);
        }

        public async Task<IEnumerable<Treatment>> GetAllAsync()
        {
            return await _treatments.Find(_ => true).ToListAsync();
        }

        public async Task<Treatment> GetByIdAsync(ObjectId id)
        {
            var filter = Builders<Treatment>.Filter.Eq(t => t.Id, id);
            return await _treatments.Find(filter).FirstOrDefaultAsync();
        }
    }
}
