using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;

public class DatabaseContext : DbContext
{
    public DbSet<Order> Orders { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Address> Addresses { get; set;}
    public DbSet<Session> Sessions { get; set; }
    public DbSet<LoginLog> LoginLogs { get; set; }
    public string DbPath { get; }

    public DatabaseContext()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = System.IO.Path.Join(path, "qlp.db");
    }

    // The following configures EF to create a Sqlite database file in the
    // special "local" folder for your platform.
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");
        
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure relationships between entities
        modelBuilder.Entity<Session>()
            .HasOne(s => s.User)
            .WithMany(u => u.Sessions)
            .HasForeignKey(s => s.UserId);

        modelBuilder.Entity<LoginLog>()
            .HasOne(ll => ll.User)
            .WithMany(u => u.LoginLogs)
            .HasForeignKey(ll => ll.UserId);
        
        modelBuilder.Entity<Order>()
            .HasOne(o => o.Customer)
            .WithMany(c => c.Orders)
            .HasForeignKey(o => o.CustomerId);
    }
}

public class Order
{
    [Key]
    public int OrderId { get; set; }
    public int Price { get; set; }
    public Payment? Payment { get; set; }
    public int CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;
    public DateTime OrderDate { get; set; }
    public Address? ShippingAddress { get; set; }
    public string SalesRep { get; set; } = null!;
    public string OrderCode { get; set; } = null!;
}

public class Address {
    [Key]
    public int AddressId { get; set; }
    public string AddressLine1 { get; set; } = null!;
    public string AddressLine2 { get; set; } = null!;
    public string City { get; set; } = null!;
    public string State { get; set; } = null!;
    public string Country { get; set; } = null!;
    public int ZipCode { get; set; }
    public int orderId { get; set; } 
    public Order Order { get; set; } = null!;
    public int PaymentId { get; set; } 
    public Payment Payment { get; set; } = null!;
}

public class Payment {
    [Key]
    public int PaymentId { get; set; }
    public int CreditCard { get; set; }
    public int SecurityCode { get; set; }
    public int ExpirationDate { get; set; }
    public Address? BillingAddress { get; set; } = null!;
    public string TaxExemptStatus { get; set; } = null!;
    public int OrderId { get; set; }
    public Order Order { get; set; } = null!;

}

public class User {
   [Key]
   public int UserId { get; set; }
   [Required]
   [MaxLength(100)]
   public string Email { get; set; } = null!;
   [Required]
   [MaxLength(255)]
   public string Password { get; set; } = null!;
   public string FistName { get; set; } = null!;
   public string LastName { get; set; } = null!;
   public ICollection<Session> Sessions { get; set; } = null!;
   public ICollection<LoginLog> LoginLogs { get; set; } = null!;
}

public class Customer {
  [Key]
  public int CustomerId { get; set; }
  public string FistName { get; set; } = null!;
  public string LastName { get; set; } = null!;
  public string CompanyName { get; set; } = null!;
  public List<Order> Orders { get; } = new();
  public string Email {get; set;} = null!;
  public int PhoneNumber {get; set; }
  public string CustomerNotes { get; set; } = null!;
  public bool EmailNotifications { get; set; }
}


public class Session
{
    [Key]
    public int SessionId { get; set; }

    public int UserId { get; set; }
    
    public string Token { get; set; } = null!;
    
    public DateTime LoginTime { get; set; }
    
    public DateTime LastActivityTime { get; set; }

    public User User { get; set; } = null!;
}

public class LoginLog
{
    [Key]
    public int LogId { get; set; }

    public int UserId { get; set; }
    
    public string Action { get; set; } = null!;
    
    public DateTime LogTime { get; set; }

    // Navigation property for user
    public User User { get; set; } = null!; 
}