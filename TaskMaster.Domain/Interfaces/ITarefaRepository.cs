using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskMaster.Domain.Entities;

namespace TaskMaster.Domain.Interfaces
{
    public interface ITarefaRepository
    {
        Task<IEnumerable<Tarefa>> GetAllAsync();
        Task<Tarefa> GetByIdAsync(int id);
        Task AddAsync(Tarefa tarefa);
        Task UpdateAsync(Tarefa tarefa);
        Task DeleteAsync(int id);
    }
}