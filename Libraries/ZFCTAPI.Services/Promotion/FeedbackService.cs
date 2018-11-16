using System;
using System.Collections.Generic;
using System.Text;
using ZFCTAPI.Data.Promotion;
using ZFCTAPI.Services.Repositorys;

namespace ZFCTAPI.Services.Promotion
{
    /// <summary>
    /// 意见反馈
    /// </summary>
    public interface IFeedbackService : IRepository<tbFeedback>
    {

    }

    public class FeedbackService: Repository<tbFeedback>, IFeedbackService
    {

    }
}
