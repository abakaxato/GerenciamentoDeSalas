using Microsoft.Data.SqlClient;
using GerenciamentoSalas.API.Models.DTOs;

namespace GerenciamentoSalas.API.Repositories
{
    public class ResponsavelRepository
    {
        private readonly string _connectionString;

        public ResponsavelRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConexaoPadrao")
                ?? throw new ArgumentNullException("String de conexão não encontrada.");
        }

        public async Task<List<ResponsavelDto>> ObterTodosResponsavelAsync()
        {
            var lista = new List<ResponsavelDto>();

            using var conexao = new SqlConnection(_connectionString);
            string sql = "SELECT Id, Nome, Telefone, Email, DataCadastro FROM Responsaveis";

            using var comando = new SqlCommand(sql, conexao);
            await conexao.OpenAsync();

            using var reader = await comando.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var resp = new ResponsavelDto
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Nome = reader["Nome"].ToString() ?? string.Empty,
                    Telefone = reader["Telefone"].ToString() ?? string.Empty,
                    Email = reader["Email"] != DBNull.Value ? reader["Email"].ToString() : null,
                    DataCadastro = Convert.ToDateTime(reader["DataCadastro"])
                };

                lista.Add(resp);
            }

            return lista;
        }

        public async Task<int> InserirAsync(ResponsavelDto responsavel)
        {
            using var conexao = new SqlConnection(_connectionString);
            string sql = @"INSERT INTO Responsaveis (Nome, Telefone, Email) 
                           OUTPUT INSERTED.Id
                           VALUES (@Nome, @Telefone, @Email);";

            using var comando = new SqlCommand(sql, conexao);
            comando.Parameters.AddWithValue("@Nome", responsavel.Nome);
            comando.Parameters.AddWithValue("@Telefone", responsavel.Telefone);
            comando.Parameters.AddWithValue("@Email", responsavel.Email ?? (object)DBNull.Value);

            await conexao.OpenAsync();
            var resultado = await comando.ExecuteScalarAsync();
            return Convert.ToInt32(resultado);
        }

        public async Task<(int AgendamentosDeletado, List<int> IdsPacientesDeletados, int ResponsavelDeletado)> DeletarResponsavelAsync(int id)
        {
            using var conexao = new SqlConnection(_connectionString);
            await conexao.OpenAsync();
            using var transaction = conexao.BeginTransaction();

            try
            {
                // 1) Apaga agendamentos dos pacientes do responsável e captura a quantidade
                string sqlAgend = @"
                    DELETE A
                    FROM dbo.Agendamentos A
                    INNER JOIN dbo.Pacientes P ON A.PacienteId = P.Id
                    WHERE P.ResponsavelId = @ResponsavelId;
                    SELECT @@ROWCOUNT;";

                int agendamentosDeletados = 0;
                using (var cmd = new SqlCommand(sqlAgend, conexao, transaction))
                {
                    cmd.Parameters.AddWithValue("@ResponsavelId", id);
                    agendamentosDeletados = Convert.ToInt32(await cmd.ExecuteScalarAsync());
                }

                // 2) Apaga todos os pacientes do responsável e retorna todos os ids deletados
                string sqlPacientes = @"
                    DELETE P
                    OUTPUT deleted.Id
                    FROM dbo.Pacientes P
                    WHERE P.ResponsavelId = @ResponsavelId;";

                var idsDeletados = new List<int>();
                using (var cmd = new SqlCommand(sqlPacientes, conexao, transaction))
                {
                    cmd.Parameters.AddWithValue("@ResponsavelId", id);
                    using var reader = await cmd.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        idsDeletados.Add(reader.GetInt32(0));
                    }
                }

                // 3) Apagar o responsável (retorna quantidade de linhas apagadas)
                string sqlResponsavel = @"DELETE FROM dbo.Responsaveis WHERE Id = @ResponsavelId; SELECT @@ROWCOUNT;";
                int responsavelDeletado = 0;
                using (var cmd = new SqlCommand(sqlResponsavel, conexao, transaction))
                {
                    cmd.Parameters.AddWithValue("@ResponsavelId", id);
                    responsavelDeletado = Convert.ToInt32(await cmd.ExecuteScalarAsync());
                }

                transaction.Commit();
                return (agendamentosDeletados, idsDeletados, responsavelDeletado);
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
    }
}