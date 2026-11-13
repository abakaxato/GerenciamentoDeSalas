using System.Data;
using Microsoft.Data.SqlClient;
using GerenciamentoSalas.API.Models.DTOs;
using GerenciamentoSalas.API.Models.Requests;

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

        public List<SalaDto> ObterTodosSala()
        {
            var lista = new List<SalaDto>();

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
                            var sala = new SalaDto
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

        public int InserirSala(SalaDto sala)
        {
            using var conexao = new SqlConnection(_connectionString);
            string sql = @"INSERT INTO Salas (Nome, Capacidade) 
                   OUTPUT INSERTED.Id
                   VALUES (@Nome, @Capacidade);";

            using var comando = new SqlCommand(sql, conexao);
            comando.Parameters.AddWithValue("@Nome", sala.Nome);
            comando.Parameters.AddWithValue("@Capacidade", sala.Capacidade);

            conexao.Open();
            return Convert.ToInt32(comando.ExecuteScalar());
        }

        public async Task<(int salaDeletada, List<int> agendamentosDeletados)> DeletarSalaAsync(DeletarSalaRequest request){ 

            using var conexao =  new SqlConnection(_connectionString);
            await conexao.OpenAsync();
            using var transaction = conexao.BeginTransaction();

            try
            {
                string sqlAgendamento = @"DELETE [bd_gerenciamento].[dbo].[Agendamentos]
                    OUTPUT deleted.Id
                    FROM [bd_gerenciamento].[dbo].[Agendamentos]
                    INNER JOIN [bd_gerenciamento].[dbo].[Salas] ON [bd_gerenciamento].[dbo].[Salas].[Id] = [bd_gerenciamento].[dbo].[Agendamentos].[SalaId]
                    where [bd_gerenciamento].[dbo].[Salas].[Id] = @IdSala";
                
                var IdsDeletadosAgendamento = new List<int>();

                using (var cmd = new SqlCommand(sqlAgendamento, conexao, transaction))
                {
                    cmd.Parameters.AddWithValue("@IdSala", request.id);
                    
                    using var reader = await cmd.ExecuteReaderAsync();

                    while (await reader.ReadAsync()) {
                        IdsDeletadosAgendamento.Add(reader.GetInt32(0));
                    }
                
                }

                string sqlSala = @"
                    DELETE [bd_gerenciamento].[dbo].[Salas]
                    OUTPUT deleted.Id
                    FROM [bd_gerenciamento].[dbo].[Salas]
                    where [bd_gerenciamento].[dbo].[Salas].[Id] = @IdSala";

                int idDeletadoSala = 0;
                using (var cmd = new SqlCommand(sqlSala, conexao, transaction))
                {
                    cmd.Parameters.AddWithValue("@IdSala",request.id);
                    idDeletadoSala = Convert.ToInt32(await cmd.ExecuteScalarAsync());
                }
                transaction.Commit();
                return (idDeletadoSala, IdsDeletadosAgendamento);
            }
            catch (Exception){
                transaction.Rollback();
                throw;
            }
        
        }
    }
}