namespace WPRProject_1A_2.Modellen.Abonnementen;

public class Adres
{
    public int Id { get; set; }
    public required string Plaats { get; set; }
    public required string Straatnaam { get; set; }
    public required int Huisnummer { get; set; }
}