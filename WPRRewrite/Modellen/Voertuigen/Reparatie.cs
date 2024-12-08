namespace WPRRewrite.Modellen.Voertuigen;

public class Reparatie
{
    public int ReparatieId { get; set; }
    public string Beschrijving { get; set; }
    public DateTime ReparatieDatum { get; set; }
    public List<String> Opmerkingen { get; set; }
}