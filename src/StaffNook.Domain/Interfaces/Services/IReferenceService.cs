using StaffNook.Domain.Common;
using StaffNook.Domain.Dtos;
using StaffNook.Domain.Dtos.Reference;

namespace StaffNook.Domain.Interfaces.Services;

public interface IReferenceService
{
    Task<AvailableValue[]> GetAllReferencesByType(ReferenceType courseReferenceType);

    Task<AvailableValue> AddReference(ReferenceType courseReferenceType, CreateReferenceDto createReferenceDto);
        
    Task<AvailableValue> UpdateReference(ReferenceType courseReferenceType, Guid id,
        UpdateReferenceDto updateReferenceDto);
}