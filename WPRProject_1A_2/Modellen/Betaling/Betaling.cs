namespace WPRProject_1A_2.Modellen.Betaling;

public class Betaling
{
    public int BetalingId { get; set; }
    public double Bedrag { get; set; }
    public bool IsBetaald { get; private set; }

    public Betaling(int betalingId, double bedrag)
    {
        BetalingId = betalingId;
        Bedrag = bedrag;
        IsBetaald = false;
    }

    public void VerwerkBetaling()
    {
        if (IsBetaald)
        {
            Console.WriteLine("Betaling is succesvol verwerkt.");
        }
        else
        {
            IsBetaald = true;
            Console.WriteLine($"Betaling met ID {BetalingId} van €{Bedrag} is verwerkt.");
        }
    }

    public override string ToString()
    {
        return $"Betaling ID: {BetalingId}\n" +
               $"Bedrag: {Bedrag}\n" +
               $"Status: {(IsBetaald ? "Betaald" : "Niet Betaald")}";
    }
}