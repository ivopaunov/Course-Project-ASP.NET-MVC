﻿namespace H8QMedia.Web.Areas.Admin.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.Script.Serialization;
    using H8QMedia.Common;
    using H8QMedia.Data.Models;
    using H8QMedia.Services.Data.Contracts;
    using H8QMedia.Web.Controllers;
    using H8QMedia.Web.Infrastructure.Mapping;
    using H8QMedia.Web.ViewModels.Article;
    using Kendo.Mvc.Extensions;
    using Kendo.Mvc.UI;

    [Authorize(Roles = ApplicationRoles.Admin)]
    public class ArticlesController : KendoAdminGridBaseController
    {
        private readonly IArticlesService articles;

        public ArticlesController(IArticlesService articles)
        {
            this.articles = articles;
        }

        // GET: Admin/Articles
        public ActionResult Index()
        {
            return this.View();
        }

        public ActionResult Read([DataSourceRequest]DataSourceRequest request)
        {
            var data = this.articles.GetAll()
                .To<ArticleInputModel>()
                .ToList();

            var serializer = new JavaScriptSerializer();
            var result = new ContentResult();
            serializer.MaxJsonLength = Int32.MaxValue; 
            result.Content = serializer.Serialize(data.ToDataSourceResult(request));
            result.ContentType = "application/json";
            return result;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create([DataSourceRequest]DataSourceRequest request, ArticleInputModel model)
        {
            if (model != null && this.ModelState.IsValid)
            {
                var currentUserId = this.UserProfile.Id;
                var newArticle = this.Mapper.Map<Article>(model);
                newArticle.AuthorId = currentUserId;

                var modelId = this.articles.Create(newArticle);

                model.Id = modelId;
                model.AuthorId = currentUserId;
                model.AuthorName = this.UserProfile.UserName;
                return this.GridOperation(model, request);
            }

            return null;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Update([DataSourceRequest]DataSourceRequest request, ArticleInputModel model)
        {
            if (model != null && this.ModelState.IsValid)
            {
                var updated = this.Mapper.Map<Article>(model);
                updated.Images = null;

                this.articles.Update(model.Id, updated);

                return this.GridOperation(model, request);
            }

            return null;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Destroy([DataSourceRequest]DataSourceRequest request, ArticleInputModel model)
        {
            if (ModelState.IsValid && model != null)
            {
                this.articles.Destroy(model.Id, null);
            }

            return this.GridOperation(model, request);
        }
    }
}