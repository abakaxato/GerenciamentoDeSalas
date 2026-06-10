-- Garante que o SQL Server entenda o formato Ano-Mês-Dia independente do idioma do sistema
SET DATEFORMAT ymd;

-- ====================================================================
-- PASSO 1: LIMPEZA TOTAL DAS TABELAS (Ordem reversa de dependência)
-- ====================================================================
DELETE FROM Agendamentos;
DELETE FROM Pacientes;
DELETE FROM Responsaveis;
DELETE FROM Profissionais;
DELETE FROM Salas;

-- ====================================================================
-- PASSO 2: INSERÇÃO DE RESPONSÁVEIS (Forçando IDs de 1 a 10)
-- ====================================================================
SET IDENTITY_INSERT Responsaveis ON;

INSERT INTO Responsaveis (Id, Nome, Telefone, Email) VALUES 
(1, 'Carlos Augusto Silva', '(81) 99123-4567', 'carlos.augusto@email.com'),
(2, 'Ana Beatriz Albuquerque', '(81) 98877-6655', 'ana.beatriz@email.com'),
(3, 'Marcos Antônio Souza', '(81) 99765-4321', 'marcos.souza@email.com'),
(4, 'Juliana Maria Costa', '(81) 98765-1234', 'juliana.costa@email.com'),
(5, 'Roberto Carlos Ferreira', '(81) 99234-5678', 'roberto.ferreira@email.com'),
(6, 'Patrícia Helena Santos', '(81) 98543-2109', 'patricia.santos@email.com'),
(7, 'Fernando José Lima', '(81) 99654-8732', 'fernando.lima@email.com'),
(8, 'Camila Vitória Mendes', '(81) 98122-3344', 'camila.mendes@email.com'),
(9, 'Ricardo Alexandre Gomes', '(81) 99344-5566', 'ricardo.gomes@email.com'),
(10, 'Letícia Oliveira Cruz', '(81) 98655-7788', 'leticia.cruz@email.com');

SET IDENTITY_INSERT Responsaveis OFF;

-- ====================================================================
-- PASSO 3: INSERÇÃO DE PACIENTES (Forçando IDs de 1 a 10)
-- ====================================================================
SET IDENTITY_INSERT Pacientes ON;

INSERT INTO Pacientes (Id, ResponsavelId, Nome, DataNascimento) VALUES 
(1, 1, 'Pedro Henrique Silva', '2018-03-12'),
(2, 1, 'Lucas Gabriel Silva', '2020-07-22'),
(3, 2, 'Sofia Albuquerque Lima', '2015-11-05'),
(4, 3, 'Júlia Maria Souza', '2019-01-30'),
(5, 4, 'Enzo Gabriel Costa', '2017-05-14'),
(6, 5, 'Arthur Ferreira Santos', '2016-08-19'),
(7, 6, 'Beatriz Mendes Lima', '2021-10-02'),
(8, 7, 'Matheus Mendes Cruz', '2014-04-25'),
(9, 8, 'Guilherme Gomes Silva', '2019-12-08'),
(10, 9, 'Valentina Cruz Albuquerque', '2022-02-15');

SET IDENTITY_INSERT Pacientes OFF;

-- ====================================================================
-- PASSO 4: INSERÇÃO DE PROFISSIONAIS (Forçando IDs de 1 a 10)
-- ====================================================================
SET IDENTITY_INSERT Profissionais ON;

INSERT INTO Profissionais (Id, Nome, Especialidade, Ativo) VALUES 
(1, 'Dr. André Rodrigues', 'Pediatria', 1),
(2, 'Dra. Amanda Cavalcanti', 'Psicologia Infantil', 1),
(3, 'Dr. Bruno César Melo', 'Fisioterapia Neurofuncional', 1),
(4, 'Dra. Carolina Freitas', 'Fonoaudiologia', 1),
(5, 'Dr. Daniel Veras', 'Psiquiatria', 1),
(6, 'Dra. Elaine Ramos', 'Terapia Ocupacional', 1),
(7, 'Dr. Felipe Dornelas', 'Nutrição Clínica', 1),
(8, 'Dra. Gisele Albuquerque', 'Psicopedagogia', 1),
(9, 'Dr. Henrique Manso', 'Neuropediatria', 1),
(10, 'Dra. Isabela Fontes', 'Psicologia Cognitivo-Comportamental', 0);

SET IDENTITY_INSERT Profissionais OFF;

-- ====================================================================
-- PASSO 5: INSERÇÃO DE SALAS (Forçando IDs de 1 a 10)
-- ====================================================================
SET IDENTITY_INSERT Salas ON;

INSERT INTO Salas (Id, Nome, Capacidade) VALUES 
(1, 'Sala Azul - Integração Sensorial', 4),
(2, 'Sala Verde - Atendimento Infantil', 2),
(3, 'Sala Amarela - Fonoaudiologia', 2),
(4, 'Sala Premium - Neurofeedback', 3),
(5, 'Consultório 01 - Geral', 3),
(6, 'Consultório 02 - Geral', 3),
(7, 'Sala de Espelhos - Psicoterapia', 2),
(8, 'Estúdio - Fisioterapia', 5),
(9, 'Sala de Avaliação Diagnóstica', 2),
(10, 'Auditório - Treinamentos/Grupos', 15);

SET IDENTITY_INSERT Salas OFF;

-- ====================================================================
-- PASSO 6: INSERÇÃO DE AGENDAMENTOS (Forçando IDs de 1 a 10)
-- ====================================================================
SET IDENTITY_INSERT Agendamentos ON;

INSERT INTO Agendamentos (Id, PacienteId, ProfissionalId, SalaId, DataHoraInicio, DataHoraFim, Status, Observacoes) VALUES 
(1, 1, 1, 1, '2026-06-15 08:00:00', '2026-06-15 09:00:00', 'Confirmado', 'Consulta mensal de acompanhamento de rotina.'),
(2, 2, 2, 2, '2026-06-15 09:00:00', '2026-06-15 10:00:00', 'Confirmado', 'Sessão semanal de psicoterapia infantil.'),
(3, 3, 4, 3, '2026-06-15 10:30:00', '2026-06-15 11:30:00', 'Agendado', 'Avaliação de processamento auditivo central.'),
(4, 4, 6, 1, '2026-06-15 14:00:00', '2026-06-15 15:30:00', 'Confirmado', 'Sessão de T.O. com foco em regulação sensorial.'),
(5, 5, 7, 5, '2026-06-16 08:00:00', '2026-06-16 09:00:00', 'Agendado', 'Retorno para entrega de plano alimentar.'),
(6, 6, 8, 7, '2026-06-16 13:00:00', '2026-06-16 14:00:00', 'Confirmado', 'Atendimento psicopedagógico focado em TDAH.'),
(7, 7, 9, 9, '2026-06-17 09:00:00', '2026-06-17 10:30:00', 'Pendente', 'Primeira consulta de avaliação neuropediátrica.'),
(8, 8, 3, 8, '2026-06-17 15:00:00', '2026-06-17 16:00:00', 'Confirmado', 'Sessão de fisioterapia para correção postural.'),
(9, 9, 2, 2, '2026-06-18 10:00:00', '2026-06-18 11:00:00', 'Cancelado', 'Paciente informou indisponibilidade de transporte.'),
(10, 1, 4, 3, '2026-06-18 14:00:00', '2026-06-18 15:00:00', 'Agendado', 'Sessão de fonoaudiologia continuada.');

SET IDENTITY_INSERT Agendamentos OFF;