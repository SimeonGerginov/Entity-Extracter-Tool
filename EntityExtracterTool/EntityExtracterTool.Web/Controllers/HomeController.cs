using System.Collections.Generic;
using System.Web.Mvc;

using Bytes2you.Validation;

using EntityExtracterTool.Web.Common;
using EntityExtracterTool.Web.Models;
using EntityExtracterTool.Web.Services.Contracts;

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

        public ActionResult ShowEntityInformation()
        {
            var entityHolder = new EntityHolder()
            {
                AddedEntities = new List<Entity>(),
                UpdatedEntities = new List<Entity>(),
                RemovedEntities = new List<Entity>()
            };

            this.entityComparer.CompareEntities(entityHolder, Constants.PreviousVersion, Constants.CurrentVersion);

            return this.View(entityHolder);
        }
    }
}
