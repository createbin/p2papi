using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dapper;
using ZFCTAPI.Data.POP;
using ZFCTAPI.Services.Repositorys;

namespace ZFCTAPI.Services.Popular
{
    public interface IPopEnvelopeRuleService:IRepository<POP_envelope_rule>
    {
        /// <summary>
        /// 获取阶梯值
        /// </summary>
        /// <returns></returns>
        List<POP_envelope_rule> GetRedStepValue(int redId);
    }

    public class PopEnvelopeRuleService:Repository<POP_envelope_rule>, IPopEnvelopeRuleService
    {
        public List<POP_envelope_rule> GetRedStepValue(int redId)
        {
            var sqlStence = new StringBuilder();
            sqlStence.Append("select * from POP_envelope_rule");
            sqlStence.Append(" where pop_envelope_id='" + redId + "'");
            sqlStence.Append(" order by pop_envelope_term desc");
            return _Conn.Query<POP_envelope_rule>(sqlStence.ToString()).ToList();
        }
    }
}
