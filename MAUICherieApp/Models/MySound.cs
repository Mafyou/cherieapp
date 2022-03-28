namespace MAUICherieApp.Models;

public record MySound
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public string Name { get; set; }
    public byte[] MyAudio { get; set; }
}