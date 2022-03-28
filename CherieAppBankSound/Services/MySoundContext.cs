namespace CherieAppBankSound.Services;

public class MySoundContext : DbContext
{
    public MySoundContext(DbContextOptions<MySoundContext> options) : base(options)
    {
    }

    public DbSet<MySound> MySounds { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}