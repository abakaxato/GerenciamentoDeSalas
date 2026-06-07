using Microsoft.Data.SqlClient;
using GerenciamentoSalas.API.Models;

namespace GerenciamentoSalas.API.Repositories
{
    public class PacienteRepository
    {
        private readonly string _connectionString;

        public PacienteRepository(IConfiguration configuration) {
            _connectionString = configuration.GetConnectionString("ConexaoPadrao")
               ?? throw new ArgumentNullException("String de conexão não encontrada");
        }

        public List<Paciente> ObterTodos() {

            var lista = new List<Paciente>();

            using (var conexao = new SqlConnection(_connectionString)) {
                
                string sql = "SELECT Id, ReponsavelId, Nome, Telefone, DataDeCadastro FROM Pacientes";
                
                using (var comando = new SqlCommand(sql, conexao)) {
                    
                    conexao.Open();
                    
                    using (SqlDataReader reader = comando.ExecuteReader()) {
                        
                        while (reader.Read()) {

                            var paci = new Paciente
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                ResponsavelId = Convert.ToInt32(reader["ResponsavelId"]),
                                Nome = reader["Nome"].ToString() ?? string.Empty,
                                Telefone = reader["Telefone"].ToString() ?? string.Empty,
                                DataCadastro = Convert.ToDateTime(reader["DataCadastro"])
                            };
                            lista.Add(paci);
                        }
                    }
                }
            }
            return lista;
        }
    }
}
