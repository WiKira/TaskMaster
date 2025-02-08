using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using TaskMaster.Domain.Enums;

namespace TaskMaster.Application.DTOs.Tarefas
{
    public class TarefaInsertDTO
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "O título é obrigatório")]
        [StringLength(100, ErrorMessage = "O título deve ter no máximo 100 caracteres")]
        public string Titulo { get; set; }
        public string? Descricao { get; set; }

        public TarefaInsertDTO(string titulo, string? descricao)
        {
            Titulo = titulo;
            Descricao = descricao;
        }
    }
}