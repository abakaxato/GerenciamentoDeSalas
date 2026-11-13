using System;
using System.Collections.Generic;

namespace GerenciamentoSalas.API.Models.DTOs;

public partial class SalaDto
{
    public int Id { get; set; }

    public string Nome { get; set; } = null!;

    public int? Capacidade { get; set; }

    public virtual ICollection<AgendamentoDto> Agendamentos { get; set; } = new List<AgendamentoDto>();
    public override string ToString()
    {
        return Id.ToString();
    }
}
