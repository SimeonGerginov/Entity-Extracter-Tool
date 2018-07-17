using System.Collections.Generic;
using EntityExtracterTool.Web.Models;

namespace EntityExtracterTool.Web.Providers
{
    public static class EntityHolderProvider
    {
        private static EntityHolder entityHolder;

        public static EntityHolder GetEntityHolder()
        {
            if (entityHolder == null)
            {
                entityHolder = new EntityHolder()
                {
                    AddedEntities = new List<Entity>(),
                    UpdatedEntities = new List<Entity>(),
                    RemovedEntities = new List<Entity>()
                };
            }

            return entityHolder;
        }
    }
}
