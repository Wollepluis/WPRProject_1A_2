namespace WPRProject_1A_2.Modellen.Abonnementen;

public class PayAsYouGo : Abonnement
{
    public int MaandelijkseKosten { get; set; }
    public int ProcentueleKorting { get; set; }

    public PayAsYouGo(int maxVoertuigen, int maxMedewerkers) : base(maxVoertuigen, maxMedewerkers)
    {
        //MaandelijkseKosten = maandelijkseKosten;
        //ProcentueleKorting = procentueleKorting;
    }
}