﻿using Microsoft.Extensions.Configuration;
using MRMDataManager.Library.Internal.DataAccess;
using MRMDataManager.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRMDataManager.Library.DataAccess
{
    public class ProductData : IProductData
    {
        private readonly ISqlDataAccess _sql;
        public ProductData(ISqlDataAccess sql)
        {
            _sql = sql;
        }
        public List<ProductModel> GetProducts()
        {
            //// Direct Dependancy
            //SqlDataAccess sql = new SqlDataAccess(_config);
            //var output = sql.LoadData<ProductModel, dynamic>("dbo.spProductGet", new { }, "MRMData");

            // Dependancy Injecton
            var output = _sql.LoadData<ProductModel, dynamic>("dbo.spProductGet", new { }, "MRMData");
            return output;
        }

        public ProductModel GetProductById(int productid)
        {
            var output = _sql.LoadData<ProductModel, dynamic>("dbo.spProductGetById", new { Id = productid }, "MRMData").FirstOrDefault();
            return output;
        }
    }
}
