using SAE_SIBILIA.Classes;
using SAE_SIBILIA.Fenetres;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace SAE_SIBILIA.UserControls
{
    public partial class DataGridClient : UserControl
    {
        public ObservableCollection<Client> lesClients { get; set; }

        public DataGridClient()
        {
            InitializeComponent();
            ChargeData();  // Charger les données au démarrage
        }

        private void ButtonCreerClient_Click(object sender, RoutedEventArgs e)
        {
            // Ouvrir la fenêtre "CreationClients" lorsqu'on clique sur "Créer Client"
            CreationClients fenetreClient = new CreationClients();
            fenetreClient.ShowDialog();
        }

        private void ChargeData()
        {
            try
            {
                // Appeler la méthode FindAll() de la classe Client pour récupérer les clients
                List<Client> clients = new Client().FindAll();

                // Convertir la liste de clients en ObservableCollection pour la liaison dynamique
                lesClients = new ObservableCollection<Client>(clients);

                // Lier l'ObservableCollection au DataContext
                this.DataContext = this;
            }
            catch (Exception ex)
            {
                LogError.Log(ex, "Erreur SQL lors du chargement des clients");
                MessageBox.Show("Problème lors de récupération des données", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();
            }
        }
    }
}
