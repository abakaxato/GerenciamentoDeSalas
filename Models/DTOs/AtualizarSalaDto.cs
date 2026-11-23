namespace GerenciamentoSalas.API.Models.DTOs;

public partial class AtualizarSalaDto
{
    public int Id { get; set; }

    public string? Nome { get; set; } = null!;

    public int? Capacidade { get; set; }

    public override string ToString()
    {
        return Id.ToString();
    }
}
