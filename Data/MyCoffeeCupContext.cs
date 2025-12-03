using System;
using System.Collections.Generic;
using API.Models.Entities;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models.Entities;

namespace WebApplication1.Data;

public partial class MyCoffeeCupContext : DbContext
{
    public MyCoffeeCupContext(DbContextOptions<MyCoffeeCupContext> options)
        : base(options)
    {
    }

   
    public DbSet<Avans> Avans { get; set; }
    public  DbSet<Cities> Cities { get; set; }
    public  DbSet<Category> Category { get; set; }

    public  DbSet<CategoryDrink> CategoryDrink { get; set; }

    public DbSet<Employee> Employee { get; set; }

    public  DbSet<EmployeeHistorySalary> EmployeeHistorySalary { get; set; }

    public  DbSet<JobTitle> JobTitle { get; set; }

    public DbSet<List> List { get; set; }

    public DbSet<PayRollPeriod> PayRollPeriod { get; set; }

    public DbSet<Payout> Payout { get; set; }

    public DbSet<Role> Role { get; set; }

    public DbSet<Shift> Shift { get; set; }

    public DbSet<TechnicalMap> TechnicalMap { get; set; }

    public DbSet<User> User { get; set; }

    public DbSet<WorkPlace> WorkPlace { get; set; }
    public DbSet<ShiftPayout> ShiftPayout { get; set; }
    public DbSet<AvansPayout> AvansPayout { get; set; }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Автоинкремент для основных таблиц
        modelBuilder.Entity<Employee>().Property(x => x.IdEmployee).ValueGeneratedOnAdd();
        modelBuilder.Entity<Avans>().Property(x => x.idAvans).ValueGeneratedOnAdd();
        modelBuilder.Entity<Category>().Property(x => x.IdCategory).ValueGeneratedOnAdd();
        modelBuilder.Entity<CategoryDrink>().Property(x => x.IdCarDrink).ValueGeneratedOnAdd();
        modelBuilder.Entity<Cities>().Property(x => x.IdCity).ValueGeneratedOnAdd();
        modelBuilder.Entity<EmployeeHistorySalary>().Property(x => x.IdHistorySalary).ValueGeneratedOnAdd();
        modelBuilder.Entity<JobTitle>().Property(x => x.IdJobTitle).ValueGeneratedOnAdd();
        modelBuilder.Entity<List>().Property(x => x.IdList).ValueGeneratedOnAdd();
        modelBuilder.Entity<Payout>().Property(x => x.IdPayouts).ValueGeneratedOnAdd();
        modelBuilder.Entity<PayRollPeriod>().Property(x => x.IdPayPeriod).ValueGeneratedOnAdd();
        modelBuilder.Entity<Role>().Property(x => x.IdRole).ValueGeneratedOnAdd();
        modelBuilder.Entity<Shift>().Property(x => x.IdShifts).ValueGeneratedOnAdd();
        modelBuilder.Entity<TechnicalMap>().Property(x => x.IdTech).ValueGeneratedOnAdd();
        modelBuilder.Entity<User>().Property(x => x.IdUsers).ValueGeneratedOnAdd();
        modelBuilder.Entity<WorkPlace>().Property(x => x.IdWorkPlace).ValueGeneratedOnAdd();

        // ============ НАСТРОЙКИ ДЛЯ ТАБЛИЦ СВЯЗИ ============

        // 1. ShiftPayout (связь смен и выплат)
        modelBuilder.Entity<ShiftPayout>()
            .HasKey(sp => new { sp.IdShift, sp.IdPayout }); // Составной ключ

        modelBuilder.Entity<ShiftPayout>()
            .HasOne(sp => sp.Shift)
            .WithMany(s => s.ShiftPayouts)
            .HasForeignKey(sp => sp.IdShift)
            .OnDelete(DeleteBehavior.Cascade); // Каскадное удаление

        modelBuilder.Entity<ShiftPayout>()
            .HasOne(sp => sp.Payout)
            .WithMany(p => p.ShiftPayouts)
            .HasForeignKey(sp => sp.IdPayout)
            .OnDelete(DeleteBehavior.Cascade); // Каскадное удаление

        // 2. AvansPayout (связь авансов и выплат)
        modelBuilder.Entity<AvansPayout>()
            .HasKey(ap => new { ap.IdAvans, ap.IdPayout }); // Составной ключ

        modelBuilder.Entity<AvansPayout>()
            .HasOne(ap => ap.Avans)
            .WithMany(a => a.AvansPayouts)
            .HasForeignKey(ap => ap.IdAvans)
            .OnDelete(DeleteBehavior.Cascade); // Каскадное удаление

        modelBuilder.Entity<AvansPayout>()
            .HasOne(ap => ap.Payout)
            .WithMany(p => p.AvansPayouts)
            .HasForeignKey(ap => ap.IdPayout)
            .OnDelete(DeleteBehavior.Cascade); // Каскадное удаление

        // ============ НАСТРОЙКИ ДЛЯ СУЩЕСТВУЮЩИХ ТАБЛИЦ ============

        // Связи для Payout
        modelBuilder.Entity<Payout>()
            .HasOne(p => p.IdEmployeeNavigation)
            .WithMany(e => e.Payouts)
            .HasForeignKey(p => p.IdEmployee)
            .OnDelete(DeleteBehavior.Restrict); // Ограниченное удаление

        // Связи для Shift (если еще нет)
        modelBuilder.Entity<Shift>()
            .HasOne(s => s.IdEmployeeNavigation)
            .WithMany(e => e.Shifts)
            .HasForeignKey(s => s.IdEmployee)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Shift>()
            .HasOne(s => s.IdWorkplaceNavigation)
            .WithMany(w => w.Shifts)
            .HasForeignKey(s => s.IdWorkplace)
            .OnDelete(DeleteBehavior.Restrict);

        // Связи для Avans (если еще нет)
        modelBuilder.Entity<Avans>()
            .HasOne(a => a.IdEmployeeNavigation)
            .WithMany(e => e.Avans)
            .HasForeignKey(a => a.IdEmployee)
            .OnDelete(DeleteBehavior.Restrict);

        // Уникальные индексы (если нужны)
        modelBuilder.Entity<ShiftPayout>()
            .HasIndex(sp => new { sp.IdShift, sp.IdPayout })
            .IsUnique(); // Гарантируем уникальность связи

        modelBuilder.Entity<AvansPayout>()
            .HasIndex(ap => new { ap.IdAvans, ap.IdPayout })
            .IsUnique(); // Гарантируем уникальность связи

        // Настройка для List (если есть связь с Shift)
        modelBuilder.Entity<List>()
            .HasOne(l => l.IdShiftNavigation)
            .WithMany(s => s.Lists)
            .HasForeignKey(l => l.IdShift)
            .OnDelete(DeleteBehavior.Cascade);

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    
}
