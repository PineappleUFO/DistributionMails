using Core.Models;

namespace EF.Interfaces
{
    internal interface ITreeRepository
    {
        /// <summary>
        /// Получить дерево письма
        /// </summary>
        Task<List<DistributionTreeElement>> GetTreeByMailId(Mail mail);

        /// <summary>
        /// Добавить распределяющего первого уровня в письмо
        /// </summary>
        void AddOneLevelDistributionInMail(Mail mail, User user, DateTime deadline, string resolution, bool isResponible, bool isReplying);
    }
}
