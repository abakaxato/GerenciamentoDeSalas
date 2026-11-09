using System.Data;
using Microsoft.Data.SqlClient;
using GerenciamentoSalas.API.Models;
using System.Transactions;
using GerenciamentoSalas.API.Requests;

namespace GerenciamentoSalas.API.Repositories
{
    public class ProfissionaisRepository
    {
        private readonly string _connectionString;

        public ProfissionaisRepository(IConfiguration configuration)
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

        public async Task<(List<int> IdAgendamentosDeletados, int IdResponsavelDeletado)> DeletarProfissionaisAsync(int id) {

            using var conexao = new SqlConnection(_connectionString);
            await conexao.OpenAsync();
            using var transaction = conexao.BeginTransaction();

            try {

                string sqlAgendamento = @"DELETE [bd_gerenciamento].[dbo].[Agendamentos]
                    OUTPUT deleted.Id
                    FROM [bd_gerenciamento].[dbo].[Agendamentos]
                    INNER JOIN [bd_gerenciamento].[dbo].[Profissionais] ON [bd_gerenciamento].[dbo].[Profissionais].[Id] = [bd_gerenciamento].[dbo].[Agendamentos].[SalaId]
                    where [bd_gerenciamento].[dbo].[Profissionais].[Id] = @IdProfissional";
                
                var IdsDeletadosAgendamento = new List<int>();

                using (var cmd = new SqlCommand(sqlAgendamento, conexao, transaction))
                {
                    cmd.Parameters.AddWithValue("@IdProfissional", id);

                    using var reader = await cmd.ExecuteReaderAsync();

                    while (await reader.ReadAsync())
                    {
                        IdsDeletadosAgendamento.Add(reader.GetInt32(0));
                    }
                     
                }

                string sqlProfissionais = @"
                    DELETE [bd_gerenciamento].[dbo].[Profissionais]
                    OUTPUT deleted.Id
                    FROM [bd_gerenciamento].[dbo].[Profissionais]
                    where [bd_gerenciamento].[dbo].[Profissionais].[Id] = @IdProfissional";

                int idDeletadoProfissionais = 0;
                using (var cmd = new SqlCommand(sqlProfissionais, conexao, transaction))
                {
                    cmd.Parameters.AddWithValue("@IdProfissional", id);
                    idDeletadoProfissionais = Convert.ToInt32(await cmd.ExecuteScalarAsync());
                }
                transaction.Commit();
                return (IdsDeletadosAgendamento, idDeletadoProfissionais);
            }
            catch (Exception) {
                transaction.Rollback();
                throw;
            }
        }
    }
}