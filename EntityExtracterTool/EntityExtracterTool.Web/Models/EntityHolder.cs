using System.Collections.Generic;

namespace EntityExtracterTool.Web.Models
{
    public class EntityHolder
    {
        public ICollection<Entity> AddedEntities { get; set; }

        public ICollection<Entity> UpdatedEntities { get; set; }

        public ICollection<Entity> RemovedEntities { get; set; }
    }
}
