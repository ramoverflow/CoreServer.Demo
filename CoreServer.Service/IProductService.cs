using System;
using System.Collections.Generic;
using CoreServer.Common.Util;
using CoreServer.Model;

namespace CoreServer.Service
{
    public interface IProductService
    {
        List<ProductDto> GetProducts();
        ProductDto GetProductByName(string name);

        ProductDto AddProduct(ProductDto product);
        void DeleteProduct(Guid productId);
        void UpdateProduct(ProductDto productDto);

        EventObservable<ProductDto> OnAddProduct();
    }
}