using System;
using System.Collections.Generic;
using CoreServer.Common.Core;
using CoreServer.Common.Util;
using CoreServer.Model;

namespace CoreServer.Service
{
    public interface IProductService
    {
        List<ProductDto> GetProducts();
        ProductDto GetProductByName(string name);

        /// <summary>
        /// 添加一个产品
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        ProductDto AddProduct(ProductDto product);

        /// <summary>
        /// 注册添加产品时的事件
        /// </summary>
        /// <param name="observer"></param>
        void RegisterAddProductEvent(EventObserver<ProductDto> observer);

        void DeleteProduct(Guid productId);
        void UpdateProduct(ProductDto productDto);
    }
}