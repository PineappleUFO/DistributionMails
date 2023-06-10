using Core.Models;

namespace EF.Interfaces
{
    internal interface ITreeRepository
    {
        /// <summary>
        /// Получить дерево письма
        /// </summary>
        Task<List<TreeItem>> GetTreeByMailId(Mail mail);

        /// <summary>
        /// Добавить распределяющего первого уровня в письмо
        /// </summary>
        void AddOneLevelDistributionInMail(Mail mail, User user, DateTime deadline, string resolution, bool isResponible, bool isReplying);

        /// <summary>
        /// Добавить распределяющего далее
        /// </summary>
        void AddDistributionInMail(Mail mail, int treeId, User toUser, DateTime deadline, string resolution, bool isResponible, bool isReplying);

        /// <summary>
        /// Изменить значение "ответсвенный" на противоположное
        /// </summary>
        /// <param name="treeId"></param>
        void SetResponibleInTree(int treeId);

        /// <summary>
        /// Изменить значение "отвечающий" на противоположное
        /// </summary>
        void SetReplyingInTree(int treeId);

        /// <summary>
        /// Удалить исполнителя
        /// </summary>
        void DeleteUserFromTree(int treeId);

        /// <summary>
        /// Выбрать другой срок исполнения
        /// </summary>
        void ChangeDeadline(int treeId,DateTime deadline);

        /// <summary>
        /// Принято
        /// </summary>
        void SetAccept(int treeId);


        /// <summary>
        /// Выполнено
        /// </summary>
        void SetDone(int treeId);
    }
}
