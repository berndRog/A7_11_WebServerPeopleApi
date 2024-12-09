using PeopleApi.Core;
using PeopleApi.Core.DomainModel;
using PeopleApi.Core.DomainModel.Entities;
namespace PeopleApi.Persistence.Repositories;

internal class AuthRepository(
   DataContext dataContext
) : AGenericRepository<User>(dataContext), IAuthRepository {
} 
