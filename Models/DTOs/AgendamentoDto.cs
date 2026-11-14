using GerenciamentoSalas.API.Enums;

namespace GerenciamentoSalas.API.Models.DTOs;

public partial class AgendamentoDto
{
    public int Id { get; set; }

    public int PacienteId { get; set; }

    public int ProfissionalId { get; set; }

    public int SalaId { get; set; }

    public DateTime DataHoraInicio { get; set; }

    public DateTime DataHoraFim { get; set; }

    public StatusAgendamentoEnum Status { get; set; }

    public string? Observacoes { get; set; }

    public virtual PacienteDto Paciente { get; set; } = null!;

    public virtual ProfissionalDto Profissional { get; set; } = null!;

    public virtual SalaDto Sala { get; set; } = null!;

    public override string ToString()
    {
        return $"Sala : {SalaId}, inicio : {DataHoraInicio}, Fim : {DataHoraFim}, Paciente : {PacienteId}";
    }
}
