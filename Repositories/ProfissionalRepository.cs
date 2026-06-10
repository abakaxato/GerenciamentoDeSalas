using System.Data;
using Microsoft.Data.SqlClient;
using GerenciamentoSalas.API.Models;

namespace GerenciamentoSalas.API.Repositories
{
    public class ProfissionalRepository
    {
        private readonly string _connectionString;

        public ProfissionalRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConexaoPadrao")
                ?? throw new ArgumentNullException("String de conexão não encontrada.");
        }

        public List<Profissional> ObterTodosProfissional()
        {
            var lista = new List<Profissional>();

            using (var conexao = new SqlConnection(_connectionString))
            {
                string sql = "SELECT Id, Nome, Especialidade, Ativo FROM Profissionais";

                using (var comando = new SqlCommand(sql, conexao))
                {
                    conexao.Open();
                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var prof = new Profissional
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Nome = reader["Nome"].ToString() ?? string.Empty,
                                Especialidade = reader["Especialidade"].ToString() ?? string.Empty,
                                Ativo = Convert.ToBoolean(reader["Ativo"])
                            };
                            lista.Add(prof);
                        }
                    }
                }
            }
            return lista;
        }

        public int InserirProfissional(Profissional profissional)
        {
            using var conexao = new SqlConnection(_connectionString);
            string sql = @"INSERT INTO Profissionais (Nome, Especialidade, Ativo) 
                   OUTPUT INSERTED.Id
                   VALUES (@Nome, @Especialidade, @Ativo);";

            using var comando = new SqlCommand(sql, conexao);
            comando.Parameters.AddWithValue("@Nome", profissional.Nome);
            comando.Parameters.AddWithValue("@Especialidade", profissional.Especialidade);
            comando.Parameters.AddWithValue("@Ativo", profissional.Ativo);

            conexao.Open();
            return Convert.ToInt32(comando.ExecuteScalar());
        }
    }
}