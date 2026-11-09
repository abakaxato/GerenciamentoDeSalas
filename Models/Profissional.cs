namespace GerenciamentoSalas.API.Models;

public partial class Profissional
{
    public int Id { get; set; }

    public string Nome { get; set; } = null!;

    public string Especialidade { get; set; } = null!;

    public string? Telefone { get; set; }

    public bool? Ativo { get; set; }

    public virtual ICollection<Agendamento> Agendamentos { get; set; } = new List<Agendamento>();
}
