namespace GerenciamentoSalas.API.Models.DTOs;

public partial class AtualizarPacienteRequest
{
    public int? ResponsavelId { get; set; }
    public string? Nome { get; set; } = null!;
    public DateTime? DataNascimento { get; set; }
}
