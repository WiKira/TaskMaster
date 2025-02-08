using System.ComponentModel.DataAnnotations;
using System.Formats.Tar;
using System.Net;
using TaskMaster.Application.DTOs.Shared;
using TaskMaster.Application.DTOs.Tarefas;
using TaskMaster.Application.Interfaces;
using TaskMaster.Domain.Entities;
using TaskMaster.Domain.Interfaces;

namespace TaskMaster.Application.Services
{
    public class TarefaService : ITarefaService
    {
        private readonly ITarefaRepository _tarefaRepository;

        public TarefaService(ITarefaRepository tarefaRepository)
        {
            _tarefaRepository = tarefaRepository;
        }

        public async Task<ResponseDTO<TarefaResponseDTO>> GetAllTarefasAsync()
        {
            var tarefas = await _tarefaRepository.GetAllAsync();
            
            if (tarefas.Count() == 0 || tarefas == null) {
                throw new KeyNotFoundException("Nenhuma tarefa encontrada.");
            }

            var tarefasResponse = new List<TarefaResponseDTO>();

            foreach (var tarefa in tarefas)
            {
                tarefasResponse.Add(
                    new TarefaResponseDTO( 
                        tarefa.Id,
                        tarefa.Titulo,
                        tarefa.Descricao, 
                        tarefa.DataCriacao, 
                        tarefa.DataConclusao, 
                        tarefa.Status
                    )
                );   
            }

            var retorno = new ResponseDTO<TarefaResponseDTO>(
                true,
                HttpStatusCode.OK, 
                new List<string>() {"Consulta realizada com sucesso."}, 
                tarefasResponse
            );
            return retorno;
        }

        public async Task<ResponseDTO<TarefaResponseDTO>> GetTarefasByIdAsync(int id)
        {
            var tarefa = await _tarefaRepository.GetByIdAsync(id);

            if (tarefa == null) {
                throw new KeyNotFoundException("Nenhuma tarefa encontrada com o Id informado.");
            }        
    
            var retorno = new ResponseDTO<TarefaResponseDTO>(
                true,
                HttpStatusCode.OK, 
                new List<string>() {"Consulta realizada com sucesso."}, 
                new List<TarefaResponseDTO>() { 
                    new TarefaResponseDTO( 
                        tarefa.Id,
                        tarefa.Titulo,
                        tarefa.Descricao, 
                        tarefa.DataCriacao, 
                        tarefa.DataConclusao, 
                        tarefa.Status
                    )
                }
            );
            return retorno;
        }
        
        public async Task<ResponseDTO<TarefaResponseDTO>> AddTarefasAsync(TarefaInsertDTO tarefaInsertDTO)
        {
            var tarefa = new Tarefa
            {
                Titulo = tarefaInsertDTO.Titulo,
                Descricao = tarefaInsertDTO.Descricao,
                DataCriacao = DateTime.Now,
                Status = Domain.Enums.Status.Pendente
            };

            try
            {
                await _tarefaRepository.AddAsync(tarefa);
            }
            catch(Exception ex){
                throw new ApplicationException("Erro ao criar tarefa.", ex);
            }

            var retorno = new ResponseDTO<TarefaResponseDTO>(
                true,
                HttpStatusCode.OK, 
                new List<string>() {"Tarefa criada com sucesso."}, 
                new List<TarefaResponseDTO>() { 
                    new TarefaResponseDTO( 
                        tarefa.Id,
                        tarefa.Titulo,
                        tarefa.Descricao, 
                        tarefa.DataCriacao, 
                        tarefa.DataConclusao, 
                        tarefa.Status
                    )
                }
            );

            return retorno;
        }

        public async Task<ResponseDTO<TarefaResponseDTO>> UpdateTarefasAsync(TarefaUpdateDTO tarefaUpdateDTO)
        {
            var tarefa = await _tarefaRepository.GetByIdAsync(tarefaUpdateDTO.Id);
            
            if (tarefa == null) {
                throw new KeyNotFoundException("Nenhuma tarefa encontrada com o Id informado.");
            }

            tarefa.Titulo = tarefaUpdateDTO.Titulo;
            tarefa.Descricao = tarefaUpdateDTO.Descricao;
            tarefa.DataConclusao = tarefaUpdateDTO.DataConclusao;
            tarefa.Status = tarefaUpdateDTO.Status;

            if (tarefa.DataConclusao != null && tarefa.DataCriacao > tarefa.DataConclusao) {
                throw new ValidationException("A data de conclusão da tarefa não pode ser menor que a data de criacao.");
            }

            try
            {
                await _tarefaRepository.UpdateAsync(tarefa);
            }
            catch (Exception ex){
                throw new ApplicationException("Erro ao atualizar tarefa.", ex);
            }

            var retorno = new ResponseDTO<TarefaResponseDTO>(
                true,
                HttpStatusCode.OK, 
                new List<string>() {"Tarefa atualizada com sucesso."}, 
                new List<TarefaResponseDTO>() { 
                    new TarefaResponseDTO( 
                        tarefa.Id,
                        tarefa.Titulo,
                        tarefa.Descricao, 
                        tarefa.DataCriacao, 
                        tarefa.DataConclusao, 
                        tarefa.Status
                    )
                }
            );
            return retorno;
        }

        public async Task<ResponseDTO<TarefaResponseDTO>> DeleteTarefasAsync(int id)
        {
            var tarefa = _tarefaRepository.GetByIdAsync(id).Result;
            
            if (tarefa == null) {
                throw new KeyNotFoundException("Nenhuma tarefa encontrada com o Id informado.");
            }

            try
            {
                await _tarefaRepository.DeleteAsync(id);
            }
            catch (Exception ex){
                throw new ApplicationException("Erro ao excluir tarefa.", ex);
            }

            var retorno = new ResponseDTO<TarefaResponseDTO>(
                true,
                HttpStatusCode.OK, 
                new List<string>() {"Tarefa excluida com sucesso."}, 
                new List<TarefaResponseDTO>() { }
            );

            return retorno;
        }
    }
}