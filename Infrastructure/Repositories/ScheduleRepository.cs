using Core.Entities;
using Infrastructure.Factories.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Infrastructure.Repositories
{
    public class ScheduleRepository : IScheduleRepository
    {

        private readonly IMongoCollection<Schedule> _schedules;

        public ScheduleRepository(IMongoCollectionFactory<Schedule> collectionFactory)
        {
            _schedules = collectionFactory.GetCollection();
        }

        public async Task AddAsync(Schedule schedule)
        {
            await _schedules.InsertOneAsync(schedule);
        }

        public async Task DeleteAsync(ObjectId id)
        {
            var filter = Builders<Schedule>
                .Filter
                .Eq(s => s.Id, id);

            await _schedules.DeleteOneAsync(filter);
        }

        public async Task<IEnumerable<Schedule>> GetAsync()
        {
            return await _schedules.Find(FilterDefinition<Schedule>.Empty).ToListAsync();
        }

        public async Task<Schedule> GetByIdAsync(ObjectId id)
        {
            var filter = Builders<Schedule>.Filter.Eq(s => s.Id, id);
            return await _schedules.Find(filter).FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(Schedule schedule)
        {
            var filter = Builders<Schedule>.Filter.Eq(s => s.Id, schedule.Id);
            await _schedules.ReplaceOneAsync(filter, schedule);
        }
    }
}
