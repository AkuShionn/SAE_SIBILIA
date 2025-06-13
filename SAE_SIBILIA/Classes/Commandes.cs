using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE_SIBILIA.Classes
{
    public class Commande : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private int numCommande;
        private DateTime dateCommande; // ← Ajouté ici
        private DateTime dateRetraitPrevue;
        private bool payee;
        private bool retiree;
        private double prixTotal;
        private string nomClient;
        private string status;

        public Commande() { }

        public int NumCommande
        {
            get { return numCommande; }
            set { numCommande = value; }
        }

        public DateTime DateCommande
        {
            get { return dateCommande; }
            set { dateCommande = value; }
        }

        public DateTime DateRetraitPrevue
        {
            get { return dateRetraitPrevue; }
            set
            {
                if (dateRetraitPrevue != value)
                    dateRetraitPrevue = value;
            }
        }

        public string HeureRetrait => this.DateRetraitPrevue.ToString("HH:mm");

        public bool Payee
        {
            get { return payee; }
            set { payee = value; }
        }

        public bool Retiree
        {
            get { return retiree; }
            set { retiree = value; }
        }

        public double PrixTotal
        {
            get { return this.prixTotal; }
            set { this.prixTotal = value; }
        }

        public string NomClient
        {
            get { return this.nomClient; }
            set { this.nomClient = value; }
        }

        public string Status
        {
            get { return this.Retiree ? "Retirée" : "En attente"; }
            set { this.status = value; }
        }

        public void MarquerCommeRetiree()
        {
            string query = "UPDATE commande SET retiree = TRUE, payee = TRUE WHERE num_commande = @id";
            using (var cmd = new NpgsqlCommand(query))
            {
                cmd.Parameters.AddWithValue("@id", this.NumCommande);
                DataAccess.Instance.ExecuteSet(cmd);
                this.Retiree = true;
                this.Payee = true;
            }
        }

        public static List<Commande> FindCommandesDuJour()
        {
            var commandes = new List<Commande>();

            string query = @"
                SELECT c.numcommande, c.datecommande, c.dateretraitprevue, c.payee, c.retiree, c.prixtotal, cl.nomclient
                FROM commande c
                JOIN client cl ON c.numclient = cl.numclient 
                WHERE DATE(c.dateretraitprevue) = CURRENT_DATE
                ORDER BY c.dateretraitprevue;";

            using (var cmd = new NpgsqlCommand(query))
            {
                var dt = DataAccess.Instance.ExecuteSelect(cmd);
                foreach (System.Data.DataRow row in dt.Rows)
                {
                    commandes.Add(new Commande
                    {
                        NumCommande = (int)row["numcommande"],
                        DateCommande = (DateTime)row["datecommande"], // ← rempli ici
                        DateRetraitPrevue = (DateTime)row["dateretraitprevue"],
                        Payee = (bool)row["payee"],
                        Retiree = (bool)row["retiree"],
                        PrixTotal = Convert.ToDouble(row["prixtotal"]),
                        NomClient = (string)row["nomclient"]
                    });
                }
            }

            return commandes;
        }
    }
}
