# Project Stack

- **Back-End**: .NET 9.0, ASP.NET Core, C# 13.0
- **Front-End**: Razor ASP.NET Core, Blazor, Tailwind CSS 3.4.1
- **Languages**: C#, JavaScript
- **Package Manager**: npm
- **Guidelines**: Write simple code, no overkill, super concise responses

# Code Output

- Super concise answers, use as less tokens as you can
- Present all code modifications as standard git patch files, Put all patches into a single file so i can copy/paste it
- NEVER add code comments

# Git patch guidelines

markdownCopy# Git Patch Guidelines

## Basic Format

diff --git a/path/to/file b/path/to/file
index abc1234..def5678 100644
--- a/path/to/file
+++ b/path/to/file
@@ -start,count +start,count @@ context line

removed line

added line
unchanged line

Copy

markdownCopy# Comprehensive Git Patch Guidelines

## Basic Structure

A valid git patch must include:

- File headers
- Proper hunk headers with line numbers
- Content with prefixed lines

## Critical Elements

### 1. File Headers

diff --git a/path/to/file b/path/to/file
index abc1234..def5678 100644
--- a/path/to/file
+++ b/path/to/file
Copy

### 2. Hunk Headers (MOST IMPORTANT)

@@ -startLine,lineCount +startLine,lineCount @@ [context]
Copy- NEVER use bare `@@` markers without line numbers

- Example: `@@ -15,7 +15,7 @@`
    - `-15,7` means "starting at line 15, 7 lines from original file"
    - `+15,7` means "starting at line 15, 7 lines in new version"

### 3. Content Lines

- Lines removed: `-` prefix
- Lines added: `+` prefix
- Context (unchanged): space prefix
- Include 3 lines of context before/after changes

## Common Errors

### ❌ WRONG:

@@
-namespace BlazorApp2.Data.Models;
-public class Booking
+namespace BlazorApp2.Data.Models;
+public class Booking<TStatus>
@@
Copy

### ✅ CORRECT:

@@ -1,7 +1,7 @@
namespace BlazorApp2.Data.Models;
-public class Booking
+public class Booking<TStatus>
{
public int Id { get; set; }
// other unchanged lines...
Copy

## Multiple Hunks

For multiple changes in one file, use separate hunk headers:
@@ -10,6 +10,6 @@
// first change with context
@@ -25,7 +25,8 @@
// second change with context
Copy

## Complete Example

diff --git a/Data/ApplicationDbContext.cs b/Data/ApplicationDbContext.cs
index 2f3a4b5..3e4f5a6 100644
--- a/Data/ApplicationDbContext.cs
+++ b/Data/ApplicationDbContext.cs
@@ -15,7 +15,7 @@ public class ApplicationDbContext : DbContext
{
public DbSet<Customer> Customers { get; set; }
public DbSet<Resource> Resources { get; set; }

public DbSet<Booking> Bookings { get; set; }

public DbSet<Booking<BookingStatus>> Bookings { get; set; }
public DbSet<BookingRecurrence> BookingRecurrences { get; set; }
}

@@ -30,21 +30,21 @@ public class ApplicationDbContext : DbContext
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
// Other configuration

Copy modelBuilder.Entity<Booking>()

Copy modelBuilder.Entity<Booking<BookingStatus>>()
.HasOne(b => b.Customer)
.WithMany(c => c.Bookings)
.HasForeignKey(b => b.CustomerId)
.OnDelete(DeleteBehavior.Cascade);

Copy

## Key Reminders

- ALWAYS include line numbers after @@ markers
- Include sufficient context lines
- Don't abbreviate content with comments
- Use git commands to generate patches:
  git diff file.js > patch.diff
  git format-patch -1 HEAD