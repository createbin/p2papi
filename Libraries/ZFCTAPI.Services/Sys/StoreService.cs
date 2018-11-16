using Dapper;
using System;
using System.Collections.Generic;
using System.Text;
using ZFCTAPI.Core;
using ZFCTAPI.Data.ApiModels.ReturnModels;
using ZFCTAPI.Data.CST;
using ZFCTAPI.Data.Pages;
using ZFCTAPI.Data.SYS;
using ZFCTAPI.Services.Repositorys;

namespace ZFCTAPI.Services.Sys
{
    public interface IStoreService : IRepository<Store>
    {
    }

    public class StoreService : Repository<Store>, IStoreService
    {
    }
}