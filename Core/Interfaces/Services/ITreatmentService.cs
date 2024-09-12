using Core.DTOs;
using Core.Entities;
using MongoDB.Bson;

namespace Core.Interfaces.Services
{
    public interface ITreatmentService
    {
            Task AddTreatmentAsync(TreatmentDto treatmentDto);
            Task UpdateTreatmentAsync(ObjectId treatmentId, TreatmentDto treatmentDto);
            Task DeleteTreatmentAsync(ObjectId id);
            Task<IEnumerable<Treatment>> GetAllTreatmentsAsync();
            Task<Treatment> GetTreatmentByIdAsync(ObjectId id);
    }
}
