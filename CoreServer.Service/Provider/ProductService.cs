using System;
using System.Collections.Generic;
using CoreServer.Common;
using CoreServer.Common.Util;
using CoreServer.Entity.MySqlDb;
using CoreServer.Model;

namespace CoreServer.Service.Provider
{
    [AutoService(typeof(IProductService))]
    public class ProductService : BaseService, IProductService
    {
        private readonly EventObservable<ProductDto> _onAddProduct;

        public ProductService() : base(cfg =>
        {
            cfg.CreateMap<Product, ProductDto>();
            cfg.CreateMap<ProductDto, Product>();
        })
        {
            _onAddProduct = new EventObservable<ProductDto>();
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

                var tran = db.Ado.UseTran(() =>
                {
                    productEntity.Id = Guid.NewGuid();
                    var newProduct = MapUtil.Map<ProductDto>(db.Insertable(productEntity).ExecuteReturnEntity());

                    _onAddProduct.Run(() => newProduct);

                    return newProduct;
                });

                return tran.Data;
            });
        }

        public EventObservable<ProductDto> OnAddProduct()
        {
            return _onAddProduct;
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