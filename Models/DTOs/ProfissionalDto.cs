namespace GerenciamentoSalas.API.Models.DTOs;

public partial class ProfissionalDto
{
    public int Id { get; set; }

    public string Nome { get; set; } = null!;

    public string Especialidade { get; set; } = null!;

    public string? Telefone { get; set; }

    public bool? Ativo { get; set; }

    public virtual ICollection<AgendamentoDto> Agendamentos { get; set; } = new List<AgendamentoDto>();
    public override string ToString()
    {
        return Id.ToString();
    }
}
