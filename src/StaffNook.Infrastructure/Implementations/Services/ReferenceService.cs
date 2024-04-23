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
    private readonly IProjectTypeRepository _projectTypeRepository;

    public ReferenceService(
        ISpecialityRepository specialityRepository, 
        IProjectTypeRepository projectTypeRepository)
    {
        _specialityRepository = specialityRepository;
        _projectTypeRepository = projectTypeRepository;
    }

    public async Task<AvailableValue[]> GetAllReferencesByType(ReferenceType courseReferenceType)
    {
        switch (courseReferenceType)
        {
            case ReferenceType.Speciality:
                var specialityEntities =
                    await _specialityRepository.GetList(x => !x.IsArchived);
                return specialityEntities.Select(x => new AvailableValue(x.Id, x.Name)).ToArray();
            
            case ReferenceType.ProjectType:
                var typeEntities =
                    await _projectTypeRepository.GetList(x => !x.IsArchived);
                return typeEntities.Select(x => new AvailableValue(x.Id, x.Name)).ToArray();
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
            
            case ReferenceType.ProjectType:
                var projectTypeEntity = await _projectTypeRepository.Insert(new ProjectTypeEntity
                {
                    Name = createReferenceDto.Name
                });
                return new AvailableValue(projectTypeEntity.Id, projectTypeEntity.Name);

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
            
            case ReferenceType.ProjectType:
                var updatedType = await _projectTypeRepository.Update(new ProjectTypeEntity
                {
                    Id = id,
                    Name = updateReferenceDto.Name
                });
                return new AvailableValue(updatedType.Id, updatedType.Name);

            default:
                throw new ArgumentOutOfRangeException(courseReferenceType.ToString());
        }
    }
}