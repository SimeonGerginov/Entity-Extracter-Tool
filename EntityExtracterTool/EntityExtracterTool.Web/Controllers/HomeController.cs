using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using Bytes2you.Validation;

using EntityExtracterTool.Web.Common;
using EntityExtracterTool.Web.Models;
using EntityExtracterTool.Web.Services.Contracts;

using PagedList;

namespace EntityExtracterTool.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEntityComparer entityComparer;

        public HomeController(IEntityComparer entityComparer)
        {
            Guard.WhenArgument(entityComparer, "Entity Comparer").IsNull().Throw();

            this.entityComparer = entityComparer;
        }

        public ActionResult Index()
        {
            return this.View();
        }

        public ActionResult AddedEntities(int? page)
        {
            var entityHolder = new EntityHolder()
            {
                AddedEntities = new List<Entity>(),
                UpdatedEntities = new List<Entity>(),
                RemovedEntities = new List<Entity>()
            };

            this.entityComparer.CompareEntities(entityHolder, Constants.PreviousVersion, Constants.CurrentVersion);

            var addedEntities = entityHolder
                .AddedEntities
                .OrderByDescending(e => e.LastModified);

            var pageNumber = page ?? Constants.DefaultPage;
            var entitiesPerPage = addedEntities.ToPagedList(pageNumber, Constants.EntitiesPerPage);

            return this.View(entitiesPerPage);
        }
    }
}
