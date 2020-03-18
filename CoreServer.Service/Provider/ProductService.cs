using System;
using System.Collections.Generic;
using CoreServer.Common;
using CoreServer.Entity.MySqlDb;
using CoreServer.Model;

namespace CoreServer.Service.Provider
{
    [AutoService(typeof(IProductService))]
    public class ProductService : BaseService, IProductService
    {
        public ProductService() : base(cfg =>
        {
            cfg.CreateMap<Product, ProductDto>();
            cfg.CreateMap<ProductDto, Product>();
        })
        {
        }

        public List<ProductDto> GetProducts()
        {
            return ProductContext.Execute(db =>
            {
                return db.Queryable<Product>()
                    .Select<ProductDto>()
                    .ToList();
            });
        }

        public ProductDto GetProductByName(string name)
        {
            throw new Exception("测试异常");
        }

        public ProductDto AddProduct(ProductDto product)
        {
            return ProductContext.Execute(db =>
            {
                var productEntity = MapUtil.Map<Product>(product);
                productEntity.Id = Guid.NewGuid();

                return MapUtil.Map<ProductDto>(db.Insertable(productEntity).ExecuteReturnEntity());
            });
        }

        public void DeleteProduct(Guid productId)
        {
            ProductContext.Execute(db =>
            {
                db.Deleteable<Product>()
                    .Where(i => i.Id == productId)
                    .ExecuteCommand();
            });
        }

        public void UpdateProduct(ProductDto productDto)
        {
            ProductContext.Execute(db =>
            {
                db.Updateable<Product>(MapUtil.Map<Product>(productDto))
                    .ExecuteCommand();
            });
        }
    }
}