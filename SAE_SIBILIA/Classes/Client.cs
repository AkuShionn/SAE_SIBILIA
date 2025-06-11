
using Npgsql;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace SAE_SIBILIA.Classes
{
    public class Client : ICrud<Client>, INotifyPropertyChanged, IEquatable<Client?>
    {
        private int numClient;
        private string nomClient;
        private string prenomClient;
        private string telClient;
        private string adresseRue;
        private string adresseCP;
        private string adresseVille;
        private ObservableCollection<Client> lesClients;

        public Client()
        {
        }

        public Client(string nomClient, string prenomClient, string telClient, string adresseRue, string adresseCP, string adresseVille)
        {
            this.NomClient = nomClient;
            this.PrenomClient = prenomClient;
            this.TelClient = telClient;
            this.AdresseRue = adresseRue;
            this.AdresseCP = adresseCP;
            this.AdresseVille = adresseVille;
        }

        public Client(int numClient, string nomClient, string prenomClient, string telClient, string adresseRue, string adresseCP, string adresseVille)
        {
            this.NumClient = numClient;
            this.NomClient = nomClient;
            this.PrenomClient = prenomClient;
            this.TelClient = telClient;
            this.AdresseRue = adresseRue;
            this.AdresseCP = adresseCP;
            this.AdresseVille = adresseVille;
        }

        public int NumClient
        {
            get
            {
                return numClient;
            }

            set
            {
                numClient = value;
            }
        }

        public string NomClient
        {
            get
            {
                return nomClient;
            }

            set
            {
                nomClient = value;
            }
        }

        public string PrenomClient
        {
            get
            {
                return prenomClient;
            }

            set
            {
                prenomClient = value;
            }
        }

        public string TelClient
        {
            get
            {
                return telClient;
            }

            set
            {
                telClient = value;
            }
        }

        public string AdresseRue
        {
            get
            {
                return adresseRue;
            }

            set
            {
                adresseRue = value;
            }
        }

        public string AdresseCP
        {
            get
            {
                return adresseCP;
            }

            set
            {
                adresseCP = value;
            }
        }

        public string AdresseVille
        {
            get
            {
                return this.adresseVille;
            }

            set
            {
                this.adresseVille = value;
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public int Create()
        {
            throw new NotImplementedException();
        }

        public int Delete()
        {
            using (var cmdUpdate = new NpgsqlCommand("delete from client where numclient =@numclient;"))
            {
                cmdUpdate.Parameters.AddWithValue("numclient", this.NumClient);
                return DataAccess.Instance.ExecuteSet(cmdUpdate);
            }

        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as Client);
        }

        public bool Equals(Client? other)
        {
            return other is not null &&
                   NomClient == other.NomClient &&
                   PrenomClient == other.PrenomClient &&
                   TelClient == other.TelClient &&
                   AdresseRue == other.AdresseRue &&
                   AdresseCP == other.AdresseCP &&
                   AdresseVille == other.AdresseVille;
        }

        public List<Client> FindAll()
        {
            List<Client> clients = new List<Client>();

            // Requête SQL pour récupérer les clients
            using (NpgsqlCommand cmdSelect = new NpgsqlCommand(@"
             SELECT nomclient, prenomclient, tel, adresserue, adressecp, adresseville
             FROM Client"))
            {
                // Exécution de la requête SELECT pour récupérer les données
                DataTable dt = DataAccess.Instance.ExecuteSelect(cmdSelect);

                // Parcours des lignes du DataTable pour créer des objets Client
                foreach (DataRow dr in dt.Rows)
                {
                    // Création d'un objet Client avec les valeurs de chaque ligne
                    clients.Add(new Client
                    {
                        NomClient = (string)dr["nomclient"],
                        PrenomClient = (string)dr["prenomclient"],
                        TelClient = (string)dr["tel"],
                        AdresseRue = (string)dr["adresserue"],
                        AdresseCP = (string)dr["adressecp"],
                        AdresseVille = (string)dr["adresseville"]
                    });
                }
            }
            return clients;
        }
        public List<Client> FindBySelection(string criteres)
        {
            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(NomClient, PrenomClient, TelClient, AdresseRue, AdresseCP, AdresseVille);
        }

        public void Read()
        {
            using (var cmdSelect = new NpgsqlCommand("SELECT * FROM Client WHERE numclient = @numclient;"))
            {
                cmdSelect.Parameters.AddWithValue("@numclient", this.NumClient);

                DataTable dt = DataAccess.Instance.ExecuteSelect(cmdSelect);
                    this.NomClient = (string)dt.Rows[0]["nomclient"];
                    this.PrenomClient = (string)dt.Rows[0]["prenomclient"];
                    this.TelClient = (string)dt.Rows[0]["tel"];
                    this.AdresseRue = (string)dt.Rows[0]["adresserue"];
                    this.AdresseCP = (string)dt.Rows[0]["adressecp"];
                    this.AdresseVille = (string)dt.Rows[0]["adresseville"];
            }
        }

        public int Update()
        {
            using (var cmdUpdate = new NpgsqlCommand("UPDATE Client SET nomclient = @nomclient, prenomclient = @prenomclient, " +
                "telClient = @tel, adresserue = @adresserue, adressecp = @adressecp, adresseville = @adresseville WHERE numclient = @numclient"))
            {
                // Ajouter les paramètres à la requête SQL
                cmdUpdate.Parameters.AddWithValue("@nomclient", this.NomClient);
                cmdUpdate.Parameters.AddWithValue("@prenomclient", this.PrenomClient);
                cmdUpdate.Parameters.AddWithValue("@tel", this.TelClient);
                cmdUpdate.Parameters.AddWithValue("@adresserue", this.AdresseRue);
                cmdUpdate.Parameters.AddWithValue("@adressecp", this.AdresseCP);
                cmdUpdate.Parameters.AddWithValue("@adresseville", this.AdresseVille);
                cmdUpdate.Parameters.AddWithValue("@numclient", this.NumClient); 

                return DataAccess.Instance.ExecuteSet(cmdUpdate);
            }
        }
    }
}
