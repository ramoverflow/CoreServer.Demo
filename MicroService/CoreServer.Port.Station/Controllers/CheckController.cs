using System;
using CoreServer.Common.Util;
using CoreServer.Model;
using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;

namespace CoreServer.Port.Station.Controllers
{
    [ApiController]
    public class CheckController : ControllerBase
    {
        private readonly ICapPublisher _capPublisher;

        public CheckController(ICapPublisher capPublisher)
        {
            _capPublisher = capPublisher;
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
    }
}