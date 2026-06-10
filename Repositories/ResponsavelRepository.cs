using Microsoft.Data.SqlClient;
using GerenciamentoSalas.API.Models;

namespace GerenciamentoSalas.API.Repositories
{
    public class ResponsavelRepository
    {
        private readonly string _connectionString;

        // O construtor recebe a String de Conexão que configuramos no appsettings
        public ResponsavelRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConexaoPadrao")
                ?? throw new ArgumentNullException("String de conexão não encontrada.");
        }

        public List<Responsavel> ObterTodosResponsavel()
        {
            var lista = new List<Responsavel>();

            // 1. Instancia a conexão encapsulada no bloco 'using' para gerenciar o escopo
            using (var conexao = new SqlConnection(_connectionString))
            {
                string sql = "SELECT Id, Nome, Telefone, Email, DataCadastro FROM Responsaveis";

                // 2. Cria o objeto de comando contendo a instrução SQL e a conexão ativa
                using (var comando = new SqlCommand(sql, conexao))
                {
                    // 3. Abre o canal de comunicação de rede (Socket)
                    conexao.Open();

                    // 4. Executa a consulta e retorna o Stream de dados binários do SQL Server
                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        // 5. O laço 'while' move o cursor para a próxima linha enquanto houver registros
                        while (reader.Read())
                        {
                            var resp = new Responsavel
                            {
                                // Mapeamento manual: transforma o dado relacional em objeto C#
                                Id = Convert.ToInt32(reader["Id"]),
                                Nome = reader["Nome"].ToString() ?? string.Empty,
                                Telefone = reader["Telefone"].ToString() ?? string.Empty,
                                Email = reader["Email"] != DBNull.Value ? reader["Email"].ToString() : null,
                                DataCadastro = Convert.ToDateTime(reader["DataCadastro"])
                            };

                            lista.Add(resp);
                        }
                    } // SqlDataReader é fechado e limpa o buffer de rede aqui
                } // SqlCommand é desalocado da memória aqui
            } // SqlConnection é FECHADA automaticamente aqui (Evita Connection Leak)

            return lista;
        }
        public int Inserir(Responsavel responsavel)
        {
            using (var conexao = new SqlConnection(_connectionString))
            {
                // 1. Usamos a cláusula OUTPUT INSERTED.Id para o SQL Server nos devolver o ID gerado na hora
                string sql = @"INSERT INTO Responsaveis (Nome, Telefone, Email) 
                       OUTPUT INSERTED.Id
                       VALUES (@Nome, @Telefone, @Email);";

                using (var comando = new SqlCommand(sql, conexao))
                {
                    // 2. SEGURANÇA MÁXIMA: Proteção contra SQL Injection usando Parâmetros
                    comando.Parameters.AddWithValue("@Nome", responsavel.Nome);
                    comando.Parameters.AddWithValue("@Telefone", responsavel.Telefone);

                    // Tratamento técnico para persistir NULL se o e-mail não for informado
                    comando.Parameters.AddWithValue("@Email", responsavel.Email ?? (object)DBNull.Value);

                    conexao.Open();

                    // 3. Executa a query e captura o primeiro valor retornado (o ID gerado pelo IDENTITY)
                    int idGerado = Convert.ToInt32(comando.ExecuteScalar());

                    return idGerado;
                }
            }
        }
    }
}