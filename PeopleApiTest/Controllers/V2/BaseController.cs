using System;
using AutoMapper;
using PeopleApi.Controllers.V2;
using PeopleApi.Core;
using PeopleApi.Persistence;
using BankingApiTest.Di;
using BankingApiTest.Di.V2;
using BankingApiTest.Persistence;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
namespace BankingApiTest.Controllers.V2;
[Collection(nameof(SystemTestCollectionDefinition))]
public class BaseControllerTest {

   protected readonly PeopleController _peopleController;
   protected readonly ImagesController _imagesController;
   protected readonly IPeopleRepository _peopleRepository;
   protected readonly ImagesRepository _imagesRepository;
   protected readonly IDataContext _dataContext;
   protected readonly ArrangeTest _arrangeTest;
   protected readonly IMapper _mapper;
   protected readonly Seed _seed;

   protected BaseControllerTest() {
      IServiceCollection serviceCollection = new ServiceCollection();
      serviceCollection.AddPersistenceTest();
      serviceCollection.AddControllersTest();
      var serviceProvider = serviceCollection.BuildServiceProvider()
         ?? throw new Exception("Failed to build Serviceprovider");

      var dbContext = serviceProvider.GetRequiredService<DataContext>()
         ?? throw new Exception("Failed to create an instance of DataContext");
      dbContext.Database.EnsureDeleted();
      dbContext.Database.EnsureCreated();
      
      _peopleController = serviceProvider.GetRequiredService<PeopleController>()
         ?? throw new Exception("Failed to create an instance of PeopleController");
      _imagesController = serviceProvider.GetRequiredService<ImagesController>()
         ?? throw new Exception("Failed to create an instance of ImagesController");
         
      _peopleRepository = serviceProvider.GetRequiredService<IPeopleRepository>()
         ?? throw new Exception("Failed to create an instance of IOwnersRepository");
      _imagesRepository = serviceProvider.GetRequiredService<IAccountsRepository>()
         ?? throw new Exception("Failed to create an instance of IAccountsRepository");
       _dataContext = serviceProvider.GetRequiredService<IDataContext>() 
         ?? throw new Exception("Failed to create an instance of IDataContext");
      _arrangeTest = serviceProvider.GetRequiredService<ArrangeTest>()
         ?? throw new Exception("Failed create an instance of ArrangeTest");
      _mapper = serviceProvider.GetRequiredService<IMapper>();
      _seed = new Seed();
   }
}