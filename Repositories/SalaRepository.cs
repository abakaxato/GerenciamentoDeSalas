using System.Data;
using Microsoft.Data.SqlClient;
using GerenciamentoSalas.API.Models;

namespace GerenciamentoSalas.API.Repositories
{
    public class SalaRepository
    {
        private readonly string _connectionString;

        public SalaRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConexaoPadrao")
                ?? throw new ArgumentNullException("String de conexão não encontrada.");
        }

        public List<Sala> ObterTodosSala()
        {
            var lista = new List<Sala>();

            using (var conexao = new SqlConnection(_connectionString))
            {
                string sql = "SELECT Id, Nome, Capacidade FROM Salas";

                using (var comando = new SqlCommand(sql, conexao))
                {
                    conexao.Open();
                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var sala = new Sala
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Nome = reader["Nome"].ToString() ?? string.Empty,
                                Capacidade = Convert.ToInt32(reader["Capacidade"]),
                            };
                            lista.Add(sala);
                        }
                    }
                }
            }
            return lista;
        }

        public int InserirSala(Sala sala)
        {
            using var conexao = new SqlConnection(_connectionString);
            string sql = @"INSERT INTO Salas (Nome, Capacidade, Ativa) 
                   OUTPUT INSERTED.Id
                   VALUES (@Nome, @Capacidade, @Ativa);";

            using var comando = new SqlCommand(sql, conexao);
            comando.Parameters.AddWithValue("@Nome", sala.Nome);
            comando.Parameters.AddWithValue("@Capacidade", sala.Capacidade);
            comando.Parameters.AddWithValue("@Ativa", sala.Ativa);

            conexao.Open();
            return Convert.ToInt32(comando.ExecuteScalar());
        }
    }
}