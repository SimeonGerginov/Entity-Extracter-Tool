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

        public IEnumerable<Assembly> GetSitefinityAssemblies(string sitefinityVersion)
        {
            var assemblies = new List<Assembly>();
            var dllsInDirectory = this.GetDllsInDirectory(sitefinityVersion);

            foreach (var dll in dllsInDirectory)
            {
                var assembly = Assembly.LoadFile(dll);

                if (assembly.FullName.Contains(Constants.SitefinityAssemblyName))
                {
                    assemblies.Add(assembly);
                }
            }

            return assemblies;
        }
    }
}
