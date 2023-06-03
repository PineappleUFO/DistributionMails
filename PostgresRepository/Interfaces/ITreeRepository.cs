using Core.Models;

namespace EF.Interfaces
{
    internal interface ITreeRepository
    {
        Task<List<DistributionTreeElement>> GetTreeByMailId(Mail mail);
    }
}
