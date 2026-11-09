using System;
using System.Collections.Generic;

namespace GerenciamentoSalas.API.Models;

public partial class Agendamento
{
    public int Id { get; set; }

    public int PacienteId { get; set; }

    public int ProfissionalId { get; set; }

    public int SalaId { get; set; }

    public DateTime DataHoraInicio { get; set; }

    public DateTime DataHoraFim { get; set; }

    public string? Status { get; set; }

    public string? Observacoes { get; set; }

    public virtual Paciente Paciente { get; set; } = null!;

    public virtual Profissionai Profissional { get; set; } = null!;

    public virtual Sala Sala { get; set; } = null!;

    public override string ToString()
    {
        return $"Sala : {SalaId}, inicio : {DataHoraInicio}, Fim : {DataHoraFim}, Paciente : {PacienteId}";
    }
}
