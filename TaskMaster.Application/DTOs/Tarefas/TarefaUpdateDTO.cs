using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TaskMaster.Domain.Enums;

namespace TaskMaster.Application.DTOs.Tarefas
{
    public class TarefaUpdateDTO
    {
        [Required(ErrorMessage = "Id e obrigatório")]
        public int Id { get; set;}
        [Required(ErrorMessage = "Titulo e obrigatório")]
        [MaxLength(100, ErrorMessage = "Tamanho maximo de 100 caracteres")]
        public string? Titulo { get; set; }
        public string? Descricao { get; set; }
        public DateTime? DataConclusao { get; set; }
        [Required(ErrorMessage ="Status e obrigatorio")]
        [EnumDataType(typeof(Status), ErrorMessage = "Status invalido")]
        public Status Status { get; set; }

        public TarefaUpdateDTO() { }

        public TarefaUpdateDTO(int id, string? titulo, string? descricao, DateTime? dataConclusao, int status)
        {
            Id = id;
            Titulo = titulo;
            Descricao = descricao;
            DataConclusao = dataConclusao;
            Status = (Status)status;
        }
    }
}