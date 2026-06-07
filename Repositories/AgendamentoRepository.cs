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
    }
}