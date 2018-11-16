using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.ApiModels.SubmitModels
{
    /// <summary>
    /// 问卷积分
    /// </summary>
    public class SQuestionScore : BaseSubmitModel
    {
        public int score { get; set; }

        public List<SQuestionnAnswer> answers { get; set; }
    }

    public class SQuestionnAnswer
    {
        public decimal Score { get; set; }

        public int Aid { get; set; }
    }
}
