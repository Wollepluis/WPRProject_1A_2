namespace WPRRewrite.Modellen.Accounts;

public abstract class AccountZakelijk : Account
{
    public int BedrijfsId { get; set; }

    public List<Reservering> Reserveringen { get; set; }
    public void AddReservering(Reservering reservering)
    {
        Reserveringen.Add(reservering);
    }
}