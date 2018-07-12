using System.Collections.Generic;
using EntityExtracterTool.Web.Models;

namespace EntityExtracterTool.Web.Services.Contracts
{
    public interface IEntityExtracter
    {
        IEnumerable<Entity> ExtractEntitiesFromSitefinity(string sitefinityVersion);
    }
}
