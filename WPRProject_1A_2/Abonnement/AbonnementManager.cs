namespace WPRProject_1A_2.Abonnement;

public class AbonnementManager
{
    
    private static readonly AbonnementManager instance = new AbonnementManager();
    public static AbonnementManager Instance
    {
        get
        {
            return instance;
        }
    }
    
    public List<Abonnement> Abonnementen = new List<Abonnement>();

    public void VoegAbonnementToe(Abonnement abonnement)
    {
        Abonnementen.Add(abonnement);
    }

    public List<Abonnement> GetAbonnementen()
    {
        return Abonnementen;
    }
}