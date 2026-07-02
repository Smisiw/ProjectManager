using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectManagement.API.Domain.Entities;

namespace ProjectManagement.API.Data.Configurations;

public class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.ToTable("Projects");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(255);
        builder.Property(p => p.CustomerCompany)
            .IsRequired()
            .HasMaxLength(255);
        builder.Property(p => p.ExecutorCompany)
            .IsRequired()
            .HasMaxLength(255);
        builder.Property(p => p.ManagerId)
            .IsRequired();
        builder.Property(p => p.Priority)
            .IsRequired();
        builder.Property(p => p.StartDate)
            .IsRequired();
        builder.Property(p => p.EndDate)
            .IsRequired();
        
        builder.HasIndex(p => p.StartDate);
        builder.HasIndex(p => p.Priority);
        builder.HasIndex(p => p.ManagerId);
        
        builder.HasOne(p => p.Manager)
            .WithMany(m => m.ManagedProjects)
            .HasForeignKey(p => p.ManagerId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasMany(p => p.Employees)
            .WithMany(e => e.Projects);
    }
}