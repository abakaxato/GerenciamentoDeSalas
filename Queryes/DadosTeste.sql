USE [bd_gerenciamento];
GO

SET NOCOUNT ON;
GO

-- =============================================
-- 1. RESPONSÁVEIS
-- =============================================
SET IDENTITY_INSERT [dbo].[Responsaveis] ON;

INSERT INTO [dbo].[Responsaveis] ([Id], [Nome], [Telefone], [Email], [DataCadastro], [Ativo])
VALUES
    (1, 'Mariana Costa Silva', '81991001101', 'mariana.silva@exemplo.com', '2026-08-10T09:00:00', 1),
    (2, 'Carlos Eduardo Rocha', '81991001102', 'carlos.rocha@exemplo.com', '2026-08-12T10:15:00', 0),
    (3, 'Fernanda Lima Alves', '81991001103', 'fernanda.alves@exemplo.com', '2026-08-15T14:30:00', 1),
    (4, 'Rafael Mendes Souza', '81991001104', NULL, '2026-08-20T11:00:00', 0),
    (5, 'Patrícia Gomes Santos', '81991001105', 'patricia.santos@exemplo.com', '2026-09-01T08:45:00', 1);

SET IDENTITY_INSERT [dbo].[Responsaveis] OFF;
GO

-- =============================================
-- 2. PROFISSIONAIS
-- =============================================
SET IDENTITY_INSERT [dbo].[Profissionais] ON;

INSERT INTO [dbo].[Profissionais] ([Id], [Nome], [Especialidade], [Telefone], [Ativo])
VALUES
    (1, 'Dra. Beatriz Nogueira', 'Psicologia', '81992002201', 1),
    (2, 'Dr. Lucas Azevedo', 'Fonoaudiologia', '81992002202', 0),
    (3, 'Dra. Camila Ribeiro', 'Terapia Ocupacional', '81992002203', 1),
    (4, 'Dr. Thiago Monteiro', 'Psicopedagogia', '81992002204', 0),
    (5, 'Dra. Juliana Martins', 'Neuropsicologia', '81992002205', 1);

SET IDENTITY_INSERT [dbo].[Profissionais] OFF;
GO

-- =============================================
-- 3. SALAS
-- =============================================
SET IDENTITY_INSERT [dbo].[Salas] ON;

INSERT INTO [dbo].[Salas] ([Id], [Nome], [Capacidade], [Ativo])
VALUES
    (1, 'Sala Girassol', 2, 1),
    (2, 'Sala Estrela', 1, 0),
    (3, 'Sala Horizonte', 3, 1),
    (4, 'Sala Arco-Íris', 2, 0),
    (5, 'Sala Jardim', 2, 1);

SET IDENTITY_INSERT [dbo].[Salas] OFF;
GO

-- =============================================
-- 4. PACIENTES
-- =============================================
SET IDENTITY_INSERT [dbo].[Pacientes] ON;

INSERT INTO [dbo].[Pacientes] ([Id], [ResponsavelId], [Nome], [DataNascimento], [DataCadastro], [Ativo])
VALUES
    (1, 1, 'Pedro Henrique Costa', '2018-04-12', '2026-09-02T09:00:00', 1),
    (2, 2, 'Sofia Rocha', '2019-07-23', '2026-09-03T10:00:00', 0),
    (3, 3, 'Miguel Alves', '2017-11-05', '2026-09-04T11:30:00', 1),
    (4, 4, 'Helena Souza', '2020-02-18', '2026-09-05T13:15:00', 0),
    (5, 5, 'Arthur Santos', '2016-09-30', '2026-09-06T15:00:00', 1),
    (6, 1, 'Laura Costa', '2021-01-14', '2026-09-07T08:30:00', 0),
    (7, 3, 'Gael Alves', '2018-06-27', '2026-09-08T10:45:00', 1),
    (8, 5, 'Alice Santos', '2019-12-09', '2026-09-09T14:00:00', 0),
    (9, NULL, 'Bernardo Lima', '2015-05-19', '2026-09-10T16:10:00', 1),
    (10, 3, 'Valentina Alves', '2020-10-03', '2026-09-11T09:20:00', 0);

SET IDENTITY_INSERT [dbo].[Pacientes] OFF;
GO

-- =============================================
-- 5. AGENDAMENTOS
-- =============================================
INSERT INTO [dbo].[Agendamentos] ([PacienteId], [ProfissionalId], [SalaId], [DataHoraInicio], [DataHoraFim], [Status], [Observacoes], [Ativo])
VALUES
    (1, 1, 1, '2026-11-23T08:00:00', '2026-11-23T08:50:00', 1, 'Sessão de rotina #01', 1),
    (2, 2, 2, '2026-11-23T09:00:00', '2026-11-23T09:50:00', 2, 'Paciente inativo - cancelado', 0),
    (3, 3, 3, '2026-11-23T10:00:00', '2026-11-23T10:50:00', 1, 'Avaliação sensorial #03', 1),
    (4, 4, 4, '2026-11-23T11:00:00', '2026-11-23T11:50:00', 3, 'Sala/profissional inativo', 0),
    (5, 5, 5, '2026-11-23T13:00:00', '2026-11-23T13:50:00', 1, 'Acompanhamento neuro #05', 1),
    (6, 1, 1, '2026-11-23T14:00:00', '2026-11-23T14:50:00', 2, 'Desativado para teste #06', 0),
    (7, 3, 3, '2026-11-23T15:00:00', '2026-11-23T15:50:00', 1, 'Sessão de terapia #07', 1),
    (8, 2, 2, '2026-11-23T16:00:00', '2026-11-23T16:50:00', 3, 'Registro desativado #08', 0),
    (9, 5, 5, '2026-11-24T08:00:00', '2026-11-24T08:50:00', 1, 'Sem responsável vinculado #09', 1),
    (10, 4, 4, '2026-11-24T09:00:00', '2026-11-24T09:50:00', 2, 'Inativo #10', 0),
    (1, 3, 3, '2026-11-24T10:00:00', '2026-11-24T10:50:00', 1, 'Segunda sessão da semana #11', 1),
    (2, 1, 1, '2026-11-24T11:00:00', '2026-11-24T11:50:00', 1, 'Desativado #12', 0),
    (3, 5, 5, '2026-11-24T13:00:00', '2026-11-24T13:50:00', 1, 'Ativo tarde #13', 1),
    (4, 2, 2, '2026-11-24T14:00:00', '2026-11-24T14:50:00', 2, 'Desativado tarde #14', 0),
    (5, 1, 1, '2026-11-24T15:00:00', '2026-11-24T15:50:00', 1, 'Ativo tarde #15', 1),
    (6, 3, 3, '2026-11-25T08:00:00', '2026-11-25T08:50:00', 3, 'Desativado #16', 0),
    (7, 5, 5, '2026-11-25T09:00:00', '2026-11-25T09:50:00', 1, 'Ativo manhã #17', 1),
    (8, 4, 4, '2026-11-25T10:00:00', '2026-11-25T10:50:00', 1, 'Desativado #18', 0),
    (9, 1, 3, '2026-11-25T11:00:00', '2026-11-25T11:50:00', 1, 'Ativo sala 3 #19', 1),
    (10, 5, 1, '2026-11-25T13:00:00', '2026-11-25T13:50:00', 2, 'Desativado #20', 0),
    (1, 5, 5, '2026-11-26T08:30:00', '2026-11-26T09:20:00', 1, 'Ativo quinta #21', 1),
    (3, 1, 1, '2026-11-26T09:30:00', '2026-11-26T10:20:00', 1, 'Ativo quinta #22', 1),
    (5, 3, 3, '2026-11-26T10:30:00', '2026-11-26T11:20:00', 2, 'Desativado quinta #23', 0),
    (7, 1, 5, '2026-11-26T14:00:00', '2026-11-26T14:50:00', 1, 'Ativo quinta #24', 1);
GO

-- =============================================
-- 6. AJUSTAR SEEDS DO IDENTITY PARA PRÓXIMOS INSERTS
-- =============================================
DBCC CHECKIDENT ('[dbo].[Responsaveis]', RESEED, 5);
DBCC CHECKIDENT ('[dbo].[Profissionais]', RESEED, 5);
DBCC CHECKIDENT ('[dbo].[Salas]', RESEED, 5);
DBCC CHECKIDENT ('[dbo].[Pacientes]', RESEED, 10);
GO

-- =============================================
-- 7. CONFERÊNCIA RÁPIDA DE ATIVOS VS INATIVOS
-- =============================================
SELECT 'Responsaveis' AS Tabela, COUNT(*) AS Total, SUM(CASE WHEN Ativo = 1 THEN 1 ELSE 0 END) AS Ativos, SUM(CASE WHEN Ativo = 0 OR Ativo IS NULL THEN 1 ELSE 0 END) AS InativosOuNulos FROM [dbo].[Responsaveis]
UNION ALL
SELECT 'Profissionais', COUNT(*), SUM(CASE WHEN Ativo = 1 THEN 1 ELSE 0 END), SUM(CASE WHEN Ativo = 0 OR Ativo IS NULL THEN 1 ELSE 0 END) FROM [dbo].[Profissionais]
UNION ALL
SELECT 'Salas', COUNT(*), SUM(CASE WHEN Ativo = 1 THEN 1 ELSE 0 END), SUM(CASE WHEN Ativo = 0 OR Ativo IS NULL THEN 1 ELSE 0 END) FROM [dbo].[Salas]
UNION ALL
SELECT 'Pacientes', COUNT(*), SUM(CASE WHEN Ativo = 1 THEN 1 ELSE 0 END), SUM(CASE WHEN Ativo = 0 OR Ativo IS NULL THEN 1 ELSE 0 END) FROM [dbo].[Pacientes]
UNION ALL
SELECT 'Agendamentos', COUNT(*), SUM(CASE WHEN Ativo = 1 THEN 1 ELSE 0 END), SUM(CASE WHEN Ativo = 0 OR Ativo IS NULL THEN 1 ELSE 0 END) FROM [dbo].[Agendamentos];
GO