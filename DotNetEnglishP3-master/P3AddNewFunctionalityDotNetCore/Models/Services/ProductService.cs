using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Localization;
using Microsoft.IdentityModel.Tokens;
using NUnit.Framework.Constraints;
using P3AddNewFunctionalityDotNetCore.Models.Entities;
using P3AddNewFunctionalityDotNetCore.Models.Repositories;
using P3AddNewFunctionalityDotNetCore.Models.ViewModels;
using P3AddNewFunctionalityDotNetCore.Resources.Models.Services;
using System;
using System.Collections.Generic;
//Adding the Data Anotations library
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace P3AddNewFunctionalityDotNetCore.Models.Services
{
    public class ProductService : IProductService
    {
        private readonly ICart _cart;
        private readonly IProductRepository _productRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IStringLocalizer<ProductServiceResources> _localizer;

        public ProductService(ICart cart, IProductRepository productRepository,
            IOrderRepository orderRepository, IStringLocalizer<ProductServiceResources> localizer)
        {
            _cart = cart;
            _productRepository = productRepository;
            _orderRepository = orderRepository;
            _localizer = localizer;
        }
        public List<ProductViewModel> GetAllProductsViewModel()
        {
             
            IEnumerable<Product> productEntities = GetAllProducts();
            return MapToViewModel(productEntities);
        }

        private static List<ProductViewModel> MapToViewModel(IEnumerable<Product> productEntities)
        {
            List <ProductViewModel> products = new List<ProductViewModel>();
            foreach (Product product in productEntities)
            {
                products.Add(new ProductViewModel
                {
                    Id = product.Id,
                    Stock = product.Quantity.ToString(),
                    Price = product.Price.ToString(CultureInfo.InvariantCulture),
                    Name = product.Name,
                    Description = product.Description,
                    Details = product.Details
                });
            }

            return products;
        }

        public List<Product> GetAllProducts()
        {
            IEnumerable<Product> productEntities = _productRepository.GetAllProducts();
            return productEntities?.ToList();
        }

        public ProductViewModel GetProductByIdViewModel(int id)
        {
            List<ProductViewModel> products = GetAllProductsViewModel().ToList();
            return products.Find(p => p.Id == id);
        }


        public Product GetProductById(int id)
        {
            List<Product> products = GetAllProducts().ToList();
            return products.Find(p => p.Id == id);
        }

        public async Task<Product> GetProduct(int id)
        {
            var product = await _productRepository.GetProduct(id);
            return product;
        }

        public async Task<IList<Product>> GetProduct()
        {
            var products = await _productRepository.GetProduct();
            return products;
        }
        public void UpdateProductQuantities()
        {
            Cart cart = (Cart) _cart;
            foreach (CartLine line in cart.Lines)
            {
                _productRepository.UpdateProductStocks(line.Product.Id, line.Quantity);
            }
        }

        public Dictionary<string, string> CheckProductModelErrors(ProductViewModel product)
        {
            /// <summary>
            /// Use of a dictionnary to ease ModelState tests.
            /// [Key ,Value]
            /// [Key = Value = "ErrorMessageName" ]
            /// </summary >
            Dictionary<string, string> modelErrors = new Dictionary<string, string>();


            /// <summary>
            /// Declaration of the same ProductViewModel RegularExpression attributes
            /// to run server side attribute validation.
            /// </summary >
            var Attribute1 = new RequiredAttribute();

            if (!Attribute1.IsValid(product.Name))
            {
                modelErrors.Add("MissingName", _localizer["MissingName"]);
            }


            var Attribute2 = new RequiredAttribute();

            if (!Attribute2.IsValid(product.Stock))
            {
                modelErrors.Add("MissingStock", _localizer["MissingStock"]);
            }


            var Attribute3 = new RegularExpressionAttribute("^((?!^Stock$)-?[0-9])+$");

            if (!Attribute3.IsValid(product.Stock))
            {
                 modelErrors.Add("StockNotAnInteger", _localizer["StockNotAnInteger"]);
            }

            var Attribute4 = new GreaterThanConstraint(product.Stock);

            if (Attribute4.Equals(0) || Attribute4.Equals(null))
            {
                modelErrors.Add("StockNotGreaterThanZero", _localizer["StockNotGreaterThanZero"]);
            }


            var Attribute5 = new RequiredAttribute();

            if (!Attribute5.IsValid(product.Price))
            {
                modelErrors.Add("MissingPrice", _localizer["MissingPrice"]);
            }


            var Attribute6 = new RegularExpressionAttribute("^((?!^Price$)-?(\\d+\\.?\\d+|\\d))+$");

            if (!Attribute6.IsValid(product.Price))
            {
                modelErrors.Add("PriceNotANumber", _localizer["PriceNotANumber"]);
            }


            var Attribute7 = new PriceNotDouble();

            if (!Attribute7.IsValid(product.Price))
            {
                modelErrors.Add("PriceNotGreaterThanZero", _localizer["PriceNotGreaterThanZero"]);
            }

            return modelErrors;
        }
        public void SaveProduct(ProductViewModel product)
        {
            var productToAdd = MapToProductEntity(product);
            _productRepository.SaveProduct(productToAdd);
        }

        private static Product MapToProductEntity(ProductViewModel product)
        {
            Product productEntity = new Product
            {
                Name = product.Name,
                Price = double.Parse(product.Price),
                Quantity = Int32.Parse(product.Stock),
                Description = product.Description,
                Details = product.Details
            };
            return productEntity;
        }

        public void DeleteProduct(int id)
        {
            // TODO what happens if a product has been added to a cart and has been later removed from the inventory ?
            // delete the product form the cart by using the specific method
            // => the choice is up to the student
            _cart.RemoveLine(GetProductById(id));

            _productRepository.DeleteProduct(id);
        }

        public class Validation
        {
            public bool IsValid { get; set; }
            public string ErrorMessage { get; set; }

        }
    }
}
