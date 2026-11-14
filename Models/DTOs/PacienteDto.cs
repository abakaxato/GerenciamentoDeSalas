namespace GerenciamentoSalas.API.Models.DTOs;

public partial class PacienteDto
{
    public int Id { get; set; }

    public int ResponsavelId { get; set; }

    public string Nome { get; set; } = null!;

    public DateTime? DataNascimento { get; set; }

    public DateTime DataCadastro { get; set; }

    public virtual ICollection<AgendamentoDto> Agendamentos { get; set; } = new List<AgendamentoDto>();

    public virtual ResponsavelDto Responsavel { get; set; } = null!;

    public override string ToString()
    {
        return Id.ToString();
    }
}
