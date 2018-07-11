using System;
using System.Collections.Generic;
using System.Reflection;

using EntityExtracterTool.Web.Models;

namespace EntityExtracterTool.Web.Services.Contracts
{
    public interface IEntityExtracter
    {
        IEnumerable<Entity> ExtractEntitiesFromSitefinity(string sitefinityVersion);

        IEnumerable<Entity> ExtractEntitiesFromAssembly(Assembly assembly);

        IEnumerable<Entity> ExtractEntitiesFromType(PropertyInfo[] properties);

        Type[] GetTypesInAssembly(Assembly assembly);

        PropertyInfo[] GetPropertiesOfType(Type type);
    }
}
