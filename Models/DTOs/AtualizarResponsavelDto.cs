namespace GerenciamentoSalas.API.Models.DTOs;

public partial class AtualizarResponsavelDto
{
    public int Id { get; set; }

    public string? Nome { get; set; } = null!;

    public string? Telefone { get; set; } = null!;

    public string? Email { get; set; }

    public bool Ativo { get; set; } = true;
    public override string ToString()
    {
        return Id.ToString();
    }
}
