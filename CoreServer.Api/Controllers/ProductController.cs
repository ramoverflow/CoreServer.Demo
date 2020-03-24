using System;
using System.Collections.Generic;
using CoreServer.Common.Core;
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

        public ProductController(IProductService productService, ICapPublisher capPublisher)
        {
            _productService = productService;
            _capPublisher = capPublisher;

            _productService.RegisterAddProductEvent(
                new EventObserver<ProductDto>(newProduct =>
                {
                    _capPublisher.Publish("core.server.addNewProduct", newProduct);
                }));
        }

        [NonAction]
        [CapSubscribe("core.server.addNewProduct")]
        public void OnAddNewProduct(ProductDto product)
        {
            try
            {
                Console.WriteLine("adding new product");
                Console.WriteLine(JsonUtil.ToJson(product));
                Console.WriteLine("throw exception...");
                throw new Exception("测试的异常");
            }
            catch
            {
                _capPublisher.Publish("core.server.addFailed", product);
            }
        }

        [NonAction]
        [CapSubscribe("core.server.addFailed")]
        public void OnAddFailed(ProductDto productDto)
        {
            Console.WriteLine("事务：新增产品时发生了故障。。。 删除");
            _productService.DeleteProduct(productDto.Id);
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