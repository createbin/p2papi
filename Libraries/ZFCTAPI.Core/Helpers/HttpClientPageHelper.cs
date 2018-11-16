using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Http;
using ZFCTAPI.Core.Infrastructure;

namespace ZFCTAPI.Core.Helpers
{
    public class HttpClientPageHelper
    {
        #region http

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHelper _webHelper;
        private readonly NameValueCollection _nameValueCollection;

        #endregion http

        #region Params

        public string Url { get; set; }

        public string Method { get; set; }

        public string FormName { get; set; }

        public string AcceptCharset { get; set; }

        public bool NewInputForEachValue { get; set; }

        public NameValueCollection Params => _nameValueCollection;

        #endregion Params

        public HttpClientPageHelper() : this(EngineContext.Current.Resolve<IHttpContextAccessor>(),
            EngineContext.Current.Resolve<IWebHelper>())
        {
        }

        public HttpClientPageHelper(IHttpContextAccessor httpContextAccessor, IWebHelper webHelper)
        {
            this._nameValueCollection = new NameValueCollection();
            this.Url = "yourownurl";
            this.Method = "post";
            this.FormName = "formName";
            this.AcceptCharset = "GB2312";
            _httpContextAccessor = httpContextAccessor;
            this._webHelper = webHelper;
        }

        /// <summary>
        /// 添加键值对
        /// </summary>
        /// <param name="name">key</param>
        /// <param name="value">value</param>
        public void Add(string name, string value)
        {
            _nameValueCollection.Add(name, value);
        }

        /// <summary>
        /// 添加键值
        /// </summary>
        /// <param name="nameValueCollection"></param>
        public void Add(NameValueCollection nameValueCollection)
        {
            Params.Add(nameValueCollection);
        }

        /// <summary>
        /// Post值到请求页面
        /// </summary>
        public async Task Post()
        {
            _httpContextAccessor.HttpContext.Response.Clear();
            _httpContextAccessor.HttpContext.Response.ContentType= "text/html;charset=UTF-8";
            await _httpContextAccessor.HttpContext.Response.WriteAsync("<html><head>");
            await _httpContextAccessor.HttpContext.Response.WriteAsync("<meta http-equiv=\"Content-Type\" content=\"text/html;charset = UTF-8\">");
            await _httpContextAccessor.HttpContext.Response.WriteAsync("<meta name=\"viewport\" content=\"width=device-width\">");
            await _httpContextAccessor.HttpContext.Response.WriteAsync($"<title>{FormName}</title>");
            await _httpContextAccessor.HttpContext.Response.WriteAsync(string.Format("</head><body onload=\"document.{0}.submit()\">", FormName));

            await _httpContextAccessor.HttpContext.Response.WriteAsync(!string.IsNullOrEmpty(AcceptCharset)
                ? $"<form name=\"{FormName}\" method=\"{Method}\" action=\"{Url}\" accept-charset=\"{AcceptCharset}\">"
                : string.Format("<form name=\"{0}\" method=\"{1}\" action=\"{2}\">", FormName, Method, Url));
            if (NewInputForEachValue)
            {
                foreach (string key in _nameValueCollection.Keys)
                {
                    string[] values = _nameValueCollection.GetValues(key);
                    if (values != null)
                    {
                        foreach (string value in values)
                        {
                            await _httpContextAccessor.HttpContext.Response.WriteAsync(string.Format("<input name=\"{0}\" type=\"hidden\" value=\"{1}\">", HttpUtility.HtmlEncode(key), HttpUtility.HtmlEncode(value)));
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < _nameValueCollection.Keys.Count; i++)
                    await _httpContextAccessor.HttpContext.Response.WriteAsync(string.Format("<input name=\"{0}\" type=\"hidden\" value=\"{1}\">", HttpUtility.HtmlEncode(_nameValueCollection.Keys[i]),HttpUtility.HtmlEncode(_nameValueCollection[_nameValueCollection.Keys[i]])));
            }
            await _httpContextAccessor.HttpContext.Response.WriteAsync("</form>");
            await _httpContextAccessor.HttpContext.Response.WriteAsync("</body></html>");
            
            _webHelper.IsPostBeingDone = true;
        }
    }
}