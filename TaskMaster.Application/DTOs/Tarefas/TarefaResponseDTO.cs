using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskMaster.Domain.Enums;

namespace TaskMaster.Application.DTOs.Tarefas
{
    public class TarefaResponseDTO
    {
        public int Id { get;}
        public string? Titulo { get; set; }
        public string? Descricao { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataConclusao { get; set; }
        public Status Status { get; set; }

        public TarefaResponseDTO(int id, string? titulo, string? descricao, DateTime dataCriacao, DateTime? dataConclusao, Status status)
        {
            Id = id;
            Titulo = titulo;
            Descricao = descricao;
            DataCriacao = dataCriacao;
            DataConclusao = dataConclusao;
            Status = (Status)status;
        }
    }
}