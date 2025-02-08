using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskMaster.Domain.Enums;

namespace TaskMaster.Domain.Entities
{
    public class Tarefa
    {
        public int Id { get; set; }
        public string? Titulo { get; set; }
        public string? Descricao { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataConclusao { get; set; }
        public Status Status { get; set; }
    }
}