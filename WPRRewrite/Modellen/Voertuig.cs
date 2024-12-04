﻿using WPRRewrite.Enums;

namespace WPRRewrite.Modellen;

public class Voertuig
{
    public int VoertuigId { get; set; }
    public string Kenteken { get; set; }
    public string Merk { get; set; }
    public string Model { get; set; }
    public string Kleur { get; set; }
    public int Aanschafjaar { get; set; }
    public int Prijs { get; set; }
    public TypeVoertuig TypeVoertuig { get; set; }
    private string typeVoertuig { get; set; }

    public void UpdateVoertuig(Voertuig updatedVoertuig)
    {
        Kenteken = updatedVoertuig.Kenteken;
        Merk = updatedVoertuig.Merk;
        Model = updatedVoertuig.Model;
        Kleur = updatedVoertuig.Kleur;
        Aanschafjaar = updatedVoertuig.Aanschafjaar;
        Prijs = updatedVoertuig.Prijs;
    }

    public void CreateVoertuigAsync(TypeVoertuig typeVoertuig)
    {
        switch (typeVoertuig)
        {
            case TypeVoertuig.Auto:
                this.typeVoertuig = "Auto";
                break;
            case TypeVoertuig.Camper:
                this.typeVoertuig = "Camper";
                break;
            case TypeVoertuig.Caravan:
                this.typeVoertuig = "Caravan";
                break;
        }
    }
}