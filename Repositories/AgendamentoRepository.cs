using System.Data;
using Microsoft.Data.SqlClient;
using GerenciamentoSalas.API.Models;

namespace GerenciamentoSalas.API.Repositories
{
    public class AgendamentoRepository
    {
        private readonly string _connectionString;

        public AgendamentoRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConexaoPadrao")
                ?? throw new ArgumentNullException("String de conexão não encontrada.");
        }

        public List<Agendamento> ObterTodos()
        {
            var lista = new List<Agendamento>();

            using (var conexao = new SqlConnection(_connectionString))
            {
                // Buscando a estrutura idêntica à que criamos no script de DROP/CREATE
                string sql = "SELECT Id, PacienteId, ProfissionalId, SalaId, DataHoraInicio, DataHoraFim, Status, Observacoes FROM Agendamentos";

                using (var comando = new SqlCommand(sql, conexao))
                {
                    conexao.Open();
                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var agendamento = new Agendamento
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                PacienteId = Convert.ToInt32(reader["PacienteId"]),
                                ProfissionalId = Convert.ToInt32(reader["ProfissionalId"]),
                                SalaId = Convert.ToInt32(reader["SalaId"]),
                                DataHoraInicio = Convert.ToDateTime(reader["DataHoraInicio"]),
                                DataHoraFim = Convert.ToDateTime(reader["DataHoraFim"]),
                                Status = reader["Status"].ToString() ?? "Confirmado",

                                // Tratamento técnico do DBNull para o campo Observacoes que permite NULL
                                Observacoes = reader["Observacoes"] != DBNull.Value
                                    ? reader["Observacoes"].ToString()
                                    : null
                            };
                            lista.Add(agendamento);
                        }
                    }
                }
            }
            return lista;
        }

        public int Inserir(Agendamento agendamento)
        {
            using var conexao = new SqlConnection(_connectionString);
            string sql = @"INSERT INTO Agendamentos (PacienteId, ProfissionalId, SalaId, DataHoraInicio, DataHoraFim, Observacoes, Status) 
               OUTPUT INSERTED.Id
               VALUES (@PacienteId, @ProfissionalId, @SalaId, @DataHoraInicio, @DataHoraFim, @Observacoes, @Status);";

            using var comando = new SqlCommand(sql, conexao);
            comando.Parameters.AddWithValue("@PacienteId", agendamento.PacienteId);
            comando.Parameters.AddWithValue("@ProfissionalId", agendamento.ProfissionalId);
            comando.Parameters.AddWithValue("@SalaId", agendamento.SalaId);
            comando.Parameters.AddWithValue("@DataHoraInicio", agendamento.DataHoraInicio);
            comando.Parameters.AddWithValue("@DataHoraFim", agendamento.DataHoraFim);
            comando.Parameters.AddWithValue("@Observacoes", agendamento.Observacoes ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Status", agendamento.Status ?? (object)DBNull.Value);

            conexao.Open();
            return Convert.ToInt32(comando.ExecuteScalar());
        }
    }
}