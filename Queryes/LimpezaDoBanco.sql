USE [bd_gerenciamento];
GO

SET NOCOUNT ON;
GO

DELETE FROM [dbo].[Agendamentos];

DELETE FROM [dbo].[Pacientes];
DELETE FROM [dbo].[Profissionais];
DELETE FROM [dbo].[Salas];

DELETE FROM [dbo].[Responsaveis];
GO

--Reseta o IDENTITY para o próximo registro começar em 1
DBCC CHECKIDENT ('[dbo].[Agendamentos]', RESEED, 0);
DBCC CHECKIDENT ('[dbo].[Pacientes]', RESEED, 0);
DBCC CHECKIDENT ('[dbo].[Profissionais]', RESEED, 0);
DBCC CHECKIDENT ('[dbo].[Salas]', RESEED, 0);
DBCC CHECKIDENT ('[dbo].[Responsaveis]', RESEED, 0);
GO