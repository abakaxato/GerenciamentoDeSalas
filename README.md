(Preciso revisar esse Readme kkkkkk)

📅 Sistema de Gerenciamento de Salas e Agendamentos Clínicos
Uma API REST robusta e escalável desenvolvida em .NET 8 para o gerenciamento de agendamentos, pacientes, responsáveis, profissionais de saúde e salas. O projeto foi desenhado seguindo boas práticas de arquitetura de software, garantindo performance e segurança na manipulação dos dados relacionais.

🚀 Tecnologias Utilizadas
Back-end: .NET 8 (C#) / ASP.NET Core Web API 

Banco de Dados: SQL Server (Transact-SQL)

Acesso a Dados: ADO.NET / SQL Client nativo (com foco em alta performance)

Documentação e Testes: OpenAPI / Scalar (/scalar/v1)

Serialização: System.Text.Json

🏗️ Diferenciais Técnicos & Arquitetura
Padrão Repository: Separação clara entre as regras de controle de rotas (Controllers) e a persistência de dados no banco (Repositories).

Segurança com Parametrização: Uso rigoroso de comandos parametrizados (SqlCommand.Parameters) em todas as queries para blindar a aplicação contra ataques de SQL Injection.

Controle de Recursos (Memory Management): Implementação do bloco using (gerenciamento e descarte correto de conexões e comandos do SQL Server) para evitar vazamentos de memória (connection pooling leaks).

Uso de DTOs (Data Transfer Objects): Camada de dados de entrada isolada dos modelos do banco de dados, limpando a interface pública da API no Scalar e impedindo que o cliente manipule propriedades automáticas (como Ids gerados por IDENTITY ou datas controladas por DEFAULT constraints do banco).

🗄️ Modelo Relacional (Banco de Dados)
O banco de dados foi modelado no SQL Server respeitando regras de integridade referencial através de Chaves Estrangeiras (FK), chaves auto-incrementais (IDENTITY) e gatilhos automáticos de auditoria de tempo (DEFAULT GETDATE()):

Responsaveis (Cadastro de pais/tutores)

Pacientes (Vinculados a um responsável)

Profissionais (Médicos/Terapeutas com controle de status ativo)

Salas (Controle de capacidade e disponibilidade)

Agendamentos (O coração do sistema, correlacionando todas as entidades acima com validações de período)

🛠️ Como Executar o Projeto
Clone o repositório:

Bash
git clone https://github.com/SEU_USUARIO/NOME_DO_REPOSITORIO.git
Configure a string de conexão com o seu SQL Server no arquivo appsettings.json.

Rode o projeto pelo Visual Studio ou via terminal:

Bash
dotnet run
Acesse a documentação interativa para testar os endpoints:

Plaintext
https://localhost:7220/scalar/v1
