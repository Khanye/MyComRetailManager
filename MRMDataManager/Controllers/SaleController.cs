﻿using Microsoft.AspNet.Identity;
using MRMDataManager.Library.DataAccess;
using MRMDataManager.Library.Models;
using MRMDataManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MRMDataManager.Controllers
{
    [Authorize]
    public class SaleController : ApiController
    {
        public void Post(SaleModel sale)
        {
            SaleData data = new SaleData();
            string userid = RequestContext.Principal.Identity.GetUserId();

            data.SaveSale(sale, userid);
        }
    }
}