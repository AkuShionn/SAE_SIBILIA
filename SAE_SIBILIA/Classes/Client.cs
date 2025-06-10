using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE_SIBILIA.Classes
{
    public class Client : ICrud<Client>, INotifyPropertyChanged
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
            throw new NotImplementedException();
        }

        public List<Client> FindAll()
        {
            throw new NotImplementedException();
        }

        public List<Client> FindBySelection(string criteres)
        {
            throw new NotImplementedException();
        }

        public void Read()
        {
            throw new NotImplementedException();
        }

        public int Update()
        {
            throw new NotImplementedException();
        }
    }
}
