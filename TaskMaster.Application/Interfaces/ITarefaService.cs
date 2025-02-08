using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TaskMaster.Application.DTOs.Shared;
using TaskMaster.Application.DTOs.Tarefas;
using TaskMaster.Domain.Entities;
using TaskMaster.Domain.Interfaces;

namespace TaskMaster.Application.Interfaces
{
    public interface ITarefaService
    {
        Task<ResponseDTO<TarefaResponseDTO>> GetAllTarefasAsync();
        Task<ResponseDTO<TarefaResponseDTO>> GetTarefasByIdAsync(int id);
        Task<ResponseDTO<TarefaResponseDTO>> AddTarefasAsync(TarefaInsertDTO tarefaInsertDTO);
        Task<ResponseDTO<TarefaResponseDTO>> UpdateTarefasAsync(TarefaUpdateDTO tarefaUpdateDTO);
        Task<ResponseDTO<TarefaResponseDTO>> DeleteTarefasAsync(int id);
    }
}