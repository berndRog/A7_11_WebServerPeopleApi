using System;
using AutoMapper;
using BankingApiTest.Di;
using Microsoft.Extensions.DependencyInjection;
using PeopleApi.Core;
using PeopleApi.Di;
using PeopleApi.Persistence;
using Xunit;
namespace BankingApiTest.Persistence.Repositories;
[Collection(nameof(SystemTestCollectionDefinition))]
public abstract class BaseRepositoryUt {
   
   protected readonly IPeopleRepository _peopleRepository;
   protected readonly ImagesRepository _imagesRepository;
   protected readonly IDataContext _dataContext;
   protected readonly IMapper _mapper;
   protected readonly ArrangeTest _arrangeTest;
   protected readonly Seed _seed;

   protected BaseRepositoryUt() {
      
      // Test DI-Container
      IServiceCollection services = new ServiceCollection();
      // Add Core, UseCases, Mapper, ...
      services.AddCore();
      // Add Repositories, Test Databases, ...
      services.AddPersistenceTest();
      // Build ServiceProvider,
      // and use Dependency Injection or Service Locator Pattern
      var serviceProvider = services.BuildServiceProvider()
         ?? throw new Exception("Failed to create an instance of ServiceProvider");

      //-- Service Locator    
      var dbContext = serviceProvider.GetRequiredService<DataContext>()
         ?? throw new Exception("Failed to create CDbContext");
      dbContext.Database.EnsureDeleted();
      dbContext.Database.EnsureCreated();

      _dataContext = serviceProvider.GetRequiredService<IDataContext>()
         ?? throw new Exception("Failed to create an instance of IDataContext");

      _peopleRepository = serviceProvider.GetRequiredService<IPeopleRepository>()
         ?? throw new Exception("Failed create an instance of IPeopleRepository");
      _imagesRepository = serviceProvider.GetRequiredService<ImagesRepository>()
         ?? throw new Exception("Failed create an instance of ImagesRepository");
      
      _mapper = serviceProvider.GetRequiredService<IMapper>()
         ?? throw new Exception("Failed create an instance of IMapper");
      
      _arrangeTest = serviceProvider.GetRequiredService<ArrangeTest>()
         ?? throw new Exception("Failed create an instance of ArrangeTest");

      _seed = new Seed();
   }
}