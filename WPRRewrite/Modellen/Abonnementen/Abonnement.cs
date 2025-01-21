using WPRRewrite.Dtos;

namespace WPRRewrite.Modellen.Abonnementen;

public abstract class Abonnement
{
    public int AbonnementId { get; set; }
    public int MaxVoertuigen { get; set; }
    public int MaxMedewerkers { get; set; }
    public string AbonnementType { get; set; }
    public DateOnly Begindatum { get; set; }

    protected Abonnement() { }
    protected Abonnement(int maxVoertuigen, int maxMedewerkers)
    {
        MaxVoertuigen = maxVoertuigen;
        MaxMedewerkers = maxMedewerkers;
    }

    public static Abonnement MaakAbonnement(AbonnementDto abonnementDto)
    {
        return abonnementDto.AbonementType switch
        {
            "PayAsYouGo" => new PayAsYouGo(
                abonnementDto.MaxMedewerkers, 
                abonnementDto.MaxVoertuigen
            ),
            "UpFront" => new UpFront(
                abonnementDto.MaxMedewerkers, 
                abonnementDto.MaxVoertuigen
                
            ),
            _ => throw new ArgumentException($"Onbekend account type: {abonnementDto.AbonementType}")
        };
    }
}