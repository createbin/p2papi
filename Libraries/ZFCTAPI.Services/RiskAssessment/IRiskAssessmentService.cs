using Dapper;
using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZFCTAPI.Data.RiskAssessment;
using ZFCTAPI.Services.Repositorys;

namespace ZFCTAPI.Services.RiskAssessment
{
    public interface IRiskAssessmentService : IRepository<RiskQuestion>
    {
        /// <summary>
        /// 获取最大版本
        /// </summary>
        /// <returns></returns>
        int GetMaxVersion();

        /// <summary>
        /// 根据版本获取问题
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        List<RiskQuestion> GetQuestionsByVersion(int version);

        /// <summary>
        /// 获取问题答案
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        List<RiskAnswer> GetRiskAnswer(int id);

    }

    public class RiskAssessmentService : Repository<RiskQuestion>, IRiskAssessmentService
    {

        public int GetMaxVersion()
        {
            var sqlStr = "SELECT top 1 VersionNo FROM RiskVersion order by VersionNo desc";
            return _Conn.Query<int>(sqlStr).FirstOrDefault();
        }

        public List<RiskQuestion> GetQuestionsByVersion(int version)
        {
            var sqlStr = $"SELECT * FROM RiskQuestion where VersionNo={version} order by Sort ";
            return _Conn.Query<RiskQuestion>(sqlStr).ToList();
        }

        public List<RiskAnswer> GetRiskAnswer(int id)
        {
            var sqlStr = $"SELECT* FROM RiskAnswer WHERE QId = {id} ORDER BY SORT";
            return _Conn.Query<RiskAnswer>(sqlStr).ToList();
        }
    }
}
