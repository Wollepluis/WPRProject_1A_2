﻿using Microsoft.AspNetCore.Identity;
using WPRRewrite.Modellen.Accounts;

namespace WPRRewrite.Interfaces;

public interface IAccount
{
    int AccountId { get; set; }
    string Email { get; set; }
    string Wachtwoord { get; set; }
    string TypeAccount { get; set; }

    void UpdateAccount(IAccount account);
    PasswordVerificationResult WachtwoordVerify(string password);
}