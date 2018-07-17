using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

using EntityExtracterTool.Web.Common;
using EntityExtracterTool.Web.Services.Contracts;

namespace EntityExtracterTool.Web.Services
{
    public class AssemblyProvider : IAssemblyProvider
    {
        private readonly IDictionary<string, Assembly> assemblies = new Dictionary<string, Assembly>();

        public string GetDirectoryPath(string sitefinityVersion)
        {
            var firstPath = AppDomain.CurrentDomain.BaseDirectory;
            var secondPath = "..\\";
            var solutionPath = Path.Combine(firstPath, secondPath);
            var directoryPath = Path.GetDirectoryName(solutionPath + Constants.ProjectName + sitefinityVersion + Constants.BinFolderPath);

            return directoryPath;
        }

        public string[] GetDllsInDirectory(string sitefinityVersion)
        {
            var directoryPath = this.GetDirectoryPath(sitefinityVersion);
            var dllsInDirectory = Directory.GetFiles(directoryPath, Constants.DllType);

            return dllsInDirectory;
        }

        public IDictionary<string, Assembly> GetSitefinityAssemblies(string sitefinityVersion)
        {
            AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve += this.ResolveAssembly;
            AppDomain.CurrentDomain.AssemblyResolve += this.ResolveAssembly;

            var appDomain = AppDomain.CreateDomain(sitefinityVersion);

            var dllsInDirectory = this.GetDllsInDirectory(sitefinityVersion);

            foreach (var dll in dllsInDirectory)
            {
                if (dll.Contains(Constants.SitefinityAssemblyName))
                {
                    var assembly = Assembly.LoadFile(dll);
                    this.assemblies.Add(assembly.FullName, assembly);
                }
                //var assembly = Assembly.LoadFile(dll);

                //if (assembly.FullName.Contains(Constants.SitefinityAssemblyName))
                //{
                //    this.assemblies.Add(assembly.FullName, assembly);
                //}
            }

            return this.assemblies;
        }

        private Assembly ResolveAssembly(Object sender, ResolveEventArgs e)
        {
            Assembly result;

            this.assemblies.TryGetValue(e.Name, out result);

            return result;
        }
    }
}
