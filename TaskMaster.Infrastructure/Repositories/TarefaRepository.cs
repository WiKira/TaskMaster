using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TaskMaster.Domain.Entities;
using TaskMaster.Domain.Interfaces;
using TaskMaster.Infrastructure.Data;

namespace TaskMaster.Infrastructure.Repositories
{
    public class TarefaRepository : ITarefaRepository
    {
        private readonly ApplicationDbContext _context;

        public TarefaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Tarefa>> GetAllAsync()
        {
            return await _context.Tarefas.ToListAsync();
        }

        public async Task<Tarefa> GetByIdAsync(int id)
        {
            return await _context.Tarefas.FindAsync(id);
        }

        public async Task AddAsync(Tarefa tarefa)
        {
            _context.Tarefas.Add(tarefa);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Tarefa tarefa)
        {
            _context.Tarefas.Update(tarefa);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var product = await _context.Tarefas.FindAsync(id);
            if (product != null)
            {
                _context.Tarefas.Remove(product);
                await _context.SaveChangesAsync();
            }
        }
    }
}