using System;
using System.Collections.Generic;

namespace GerenciamentoSalas.API.Models;

public partial class Responsavei
{
    public int Id { get; set; }

    public string Nome { get; set; } = null!;

    public string Telefone { get; set; } = null!;

    public string? Email { get; set; }

    public DateTime DataCadastro { get; set; }

    public virtual ICollection<Paciente> Pacientes { get; set; } = new List<Paciente>();
}
