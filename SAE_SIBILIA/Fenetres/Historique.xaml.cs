using Npgsql;
using SAE_SIBILIA.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SAE_SIBILIA.Fenetres
{
    public partial class Historique : Window
    {
        private List<Commande> toutesCommandes;

        public Historique()
        {
            InitializeComponent();
            ChargerHistoriqueCommandes();
        }

        private void ChargerHistoriqueCommandes()
        {
            toutesCommandes = new List<Commande>();

            string connString = "Host=srv-peda-new;Port=5433;Username=alvesdjo;Password=NgRpEr;Database=sibilia;Options='-c search_path=sibilia_shema'";

            using (NpgsqlConnection conn = new NpgsqlConnection(connString))
            {
                conn.Open();

                string sql = @"
                    SELECT numcommande, datecommande, dateretraitprevue, payee, retiree, prixtotal
                    FROM COMMANDE
                    ORDER BY datecommande DESC;
                ";

                using (var cmd = new NpgsqlCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        toutesCommandes.Add(new Commande
                        {
                            NumCommande = reader.GetInt32(0),
                            DateCommande = reader.GetDateTime(1),
                            DateRetraitPrevue = reader.GetDateTime(2),
                            Payee = reader.GetBoolean(3),
                            Retiree = reader.GetBoolean(4),
                            PrixTotal = reader.GetDouble(5)
                        });
                    }
                }
            }

            dataHistorique.ItemsSource = toutesCommandes;
        }

        private void txtRechercheCommande_TextChanged(object sender, TextChangedEventArgs e)
        {
            string filtre = txtRechercheCommande.Text.ToLower();

            var filtres = toutesCommandes.FindAll(c =>
                c.NumCommande.ToString().Contains(filtre) ||
                c.DateCommande.ToString("dd/MM/yyyy").Contains(filtre) ||
                c.DateRetraitPrevue.ToString("dd/MM/yyyy").Contains(filtre)
            );

            dataHistorique.ItemsSource = filtres;
        }

        
    }
}
