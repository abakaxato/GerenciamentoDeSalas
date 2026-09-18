USE [master]
GO

IF EXISTS (SELECT name FROM sys.databases WHERE name = N'bd_gerenciamento')
BEGIN
    DROP DATABASE [bd_gerenciamento];
END
GO

CREATE DATABASE [bd_gerenciamento]
 ON PRIMARY 
( NAME = N'BancoGerenciamento', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL17.SQLEXPRESS\MSSQL\DATA\BancoGerenciamento.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'BancoGerenciamento_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL17.SQLEXPRESS\MSSQL\DATA\BancoGerenciamento_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO

USE [bd_gerenciamento]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- 1. TABELA DE RESPONSAEIS
-- =============================================
CREATE TABLE [dbo].[Responsaveis](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Nome] [varchar](100) NOT NULL,
	[Telefone] [varchar](20) NOT NULL,
	[Email] [varchar](100) NULL,
	[DataCadastro] [datetime2](7) NOT NULL CONSTRAINT [DF_Responsaveis_DataCadastro] DEFAULT (sysdatetime()),
PRIMARY KEY CLUSTERED ([Id] ASC)
) ON [PRIMARY]
GO

-- =============================================
-- 2. TABELA DE PACIENTES
-- =============================================
CREATE TABLE [dbo].[Pacientes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ResponsavelId] [int] NOT NULL,
	[Nome] [varchar](100) NOT NULL,
	[DataNascimento] [date] NULL,
	[DataCadastro] [datetime2](7) NOT NULL CONSTRAINT [DF_Pacientes_DataCadastro] DEFAULT (sysdatetime()),
PRIMARY KEY CLUSTERED ([Id] ASC),
CONSTRAINT [FK_Pacientes_Responsaveis] FOREIGN KEY([ResponsavelId]) REFERENCES [dbo].[Responsaveis] ([Id])
) ON [PRIMARY]
GO

-- =============================================
-- 3. TABELA DE PROFISSIONAIS
-- =============================================
CREATE TABLE [dbo].[Profissionais](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Nome] [varchar](100) NOT NULL,
	[Especialidade] [varchar](50) NOT NULL,
	[Telefone] [varchar](20) NULL,
	[Ativo] [bit] NULL CONSTRAINT [DF_Profissionais_Ativo] DEFAULT ((1)),
PRIMARY KEY CLUSTERED ([Id] ASC)
) ON [PRIMARY]
GO

-- =============================================
-- 4. TABELA DE SALAS
-- =============================================
CREATE TABLE [dbo].[Salas](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Nome] [varchar](50) NOT NULL,
	[Capacidade] [int] NULL CONSTRAINT [DF_Salas_Capacidade] DEFAULT ((1)),
PRIMARY KEY CLUSTERED ([Id] ASC)
) ON [PRIMARY]
GO

-- =============================================
-- 5. TABELA DE AGENDAMENTOS
-- =============================================
CREATE TABLE [dbo].[Agendamentos](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PacienteId] [int] NOT NULL,
	[ProfissionalId] [int] NOT NULL,
	[SalaId] [int] NOT NULL,
	[DataHoraInicio] [datetime2](7) NOT NULL,
	[DataHoraFim] [datetime2](7) NOT NULL,
	[Status] [varchar](20) NULL CONSTRAINT [DF_Agendamentos_Status] DEFAULT ('Confirmado'),
	[Observacoes] [varchar](max) NULL,
PRIMARY KEY CLUSTERED ([Id] ASC),
CONSTRAINT [FK_Agendamentos_Pacientes] FOREIGN KEY([PacienteId]) REFERENCES [dbo].[Pacientes] ([Id]),
CONSTRAINT [FK_Agendamentos_Profissionais] FOREIGN KEY([ProfissionalId]) REFERENCES [dbo].[Profissionais] ([Id]),
CONSTRAINT [FK_Agendamentos_Salas] FOREIGN KEY([SalaId]) REFERENCES [dbo].[Salas] ([Id])
) ON [PRIMARY]
GO

-- =============================================
-- 6. ÍNDICES PARA PERFORMANCE DA API
-- =============================================
CREATE NONCLUSTERED INDEX [IX_Agendamentos_DataHoraInicio] 
ON [dbo].[Agendamentos] ([DataHoraInicio] ASC);
GO

CREATE NONCLUSTERED INDEX [IX_Agendamentos_Status] 
ON [dbo].[Agendamentos] ([Status] ASC);
GO