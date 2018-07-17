using System.Collections.Generic;
using System.Reflection;

namespace EntityExtracterTool.Web.Services.Contracts
{
    public interface IAssemblyProvider
    {
        string GetDirectoryPath(string sitefinityVersion);

        string[] GetDllsInDirectory(string sitefinityVersion);

        IDictionary<string, Assembly> GetSitefinityAssemblies(string sitefinityVersion);
    }
}
