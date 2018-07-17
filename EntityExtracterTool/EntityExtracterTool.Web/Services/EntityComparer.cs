using System.Collections.Generic;
using System.Linq;

using Bytes2you.Validation;

using EntityExtracterTool.Web.Models;
using EntityExtracterTool.Web.Services.Contracts;

namespace EntityExtracterTool.Web.Services
{
    public class EntityComparer : IEntityComparer
    {
        private readonly IEntityExtracter entityExtracter;

        public EntityComparer(IEntityExtracter entityExtracter)
        {
            Guard.WhenArgument(entityExtracter, "Entity Extracter").IsNull().Throw();

            this.entityExtracter = entityExtracter;
        }

        public void CompareEntities(EntityHolder entityHolder, string previousVersion, string currentVersion)
        {
            Guard.WhenArgument(entityHolder, "Entity Holder").IsNull().Throw();

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
            var entityFromCurrentVersion = entitiesFromCurrentVersion
                .FirstOrDefault(e => e.Key == entity.Key && 
                                     e.Value == entity.Value && 
                                     e.Description == entity.Description);

            if (entityFromCurrentVersion != null)
            {
                updatedEntities.Add(entityFromCurrentVersion);
            }
        }

        private void CheckIfEntityIsRemoved(Entity entity,
            ICollection<Entity> entitiesFromCurrentVersion, ICollection<Entity> removedEntities)
        {
            var entityFromCurrentVersion = entitiesFromCurrentVersion
                .FirstOrDefault(e => e.Key == entity.Key &&
                                     e.Value == entity.Value &&
                                     e.Description == entity.Description);

            if (entityFromCurrentVersion == null)
            {
                removedEntities.Add(entity);
            }
        }

        private void CheckIfEntityIsAdded(Entity entity,
            ICollection<Entity> entitiesFromPreviousVersion, ICollection<Entity> addedEntities)
        {
            var entityFromPreviousVersion = entitiesFromPreviousVersion
                .FirstOrDefault(e => e.Key == entity.Key &&
                                     e.Value == entity.Value &&
                                     e.Description == entity.Description);

            if (entityFromPreviousVersion == null)
            {
                addedEntities.Add(entity);
            }
        }
    }
}
