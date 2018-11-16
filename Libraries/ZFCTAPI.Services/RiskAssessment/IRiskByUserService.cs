using System;
using System.Collections.Generic;
using System.Text;
using ZFCTAPI.Data.RiskAssessment;
using ZFCTAPI.Services.Repositorys;

namespace ZFCTAPI.Services.RiskAssessment
{
    public interface IRiskByUserService : IRepository<RiskByUser>
    {
    }

    public class RiskByUserService : Repository<RiskByUser>, IRiskByUserService
    {

    }
}
