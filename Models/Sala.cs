using System;
using System.Collections.Generic;

namespace GerenciamentoSalas.API.Models;

public partial class Sala
{
    public int Id { get; set; }

    public string Nome { get; set; } = null!;

    public int? Capacidade { get; set; }

    public virtual ICollection<Agendamento> Agendamentos { get; set; } = new List<Agendamento>();
}
