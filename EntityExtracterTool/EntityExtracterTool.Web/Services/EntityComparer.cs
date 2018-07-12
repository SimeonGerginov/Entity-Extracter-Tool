using System.Collections.Generic;
using System.Linq;

using EntityExtracterTool.Web.Models;
using EntityExtracterTool.Web.Services.Contracts;

namespace EntityExtracterTool.Web.Services
{
    public class EntityComparer : IEntityComparer
    {
        private readonly IEntityExtracter entityExtracter;

        public EntityComparer(IEntityExtracter entityExtracter)
        {
            this.entityExtracter = entityExtracter;
        }

        public void CompareEntities(EntityHolder entityHolder, string previousVersion, string currentVersion)
        {
            var entitiesFromPreviousVersion = this.entityExtracter
                .ExtractEntitiesFromSitefinity(previousVersion)
                .ToList();

            var entitiesFromCurrentVersion = this.entityExtracter
                .ExtractEntitiesFromSitefinity(currentVersion)
                .ToList();

            var addedEntities = entityHolder.AddedEntities;
            var updatedEntities = entityHolder.UpdatedEntities;
            var removedEntities = entityHolder.RemovedEntities;

            foreach (var entity in entitiesFromPreviousVersion)
            {
                this.CheckIfEntityIsUpdated(entity, entitiesFromCurrentVersion, updatedEntities);
                this.CheckIfEntityIsRemoved(entity, entitiesFromCurrentVersion, removedEntities);
            }

            foreach (var entity in entitiesFromCurrentVersion)
            {
                this.CheckIfEntityIsAdded(entity, entitiesFromPreviousVersion, addedEntities);
            }
        }

        private void CheckIfEntityIsUpdated(Entity entity,
            ICollection<Entity> entitiesFromCurrentVersion, ICollection<Entity> updatedEntities)
        {
            if (entitiesFromCurrentVersion.Contains(entity))
            {
                var entityFromCurrentVersion = entitiesFromCurrentVersion.First(e => e == entity);

                if (entity.LastModified != entityFromCurrentVersion.LastModified)
                {
                    updatedEntities.Add(entityFromCurrentVersion);
                }
            }
        }

        private void CheckIfEntityIsRemoved(Entity entity,
            ICollection<Entity> entitiesFromCurrentVersion, ICollection<Entity> removedEntities)
        {
            if (!entitiesFromCurrentVersion.Contains(entity))
            {
                removedEntities.Add(entity);
            }
        }

        private void CheckIfEntityIsAdded(Entity entity,
            ICollection<Entity> entitiesFromPreviousVersion, ICollection<Entity> addedEntities)
        {
            if (!entitiesFromPreviousVersion.Contains(entity))
            {
                addedEntities.Add(entity);
            }
        }
    }
}
