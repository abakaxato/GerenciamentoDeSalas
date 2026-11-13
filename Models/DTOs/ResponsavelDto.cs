using System;
using System.Collections.Generic;

namespace GerenciamentoSalas.API.Models.DTOs;

public partial class ResponsavelDto
{
    public int Id { get; set; }

    public string Nome { get; set; } = null!;

    public string Telefone { get; set; } = null!;

    public string? Email { get; set; }

    public DateTime DataCadastro { get; set; }

    public virtual ICollection<PacienteDto> Pacientes { get; set; } = new List<PacienteDto>();
    public override string ToString()
    {
        return Id.ToString();
    }
}
