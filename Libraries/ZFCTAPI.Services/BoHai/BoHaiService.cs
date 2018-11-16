using System;
using System.Collections.Generic;
using System.Text;
using ZFCTAPI.Core.Configuration;
using ZFCTAPI.Core.Helpers;

namespace ZFCTAPI.Services.BoHai
{
    public interface IBoHaiService
    {
        void PostPageTest();
    }

    public class BoHaiService : IBoHaiService
    {
        public async void PostPageTest()
        {
            var url = ApiEngineToConfiguration.GetAppSettingsUrl("UserAdd");
            var post = new HttpClientPageHelper
            {
                FormName = "companyopenaccount",
                Url = "http://localhost:52599/api/infopublic/ToPage",
                Method = "POST"
            };
            post.Add("hello", "baidu");
            post.Add("username", "123");
            await post.Post();
        }
    }
}