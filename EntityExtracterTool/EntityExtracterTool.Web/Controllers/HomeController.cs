using System.Linq;
using System.Web.Mvc;

using EntityExtracterTool.Web.Common;
using EntityExtracterTool.Web.Providers;

using PagedList;

namespace EntityExtracterTool.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return this.View();
        }

        public ActionResult AddedEntities(int? page)
        {
            var entityHolder = EntityHolderProvider.GetEntityHolder();

            var addedEntities = entityHolder
                .AddedEntities
                .OrderByDescending(e => e.LastModified);

            var pageNumber = page ?? Constants.DefaultPage;
            var entitiesPerPage = addedEntities.ToPagedList(pageNumber, Constants.EntitiesPerPage);

            return this.View(entitiesPerPage);
        }

        public ActionResult UpdatedEntities(int? page)
        {
            var entityHolder = EntityHolderProvider.GetEntityHolder();

            var updatedEntities = entityHolder
                .UpdatedEntities
                .OrderByDescending(e => e.LastModified);

            var pageNumber = page ?? Constants.DefaultPage;
            var entitiesPerPage = updatedEntities.ToPagedList(pageNumber, Constants.EntitiesPerPage);

            return this.View(entitiesPerPage);
        }

        public ActionResult RemovedEntities(int? page)
        {
            var entityHolder = EntityHolderProvider.GetEntityHolder();

            var removedEntities = entityHolder
                .RemovedEntities
                .OrderByDescending(e => e.LastModified);

            var pageNumber = page ?? Constants.DefaultPage;
            var entitiesPerPage = removedEntities.ToPagedList(pageNumber, Constants.EntitiesPerPage);

            return this.View(entitiesPerPage);
        }
    }
}
