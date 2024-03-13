using StaffNook.Domain.Common;
using StaffNook.Domain.Dtos;
using StaffNook.Domain.Dtos.Reference;
using StaffNook.Domain.Entities.Reference;
using StaffNook.Domain.Interfaces.Repositories.Reference;
using StaffNook.Domain.Interfaces.Services;

namespace StaffNook.Infrastructure.Implementations.Services;

public class ReferenceService : IReferenceService
{
    private readonly ISpecialityRepository _specialityRepository;

    public ReferenceService(ISpecialityRepository specialityRepository)
    {
        _specialityRepository = specialityRepository;
    }

    public async Task<AvailableValue[]> GetAllReferencesByType(ReferenceType courseReferenceType)
    {
        switch (courseReferenceType)
        {
            case ReferenceType.Speciality:
                var directions =
                    await _specialityRepository.GetList(x => !x.IsArchived);
                return directions.Select(x => new AvailableValue(x.Id, x.Name)).ToArray();
        }

        return Array.Empty<AvailableValue>();
    }

    public async Task<AvailableValue> AddReference(ReferenceType courseReferenceType,
        CreateReferenceDto createReferenceDto)
    {
        switch (courseReferenceType)
        {
            case ReferenceType.Speciality:
                var insertData = await _specialityRepository.Insert(new SpecialityEntity
                {
                    Name = createReferenceDto.Name
                });
                return new AvailableValue(insertData.Id, insertData.Name);

            default:
                throw new ArgumentOutOfRangeException(courseReferenceType.ToString());
        }
    }

    public async Task<AvailableValue> UpdateReference(ReferenceType courseReferenceType, Guid id,
        UpdateReferenceDto updateReferenceDto)
    {
        switch (courseReferenceType)
        {
            case ReferenceType.Speciality:
                var updatedDirectionData = await _specialityRepository.Update(new SpecialityEntity
                {
                    Id = id,
                    Name = updateReferenceDto.Name
                });

                return new AvailableValue(updatedDirectionData.Id, updatedDirectionData.Name);

            default:
                throw new ArgumentOutOfRangeException(courseReferenceType.ToString());
        }
    }
}