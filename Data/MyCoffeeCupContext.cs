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

    

    public DbSet<Payouts> Payouts { get; set; }

    public DbSet<Role> Role { get; set; }

    public DbSet<Shift> Shift { get; set; }

    public DbSet<TechnicalMap> TechnicalMap { get; set; }

    public DbSet<User> User { get; set; }

    public DbSet<WorkPlace> WorkPlace { get; set; }
    public DbSet<ShiftPayouts> ShiftPayout { get; set; }
    public DbSet<AvansPayouts> AvansPayout { get; set; }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Автоинкремент для основных таблиц
        modelBuilder.Entity<Employee>().Property(x => x.IdEmployee).ValueGeneratedOnAdd();
        modelBuilder.Entity<Avans>().Property(x => x.IdAvans).ValueGeneratedOnAdd();
        modelBuilder.Entity<Category>().Property(x => x.IdCategory).ValueGeneratedOnAdd();
        modelBuilder.Entity<CategoryDrink>().Property(x => x.IdCarDrink).ValueGeneratedOnAdd();
        modelBuilder.Entity<Cities>().Property(x => x.IdCity).ValueGeneratedOnAdd();
        modelBuilder.Entity<EmployeeHistorySalary>().Property(x => x.IdHistorySalary).ValueGeneratedOnAdd();
        modelBuilder.Entity<JobTitle>().Property(x => x.IdJobTitle).ValueGeneratedOnAdd();
        modelBuilder.Entity<List>().Property(x => x.IdList).ValueGeneratedOnAdd();
        modelBuilder.Entity<Payouts>().Property(x => x.IdPayouts).ValueGeneratedOnAdd();
        modelBuilder.Entity<Role>().Property(x => x.IdRole).ValueGeneratedOnAdd();
        modelBuilder.Entity<Shift>().Property(x => x.IdShifts).ValueGeneratedOnAdd();
        modelBuilder.Entity<TechnicalMap>().Property(x => x.IdTech).ValueGeneratedOnAdd();
        modelBuilder.Entity<User>().Property(x => x.IdUsers).ValueGeneratedOnAdd();
        modelBuilder.Entity<WorkPlace>().Property(x => x.IdWorkPlace).ValueGeneratedOnAdd();

        // ============ НАСТРОЙКИ ДЛЯ ТАБЛИЦ СВЯЗИ ============

        // 1. ShiftPayout (связь смен и выплат)
        modelBuilder.Entity<ShiftPayouts>()
            .HasKey(sp => new { sp.IdShift, sp.IdPayouts }); // Составной ключ

        modelBuilder.Entity<ShiftPayouts>()
            .HasOne(sp => sp.Shift)
            .WithMany(s => s.ShiftPayouts)
            .HasForeignKey(sp => sp.IdShift)
            .OnDelete(DeleteBehavior.Cascade); // Каскадное удаление

        modelBuilder.Entity<ShiftPayouts>()
            .HasOne(sp => sp.Payouts)
            .WithMany(p => p.ShiftPayouts)
            .HasForeignKey(sp => sp.IdPayouts)
            .OnDelete(DeleteBehavior.Cascade); // Каскадное удаление

        // 2. AvansPayout (связь авансов и выплат)
        modelBuilder.Entity<AvansPayouts>()
            .HasKey(ap => new { ap.IdAvans, ap.IdPayouts }); // Составной ключ

        modelBuilder.Entity<AvansPayouts>()
            .HasOne(ap => ap.Avans)
            .WithMany(a => a.AvansPayouts)
            .HasForeignKey(ap => ap.IdAvans)
            .OnDelete(DeleteBehavior.Cascade); // Каскадное удаление

        modelBuilder.Entity<AvansPayouts>()
            .HasOne(ap => ap.Payouts)
            .WithMany(p => p.AvansPayouts)
            .HasForeignKey(ap => ap.IdPayouts)
            .OnDelete(DeleteBehavior.Cascade); // Каскадное удаление

        // ============ НАСТРОЙКИ ДЛЯ СУЩЕСТВУЮЩИХ ТАБЛИЦ ============

        // Связи для Payout
        modelBuilder.Entity<Payouts>()
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
        modelBuilder.Entity<Avans>().Property(x => x.IdAvans).ValueGeneratedOnAdd();

        // Уникальные индексы (если нужны)
        modelBuilder.Entity<ShiftPayouts>()
            .HasIndex(sp => new { sp.IdShift, sp.IdPayouts })
            .IsUnique(); // Гарантируем уникальность связи

        modelBuilder.Entity<AvansPayouts>()
            .HasIndex(ap => new { ap.IdAvans, ap.IdPayouts })
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
