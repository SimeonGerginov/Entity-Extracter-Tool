using EntityExtracterTool.Web.Models;

namespace EntityExtracterTool.Web.Services.Contracts
{
    public interface IEntityComparer
    {
        void CompareEntities(EntityHolder entityHolder, string previousVersion, string currentVersion);
    }
}
