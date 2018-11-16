using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.ApiModels.ReturnModels
{
    /// <summary>
    /// 风险问卷
    /// </summary>
    public class RiskQuestionnaireModel
    {
        public int Qid { get; set; }

        public string Description { get; set; }

        public List<RiskQuestionnaireAnswerModel> Answer { get; set; }
    }

    public class RiskQuestionnaireAnswerModel
    {
        public int Aid { get; set; }

        public string Description { get; set; }

        /// <summary>
        /// 积分
        /// </summary>
        public decimal Integral { get; set; }
    }
}
