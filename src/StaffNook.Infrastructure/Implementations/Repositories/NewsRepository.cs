using StaffNook.Domain.Entities.News;
using StaffNook.Domain.Interfaces.Repositories;
using StaffNook.Domain.Interfaces.Services.Identity;
using StaffNook.Infrastructure.Persistence;

namespace StaffNook.Infrastructure.Implementations.Repositories;

public class NewsRepository : Repository<NewsEntity>, INewsRepository
{
    public NewsRepository(Context context, ICurrentUserService currentUserService) : base(context, currentUserService)
    {
    }
}