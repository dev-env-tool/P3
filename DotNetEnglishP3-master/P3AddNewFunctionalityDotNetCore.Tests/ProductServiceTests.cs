using Castle.Core.Resource;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Moq;
using P3AddNewFunctionalityDotNetCore.Controllers;
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
using System.Linq;
using System.Reflection;
using Xunit;
//using System.Security.Cryptography.X509Certificates;


namespace P3AddNewFunctionalityDotNetCore.Tests
{
    

    public class DatabaseFixture : IDisposable
    {

        public P3Referential Context { get; private set; }

        public DatabaseFixture()
        {
            /// <summary>
            /// Build a new configuration path and name of the sql connection string folder.
            /// </summary >
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            /// <summary>
            /// Choose the database.
            /// </summary >
            var connectionString = configuration.GetConnectionString("P3ReferentialForTests");

            /// <summary>
            /// Allows for multiple sql connection attempts.
            /// </summary >
            var options = new DbContextOptionsBuilder<P3Referential>()
            .UseSqlServer(connectionString, providerOptions => providerOptions.EnableRetryOnFailure())
            .Options;


            /// <summary>
            /// Preparing options to match DbContextOptions type.
            /// </summary >
            DbContextOptions<P3Referential> optionsForInjection = options;

            /// <summary>
            /// Instanciate the new context using data injection.
            /// </summary >
            Context = new P3Referential(optionsForInjection, configuration);
            

        }

        public void Dispose()
        {
            /// <summary>
            /// Opens the database for use.
            /// </summary >
            Context.Dispose();
        }


        public class DatabaseTests : IClassFixture<DatabaseFixture>
        {
            private readonly P3Referential _context;

            /// <summary>
            /// Polymorphism : P3ReferentialForTests _context is derivated in each test (below) using the public _context.
            /// </summary >
            public DatabaseTests(DatabaseFixture fixture)
            {
                _context = fixture.Context;
            }
        }

        [Fact]
        public void Test1()
        {
            // Arrange
            /// <summary>
            /// Creating new services linked with Sql Context.
            /// </summary >
            ICart cart = new Cart();
            IProductRepository productRepository = new ProductRepository(Context);
            IOrderRepository orderRepository = new OrderRepository(Context);
            ILanguageService languageService = new LanguageService();
            

            /// <summary>
            /// Creating new services to simulate localizer.
            /// </summary >
            var service = new ServiceCollection();
            service.AddLogging();
            service.AddLocalization(options => options.ResourcesPath = "P3AddNewFunctionalityDotNetCore.Resources.Models.Services.ProductServiceResources");
            var serviceProvider = service.BuildServiceProvider();

            var localizer = serviceProvider.GetService<IStringLocalizer<P3AddNewFunctionalityDotNetCore.Resources.Models.Services.ProductServiceResources>>();


            /// <summary>
            /// Creating a complete ÎproductService.
            /// </summary >
            IProductService productService = new P3AddNewFunctionalityDotNetCore.Models.Services.ProductService(cart, productRepository, orderRepository, localizer);




            // Arrange
            /// <summary>
            /// Simulate user product creation by filling fields.
            /// </summary >
            ProductViewModel productViewModel = new ProductViewModel
            {
                Name = "",
                Price = "1",
                Stock = "1",
                Description = "DescriptionTest1’",
                Details = "DetailsTest1"
            };

            /// <summary>
            /// Access to the ProductController.
            /// </summary >
            ProductController productController = new ProductController(productService,languageService);

            /// <summary>
            /// Access the private static ProductService.MapToProductEntity() function.
            /// </summary >
            var accessProductMapper = typeof(ProductService).GetField("MapToProductEntity", BindingFlags.NonPublic | BindingFlags.Static);


            // Act Product creation

            //productService.SaveProduct(productViewModel);
            productController.Create(productViewModel);
            int productIdFound = productService.GetAllProducts().Select(p => p.Id).Max();

            // Assert
            Assert.True(productController.ModelState.Count == 1);
            Assert.Contains("MissingName", productController.ModelState);
        }

        [Fact]
        public void Test2()
        {
            // Arrange
            /// <summary>
            /// Creating new services linked with Sql Context.
            /// </summary >
            ICart cart = new Cart();
            IProductRepository productRepository = new ProductRepository(Context);
            IOrderRepository orderRepository = new OrderRepository(Context);
            ILanguageService languageService = new LanguageService();


            /// <summary>
            /// Creating new services to simulate localizer.
            /// </summary >
            var service = new ServiceCollection();
            service.AddLogging();
            service.AddLocalization(options => options.ResourcesPath = "P3AddNewFunctionalityDotNetCore.Resources.Models.Services.ProductServiceResources");
            var serviceProvider = service.BuildServiceProvider();

            var localizer = serviceProvider.GetService<IStringLocalizer<P3AddNewFunctionalityDotNetCore.Resources.Models.Services.ProductServiceResources>>();


            /// <summary>
            /// Creating a complete ÎproductService.
            /// </summary >
            IProductService productService = new P3AddNewFunctionalityDotNetCore.Models.Services.ProductService(cart, productRepository, orderRepository, localizer);




            // Arrange
            /// <summary>
            /// Simulate user product creation by filling fields.
            /// </summary >
            ProductViewModel productViewModel = new ProductViewModel
            {
                Name = "",
                Price = "",
                Stock = "",
                Description = "DescriptionTest1’",
                Details = "DetailsTest1"
            };

            /// <summary>
            /// Access to the ProductController.
            /// </summary >
            ProductController productController = new ProductController(productService, languageService);

            /// <summary>
            /// Access the private static ProductService.MapToProductEntity() function.
            /// </summary >
            var accessProductMapper = typeof(ProductService).GetField("MapToProductEntity", BindingFlags.NonPublic | BindingFlags.Static);


            // Act Product creation

            //productService.SaveProduct(productViewModel);
            productController.Create(productViewModel);
            int productIdFound = productService.GetAllProducts().Select(p => p.Id).Max();

            // Assert
            Assert.True(productController.ModelState.Count == 5);
            Assert.Contains("MissingName", productController.ModelState);
            Assert.Contains("MissingStock", productController.ModelState);
            Assert.Contains("MissingPrice", productController.ModelState);
            Assert.Contains("StockNotGreaterThanZero", productController.ModelState);
            Assert.Contains("PriceNotGreaterThanZero", productController.ModelState);

        }

        [Fact]
        public void Test15()
        {
            //// Arrange
            ///// <summary>
            ///// Creating new services linked with Sql Context.
            ///// </summary >
            //ICart cart = new Cart();
            //IProductRepository productRepository = new ProductRepository(Context);
            //IOrderRepository orderRepository = new OrderRepository(Context);


            ///// <summary>
            ///// Creating new services to simulate localizer.
            ///// </summary >
            //var service = new ServiceCollection();
            //service.AddLogging();
            //service.AddLocalization(options => options.ResourcesPath = "P3AddNewFunctionalityDotNetCore.Resources.Models.Services.ProductServiceResources");
            //var serviceProvider = service.BuildServiceProvider();

            //var localizer = serviceProvider.GetService<IStringLocalizer<P3AddNewFunctionalityDotNetCore.Resources.Models.Services.ProductServiceResources>>();


            ///// <summary>
            ///// Creating a complete ÎproductService.
            ///// </summary >
            //IProductService productService = new P3AddNewFunctionalityDotNetCore.Models.Services.ProductService(cart, productRepository, orderRepository, localizer);

            //// Arrange
            ///// <summary>
            ///// Simulate user product creation by filling fields.
            ///// </summary >
            //ProductViewModel productViewModel = new ProductViewModel
            //{
            //    Name = "ProductTest15",
            //    Price = "15",
            //    Stock = "15",
            //    Description = "DescriptionTest15’",
            //    Details = "DetailsTest15"
            //};

            ///// <summary>
            ///// Access the private static ProductService.MapToProductEntity() function.
            ///// </summary >
            //var accessProductMapper = typeof(ProductService).GetField("MapToProductEntity", BindingFlags.NonPublic | BindingFlags.Static);



            //// Act Product creation 
            //productService.SaveProduct(productViewModel);
            //int productIdFound = productService.GetAllProducts().Select(p => p.Id).Max();
            //var productFound = productService.GetProductById(productIdFound);
            ////Assert
            //Assert.True(productService.GetAllProducts().Select(p => p.Id).Max() == productIdFound);
            //Assert.True(productFound.Name == productViewModel.Name);

            //// Act Product deletion
            //productService.DeleteProduct(productIdFound);
            ////Assert
            //Assert.False(productService.GetAllProducts().Select(p => p.Id).Max() == productIdFound);

      

            // Arrange
            /// <summary>
            /// Creating new services linked with Sql Context.
            /// </summary >

            IProductRepository productRepository = new ProductRepository(Context);
            IOrderRepository orderRepository = new OrderRepository(Context);
            ILanguageService languageService = new LanguageService();


            /// <summary>
            /// Creating new services to simulate localizer.
            /// </summary >
            var service = new ServiceCollection();
            service.AddLogging();
            service.AddLocalization(options => options.ResourcesPath = "P3AddNewFunctionalityDotNetCore.Resources.Models.Services.ProductServiceResources");
            var serviceProvider = service.BuildServiceProvider();

            var localizer = serviceProvider.GetService<IStringLocalizer<P3AddNewFunctionalityDotNetCore.Resources.Models.Services.ProductServiceResources>>();


            //var mockedICart = new Mock<ICart>();
            //var mockedIProductRepository = new Mock<IProductRepository>();
            //var mockedIOrerRepository = new Mock<IOrderRepository>();
            //var mockedILanguageService = new Mock<ILanguageService>();

            var mockedIProductService = new Mock<IProductService>();

            /// <summary>
            /// Creating a complete ÎproductService.
            /// </summary >
            //IProductService productService = new P3AddNewFunctionalityDotNetCore.Models.Services.ProductService(mockedICart, productRepository, orderRepository, localizer);

            ProductViewModel productViewModel15a = new ProductViewModel
            {
                Name = "ProductTest15a",
                Price = "15",
                Stock = "15",
                Description = "DescriptionTest15a’",
                Details = "DétailsTest15a"
            };

            ProductViewModel productViewModel15b = new ProductViewModel
            {
                Name = "ProductTest15b",
                Price = "15",
                Stock = "15",
                Description = "DescriptionTest15b’",
                Details = "DétailsTest15b"
            };


            mockedIProductService.Setup(s => s.GetAllProductsViewModel());
            mockedIProductService.Setup(s => s.SaveProduct(productViewModel15a));
            mockedIProductService.Setup(s => s.SaveProduct(productViewModel15b));











        }

        [Fact]
        public void Test16()
        {
            // Arrange
            /// <summary>
            /// Creating new services linked with Sql Context.
            /// </summary >
            ICart cart = new Cart();
            IProductRepository productRepository = new ProductRepository(Context);
            IOrderRepository orderRepository = new OrderRepository(Context);


            /// <summary>
            /// Creating new services to simulate localizer.
            /// </summary >
            var service = new ServiceCollection();
            service.AddLogging();
            service.AddLocalization(options => options.ResourcesPath = "P3AddNewFunctionalityDotNetCore.Resources.Models.Services.ProductServiceResources");
            var serviceProvider = service.BuildServiceProvider();

            var localizer = serviceProvider.GetService<IStringLocalizer<P3AddNewFunctionalityDotNetCore.Resources.Models.Services.ProductServiceResources>>();


            /// <summary>
            /// Creating a complete ÎproductService.
            /// </summary >
            IProductService productService = new P3AddNewFunctionalityDotNetCore.Models.Services.ProductService(cart, productRepository, orderRepository, localizer);

            // Arrange
            /// <summary>
            /// Simulate user product creation by filling fields.
            /// </summary >
            ProductViewModel productViewModel = new ProductViewModel
            {
                Name = "ProductTest16",
                Price = "16",
                Stock = "16",
                Description = "DescriptionTest16’",
                Details = "DetailsTest16"
            };

            /// <summary>
            /// Access the private static ProductService.MapToProductEntity() function.
            /// </summary >
            var accessProductMapper = typeof(ProductService).GetField("MapToProductEntity", BindingFlags.NonPublic | BindingFlags.Static);



            // Act Product creation 
            productService.SaveProduct(productViewModel);
            int productIdFound = productService.GetAllProducts().Select(p => p.Id).Max();
            var productFound = productService.GetProductById(productIdFound);
            //Assert
            Assert.True(productService.GetAllProducts().Select(p => p.Id).Max() == productIdFound);
            Assert.True(productFound.Name == productViewModel.Name);

            // Act Product deletion
            productService.DeleteProduct(productIdFound);
            //Assert
            Assert.False(productService.GetAllProducts().Select(p => p.Id).Max() == productIdFound);
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