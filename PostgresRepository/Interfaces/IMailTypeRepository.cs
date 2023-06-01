using Core.Models;

namespace PostgresRepository.Interfaces
{
    public interface IMailTypeRepository
    {
        /// <summary>
        /// Список доступов к конф. разделам
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<List<MailType>> GetTypesAccessByUser(User user);
    }
}
