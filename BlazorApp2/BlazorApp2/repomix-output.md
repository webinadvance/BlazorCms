This file is a merged representation of a subset of the codebase, containing specifically included files and files not matching ignore patterns, combined into a single document by Repomix.
The content has been processed where comments have been removed, empty lines have been removed.

# File Summary

## Purpose
This file contains a packed representation of the entire repository's contents.
It is designed to be easily consumable by AI systems for analysis, code review,
or other automated processes.

## File Format
The content is organized as follows:
1. This summary section
2. Repository information
3. Directory structure
4. Multiple file entries, each consisting of:
  a. A header with the file path (## File: path/to/file)
  b. The full contents of the file in a code block

## Usage Guidelines
- This file should be treated as read-only. Any changes should be made to the
  original repository files, not this packed version.
- When processing this file, use the file path to distinguish
  between different files in the repository.
- Be aware that this file may contain sensitive information. Handle it with
  the same level of security as you would the original repository.

- Pay special attention to the Repository Instruction. These contain important context and guidelines specific to this project.

## Notes
- Some files may have been excluded based on .gitignore rules and Repomix's configuration
- Binary files are not included in this packed representation. Please refer to the Repository Structure section for a complete list of file paths, including binary files
- Only files matching these patterns are included: **/*.cs, **/*.razor
- Files matching these patterns are excluded: **/obj/**, **/debug/**, **/appsettings*.json
- Files matching patterns in .gitignore are excluded
- Files matching default ignore patterns are excluded
- Code comments have been removed from supported file types
- Empty lines have been removed from all files

## Additional Info

# Directory Structure
```
Components/_Imports.razor
Components/App.razor
Components/Pages/Error.razor
Data/ApplicationDbContext.cs
Data/DbInitializer.cs
Data/Models/Booking.cs
Data/Models/BookingStatus.cs
Data/Models/Customer.cs
Data/Models/Resource.cs
Data/Models/ResourceType.cs
Data/Services/BookingService.cs
Migrations/20250308141958_InitialCreate.cs
Migrations/20250308141958_InitialCreate.Designer.cs
Migrations/ApplicationDbContextModelSnapshot.cs
Program.cs
```

# Files

## File: Components/_Imports.razor
```
@using System.Net.Http
@using System.Net.Http.Json
@using Microsoft.AspNetCore.Components.Forms
@using Microsoft.AspNetCore.Components.Routing
@using Microsoft.AspNetCore.Components.Web
@using static Microsoft.AspNetCore.Components.Web.RenderMode
@using Microsoft.AspNetCore.Components.Web.Virtualization
@using Microsoft.JSInterop
@using BlazorApp2
@using BlazorApp2.Client
@using BlazorApp2.Components
```

## File: Components/App.razor
```
<!DOCTYPE html>
<html lang="en">

<head>
    <meta charset="utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <base href="/"/>
    <script src="https://cdn.tailwindcss.com/3.4.1"></script>
    <link href="/app.css" rel="stylesheet"/>
    <link rel="stylesheet" href="@Assets["app.css"]"/>
    <ImportMap/>
    <link rel="icon" type="image/png" href="favicon.png"/>
    <HeadOutlet @rendermode="InteractiveWebAssembly"/>
</head>

<body>
<Routes @rendermode="InteractiveWebAssembly"/>
<script src="_framework/blazor.web.js"></script>
</body>

</html>
```

## File: Components/Pages/Error.razor
```
@page "/Error"
@using System.Diagnostics

<PageTitle>Error</PageTitle>

<h1 class="text-danger">Error.</h1>
<h2 class="text-danger">An error occurred while processing your request.</h2>

@if (ShowRequestId)
{
    <p>
        <strong>Request ID:</strong> <code>@RequestId</code>
    </p>
}

<h3>Development Mode</h3>
<p>
    Swapping to <strong>Development</strong> environment will display more detailed information about the error that occurred.
</p>
<p>
    <strong>The Development environment shouldn't be enabled for deployed applications.</strong>
    It can result in displaying sensitive information from exceptions to end users.
    For local debugging, enable the <strong>Development</strong> environment by setting the <strong>ASPNETCORE_ENVIRONMENT</strong> environment variable to <strong>Development</strong>
    and restarting the app.
</p>

@code{
    [CascadingParameter] private HttpContext? HttpContext { get; set; }

    private string? RequestId { get; set; }
    private bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

    protected override void OnInitialized() =>
        RequestId = Activity.Current?.Id ?? HttpContext?.TraceIdentifier;

}
```

## File: Data/ApplicationDbContext.cs
```csharp
using BlazorApp2.Data.Models;
using Microsoft.EntityFrameworkCore;
namespace BlazorApp2.Data;
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Resource> Resources { get; set; }
    public DbSet<ResourceType> ResourceTypes { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Booking>()
            .HasOne(b => b.Customer)
            .WithMany(c => c.Bookings)
            .HasForeignKey(b => b.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Booking>()
            .HasOne(b => b.Resource)
            .WithMany(r => r.Bookings)
            .HasForeignKey(b => b.ResourceId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Resource>()
            .HasOne(r => r.ResourceType)
            .WithMany(rt => rt.Resources)
            .HasForeignKey(r => r.ResourceTypeId)
            .OnDelete(DeleteBehavior.SetNull);
        modelBuilder.Entity<ResourceType>()
            .HasData(new ResourceType { Id = 1, Name = "Room", Description = "Meeting or event rooms" },
                new ResourceType { Id = 2, Name = "Equipment", Description = "Rentable equipment" },
                new ResourceType { Id = 3, Name = "Service", Description = "Bookable services" });
        modelBuilder.Entity<Resource>()
            .HasData(
                new Resource
                {
                    Id = 1,
                    Name = "Conference Room A",
                    Description = "Large conference room",
                    IsAvailable = true,
                    HourlyRate = 50,
                    DailyRate = 300,
                    ResourceTypeId = 1
                },
                new Resource
                {
                    Id = 2,
                    Name = "Meeting Room B",
                    Description = "Small meeting room",
                    IsAvailable = true,
                    HourlyRate = 30,
                    DailyRate = 180,
                    ResourceTypeId = 1
                }, new Resource
                {
                    Id = 3,
                    Name = "Projector",
                    Description = "HD Projector",
                    IsAvailable = true,
                    HourlyRate = 15,
                    DailyRate = 60,
                    ResourceTypeId = 2
                });
    }
}
```

## File: Data/DbInitializer.cs
```csharp
using Microsoft.EntityFrameworkCore;
namespace BlazorApp2.Data;
public static class DbInitializer
{
    public static async Task InitializeAsync(WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await context.Database.MigrateAsync();
    }
}
```

## File: Data/Models/Booking.cs
```csharp
namespace BlazorApp2.Data.Models;
public class Booking
{
    public int Id { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public BookingStatus Status { get; set; } = BookingStatus.Pending;
    public string? Notes { get; set; }
    public decimal TotalPrice { get; set; }
    public int CustomerId { get; set; }
    public Customer? Customer { get; set; }
    public int ResourceId { get; set; }
    public Resource? Resource { get; set; }
}
```

## File: Data/Models/BookingStatus.cs
```csharp
namespace BlazorApp2.Data.Models;
public enum BookingStatus
{
    Pending,
    Confirmed,
    Cancelled,
    Completed
}
```

## File: Data/Models/Customer.cs
```csharp
namespace BlazorApp2.Data.Models;
public class Customer
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public List<Booking> Bookings { get; set; } = new();
}
```

## File: Data/Models/Resource.cs
```csharp
namespace BlazorApp2.Data.Models;
public class Resource
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsAvailable { get; set; } = true;
    public decimal? HourlyRate { get; set; }
    public decimal? DailyRate { get; set; }
    public List<Booking> Bookings { get; set; } = new();
    public int? ResourceTypeId { get; set; }
    public ResourceType? ResourceType { get; set; }
}
```

## File: Data/Models/ResourceType.cs
```csharp
namespace BlazorApp2.Data.Models;
public class ResourceType
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<Resource> Resources { get; set; } = new();
}
```

## File: Data/Services/BookingService.cs
```csharp
using BlazorApp2.Data.Models;
using Microsoft.EntityFrameworkCore;
namespace BlazorApp2.Data.Services;
public class BookingService
{
    private readonly ApplicationDbContext _context;
    public BookingService(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<List<Resource>> GetResourcesAsync()
    {
        return await _context.Resources.Include(r => r.ResourceType)
            .ToListAsync();
    }
    public async Task<Resource?> GetResourceByIdAsync(int id)
    {
        return await _context.Resources.Include(r => r.ResourceType)
            .FirstOrDefaultAsync(r => r.Id == id);
    }
    public async Task<List<ResourceType>> GetResourceTypesAsync()
    {
        return await _context.ResourceTypes.ToListAsync();
    }
    public async Task<List<Customer>> GetCustomersAsync()
    {
        return await _context.Customers.ToListAsync();
    }
    public async Task<Customer?> GetCustomerByIdAsync(int id)
    {
        return await _context.Customers.Include(c => c.Bookings)
            .FirstOrDefaultAsync(c => c.Id == id);
    }
    public async Task<Customer> AddCustomerAsync(Customer customer)
    {
        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();
        return customer;
    }
    public async Task<List<Booking>> GetBookingsAsync()
    {
        return await _context.Bookings.Include(b => b.Customer)
            .Include(b => b.Resource)
            .ToListAsync();
    }
    public async Task<Booking?> GetBookingByIdAsync(int id)
    {
        return await _context.Bookings.Include(b => b.Customer)
            .Include(b => b.Resource)
            .FirstOrDefaultAsync(b => b.Id == id);
    }
    public async Task<List<Booking>> GetBookingsByResourceIdAsync(int resourceId)
    {
        return await _context.Bookings.Where(b => b.ResourceId == resourceId)
            .Include(b => b.Customer)
            .ToListAsync();
    }
    public async Task<List<Booking>> GetBookingsByCustomerIdAsync(int customerId)
    {
        return await _context.Bookings.Where(b => b.CustomerId == customerId)
            .Include(b => b.Resource)
            .ToListAsync();
    }
    public async Task<Booking> CreateBookingAsync(Booking booking)
    {
        var conflictingBookings = await _context.Bookings.Where(b =>
                b.ResourceId == booking.ResourceId && b.Status != BookingStatus.Cancelled &&
                ((b.StartTime <= booking.StartTime && b.EndTime > booking.StartTime) ||
                 (b.StartTime < booking.EndTime && b.EndTime >= booking.EndTime) ||
                 (b.StartTime >= booking.StartTime && b.EndTime <= booking.EndTime)))
            .AnyAsync();
        if (conflictingBookings)
            throw new InvalidOperationException("The resource is not available during the selected time period.");
        var resource = await _context.Resources.FindAsync(booking.ResourceId);
        if (resource != null)
        {
            var duration = booking.EndTime - booking.StartTime;
            if (duration.TotalHours <= 24 && resource.HourlyRate.HasValue)
            {
                booking.TotalPrice = (decimal)duration.TotalHours * resource.HourlyRate.Value;
            }
            else if (resource.DailyRate.HasValue)
            {
                var days = Math.Ceiling(duration.TotalDays);
                booking.TotalPrice = (decimal)days * resource.DailyRate.Value;
            }
        }
        _context.Bookings.Add(booking);
        await _context.SaveChangesAsync();
        return booking;
    }
    public async Task<Booking?> UpdateBookingStatusAsync(int id, BookingStatus status)
    {
        var booking = await _context.Bookings.FindAsync(id);
        if (booking == null) return null;
        booking.Status = status;
        await _context.SaveChangesAsync();
        return booking;
    }
}
```

## File: Migrations/20250308141958_InitialCreate.cs
```csharp
using System;
using Microsoft.EntityFrameworkCore.Migrations;
#nullable disable
#pragma warning disable CA1814
namespace BlazorApp2.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });
            migrationBuilder.CreateTable(
                name: "ResourceTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResourceTypes", x => x.Id);
                });
            migrationBuilder.CreateTable(
                name: "Resources",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false),
                    HourlyRate = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DailyRate = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ResourceTypeId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resources", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Resources_ResourceTypes_ResourceTypeId",
                        column: x => x.ResourceTypeId,
                        principalTable: "ResourceTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });
            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    ResourceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bookings_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bookings_Resources_ResourceId",
                        column: x => x.ResourceId,
                        principalTable: "Resources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
            migrationBuilder.InsertData(
                table: "ResourceTypes",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Meeting or event rooms", "Room" },
                    { 2, "Rentable equipment", "Equipment" },
                    { 3, "Bookable services", "Service" }
                });
            migrationBuilder.InsertData(
                table: "Resources",
                columns: new[] { "Id", "DailyRate", "Description", "HourlyRate", "IsAvailable", "Name", "ResourceTypeId" },
                values: new object[,]
                {
                    { 1, 300m, "Large conference room", 50m, true, "Conference Room A", 1 },
                    { 2, 180m, "Small meeting room", 30m, true, "Meeting Room B", 1 },
                    { 3, 60m, "HD Projector", 15m, true, "Projector", 2 }
                });
            migrationBuilder.CreateIndex(
                name: "IX_Bookings_CustomerId",
                table: "Bookings",
                column: "CustomerId");
            migrationBuilder.CreateIndex(
                name: "IX_Bookings_ResourceId",
                table: "Bookings",
                column: "ResourceId");
            migrationBuilder.CreateIndex(
                name: "IX_Resources_ResourceTypeId",
                table: "Resources",
                column: "ResourceTypeId");
        }
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bookings");
            migrationBuilder.DropTable(
                name: "Customers");
            migrationBuilder.DropTable(
                name: "Resources");
            migrationBuilder.DropTable(
                name: "ResourceTypes");
        }
    }
}
```

## File: Migrations/20250308141958_InitialCreate.Designer.cs
```csharp
using System;
using BlazorApp2.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
#nullable disable
namespace BlazorApp2.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250308141958_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);
            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);
            modelBuilder.Entity("BlazorApp2.Data.Models.Booking", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");
                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));
                    b.Property<int>("CustomerId")
                        .HasColumnType("int");
                    b.Property<DateTime>("EndTime")
                        .HasColumnType("datetime2");
                    b.Property<string>("Notes")
                        .HasColumnType("nvarchar(max)");
                    b.Property<int>("ResourceId")
                        .HasColumnType("int");
                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime2");
                    b.Property<int>("Status")
                        .HasColumnType("int");
                    b.Property<decimal>("TotalPrice")
                        .HasColumnType("decimal(18,2)");
                    b.HasKey("Id");
                    b.HasIndex("CustomerId");
                    b.HasIndex("ResourceId");
                    b.ToTable("Bookings");
                });
            modelBuilder.Entity("BlazorApp2.Data.Models.Customer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");
                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));
                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");
                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");
                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");
                    b.HasKey("Id");
                    b.ToTable("Customers");
                });
            modelBuilder.Entity("BlazorApp2.Data.Models.Resource", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");
                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));
                    b.Property<decimal?>("DailyRate")
                        .HasColumnType("decimal(18,2)");
                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");
                    b.Property<decimal?>("HourlyRate")
                        .HasColumnType("decimal(18,2)");
                    b.Property<bool>("IsAvailable")
                        .HasColumnType("bit");
                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");
                    b.Property<int?>("ResourceTypeId")
                        .HasColumnType("int");
                    b.HasKey("Id");
                    b.HasIndex("ResourceTypeId");
                    b.ToTable("Resources");
                    b.HasData(
                        new
                        {
                            Id = 1,
                            DailyRate = 300m,
                            Description = "Large conference room",
                            HourlyRate = 50m,
                            IsAvailable = true,
                            Name = "Conference Room A",
                            ResourceTypeId = 1
                        },
                        new
                        {
                            Id = 2,
                            DailyRate = 180m,
                            Description = "Small meeting room",
                            HourlyRate = 30m,
                            IsAvailable = true,
                            Name = "Meeting Room B",
                            ResourceTypeId = 1
                        },
                        new
                        {
                            Id = 3,
                            DailyRate = 60m,
                            Description = "HD Projector",
                            HourlyRate = 15m,
                            IsAvailable = true,
                            Name = "Projector",
                            ResourceTypeId = 2
                        });
                });
            modelBuilder.Entity("BlazorApp2.Data.Models.ResourceType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");
                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));
                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");
                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");
                    b.HasKey("Id");
                    b.ToTable("ResourceTypes");
                    b.HasData(
                        new
                        {
                            Id = 1,
                            Description = "Meeting or event rooms",
                            Name = "Room"
                        },
                        new
                        {
                            Id = 2,
                            Description = "Rentable equipment",
                            Name = "Equipment"
                        },
                        new
                        {
                            Id = 3,
                            Description = "Bookable services",
                            Name = "Service"
                        });
                });
            modelBuilder.Entity("BlazorApp2.Data.Models.Booking", b =>
                {
                    b.HasOne("BlazorApp2.Data.Models.Customer", "Customer")
                        .WithMany("Bookings")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                    b.HasOne("BlazorApp2.Data.Models.Resource", "Resource")
                        .WithMany("Bookings")
                        .HasForeignKey("ResourceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                    b.Navigation("Customer");
                    b.Navigation("Resource");
                });
            modelBuilder.Entity("BlazorApp2.Data.Models.Resource", b =>
                {
                    b.HasOne("BlazorApp2.Data.Models.ResourceType", "ResourceType")
                        .WithMany("Resources")
                        .HasForeignKey("ResourceTypeId")
                        .OnDelete(DeleteBehavior.SetNull);
                    b.Navigation("ResourceType");
                });
            modelBuilder.Entity("BlazorApp2.Data.Models.Customer", b =>
                {
                    b.Navigation("Bookings");
                });
            modelBuilder.Entity("BlazorApp2.Data.Models.Resource", b =>
                {
                    b.Navigation("Bookings");
                });
            modelBuilder.Entity("BlazorApp2.Data.Models.ResourceType", b =>
                {
                    b.Navigation("Resources");
                });
#pragma warning restore 612, 618
        }
    }
}
```

## File: Migrations/ApplicationDbContextModelSnapshot.cs
```csharp
using System;
using BlazorApp2.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
#nullable disable
namespace BlazorApp2.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);
            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);
            modelBuilder.Entity("BlazorApp2.Data.Models.Booking", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");
                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));
                    b.Property<int>("CustomerId")
                        .HasColumnType("int");
                    b.Property<DateTime>("EndTime")
                        .HasColumnType("datetime2");
                    b.Property<string>("Notes")
                        .HasColumnType("nvarchar(max)");
                    b.Property<int>("ResourceId")
                        .HasColumnType("int");
                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime2");
                    b.Property<int>("Status")
                        .HasColumnType("int");
                    b.Property<decimal>("TotalPrice")
                        .HasColumnType("decimal(18,2)");
                    b.HasKey("Id");
                    b.HasIndex("CustomerId");
                    b.HasIndex("ResourceId");
                    b.ToTable("Bookings");
                });
            modelBuilder.Entity("BlazorApp2.Data.Models.Customer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");
                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));
                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");
                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");
                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");
                    b.HasKey("Id");
                    b.ToTable("Customers");
                });
            modelBuilder.Entity("BlazorApp2.Data.Models.Resource", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");
                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));
                    b.Property<decimal?>("DailyRate")
                        .HasColumnType("decimal(18,2)");
                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");
                    b.Property<decimal?>("HourlyRate")
                        .HasColumnType("decimal(18,2)");
                    b.Property<bool>("IsAvailable")
                        .HasColumnType("bit");
                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");
                    b.Property<int?>("ResourceTypeId")
                        .HasColumnType("int");
                    b.HasKey("Id");
                    b.HasIndex("ResourceTypeId");
                    b.ToTable("Resources");
                    b.HasData(
                        new
                        {
                            Id = 1,
                            DailyRate = 300m,
                            Description = "Large conference room",
                            HourlyRate = 50m,
                            IsAvailable = true,
                            Name = "Conference Room A",
                            ResourceTypeId = 1
                        },
                        new
                        {
                            Id = 2,
                            DailyRate = 180m,
                            Description = "Small meeting room",
                            HourlyRate = 30m,
                            IsAvailable = true,
                            Name = "Meeting Room B",
                            ResourceTypeId = 1
                        },
                        new
                        {
                            Id = 3,
                            DailyRate = 60m,
                            Description = "HD Projector",
                            HourlyRate = 15m,
                            IsAvailable = true,
                            Name = "Projector",
                            ResourceTypeId = 2
                        });
                });
            modelBuilder.Entity("BlazorApp2.Data.Models.ResourceType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");
                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));
                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");
                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");
                    b.HasKey("Id");
                    b.ToTable("ResourceTypes");
                    b.HasData(
                        new
                        {
                            Id = 1,
                            Description = "Meeting or event rooms",
                            Name = "Room"
                        },
                        new
                        {
                            Id = 2,
                            Description = "Rentable equipment",
                            Name = "Equipment"
                        },
                        new
                        {
                            Id = 3,
                            Description = "Bookable services",
                            Name = "Service"
                        });
                });
            modelBuilder.Entity("BlazorApp2.Data.Models.Booking", b =>
                {
                    b.HasOne("BlazorApp2.Data.Models.Customer", "Customer")
                        .WithMany("Bookings")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                    b.HasOne("BlazorApp2.Data.Models.Resource", "Resource")
                        .WithMany("Bookings")
                        .HasForeignKey("ResourceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                    b.Navigation("Customer");
                    b.Navigation("Resource");
                });
            modelBuilder.Entity("BlazorApp2.Data.Models.Resource", b =>
                {
                    b.HasOne("BlazorApp2.Data.Models.ResourceType", "ResourceType")
                        .WithMany("Resources")
                        .HasForeignKey("ResourceTypeId")
                        .OnDelete(DeleteBehavior.SetNull);
                    b.Navigation("ResourceType");
                });
            modelBuilder.Entity("BlazorApp2.Data.Models.Customer", b =>
                {
                    b.Navigation("Bookings");
                });
            modelBuilder.Entity("BlazorApp2.Data.Models.Resource", b =>
                {
                    b.Navigation("Bookings");
                });
            modelBuilder.Entity("BlazorApp2.Data.Models.ResourceType", b =>
                {
                    b.Navigation("Resources");
                });
#pragma warning restore 612, 618
        }
    }
}
```

## File: Program.cs
```csharp
using BlazorApp2.Components;
using BlazorApp2.Data;
using BlazorApp2.Data.Services;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<BookingService>();
var app = builder.Build();
await DbInitializer.InitializeAsync(app);
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", true);
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();
app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(BlazorApp2.Client._Imports).Assembly);
app.Run();
```


# Instruction
﻿# Guidelines

- Write simple and clean code, prioritize readability and maintainability.
- Avoid unnecessary complexity, keep it concise and straightforward.
- Keep the output super minimal and concise.
- in Output, generate PowerShell script for automating modifications to the code.

# Project Stack

- **Back-End**: .NET 9.0, ASP.NET Core, C# 13.0
- **Front-End**: Razor ASP.NET Core, Blazor, Tailwind CSS 3.4.1
- **Languages**: C#, JavaScript
- **Package Manager**: npm
- **Guidelines**: Write simple code, no overkill, super concise responses
