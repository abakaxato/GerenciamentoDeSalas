using System;
using System.Collections.Generic;

namespace GerenciamentoSalas.API.Models;

public partial class Paciente
{
    public int Id { get; set; }

    public int ResponsavelId { get; set; }

    public string Nome { get; set; } = null!;

    public DateTime? DataNascimento { get; set; }

    public DateTime DataCadastro { get; set; }

    public virtual ICollection<Agendamento> Agendamentos { get; set; } = new List<Agendamento>();

    public virtual Responsavei Responsavel { get; set; } = null!;
}
