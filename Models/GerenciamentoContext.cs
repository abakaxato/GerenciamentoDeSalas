using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace GerenciamentoSalas.API.Models;

public partial class GerenciamentoContext : DbContext
{
    public GerenciamentoContext()
    {
    }

    public GerenciamentoContext(DbContextOptions<GerenciamentoContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Agendamento> Agendamentos { get; set; }

    public virtual DbSet<Paciente> Pacientes { get; set; }

    public virtual DbSet<Profissionai> Profissionais { get; set; }

    public virtual DbSet<Responsavei> Responsaveis { get; set; }

    public virtual DbSet<Sala> Salas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Agendamento>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Agendame__3214EC07F50E89A4");

            entity.HasIndex(e => e.DataHoraInicio, "IX_Agendamentos_DataHoraInicio");

            entity.HasIndex(e => e.Status, "IX_Agendamentos_Status");

            entity.Property(e => e.Observacoes).IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("Confirmado", "DF_Agendamentos_Status");

            entity.HasOne(d => d.Paciente).WithMany(p => p.Agendamentos)
                .HasForeignKey(d => d.PacienteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Agendamentos_Pacientes");

            entity.HasOne(d => d.Profissional).WithMany(p => p.Agendamentos)
                .HasForeignKey(d => d.ProfissionalId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Agendamentos_Profissionais");

            entity.HasOne(d => d.Sala).WithMany(p => p.Agendamentos)
                .HasForeignKey(d => d.SalaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Agendamentos_Salas");
        });

        modelBuilder.Entity<Paciente>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Paciente__3214EC07D3243B0C");

            entity.Property(e => e.DataCadastro).HasDefaultValueSql("(sysdatetime())", "DF_Pacientes_DataCadastro");
            entity.Property(e => e.Nome)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Responsavel).WithMany(p => p.Pacientes)
                .HasForeignKey(d => d.ResponsavelId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Pacientes_Responsaveis");
        });

        modelBuilder.Entity<Profissionai>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Profissi__3214EC07333F1594");

            entity.Property(e => e.Ativo).HasDefaultValue(true, "DF_Profissionais_Ativo");
            entity.Property(e => e.Especialidade)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Nome)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Telefone)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Responsavei>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Responsa__3214EC075CB83DF2");

            entity.Property(e => e.DataCadastro).HasDefaultValueSql("(sysdatetime())", "DF_Responsaveis_DataCadastro");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Nome)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Telefone)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Sala>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Salas__3214EC07A984516E");

            entity.Property(e => e.Capacidade).HasDefaultValue(1, "DF_Salas_Capacidade");
            entity.Property(e => e.Nome)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
