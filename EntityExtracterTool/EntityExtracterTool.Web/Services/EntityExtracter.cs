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

            foreach (var assembly in assemblies.Values)
            {
                var entitiesOfAssembly = this.ExtractEntitiesFromAssembly(assembly);
                entities.AddRange(entitiesOfAssembly);
            }

            return entities;
        }

        private IEnumerable<Entity> ExtractEntitiesFromAssembly(Assembly assembly)
        {
            var entitiesOfAssembly = new List<Entity>();
            var assemblyVersion = assembly.GetName().Version.ToString();
            var types = this.GetTypesInAssembly(assembly);

            foreach (var type in types)
            {
                var properties = this.GetPropertiesOfType(type);
                var entitiesOfType = this.ExtractEntitiesFromType(properties, assemblyVersion);

                entitiesOfAssembly.AddRange(entitiesOfType);
            }

            return entitiesOfAssembly;
        }

        private IEnumerable<Entity> ExtractEntitiesFromType(PropertyInfo[] properties, string assemblyVersion)
        {
            var entites = new List<Entity>();

            foreach (var property in properties)
            {
                var attributeName = "ResourceEntry";
                var inherit = false;
                var resource = property
                    .GetCustomAttributes(inherit)
                    .FirstOrDefault(attr => attr.GetType().Name.Contains(attributeName));

                if (resource != null)
                {
                    var entity = new Entity()
                    {
                        AssemblyVersion = assemblyVersion,
                        Name = property.Name,
                        //Key = resource.Key,
                        //Value = resource.Value,
                        //Description = resource.Description,
                        //LastModified = resource.LastModified
                    };

                    this.SetEntityValues(entity, resource);

                    entites.Add(entity);
                }
            }

            return entites;
        }

        private void SetEntityValues(Entity entity, object resource)
        {
            var resourceType = resource.GetType();
            var entityType = entity.GetType();

            var resourceKeyProperty = resourceType.GetProperty("Key").GetValue(resource);
            var resourceValueProperty = resourceType.GetProperty("Value").GetValue(resource);
            var resourceDescriptionProperty = resourceType.GetProperty("Description").GetValue(resource);
            var resourceLastModifiedProperty = resourceType.GetProperty("LastModified").GetValue(resource);

            var entityKeyProperty = entityType.GetProperty("Key");
            var entityValueProperty = entityType.GetProperty("Value");
            var entityDescriptionProperty = entityType.GetProperty("Description");
            var entityLastModifiedProperty = entityType.GetProperty("LastModified");

            entityKeyProperty.SetValue(entity, resourceKeyProperty);
            entityValueProperty.SetValue(entity, resourceValueProperty);
            entityDescriptionProperty.SetValue(entity, resourceDescriptionProperty);
            entityLastModifiedProperty.SetValue(entity, resourceLastModifiedProperty);
        }

        private Type[] GetTypesInAssembly(Assembly assembly)
        {
            Guard.WhenArgument(assembly, "Assembly").IsNull().Throw();

            Type[] types;

            try
            {
                types = assembly
                    .GetTypes()
                    .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(Resource)))
                    .ToArray();

            }
            catch (ReflectionTypeLoadException e)
            {
                var loaderExceptions = e.LoaderExceptions;
                types = e.Types
                    .Where(t => t != null && t.IsClass && !t.IsAbstract && t.Name.Contains("Resources"))
                    .ToArray();
            }

            return types;
        }

        private PropertyInfo[] GetPropertiesOfType(Type type)
        {
            Guard.WhenArgument(type, "Type").IsNull().Throw();

            var inherit = false;
            var attributeName = "ResourceEntry";

            var properties = type
                .GetProperties()
                .Where(p => p.CustomAttributes.Any() && 
                            p.GetCustomAttributes(inherit)
                                .Any(attr => attr.GetType().Name.Contains(attributeName)))
                .ToArray();

            return properties;
        }
    }
}
