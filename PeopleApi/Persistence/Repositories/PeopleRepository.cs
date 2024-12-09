using PeopleApi.Core;
using PeopleApi.Core.DomainModel.Entities;
namespace PeopleApi.Persistence.Repositories;

internal class PeopleRepository(
   DataContext dataContext
) : AGenericRepository<Person>(dataContext), IPeopleRepository {
   
}