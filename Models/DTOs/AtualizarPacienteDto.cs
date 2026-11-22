namespace GerenciamentoSalas.API.Models.DTOs;

public partial class AtualizarPacienteDto
{
    public required int Id { get; set; }
    public int? ResponsavelId { get; set; }
    public string? Nome { get; set; }
    public DateTime? DataNascimento { get; set; }
}
