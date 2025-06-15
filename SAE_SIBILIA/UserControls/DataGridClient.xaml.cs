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
            ChargeData();
            dgClients.Items.Filter = RechercheMotClefClient;

        }

        private void ButtonCreerClient_Click(object sender, RoutedEventArgs e)
        {
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
            if (dgClients.SelectedItem is Client clientSelectionne)
            {
                ModificationClients fenetreModifClient = new ModificationClients(clientSelectionne);
                fenetreModifClient.ShowDialog();
                dgClients.Items.Refresh();
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un client à modifier.", "Aucune sélection", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void SupprimerClient(object sender, RoutedEventArgs e)
        {
            if (dgClients.SelectedItem == null)
            {
                MessageBox.Show("Veuillez sélectionner un client"," ",MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                Client clientSelectionne = (Client)dgClients.SelectedItem;
                if (MessageBox.Show($"Attention, ce client sera définitivement supprimé. " + "Désirez-vous tout de même supprimer ce client ?","Attention", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    try
                    {
                        clientSelectionne.Delete();  
                        lesClients.Remove(clientSelectionne);  
                        MessageBox.Show("Client supprimé.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
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
