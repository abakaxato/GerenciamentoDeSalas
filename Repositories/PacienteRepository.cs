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

        public async Task<(int AgendamentosDeletados, int IdPacienteDeletado)> DeletarPacienteAsync(DeletarPacienteRequest deletarPacienteRequest)
        {
            using var conexao = new SqlConnection(_connectionString);
            await conexao.OpenAsync();
            using var transaction = conexao.BeginTransaction();
            try
            {
                string sql = @"
                    DELETE A
                    FROM dbo.Agendamentos A
                    INNER JOIN dbo.Pacientes P ON A.PacienteId = P.Id
                    WHERE P.Id = @PacienteId;
                    SELECT @@ROWCOUNT;";

                int AgendamentosDeletados = 0;
                using (var DelAgendamento = new SqlCommand(sql, conexao,transaction))
                {
                    DelAgendamento.Parameters.AddWithValue("@PacienteId", deletarPacienteRequest.PacienteId);

                    AgendamentosDeletados = Convert.ToInt32(await DelAgendamento.ExecuteScalarAsync());
                }

                string sql2 = @"
                    DELETE P
                    OUTPUT deleted.Id
                    FROM dbo.Pacientes P
                    WHERE P.Id = @PacienteId;";

                int idDeletado = 0;
                using (var delPaciente = new SqlCommand(sql2, conexao,transaction))
                {
                    delPaciente.Parameters.AddWithValue("@PacienteId", deletarPacienteRequest.PacienteId);

                    idDeletado = Convert.ToInt32(await delPaciente.ExecuteScalarAsync());
                }
                transaction.Commit();
                return (AgendamentosDeletados, idDeletado);
            }
            catch (Exception ex){
                transaction.Rollback();
                throw new InvalidOperationException("Falha ao deletar paciente", ex);
            }
            
        }

    }
    
}
