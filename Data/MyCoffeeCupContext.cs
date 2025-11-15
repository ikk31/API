using System;
using System.Collections.Generic;
using API.Models.Entities;
using Microsoft.EntityFrameworkCore;

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

    public  DbSet<JobTittle> JobTittle { get; set; }

    public DbSet<List> List { get; set; }

    public DbSet<PayRollPeriod> PayRollPeriod { get; set; }

    public DbSet<Payout> Payout { get; set; }

    public DbSet<Role> Role { get; set; }

    public DbSet<Shift> Shift { get; set; }

    public DbSet<TechnicalMap> TechnicalMap { get; set; }

    public DbSet<User> User { get; set; }

    public DbSet<WorkPlace> WorkPlace { get; set; }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Employee>().Property(x => x.IdEmployee).ValueGeneratedOnAdd();
        modelBuilder.Entity<Avans>().Property(x => x.idAvans).ValueGeneratedOnAdd();
        modelBuilder.Entity<Category>().Property(x => x.IdCategory).ValueGeneratedOnAdd();
        modelBuilder.Entity<CategoryDrink>().Property(x => x.IdCarDrink).ValueGeneratedOnAdd();
        modelBuilder.Entity<Cities>().Property(x => x.IdCity).ValueGeneratedOnAdd();
        modelBuilder.Entity<EmployeeHistorySalary>().Property(x => x.IdHistorySalary).ValueGeneratedOnAdd();
        modelBuilder.Entity<JobTittle>().Property(x => x.IdJobTittle).ValueGeneratedOnAdd();
        modelBuilder.Entity<List>().Property(x => x.IdList).ValueGeneratedOnAdd();
        modelBuilder.Entity<Payout>().Property(x => x.IdPayouts).ValueGeneratedOnAdd();
        modelBuilder.Entity<PayRollPeriod>().Property(x => x.IdPayPeriod).ValueGeneratedOnAdd();
        modelBuilder.Entity<Role>().Property(x => x.IdRole).ValueGeneratedOnAdd();
        modelBuilder.Entity<Shift>().Property(x => x.IdShifts).ValueGeneratedOnAdd();
        modelBuilder.Entity<TechnicalMap>().Property(x => x.IdTech).ValueGeneratedOnAdd();
        modelBuilder.Entity<User>().Property(x => x.IdUsers).ValueGeneratedOnAdd();
        modelBuilder.Entity<WorkPlace>().Property(x => x.IdWorkplace).ValueGeneratedOnAdd();

    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
