using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoreServer.Common.Util;
using CoreServer.Model;
using CoreServer.Service;
using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;

namespace CoreServer.Api.Controllers
{
    //[Route("[controller]")]
    [Route("product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        private readonly ICapPublisher _capPublisher;

        private readonly EventObserver<ProductDto> OnAddNewProduct;

        public ProductController(IProductService productService, ICapPublisher capPublisher)
        {
            _productService = productService;
            _capPublisher = capPublisher;

            OnAddNewProduct =
                new EventObserver<ProductDto>(newProduct => { _capPublisher.Publish("xxxx", newProduct); });
            OnAddNewProduct.Subscribe(_productService.OnAddProduct());
        }

        [HttpGet("getProducts")]
        public List<ProductDto> GetProducts()
        {
            return _productService.GetProducts();
        }

        [HttpPost("addProduct")]
        public ProductDto AddProduct(ProductDto productDto)
        {
            var product = _productService.AddProduct(productDto);
            return product;
        }

        [HttpPut("updateProduct")]
        public void UpdateProduct(ProductDto productDto)
        {
            _productService.UpdateProduct(productDto);
        }

        [HttpDelete("deleteProduct")]
        public void DeleteProduct(Guid id)
        {
            _productService.DeleteProduct(id);
        }
    }
}