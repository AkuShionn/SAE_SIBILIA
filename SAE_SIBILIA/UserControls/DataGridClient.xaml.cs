using SAE_SIBILIA.Classes;
using SAE_SIBILIA.Fenetres;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace SAE_SIBILIA.UserControls
{
    public partial class DataGridClient : UserControl
    {
        public ObservableCollection<Client> lesClients { get; set; }

        public DataGridClient()
        {
            InitializeComponent();
            ChargeData();  // Charger les données au démarrage
            dgClients.Items.Filter = RechercheMotClefClient;

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
                List<Client> clients = new Client().FindAll();
                lesClients = new ObservableCollection<Client>(clients);
                this.DataContext = this;
            }
            catch (Exception ex)
            {
                LogError.Log(ex, "Erreur SQL lors du chargement des clients");
                MessageBox.Show("Problème lors de récupération des données", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();
            }
        }

        private void ButtonModifierClient(object sender, RoutedEventArgs e)
        {
            // 1. Vérifier si un client est bien sélectionné dans le DataGrid
            if (dgClients.SelectedItem is Client clientSelectionne)
            {
                // 2. Créer la fenêtre de modification en utilisant le BON constructeur,
                //    celui qui accepte un client en paramètre.
                ModificationClients fenetreModifClient = new ModificationClients(clientSelectionne);

                // 3. Ouvrir la fenêtre. Le code s'arrête ici jusqu'à ce que la fenêtre soit fermée.
                fenetreModifClient.ShowDialog();

                // 4. Une fois la fenêtre fermée, on rafraîchit le DataGrid pour voir les changements.
                //    C'est important pour que les modifications s'affichent sans redémarrer.
                dgClients.Items.Refresh();
            }
            else
            {
                // 5. Si aucun client n'est sélectionné, on prévient l'utilisateur.
                MessageBox.Show("Veuillez sélectionner un client à modifier.", "Aucune sélection", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void SupprimerClient(object sender, RoutedEventArgs e)
        {
            // Vérifie si un client est sélectionné dans le DataGrid
            if (dgClients.SelectedItem == null)
            {
                MessageBox.Show("Veuillez sélectionner un client"," ",MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                // Récupérer le client sélectionné dans le DataGrid
                Client clientSelectionne = (Client)dgClients.SelectedItem;

                // Confirmer la suppression avec l'utilisateur
                if (MessageBox.Show($"Attention, ce client sera définitivement supprimé. " + "Désirez-vous tout de même supprimer ce client ?","Attention", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    try
                    {
                        // Suppression du client dans la base de données
                        clientSelectionne.Delete();  // Assure-toi que la méthode Delete() supprime le client de la base de données

                        // Retirer le client de la collection des clients affichés
                        lesClients.Remove(clientSelectionne);  // LesClients est la collection bindée au DataGrid

                        // Afficher un message pour confirmer la suppression
                        MessageBox.Show("Client supprimé.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        // Gérer les erreurs si la suppression échoue
                        MessageBox.Show($"Erreur lors de la suppression du client: {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Client non supprimé", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private bool RechercheMotClefClient(object obj)
        {
            if (string.IsNullOrEmpty(textMotClefClients.Text))
                return true;

            Client client = obj as Client;
            return client != null && client.NomClient.Contains(textMotClefClients.Text, StringComparison.OrdinalIgnoreCase);
        }
        private void Refresh_Clients(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(dgClients.ItemsSource).Refresh();
        }

    }
}
