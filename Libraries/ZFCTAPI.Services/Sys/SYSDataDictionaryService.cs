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
    public interface ISYSDataDictionaryService : IRepository<SYS_data_dictionary>
    {
    }

    public class SYSDataDictionaryService : Repository<SYS_data_dictionary>, ISYSDataDictionaryService
    {
    }
}