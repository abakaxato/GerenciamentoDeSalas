using Microsoft.Data.SqlClient;
using GerenciamentoSalas.API.Models;
using GerenciamentoSalas.API.Requests;

namespace GerenciamentoSalas.API.Repositories
{
    public class PacienteRepository
    {
        private readonly string _connectionString;

        public PacienteRepository(IConfiguration configuration) {
            _connectionString = configuration.GetConnectionString("ConexaoPadrao")
               ?? throw new ArgumentNullException("String de conexão não encontrada");
        }

        public List<Paciente> ObterTodosPaciente() {

            var lista = new List<Paciente>();

            using (var conexao = new SqlConnection(_connectionString)) {
                
                string sql = "SELECT Id, ResponsavelId, Nome, DataCadastro, DataNascimento FROM Pacientes";
                
                using (var comando = new SqlCommand(sql, conexao)) {
                    
                    conexao.Open();
                    
                    using (SqlDataReader reader = comando.ExecuteReader()) {
                        
                        while (reader.Read()) {

                            var paci = new Paciente
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                ResponsavelId = Convert.ToInt32(reader["ResponsavelId"]),
                                Nome = reader["Nome"].ToString() ?? string.Empty,
                                DataCadastro = Convert.ToDateTime(reader["DataCadastro"]),
                                DataNascimento = reader["DataNascimento"] != DBNull.Value ? 
                                    Convert.ToDateTime(reader["DataNascimento"]) : null
                            };
                            lista.Add(paci);
                        }
                    }
                }
            }
            return lista;
        }

        public int InserirPaciente(Paciente paciente) {
            using (var conexao = new SqlConnection(_connectionString))
            {
                string sql = @"INSERT INTO Pacientes (ResponsavelId, Nome, DataNascimento) 
                       OUTPUT INSERTED.Id
                       VALUES (@ResponsavelId, @Nome, @DataNascimento);";
                using (var comando = new SqlCommand(sql, conexao)) {
                    comando.Parameters.AddWithValue("@ResponsavelId", paciente.ResponsavelId);
                    comando.Parameters.AddWithValue("@Nome", paciente.Nome);
                    comando.Parameters.AddWithValue("@DataNascimento", paciente.DataNascimento);

                    conexao.Open();

                    int idGerado = Convert.ToInt32(comando.ExecuteScalar());

                    return idGerado;
                }
            }
        }

        public (int AgendamentosDeletados, int IdPacienteDeletado) DeletarPaciente(DeletarPacienteRequest deletarPacienteRequest)
        {
            using (var conexao = new SqlConnection(_connectionString))
            {
                conexao.Open();

                string sql = @"DELETE dbo.Agendamentos 
                       FROM dbo.Agendamentos 
                       INNER JOIN dbo.Pacientes ON Agendamentos.PacienteId = Pacientes.Id 
                       INNER JOIN dbo.responsaveis ON Pacientes.ResponsavelId = Responsaveis.Id 
                       WHERE Pacientes.Nome = @NomePaciente AND Responsaveis.Nome = @NomeResponsavel; 
                       SELECT @@ROWCOUNT;";

                int linhasDeletadas = 0;
                using (var DelAgendamento = new SqlCommand(sql, conexao))
                {
                    DelAgendamento.Parameters.AddWithValue("@NomePaciente", deletarPacienteRequest.NomePaciente);
                    DelAgendamento.Parameters.AddWithValue("@NomeResponsavel", deletarPacienteRequest.NomeResponsavel);

                    linhasDeletadas = Convert.ToInt32(DelAgendamento.ExecuteScalar());
                }

                string sql2 = @"DELETE Pacientes 
                        OUTPUT deleted.Id 
                        FROM [bd_gerenciamento].[dbo].[Pacientes] 
                        INNER JOIN [bd_gerenciamento].[dbo].[Responsaveis] ON Pacientes.ResponsavelId = Responsaveis.Id 
                        WHERE Pacientes.Nome = @NomePaciente AND Responsaveis.Nome = @NomeResponsavel";

                int idDeletado = 0;
                using (var delPaciente = new SqlCommand(sql2, conexao))
                {
                    delPaciente.Parameters.AddWithValue("@NomePaciente", deletarPacienteRequest.NomePaciente);
                    delPaciente.Parameters.AddWithValue("@NomeResponsavel", deletarPacienteRequest.NomeResponsavel);

                    idDeletado = Convert.ToInt32(delPaciente.ExecuteScalar());
                }
                return (linhasDeletadas, idDeletado);
            }
        }

    }
    
}
