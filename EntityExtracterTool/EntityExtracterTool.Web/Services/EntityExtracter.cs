using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Bytes2you.Validation;

using EntityExtracterTool.Web.Models;
using EntityExtracterTool.Web.Services.Contracts;

using Telerik.Sitefinity.Localization;

namespace EntityExtracterTool.Web.Services
{
    public class EntityExtracter : IEntityExtracter
    {
        private readonly IAssemblyProvider assemblyProvider;

        public EntityExtracter(IAssemblyProvider assemblyProvider)
        {
            Guard.WhenArgument(assemblyProvider, "Assembly Provider").IsNull().Throw();

            this.assemblyProvider = assemblyProvider;
        }

        public IEnumerable<Entity> ExtractEntitiesFromSitefinity(string sitefinityVersion)
        {
            var entities = new List<Entity>();
            var assemblies = this.assemblyProvider.GetSitefinityAssemblies(sitefinityVersion);

            foreach (var assembly in assemblies)
            {
                var entitiesOfAssembly = this.ExtractEntitiesFromAssembly(assembly);
                entities.AddRange(entitiesOfAssembly);
            }

            return entities;
        }

        private IEnumerable<Entity> ExtractEntitiesFromAssembly(Assembly assembly)
        {
            var entitiesOfAssembly = new List<Entity>();
            var types = this.GetTypesInAssembly(assembly);

            foreach (var type in types)
            {
                var properties = this.GetPropertiesOfType(type);
                var entitiesOfType = this.ExtractEntitiesFromType(properties);

                entitiesOfAssembly.AddRange(entitiesOfType);
            }

            return entitiesOfAssembly;
        }

        private IEnumerable<Entity> ExtractEntitiesFromType(PropertyInfo[] properties)
        {
            var entites = new List<Entity>();

            foreach (var property in properties)
            {
                var attributeType = typeof(ResourceEntryAttribute);
                var inherit = false;
                var resource = (ResourceEntryAttribute) property
                    .GetCustomAttributes(attributeType, inherit)
                    .FirstOrDefault();

                if (resource != null)
                {
                    var entity = new Entity()
                    {
                        Name = property.Name,
                        Key = resource.Key,
                        Value = resource.Value,
                        Description = resource.Description,
                        LastModified = resource.LastModified
                    };

                    entites.Add(entity);
                }
            }

            return entites;
        }

        private Type[] GetTypesInAssembly(Assembly assembly)
        {
            Guard.WhenArgument(assembly, "Assembly").IsNull().Throw();

            var types = assembly.GetTypes();

            return types;
        }

        private PropertyInfo[] GetPropertiesOfType(Type type)
        {
            Guard.WhenArgument(type, "Type").IsNull().Throw();

            var properties = type.GetProperties();

            return properties;
        }
    }
}
