using System;
using System.Collections.Generic;
using CoreServer.Common;
using CoreServer.Common.Core;
using CoreServer.Common.Util;
using CoreServer.Entity.MySqlDb;
using CoreServer.Model;

namespace CoreServer.Service.Provider
{
    [AutoService(typeof(IProductService))]
    public class ProductService : BaseService, IProductService
    {
        private readonly EventObservable<ProductDto> _onAddProduct;
        private EventObserver<ProductDto> _onAddProductObserver;

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

                try
                {
                    db.Ado.BeginTran();

                    productEntity.Id = Guid.NewGuid();
                    var newProduct = MapUtil.Map<ProductDto>(db.Insertable(productEntity).ExecuteReturnEntity());

                    _onAddProduct.Run(() => newProduct);
                    
                    db.Ado.CommitTran();

                    return newProduct;
                }
                catch (Exception e)
                {
                    db.Ado.RollbackTran();
                    throw e;
                }
            });
        }

        public void RegisterAddProductEvent(EventObserver<ProductDto> observer)
        {
            if (_onAddProductObserver == null)
            {
                _onAddProductObserver = observer;
                _onAddProductObserver.Subscribe(_onAddProduct);
            }
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