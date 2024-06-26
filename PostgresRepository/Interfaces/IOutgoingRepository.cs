﻿using Core.Models;

namespace PostgresRepository.Interfaces
{
    public interface IOutgoingRepository
    {
        Task<List<OutgoingMail>> GetAllOutputMail();

        OutgoingMail GetOutgoingMailById(int id);
    }
}
