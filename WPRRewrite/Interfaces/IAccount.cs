using WPRRewrite.Dtos;
using WPRRewrite.Enums;

namespace WPRRewrite.Interfaces;

public interface IAccount
{
    int AccountId { get; set; }
    string Email { get; set; }
    string Wachtwoord { get; set; }
    AccountTypeEnum AccountType { get; set; }

    void UpdateAccount(AccountDto nieuweGegevens);
}