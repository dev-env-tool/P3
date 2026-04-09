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
        public void CheckProductModelErrorsShouldReturnMissingName()
        {
            /// Arrange
            

            /// 


            /// <summary>
            /// Use of Moq to replicate I(Name)Service.
            /// No need to buildup dependances like intermediary Interfaces or even SQL.
            /// </summary >

            var mockedICart = new Mock<ICart>();
            var mockedIProductRepository = new Mock<IProductRepository>();
            var mockedIOrderRepository = new Mock<IOrderRepository>();
            var mockedILanguageService = new Mock<ILanguageService>();



            /// <summary>
            /// Creating new services to simulate localizer
            /// as there is no Interface ILocalizer.
            /// </summary >
            var service = new ServiceCollection();
            service.AddLogging();
            service.AddLocalization(options => options.ResourcesPath = "P3AddNewFunctionalityDotNetCore.Resources.Models.Services.ProductServiceResources");
            var serviceProvider = service.BuildServiceProvider();

            var localizer = serviceProvider.GetService<IStringLocalizer<P3AddNewFunctionalityDotNetCore.Resources.Models.Services.ProductServiceResources>>();

            /// <summary>
            /// Here we call the real base functions as the tests have been created after the programme.
            /// We can mock virtual Interfaces to use the constructor.
            /// localizer is created in the test via ServiceCollection as an alternative to ILocalizer.
            /// 
            /// If we used mockedIProductService we would copy/paste the whole logic of CheckProductModelErrors.
            /// The issue is : when we modifiy something ito the real function, we should change it in every test as well.
            /// 
            /// Possible solution but it adds a new independent class to be called :
            /// 
            ///     mockedProductService.Setup(s => s.CheckProductModelErrors(It.IsAny<ProductViewModel>()))
            ///     .Returns((ProductViewModel pvm) =>
            ///     {
            ///     var testErrorDictionnary = new Dictionary<string, string>();
            ///     testErrorDictionnary = HelperClass.CheckProductModelErrorsHelper(pvm);
            ///     return testErrorDictionnary;
            ///     });
            /// 
            /// Otherwise we would have to create a new independent class containing the logic via a helper in it.


            var mockedProductService = new Mock<ProductService>(mockedICart.Object, mockedIProductRepository.Object, mockedIOrderRepository.Object, localizer) { CallBase = true };



            /// <summary>
            /// Access to the ProductController.
            /// </summary >
            //ProductController productController = new ProductController(mockedIProductService, mockedILanguageService);

            /// <summary>
            /// Creating minimal Lists to simulate SQL database.
            /// SaveProduct creates and returns ProductViewModel whereas GetAllProducts returns Product.
            /// </summary >
            //var productViewModelNoSqlDb = new List<ProductViewModel>();
            //var productNoSqlDb = new List<Product>();



            /// <summary>
            /// Use of Moq to Setup the functions we then use and test.
            /// Setup and Callback must be of the same type.
            /// (It.IsAny<ProductViewModel>()) in Moq context 
            /// means the same as (ProductViewModel Product) in regular use context.
            /// </summary >
            /// mockedProductService.Setup(s => s.CheckProductModelErrors(It.IsAny<ProductViewModel>()))
            ///    //.Callback<ProductViewModel>(pvm => productViewModelNoSqlDb.Add(pvm))
            ///    .Returns((ProductViewModel pvm) =>
            ///    {
            ///        var testErrorDictionnary = new Dictionary<string, string>();
            ///        testErrorDictionnary = mockedProductService.Object.CheckProductModelErrorsHelper(pvm);
            ///        return testErrorDictionnary;
            ///    });
            
            //mockedIProductService.Setup(s => s.GetAllProducts()).Returns(productNoSqlDb);


            /// <summary>
            /// Creating our two test productViewModels.
            /// Both are correctly filled up.
            /// </summary >
            ProductViewModel productViewModel = new ProductViewModel
            {
                Name = "",
                Price = "10",
                Stock = "10",
                Description = "DescriptionTest’",
                Details = "DétailsTest"
            };

            ///// <summary>
            ///// Creating a temporary 
            ///// </summary >
            //var testErrorDictionnary = new Dictionary<string, string>();

            //testErrorDictionnary = mockedIProductService.Object.CheckProductModelErrors(productViewModel);
            Dictionary<string, string> errorTempDictionary = new Dictionary<string, string>();


            ///Act
            errorTempDictionary = mockedProductService.Object.CheckProductModelErrors(productViewModel);

            ///Assert
            Assert.True(errorTempDictionary.Count == 1);
            Assert.True(errorTempDictionary.ContainsKey("MissingName"));


        }




        [Fact]

        public void CheckProductModelErrorsShouldReturnMissingPrice()
        {
            /// Arrange

            /// <summary>
            /// Use of Moq to replicate I(Name)Service.
            /// No need to buildup dependances like intermediary Interfaces or even SQL.
            /// </summary >

            var mockedICart = new Mock<ICart>();
            var mockedIProductRepository = new Mock<IProductRepository>();
            var mockedIOrderRepository = new Mock<IOrderRepository>();
            var mockedILanguageService = new Mock<ILanguageService>();



            /// <summary>
            /// Creating new services to simulate localizer
            /// as there is no Interface ILocalizer.
            /// </summary >
            var service = new ServiceCollection();
            service.AddLogging();
            service.AddLocalization(options => options.ResourcesPath = "P3AddNewFunctionalityDotNetCore.Resources.Models.Services.ProductServiceResources");
            var serviceProvider = service.BuildServiceProvider();

            var localizer = serviceProvider.GetService<IStringLocalizer<P3AddNewFunctionalityDotNetCore.Resources.Models.Services.ProductServiceResources>>();

            /// <summary>
            /// Here we call the real base functions as the tests have been created after the programme.
            /// We can mock virtual Interfaces to use the constructor.
            /// localizer is created in the test via ServiceCollection as an alternative to ILocalizer.
            /// 
            /// If we used mockedIProductService we would copy/paste the whole logic of CheckProductModelErrors.
            /// The issue is : when we modifiy something ito the real function, we should change it in every test as well.
            /// 
            /// Possible solution but it adds a new independent class to be called :
            /// 
            ///     mockedProductService.Setup(s => s.CheckProductModelErrors(It.IsAny<ProductViewModel>()))
            ///     .Returns((ProductViewModel pvm) =>
            ///     {
            ///     var testErrorDictionnary = new Dictionary<string, string>();
            ///     testErrorDictionnary = HelperClass.CheckProductModelErrorsHelper(pvm);
            ///     return testErrorDictionnary;
            ///     });
            /// 
            /// Otherwise we would have to create a new independent class containing the logic via a helper in it.


            var mockedProductService = new Mock<ProductService>(mockedICart.Object, mockedIProductRepository.Object, mockedIOrderRepository.Object, localizer) { CallBase = true };


            /// <summary>
            /// Creating our two test productViewModels.
            /// Both are correctly filled up.
            /// </summary >
            ProductViewModel productViewModel = new ProductViewModel
            {
                Name = "Name",
                Price = "",
                Stock = "10",
                Description = "DescriptionTest’",
                Details = "DétailsTest"
            };

            ///// <summary>
            ///// Creating a temporary 
            ///// </summary >
            //var testErrorDictionnary = new Dictionary<string, string>();

            //testErrorDictionnary = mockedIProductService.Object.CheckProductModelErrors(productViewModel);
            Dictionary<string, string> errorTempDictionary = new Dictionary<string, string>();

            ///Act
            errorTempDictionary = mockedProductService.Object.CheckProductModelErrors(productViewModel);

            ///Assert
            Assert.True(errorTempDictionary.Count == 1);
            Assert.True(errorTempDictionary.ContainsKey("MissingPrice"));
        }



        [Fact]

        public void CheckProductModelErrorsShouldReturnMissingStock()
        {
            /// Arrange

            /// <summary>
            /// Use of Moq to replicate I(Name)Service.
            /// No need to buildup dependances like intermediary Interfaces or even SQL.
            /// </summary >

            var mockedICart = new Mock<ICart>();
            var mockedIProductRepository = new Mock<IProductRepository>();
            var mockedIOrderRepository = new Mock<IOrderRepository>();
            var mockedILanguageService = new Mock<ILanguageService>();



            /// <summary>
            /// Creating new services to simulate localizer
            /// as there is no Interface ILocalizer.
            /// </summary >
            var service = new ServiceCollection();
            service.AddLogging();
            service.AddLocalization(options => options.ResourcesPath = "P3AddNewFunctionalityDotNetCore.Resources.Models.Services.ProductServiceResources");
            var serviceProvider = service.BuildServiceProvider();

            var localizer = serviceProvider.GetService<IStringLocalizer<P3AddNewFunctionalityDotNetCore.Resources.Models.Services.ProductServiceResources>>();

            /// <summary>
            /// Here we call the real base functions as the tests have been created after the programme.
            /// We can mock virtual Interfaces to use the constructor.
            /// localizer is created in the test via ServiceCollection as an alternative to ILocalizer.
            /// 
            /// If we used mockedIProductService we would copy/paste the whole logic of CheckProductModelErrors.
            /// The issue is : when we modifiy something ito the real function, we should change it in every test as well.
            /// 
            /// Possible solution but it adds a new independent class to be called :
            /// 
            ///     mockedProductService.Setup(s => s.CheckProductModelErrors(It.IsAny<ProductViewModel>()))
            ///     .Returns((ProductViewModel pvm) =>
            ///     {
            ///     var testErrorDictionnary = new Dictionary<string, string>();
            ///     testErrorDictionnary = HelperClass.CheckProductModelErrorsHelper(pvm);
            ///     return testErrorDictionnary;
            ///     });
            /// 
            /// Otherwise we would have to create a new independent class containing the logic via a helper in it.


            var mockedProductService = new Mock<ProductService>(mockedICart.Object, mockedIProductRepository.Object, mockedIOrderRepository.Object, localizer) { CallBase = true };


            /// <summary>
            /// Creating our two test productViewModels.
            /// Both are correctly filled up.
            /// </summary >
            ProductViewModel productViewModel = new ProductViewModel
            {
                Name = "Name",
                Price = "10",
                Stock = "",
                Description = "DescriptionTest’",
                Details = "DétailsTest"
            };

            ///// <summary>
            ///// Creating a temporary 
            ///// </summary >
            //var testErrorDictionnary = new Dictionary<string, string>();

            //testErrorDictionnary = mockedIProductService.Object.CheckProductModelErrors(productViewModel);
            Dictionary<string, string> errorTempDictionary = new Dictionary<string, string>();

            ///Act
            errorTempDictionary = mockedProductService.Object.CheckProductModelErrors(productViewModel);

            ///Assert
            Assert.True(errorTempDictionary.Count == 1);
            Assert.True(errorTempDictionary.ContainsKey("MissingStock"));
        }


        [Fact]

        public void CheckProductModelErrorsShouldReturnPriceNotANumberAndPriceNotGreaterThanZero()
        {
            /// Arrange

            /// <summary>
            /// Use of Moq to replicate I(Name)Service.
            /// No need to buildup dependances like intermediary Interfaces or even SQL.
            /// </summary >

            var mockedICart = new Mock<ICart>();
            var mockedIProductRepository = new Mock<IProductRepository>();
            var mockedIOrderRepository = new Mock<IOrderRepository>();
            var mockedILanguageService = new Mock<ILanguageService>();



            /// <summary>
            /// Creating new services to simulate localizer
            /// as there is no Interface ILocalizer.
            /// </summary >
            var service = new ServiceCollection();
            service.AddLogging();
            service.AddLocalization(options => options.ResourcesPath = "P3AddNewFunctionalityDotNetCore.Resources.Models.Services.ProductServiceResources");
            var serviceProvider = service.BuildServiceProvider();

            var localizer = serviceProvider.GetService<IStringLocalizer<P3AddNewFunctionalityDotNetCore.Resources.Models.Services.ProductServiceResources>>();

            /// <summary>
            /// Here we call the real base functions as the tests have been created after the programme.
            /// We can mock virtual Interfaces to use the constructor.
            /// localizer is created in the test via ServiceCollection as an alternative to ILocalizer.
            /// 
            /// If we used mockedIProductService we would copy/paste the whole logic of CheckProductModelErrors.
            /// The issue is : when we modifiy something ito the real function, we should change it in every test as well.
            /// 
            /// Possible solution but it adds a new independent class to be called :
            /// 
            ///     mockedProductService.Setup(s => s.CheckProductModelErrors(It.IsAny<ProductViewModel>()))
            ///     .Returns((ProductViewModel pvm) =>
            ///     {
            ///     var testErrorDictionnary = new Dictionary<string, string>();
            ///     testErrorDictionnary = HelperClass.CheckProductModelErrorsHelper(pvm);
            ///     return testErrorDictionnary;
            ///     });
            /// 
            /// Otherwise we would have to create a new independent class containing the logic via a helper in it.


            var mockedProductService = new Mock<ProductService>(mockedICart.Object, mockedIProductRepository.Object, mockedIOrderRepository.Object, localizer) { CallBase = true };


            /// <summary>
            /// Creating our two test productViewModels.
            /// Both are correctly filled up.
            /// </summary >
            ProductViewModel productViewModel = new ProductViewModel
            {
                Name = "Name",
                Price = "price-+=",
                Stock = "10",
                Description = "DescriptionTest’",
                Details = "DétailsTest"
            };

            ///// <summary>
            ///// Creating a temporary 
            ///// </summary >
            //var testErrorDictionnary = new Dictionary<string, string>();

            //testErrorDictionnary = mockedIProductService.Object.CheckProductModelErrors(productViewModel);
            Dictionary<string, string> errorTempDictionary = new Dictionary<string, string>();

            ///Act
            errorTempDictionary = mockedProductService.Object.CheckProductModelErrors(productViewModel);

            ///Assert
            Assert.True(errorTempDictionary.Count == 2);
            Assert.True(errorTempDictionary.ContainsKey("PriceNotANumber"));
            Assert.True(errorTempDictionary.ContainsKey("PriceNotGreaterThanZero"));
        }


        [Fact]

        public void CheckProductModelErrorsShouldReturnPriceNotGreaterThan0()
        {
            /// Arrange

            /// <summary>
            /// Use of Moq to replicate I(Name)Service.
            /// No need to buildup dependances like intermediary Interfaces or even SQL.
            /// </summary >

            var mockedICart = new Mock<ICart>();
            var mockedIProductRepository = new Mock<IProductRepository>();
            var mockedIOrderRepository = new Mock<IOrderRepository>();
            var mockedILanguageService = new Mock<ILanguageService>();



            /// <summary>
            /// Creating new services to simulate localizer
            /// as there is no Interface ILocalizer.
            /// </summary >
            var service = new ServiceCollection();
            service.AddLogging();
            service.AddLocalization(options => options.ResourcesPath = "P3AddNewFunctionalityDotNetCore.Resources.Models.Services.ProductServiceResources");
            var serviceProvider = service.BuildServiceProvider();

            var localizer = serviceProvider.GetService<IStringLocalizer<P3AddNewFunctionalityDotNetCore.Resources.Models.Services.ProductServiceResources>>();

            /// <summary>
            /// Here we call the real base functions as the tests have been created after the programme.
            /// We can mock virtual Interfaces to use the constructor.
            /// localizer is created in the test via ServiceCollection as an alternative to ILocalizer.
            /// 
            /// If we used mockedIProductService we would copy/paste the whole logic of CheckProductModelErrors.
            /// The issue is : when we modifiy something ito the real function, we should change it in every test as well.
            /// 
            /// Possible solution but it adds a new independent class to be called :
            /// 
            ///     mockedProductService.Setup(s => s.CheckProductModelErrors(It.IsAny<ProductViewModel>()))
            ///     .Returns((ProductViewModel pvm) =>
            ///     {
            ///     var testErrorDictionnary = new Dictionary<string, string>();
            ///     testErrorDictionnary = HelperClass.CheckProductModelErrorsHelper(pvm);
            ///     return testErrorDictionnary;
            ///     });
            /// 
            /// Otherwise we would have to create a new independent class containing the logic via a helper in it.


            var mockedProductService = new Mock<ProductService>(mockedICart.Object, mockedIProductRepository.Object, mockedIOrderRepository.Object, localizer) { CallBase = true };


            /// <summary>
            /// Creating our two test productViewModels.
            /// Both are correctly filled up.
            /// </summary >
            ProductViewModel productViewModel = new ProductViewModel
            {
                Name = "Name",
                Price = "-10",
                Stock = "10",
                Description = "DescriptionTest’",
                Details = "DétailsTest"
            };

            ///// <summary>
            ///// Creating a temporary 
            ///// </summary >
            //var testErrorDictionnary = new Dictionary<string, string>();

            //testErrorDictionnary = mockedIProductService.Object.CheckProductModelErrors(productViewModel);
            Dictionary<string, string> errorTempDictionary = new Dictionary<string, string>();

            ///Act
            errorTempDictionary = mockedProductService.Object.CheckProductModelErrors(productViewModel);

            ///Assert
            Assert.True(errorTempDictionary.Count == 1);
            Assert.True(errorTempDictionary.ContainsKey("PriceNotGreaterThanZero"));
        }

        [Fact]

        public void CheckProductModelErrorsShouldReturnStockNotGreaterThan0()
        {
            /// Arrange

            /// <summary>
            /// Use of Moq to replicate I(Name)Service.
            /// No need to buildup dependances like intermediary Interfaces or even SQL.
            /// </summary >

            var mockedICart = new Mock<ICart>();
            var mockedIProductRepository = new Mock<IProductRepository>();
            var mockedIOrderRepository = new Mock<IOrderRepository>();
            var mockedILanguageService = new Mock<ILanguageService>();



            /// <summary>
            /// Creating new services to simulate localizer
            /// as there is no Interface ILocalizer.
            /// </summary >
            var service = new ServiceCollection();
            service.AddLogging();
            service.AddLocalization(options => options.ResourcesPath = "P3AddNewFunctionalityDotNetCore.Resources.Models.Services.ProductServiceResources");
            var serviceProvider = service.BuildServiceProvider();

            var localizer = serviceProvider.GetService<IStringLocalizer<P3AddNewFunctionalityDotNetCore.Resources.Models.Services.ProductServiceResources>>();

            /// <summary>
            /// Here we call the real base functions as the tests have been created after the programme.
            /// We can mock virtual Interfaces to use the constructor.
            /// localizer is created in the test via ServiceCollection as an alternative to ILocalizer.
            /// 
            /// If we used mockedIProductService we would copy/paste the whole logic of CheckProductModelErrors.
            /// The issue is : when we modifiy something ito the real function, we should change it in every test as well.
            /// 
            /// Possible solution but it adds a new independent class to be called :
            /// 
            ///     mockedProductService.Setup(s => s.CheckProductModelErrors(It.IsAny<ProductViewModel>()))
            ///     .Returns((ProductViewModel pvm) =>
            ///     {
            ///     var testErrorDictionnary = new Dictionary<string, string>();
            ///     testErrorDictionnary = HelperClass.CheckProductModelErrorsHelper(pvm);
            ///     return testErrorDictionnary;
            ///     });
            /// 
            /// Otherwise we would have to create a new independent class containing the logic via a helper in it.


            var mockedProductService = new Mock<ProductService>(mockedICart.Object, mockedIProductRepository.Object, mockedIOrderRepository.Object, localizer) { CallBase = true };


            /// <summary>
            /// Creating our two test productViewModels.
            /// Both are correctly filled up.
            /// </summary >
            ProductViewModel productViewModel = new ProductViewModel
            {
                Name = "Name",
                Price = "10",
                Stock = "-0",
                Description = "DescriptionTest’",
                Details = "DétailsTest"
            };

            ///// <summary>
            ///// Creating a temporary 
            ///// </summary >
            //var testErrorDictionnary = new Dictionary<string, string>();

            //testErrorDictionnary = mockedIProductService.Object.CheckProductModelErrors(productViewModel);
            Dictionary<string, string> errorTempDictionary = new Dictionary<string, string>();

            ///Act
            errorTempDictionary = mockedProductService.Object.CheckProductModelErrors(productViewModel);

            ///Assert
            Assert.True(errorTempDictionary.Count == 1);
            Assert.True(errorTempDictionary.ContainsKey("StockNotGreaterThanZero"));
        }

        //[Fact]

        //public void CheckProductControllerModelStateReturnsMissingNameMissingPriceMissingStock()
        //{

        //}

        //[Fact]

        //public void CheckProductControllerModelStateReturnsPriceNotANumberStockNotANumber()
        //{

        //}


        //[Fact]

        //public void CheckProductControllerCreateAdds1ProductToSqlDatabase()
        //{

        //}

        //[Fact]

        //public void CheckProductControllerDeleteRemoves1ProductFromSqlDatabase()
        //{

        //}







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

                    /// TOREMOVE <summary>
                    /// Access the private static ProductService.MapToProductEntity() function.
                    /// </summary >
                    //var accessProductMapper = typeof(ProductService).GetField("MapToProductEntity", BindingFlags.NonPublic | BindingFlags.Static);


            // Act Product creation

            //productService.SaveProduct(productViewModel);
            productController.Create(productViewModel);
            int productIdFound = productService.GetAllProducts().Select(p => p.Id).Max();

            // Assert
            Assert.True(productController.ModelState.Count == 1);
            Assert.Contains("MissingName", productController.ModelState);
        }


        [Fact]
        public void CheckProductModelErrorsShouldReturnMissingName2()
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
            Assert.True(productController.ModelState.Count == 3);
            Assert.Contains("MissingName", productController.ModelState);
            Assert.Contains("MissingStock", productController.ModelState);
            Assert.Contains("MissingPrice", productController.ModelState);

        }

        //[Fact]
        //public void Test13()
        //{

        //    /// <summary>
        //    /// Use of Moq to replicate I(Name)Service.
        //    /// No need to buildup dependances like intermediary Interfaces or even SQL.
        //    /// </summary >
        //    var mockedIProductService = new Mock<IProductService>();
        //    var mockedILanguageService = new Mock<ILanguageService>();

        //    //var productController = new ProductController(mockedIProductService.Object,mockedILanguageService.Object);



        //    /// <summary>
        //    /// Creating minimal Lists to simulate SQL database.
        //    /// SaveProduct creates and returns ProductViewModel whereas GetAllProducts returns Product.
        //    /// </summary >
        //    var productViewModelNoSqlDb = new List<ProductViewModel>();
        //    var productNoSqlDb = new List<Product>();



        //    /// <summary>
        //    /// Use of Moq to Setup the functions we then use and test.
        //    /// Setup and Callback must be of the same type.
        //    /// (It.IsAny<ProductViewModel>()) in Moq context 
        //    /// means the same as (ProductViewModel Product) in regular use context.
        //    /// </summary >
        //    mockedIProductService.Setup(s => s.SaveProduct(It.IsAny<ProductViewModel>()))
        //        .Callback<ProductViewModel>(pvm => productViewModelNoSqlDb.Add(pvm));

        //    mockedIProductService.Setup(s => s.GetAllProducts()).Returns(productNoSqlDb);


        //    /// <summary>
        //    /// Creating our two test productViewModels.
        //    /// Both are correctly filled up.
        //    /// </summary >
        //    ProductViewModel productViewModel13 = new ProductViewModel
        //    {
        //        Name = "ProductTest13",
        //        Price = "-13.13",
        //        Stock = "-13",
        //        Description = "DescriptionTest13’",
        //        Details = "DétailsTest13"
        //    };


        //    /// <summary>
        //    /// Use of Moq to call the SaveProduct function
        //    /// </summary >
        //    mockedIProductService.Object.SaveProduct(productViewModel13);

        //    //productController.Create(productViewModel13);

        //    /// <summary>
        //    /// Counting the number of productViewModel in the list
        //    /// </summary >
        //    int productViewModelNoSqlDbCount = productViewModelNoSqlDb.Count;

        //    ///Assert
        //    Assert.True(productViewModelNoSqlDbCount == 2);
        //    //Assert.True(productController.ModelState.ContainsKey("PriceNotGreaterThan0"));
        //    //Assert.Contains("StockNotGreaterThanZero", mockedIProductService.);
        //}





        [Fact]
        public void Test15()
        {

            /// <summary>
            /// Use of Moq to replicate I(Name)Service.
            /// No need to buildup dependances like intermediary Interfaces or even SQL.
            /// </summary >
            var mockedIProductService = new Mock<IProductService>();
            var mockedILanguageService = new Mock<ILanguageService>();

            //var productController = new ProductController(mockedIProductService.Object,mockedILanguageService.Object);



            /// <summary>
            /// Creating minimal Lists to simulate SQL database.
            /// SaveProduct creates and returns ProductViewModel whereas GetAllProducts returns Product.
            /// </summary >
            var productViewModelNoSqlDb = new List<ProductViewModel>();
            var productNoSqlDb = new List<Product>();



            /// <summary>
            /// Use of Moq to Setup the functions we then use and test.
            /// Setup and Callback must be of the same type.
            /// (It.IsAny<ProductViewModel>()) in Moq context 
            /// means the same as (ProductViewModel Product) in regular use context.
            /// </summary >
            mockedIProductService.Setup(s => s.SaveProduct(It.IsAny<ProductViewModel>()))
                .Callback<ProductViewModel>(pvm => productViewModelNoSqlDb.Add(pvm));

            mockedIProductService.Setup(s => s.GetAllProducts()).Returns(productNoSqlDb);

            /// <summary>
            /// Creating our two test productViewModels.
            /// Both are correctly filled up.
            /// </summary >
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

            /// <summary>
            /// Use of Moq to call the SaveProduct function
            /// </summary >
            mockedIProductService.Object.SaveProduct(productViewModel15a);
            mockedIProductService.Object.SaveProduct(productViewModel15b);

            /// <summary>
            /// Counting the number of productViewModel in the list
            /// </summary >
            int productViewModelNoSqlDbCount = productViewModelNoSqlDb.Count;

            ///Assert
            Assert.True(productViewModelNoSqlDbCount == 2);
            Assert.True(productViewModelNoSqlDb[0].Name == "ProductTest15a");
            Assert.True(productViewModelNoSqlDb[1].Name == "ProductTest15b");

        }

        //[Fact]
        //public void Test16()
        //{
        //    // Arrange
        //    /// <summary>
        //    /// Creating new services linked with Sql Context.
        //    /// </summary >
        //    ICart cart = new Cart();
        //    IProductRepository productRepository = new ProductRepository(Context);
        //    IOrderRepository orderRepository = new OrderRepository(Context);
        //    ILanguageService languageService = new LanguageService();

        //    /// <summary>
        //    /// Creating new services to simulate localizer.
        //    /// </summary >
        //    var service = new ServiceCollection();
        //    service.AddLogging();
        //    service.AddLocalization(options => options.ResourcesPath = "P3AddNewFunctionalityDotNetCore.Resources.Models.Services.ProductServiceResources");
        //    var serviceProvider = service.BuildServiceProvider();

        //    var localizer = serviceProvider.GetService<IStringLocalizer<P3AddNewFunctionalityDotNetCore.Resources.Models.Services.ProductServiceResources>>();


        //    /// <summary>
        //    /// Creating a complete ÎproductService.
        //    /// </summary >
        //    IProductService productService = new P3AddNewFunctionalityDotNetCore.Models.Services.ProductService(cart, productRepository, orderRepository, localizer);


        //    /// <summary>
        //    /// Simulate user product creation by filling fields.
        //    /// </summary >
        //    ProductViewModel productViewModel = new ProductViewModel
        //    {
        //        Name = "ProductTest16",
        //        Price = "16",
        //        Stock = "16",
        //        Description = "DescriptionTest16’",
        //        Details = "DetailsTest16"
        //    };

        //    var productController = new ProductController(productService, languageService);

        //    var cartController = new CartController(cart, productService);

        //                        /// <summary>
        //                        /// TO REMOVE Access the private static ProductService.MapToProductEntity() function.
        //                        /// </summary >
        //                        ///var accessProductMapper = typeof(ProductService).GetField("MapToProductEntity", BindingFlags.NonPublic | BindingFlags.Static);



        //    // Act Product creation 
        //    productController.Create(productViewModel);
        //    int productIdFound = productService.GetAllProducts().Select(p => p.Id).Max();
        //    var productFound = productService.GetProductById(productIdFound);

        //    cartController.AddToCart(productIdFound);
        //    cartController.AddToCart(productIdFound);

        //    int cartLineProductIdFoundInCart = cart.GetCartLineByProductId(productFound).Product.Id;


        //    // Assert
        //    Assert.True(productService.GetAllProducts().Select(p => p.Id).Max() == productIdFound);
        //    Assert.True(productFound.Name == productViewModel.Name);

        //    Assert.Equal(productIdFound, cartLineProductIdFoundInCart);

        //    // Act Product deletion
        //    productService.DeleteProduct(productIdFound);
            

        //    //Assert
        //    Assert.Null(cart.GetCartLineByProductId(productFound).Product);
        //    Assert.False(productService.GetAllProducts().Select(p => p.Id).Max() == productIdFound);
        //    Assert.NotEqual(productIdFound, cartLineProductIdFoundInCart);
        //}
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