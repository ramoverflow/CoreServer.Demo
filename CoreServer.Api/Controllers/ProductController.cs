using System;
using System.Collections.Generic;
using CoreServer.Model;
using CoreServer.Service;
using Microsoft.AspNetCore.Mvc;

namespace CoreServer.Api.Controllers
{
    //[Route("[controller]")]
    [Route("product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("getProducts")]
        public List<ProductDto> GetProducts()
        {
            return _productService.GetProducts();
        }

        [HttpPost("addProduct")]
        public ProductDto AddProduct(ProductDto productDto)
        {
            return _productService.AddProduct(productDto);
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