using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Dapper;
using ZFCTAPI.Data.SYS;
using ZFCTAPI.Services.Repositorys;

namespace ZFCTAPI.Services.Sys
{
    public interface IUploadInfoService : IRepository<SYS_uploadInfo>
    {
        List<SYS_uploadInfo> GetUploadInfoAll(int classId, int classType);
    }

    public class UploadInfoService : Repository<SYS_uploadInfo>, IUploadInfoService
    {
        public List<SYS_uploadInfo> GetUploadInfoAll(int classId, int classType)
        {
            var selectSql = $"select * from SYS_uploadInfo where file_ClassId='{classId}' and file_type='{classType}'";
            if(_Conn.State==ConnectionState.Closed)
                Open();
            return _Conn.Query<SYS_uploadInfo>(selectSql).ToList();
        }
    }
}