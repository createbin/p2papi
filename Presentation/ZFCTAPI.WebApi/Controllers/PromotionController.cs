using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ZFCTAPI.Services.Promotion;
using Newtonsoft.Json;
using ZFCTAPI.Data.ApiModels.ReturnModels;
using ZFCTAPI.Data.ApiModels.SubmitModels;
using ZFCTAPI.Core.Enums;
using ZFCTAPI.Data.Pages;
using ZFCTAPI.Data.Promotion;
using ZFCTAPI.WebApi.RequestAttribute;
using ZFCTAPI.WebApi.Validates;

namespace ZFCTAPI.WebApi.Controllers
{
    /// <summary>
    /// 推广运营
    /// </summary>
    [Route("api/[controller]/[action]")]
    [RequestLog]
    public class PromotionController : Controller
    {
        #region feild

        private readonly INewsCategoryService _inewsCategoryService;
        private readonly INewsService _inewsService;
        private readonly IAdvertisPositionService _iadvertisPositionService;
        private readonly IAdvertisementService _iadvertisementService;
        private readonly IFeedbackService _ifeedbackService;
        private readonly IInternalMagazineService _iinternalMagazineService;

        public PromotionController(INewsCategoryService inewsCategoryService,
            INewsService inewsService,
            IAdvertisPositionService iadvertisPositionService,
            IAdvertisementService iadvertisementService,
            IFeedbackService ifeedbackService,
            IInternalMagazineService iinternalMagazineService)
        {
            this._inewsCategoryService = inewsCategoryService;
            this._inewsService = inewsService;
            this._iadvertisPositionService = iadvertisPositionService;
            this._iadvertisementService = iadvertisementService;
            this._ifeedbackService = ifeedbackService;
            this._iinternalMagazineService = iinternalMagazineService;
        }

        #endregion

        #region 广告

        /// <summary>
        /// 按条获取广告
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<RAdvertisementCountModel,string> AdvertisementsCount([FromBody]SPromotionCountModel model)
        {
            ReturnModel<RAdvertisementCountModel, string> rModel = new ReturnModel<RAdvertisementCountModel, string>();

            #region 验签

            var signResult = VerifyBase.Sign<SPromotionCountModel>(model);
            if (signResult == ReturnCode.SignatureFailure)
            {
                rModel.Message = "签名错误";
                rModel.ReturnCode = (int)ReturnCode.SignatureFailure;
                rModel.ReturnData = null;
                rModel.Token = model.Token;
                return rModel;
            }

            #endregion 验签
            RAdvertisementCountModel result = new RAdvertisementCountModel();
            var advertisePos= _iadvertisPositionService.GetAdvertPositionByCode(model.Code);
            if (advertisePos != null)
            {
                result.CategoryId = advertisePos.Id;
                result.CategoryName = advertisePos.Title;
                var advertisements = _iadvertisementService.GetAdvertisementsCount(advertisePos.Id, model.Count);
                result.AdvertisementList = advertisements;
                result.Count = advertisements!=null? advertisements.Count:0;
            }
            rModel.Message = "查询成功";
            rModel.ReturnCode = (int)ReturnCode.Success;
            rModel.ReturnData = result;
            return rModel;
        }

        /// <summary>
        /// 分页获取广告
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<RAdvertisementPageModel, string> AdvertisementsPage([FromBody]SPromotionPageModel model)
        {
            ReturnModel<RAdvertisementPageModel, string> rModel = new ReturnModel<RAdvertisementPageModel, string>();

            #region 验签

            var signResult = VerifyBase.Sign<SPromotionPageModel>(model);
            if (signResult == ReturnCode.SignatureFailure)
            {
                rModel.Message = "签名错误";
                rModel.ReturnCode = (int)ReturnCode.SignatureFailure;
                rModel.ReturnData = null;
                rModel.Token = model.Token;
                return rModel;
            }

            #endregion 验签

            RAdvertisementPageModel result = new RAdvertisementPageModel();
            var advertisePos = _iadvertisPositionService.GetAdvertPositionByCode(model.Code);
            if (advertisePos != null)
            {
                result.CategoryId = advertisePos.Id;
                result.CategoryName = advertisePos.Title;
                var advertises = _iadvertisementService.GetAdvertisementsPage(advertisePos.Id, model.Page, model.PageSize);
                if (advertises != null)
                {
                    result.AdvertisementList = new ReturnPageData<AdvertisementDetail>
                    {
                        Total= advertises.TotalNum,
                        TotalPageCount= advertises.TotalPageCount,
                        PagingData = advertises.Items
                    };
                }
            }
            rModel.Message = "查询成功";
            rModel.ReturnData = result;
            rModel.ReturnCode= (int)ReturnCode.Success;
            return rModel;
        }

        //public ReturnModel<tbAdvertising, string> AdvertisementDetail([FromBody]SPromotionDetailModel model)
        //{
        //    ReturnModel<tbAdvertising, string> rModel = new ReturnModel<tbAdvertising, string>();

        //    #region 验签

        //    var signResult = VerifyBase.Sign<SPromotionDetailModel>(model);
        //    if (signResult == ReturnCode.SignatureFailure)
        //    {
        //        rModel.Message = "签名错误";
        //        rModel.ReturnCode = (int)ReturnCode.SignatureFailure;
        //        rModel.ReturnData = null;
        //        rModel.Token = model.Token;
        //        return rModel;
        //    }

        //    #endregion 验签

        //    rModel.ReturnData = _iadvertisementService.Find(model.Id);
        //    rModel.Message = "查询成功";
        //    rModel.ReturnCode = (int)ReturnCode.Success;
        //    return rModel;
        //}

        #endregion

        #region 新闻

        /// <summary>
        /// 按条获取新闻
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<RNewsCountModel, string> NewsCount([FromBody]SPromotionCountModel model)
        {
            ReturnModel<RNewsCountModel, string> rModel = new ReturnModel<RNewsCountModel, string>();

            #region 验签

            var signResult = VerifyBase.Sign<SPromotionCountModel>(model);
            if (signResult == ReturnCode.SignatureFailure)
            {
                rModel.Message = "签名错误";
                rModel.ReturnCode = (int)ReturnCode.SignatureFailure;
                rModel.ReturnData = null;
                rModel.Token = model.Token;
                return rModel;
            }

            #endregion 验签
            RNewsCountModel result = new RNewsCountModel();
            var newsCategory = _inewsCategoryService.GetNewsCategoryByCode(new string[] { model.Code});
            if (newsCategory != null&& newsCategory.Count==1)
            {
                result.CategoryName = newsCategory[0].Name;
                result.CategoryId = newsCategory[0].Id;
                var newsList = _inewsService.GetNewsCount(newsCategory[0].Id, model.Count);
                result.NewsList = newsList;
                result.Count = newsList != null ? newsList.Count : 0;
            }
            rModel.Message = "查询成功";
            rModel.ReturnCode = (int)ReturnCode.Success;
            rModel.ReturnData = result;
            return rModel;
        }

        /// <summary>
        /// 分页获取新闻
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<RNewsPageModel,string> NewsPage([FromBody]SPromotionPageModel model)
        {
            ReturnModel<RNewsPageModel, string> rModel = new ReturnModel<RNewsPageModel, string>();

            #region 验签

            var signResult = VerifyBase.Sign<SPromotionPageModel>(model);
            if (signResult == ReturnCode.SignatureFailure)
            {
                rModel.Message = "签名错误";
                rModel.ReturnCode = (int)ReturnCode.SignatureFailure;
                rModel.ReturnData = null;
                rModel.Token = model.Token;
                return rModel;
            }

            #endregion 验签
            RNewsPageModel result = new RNewsPageModel();
            var newsCategories = _inewsCategoryService.GetNewsCategoryByCode(new string[] { model.Code });
            if (newsCategories != null&& newsCategories.Count==1)
            {
                result.CategoryId = newsCategories[0].Id;
                result.CategoryName = newsCategories[0].Name;
                var newsList = _inewsService.GetNewsPage(new int[] { newsCategories[0].Id },model.Page,model.PageSize);
                result.NewsPage = new ReturnPageData<NewsDetail>()
                {
                    Total=newsList.TotalNum,
                    TotalPageCount=newsList.TotalPageCount,
                    PagingData=newsList.Items
                };
            }
            rModel.Message = "查询成功";
            rModel.ReturnCode = (int)ReturnCode.Success;
            rModel.ReturnData = result;
            return rModel;
        }

        /// <summary>
        /// APP动态资讯
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<RNewsPageModel,string> AppActivitiesPage([FromBody]BasePageModel model)
        {
            ReturnModel<RNewsPageModel, string> rModel = new ReturnModel<RNewsPageModel, string>();

            #region 验签

            var signResult = VerifyBase.Sign<BasePageModel>(model);
            if (signResult == ReturnCode.SignatureFailure)
            {
                rModel.Message = "签名错误";
                rModel.ReturnCode = (int)ReturnCode.SignatureFailure;
                rModel.ReturnData = null;
                rModel.Token = model.Token;
                return rModel;
            }

            #endregion 验签

            RNewsPageModel result = new RNewsPageModel();
            string[] strArr = { "PresidentWords", "Industrydynamic", "CompanyActivity" };// '总裁专栏' '行业动态' '集团动态'关键字
            var newsCategories = _inewsCategoryService.GetNewsCategoryByCode(strArr);
            if(newsCategories!=null&& newsCategories.Count > 0)
            {
                List<int> ids = new List<int>();//存类别ID
                foreach(var newsCategory in newsCategories)
                {
                    ids.Add(newsCategory.Id);
                    result.CategoryName += newsCategory.Name+",";
                }
                result.CategoryName = string.IsNullOrEmpty(result.CategoryName)?"":result.CategoryName.Substring(0, result.CategoryName.Length - 1);//截取最后一个逗号

                var newsList = _inewsService.GetNewsPage(ids.ToArray(), model.Page, model.PageSize);//查询类别ID集合中的新闻

                result.NewsPage = new ReturnPageData<NewsDetail>()
                {
                    Total = newsList.TotalNum,
                    TotalPageCount = newsList.TotalPageCount,
                    PagingData = newsList.Items
                };
            }
            rModel.Message = "查询成功";
            rModel.ReturnCode = (int)ReturnCode.Success;
            rModel.ReturnData = result;
            return rModel;
        }

        /// <summary>
        /// 新闻详情
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<News,string> NewsDetail([FromBody]SDetailModel model)
        {
            ReturnModel<News, string> rModel = new ReturnModel<News, string>();

            #region 验签

            var signResult = VerifyBase.Sign<SDetailModel>(model);
            if (signResult == ReturnCode.SignatureFailure)
            {
                rModel.Message = "签名错误";
                rModel.ReturnCode = (int)ReturnCode.SignatureFailure;
                rModel.ReturnData = null;
                rModel.Token = model.Token;
                return rModel;
            }

            #endregion 验签

            var result = _inewsService.Find(model.Id);
            rModel.Message = "查询成功";
            rModel.ReturnCode = (int)ReturnCode.Success;
            rModel.ReturnData = result;
            return rModel;
        }

        #endregion

        #region 意见反馈

        /// <summary>
        /// 意见反馈
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<bool,string> Feedback([FromBody]SFeedbackModel model)
        {
            ReturnModel<bool, string> rModel = new ReturnModel<bool, string>();

            #region 验签

            var signResult = VerifyBase.Sign<SFeedbackModel>(model);
            if (signResult == ReturnCode.SignatureFailure)
            {
                rModel.Message = "签名错误";
                rModel.ReturnCode = (int)ReturnCode.SignatureFailure;
                rModel.ReturnData = false;
                rModel.Token = model.Token;
                return rModel;
            }

            #endregion 验签

            tbFeedback feedback = new tbFeedback();
            feedback.Content = model.Content;
            feedback.Telephone = model.Phone;
            feedback.Source = model.Source;
            feedback.Feedbacktime = DateTime.Now;
            feedback.IsDel = false;
            feedback.State = (int)FeedbackStateEnum.Handing;

            _ifeedbackService.Add(feedback);
            rModel.Message = "反馈成功";
            rModel.ReturnCode= (int)ReturnCode.Success;
            rModel.ReturnData = true;
            return rModel;
        }

        #endregion

        #region 企业内刊，运营报告
        /// <summary>
        /// 企业内刊，运营报告
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ReturnModel<ReturnPageData<tbInternalMagazine>, string> Managizes([FromBody]SManagizeModel model)
        {
            ReturnModel<ReturnPageData<tbInternalMagazine>, string> rModel = new ReturnModel<ReturnPageData<tbInternalMagazine>, string>();

            #region 验签

            var signResult = VerifyBase.Sign<SManagizeModel>(model);
            if (signResult == ReturnCode.SignatureFailure)
            {
                rModel.Message = "签名错误";
                rModel.ReturnCode = (int)ReturnCode.SignatureFailure;
                rModel.ReturnData = null;
                rModel.Token = model.Token;
                return rModel;
            }

            #endregion 验签

            ReturnPageData<tbInternalMagazine> result = null;
            //企业内刊分页，运营报告不需要分页
            if (model.Category == 1)
            {
                var managizes = _iinternalMagazineService.GetManagize(model.Category, model.Page, model.PageSize);
                result = new ReturnPageData<tbInternalMagazine>()
                {
                    Total = managizes.TotalNum,
                    TotalPageCount = managizes.TotalPageCount,
                    PagingData = managizes.Items
                };
            }
            else if (model.Category == 2)
            {
                var managizes = _iinternalMagazineService.GetManagizeCount(model.Category, model.Count, model.Year);
                result = new ReturnPageData<tbInternalMagazine>()
                {
                    PagingData = managizes
                };
            }

            rModel.Message = "查询成功";
            rModel.ReturnCode = (int)ReturnCode.Success;
            rModel.ReturnData = result;
            return rModel;
        }

        #endregion
    }
}