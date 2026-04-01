using Castle.Core.Resource;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Moq;
using P3AddNewFunctionalityDotNetCore.Data;
using P3AddNewFunctionalityDotNetCore.Models;
using P3AddNewFunctionalityDotNetCore.Models.Entities;
using P3AddNewFunctionalityDotNetCore.Models.Repositories;
using P3AddNewFunctionalityDotNetCore.Models.Services;
using P3AddNewFunctionalityDotNetCore.Models.ViewModels;
using P3AddNewFunctionalityDotNetCore.Resources.Models.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Drawing.Text;
using System.Globalization;
using System.IO;
using System.Reflection;
using Xunit;



namespace P3AddNewFunctionalityDotNetCore.Tests
{
    

    public class DatabaseFixture : IDisposable
    {

        public P3Referential Context { get; private set; }

        public DatabaseFixture()
        {
             

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var connectionString = configuration.GetConnectionString("P3ReferentialForTests");


            var options = new DbContextOptionsBuilder<P3Referential>()
            .UseSqlServer(connectionString, providerOptions => providerOptions.EnableRetryOnFailure())
            .Options;


            /// <summary>
            /// Preparing options to match DbContextOptions type
            DbContextOptions <P3Referential> optionsForInjection = options;
            /// </summary >
            /// 
            /// 
            /// <summary>
            /// Instanciate the new context using data injection
            Context = new P3Referential(optionsForInjection, configuration);
            /// </summary >

        }

        public void Dispose()
        {
            Context.Dispose();
        }

        public class DatabaseTests : IClassFixture<DatabaseFixture>
        {
            private readonly P3Referential _context;


            public DatabaseTests(DatabaseFixture fixture)
            {
                _context = fixture.Context;
            }


        }


        [Fact]
        public void Test1()
        {
            // Arrange
            ICart cart = new Cart();
            IProductRepository productRepository = new ProductRepository(Context);
            IOrderRepository orderRepository = new OrderRepository(Context);

            var service = new ServiceCollection();
            service.AddLogging();
            service.AddLocalization(options => options.ResourcesPath = "P3AddNewFunctionalityDotNetCore.Resources.Models.Services.ProductServiceResources");
            var serviceProvider = service.BuildServiceProvider();

            var localizer = serviceProvider.GetService<IStringLocalizer<P3AddNewFunctionalityDotNetCore.Resources.Models.Services.ProductServiceResources>>();

            IProductService productService = new P3AddNewFunctionalityDotNetCore.Models.Services.ProductService(cart, productRepository, orderRepository, localizer);

            ProductViewModel productViewModel = new ProductViewModel
            {
                Id = 10,
                Name = "product.Name1",
                Price = "20",
                Stock = "10",
                Description = "product1",
                Details = "test"
            };

            var accessProductMapper = typeof(ProductService).GetField("MapToProductEntity", BindingFlags.NonPublic | BindingFlags.Static);

            //var Product = new Product();

            //var Mapper = new ProductViewModel();
                
            //var productToSaveInDatabase = accessProductMapper.GetValue(productViewModel);

            //ProductViewModel ProductTest = new ProductViewModel
            //{
            //    Id = productEntity.Id,
            //    Name = productEntity.Name,
            //    Price = productEntity.Price.ToString(CultureInfo.InvariantCulture),
            //    Stock = productEntity.Quantity.ToString(),
            //    Description = productEntity.Description,
            //    Details = productEntity.Details
            //};



            // Act
            productService.SaveProduct(productViewModel);

            //Assert
            Assert.True(productViewModel.Id == 10);
            //Assert.True(1 > 2);
        }
    }

 }


























//{


//    [TestFixture]
//    public class SampleTests
//    {

//        [Test]
//        public void TestIsPositive()
//        {
//            int number = 1;
//            int number2 = 2;
//            //Assert.IsTrue(number < number2);
//            Assert.That(number, Is.EqualTo(number2),$"Expected {number} to be 42");
//        }
//    }







//    //[TestFixture]
//    //public class ProductServiceTests
//    //{
//    //    /// <summary>
//    //    /// Take this test method as a template to write your test method.
//    //    /// A test method must check if a definite method does its job:
//    //    /// returns an expected value from a particular set of parameters
//    //    /// </summary>
//    //    /// 

//    //    private static IConfiguration _configuration;
//    //    private static P3Referential _context;

//    //    [OneTimeSetUp]

//    //    /// <summary>
//    //    /// In order to make tests, we can use a complete new test sql database. 
//    //    /// We also may use mocked objects in case we don't create any new sql database dedicated (test dedicated).
//    //    /// In this situation we create a new 
//    //    /// </summary>

//    //    public void ContextBuild()
//    //    {
//    //        /// <summary>
//    //        /// In order to make tests, we can use a complete new test sql database. 
//    //        /// We also may use mocked objects in case we don't create any new sql database dedicated (test dedicated).
//    //        /// In this situation we create a new  
//    //        var configuration = new ConfigurationBuilder()
//    //            .SetBasePath(Directory.GetCurrentDirectory())
//    //            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
//    //            .Build();
//    //        /// </summary>

//    //        /// <summary> Retrieve the corect sqldatabase connectionstring coming from appsettings.json
//    //        string connectionString = configuration.GetConnectionString("P3ReferentialForTests");
//    //        /// </summary>

//    //        /// <summary>
//    //        /// Configure connection options like the connectionstring
//    //        var options = new DbContextOptionsBuilder<P3Referential>()
//    //            .UseSqlServer(connectionString, providerOptions => providerOptions.EnableRetryOnFailure());
//    //        /// </summary >
//    //        /// 

//    //        /// <summary>
//    //        /// Preparing options to match DbContextOptions type
//    //        DbContextOptions<P3Referential> optionsForInjection = options.Options;
//    //        /// </summary >
//    //        /// 
//    //        /// 
//    //        /// <summary>
//    //        /// Instanciate the new context using data injection
//    //        _context = new P3Referential(optionsForInjection,_configuration);
//    //        /// </summary >
//    //    }


//    //    [TestCase(1)]
//    //    public void Test1()
//    //    {
//    //        // Arrange
//    //        ICart cart = new Cart();
//    //        IProductRepository productRepository = new ProductRepository(_context);
//    //        IOrderRepository orderRepository = new OrderRepository(_context);

//    //        var service = new ServiceCollection();
//    //        service.AddLocalization(options => options.ResourcesPath = "P3AddNewFunctionalityDotNetCore.Resources.Models.Services.ProductServiceResources");
//    //        var serviceProvider = service.BuildServiceProvider();

//    //        var localizer = serviceProvider.GetService<IStringLocalizer<P3AddNewFunctionalityDotNetCore.Resources.Models.Services.ProductServiceResources>>();

//    //        IProductService productService = new P3AddNewFunctionalityDotNetCore.Models.Services.ProductService(cart, productRepository, orderRepository,localizer);

//    //        Product productEntity = new Product
//    //        {
//    //            Id = 10,
//    //            Name = "product.Name1",
//    //            Price = 20,
//    //            Quantity = 10,
//    //            Description = "product1",
//    //            Details = "test"
//    //        };

//    //        ProductViewModel ProductTest = new ProductViewModel
//    //        {
//    //            Id = productEntity.Id,
//    //            Name = productEntity.Name,
//    //            Price = productEntity.Price.ToString(CultureInfo.InvariantCulture),
//    //            Stock = productEntity.Quantity.ToString(),
//    //            Description = productEntity.Description,
//    //            Details = productEntity.Details
//    //        };



//    //        // Act
//    //        productService.SaveProduct(ProductTest);

//    //        // Assert
//    //        Assert.Equals(ProductTest.Id, 10);
//    //    }

//    //    // TODO write test methods to ensure a correct coverage of all possibilities
//    //}
//}