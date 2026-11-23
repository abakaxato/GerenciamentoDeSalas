namespace GerenciamentoSalas.API.Models.DTOs;

public partial class AtualizarProfissionalDto
{
    public int Id { get; set; }
    public string? Nome { get; set; } = null!;
    public string? Especialidade { get; set; } = null!;
    public string? Telefone { get; set; }
    public string? Email { get; set; } = null!;

}
