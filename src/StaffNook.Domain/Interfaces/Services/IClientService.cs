using StaffNook.Domain.Dtos;
using StaffNook.Domain.Dtos.Client;
using StaffNook.Domain.Filters;

namespace StaffNook.Domain.Interfaces.Services;

public interface IClientService
{
    Task Create(CreateClientDto createClientDto, CancellationToken cancellationToken = default);
    Task Update(Guid id, UpdateClientDto updateClientDto, CancellationToken cancellationToken = default);
    Task<ClientInfoDto> GetById(Guid id, CancellationToken cancellationToken = default);
    Task Delete(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<ClientInfoDto>> GetAll(CancellationToken cancellationToken = default);
    Task<AvailableValue[]> GetAvailableValues();
    Task<PaginationResult<ClientInfoDto>> GetByPageFilter(ClientPageFilter pageFilter = default, CancellationToken cancellationToken = default);
}