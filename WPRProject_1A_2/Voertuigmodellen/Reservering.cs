namespace WPRProject_1A_2.Voertuigmodellen;

public class Reservering
{
    public int Id { get; set; }
    public required Voertuig Voertuig { get; set; }
    public required DateTime Begindatum { get; set; }
    public required DateTime Einddatum { get; set; }
}