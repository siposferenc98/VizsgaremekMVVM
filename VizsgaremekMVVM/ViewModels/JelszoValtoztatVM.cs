﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using VizsgaremekMVVM.Models;
using VizsgaremekMVVM.Models.Buttons;

namespace VizsgaremekMVVM.ViewModels
{
    internal class JelszoValtoztatVM : INotifyPropertyChanged
    {
        private HttpClientClass _http = new();
        public string AktualisJelszo { get; set; } = string.Empty;
        public string UjJelszo { get; set; } = string.Empty;
        public string UjJelszoEllenoriz { get; set; } = string.Empty;
        public ICommand JelszoValtoztatButton => new ButtonCE(JelszoValtoztatas,JelszoValtoztatasCE);

        private async void JelszoValtoztatas(object? o)
        {
            if (MD5Hashing.hashPW(AktualisJelszo) == AktivFelhasznalo.Aktiv.Pw)
            {
                if (UjJelszo == UjJelszoEllenoriz)
                {
                    AktivFelhasznalo.Aktiv.Pw = MD5Hashing.hashPW(UjJelszo);
                    var jelszoValtoztatEredmeny = await _http.httpClient.PutAsync(_http.url + $"Felhasznalok/{AktivFelhasznalo.Aktiv.Azon}", _http.contentKrealas(AktivFelhasznalo.Aktiv));
                    if (jelszoValtoztatEredmeny.IsSuccessStatusCode)
                    {
                        MessageBox.Show("A jelszava sikeren megváltozott!");
                        ((Window)o).Close();
                    }
                    else
                    {
                        MessageBox.Show("Hiba történt a jelszava változtatása közben! " + jelszoValtoztatEredmeny.ReasonPhrase);
                    }
                }
                else
                {
                    MessageBox.Show("Az új jelszavaknak egyezniük kell!");
                }
            }
            else
            {
                MessageBox.Show("Az aktuális jelszó hibás!");
            }
        }
        private bool JelszoValtoztatasCE()
        {
            if (AktualisJelszo.Length > 0 && UjJelszo.Length > 0 && UjJelszoEllenoriz.Length > 0)
                return true;
            return false;
        }
        

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}