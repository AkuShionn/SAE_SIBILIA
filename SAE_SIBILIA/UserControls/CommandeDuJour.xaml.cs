using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SAE_SIBILIA.UserControls
{
    /// <summary>
    /// Logique d'interaction pour CommandeDuJour.xaml
    /// </summary>
    public partial class CommandeDuJour : UserControl
    {
        private List<Commande> toutesLesCommandes;

        public CommandeDuJour()
        {
            InitializeComponent();
        }

        // Le chargement se fait quand la page devient visible.
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ChargerDonnees();
        }

        // Méthode centrale pour charger et recharger les données.
        public void ChargerDonnees()
        {
            try
            {
                // On utilise la méthode statique de la classe Commande pour récupérer les données.
                toutesLesCommandes = Commande.FindCommandesDuJour();
                // On affiche les données dans le DataGrid en appliquant les filtres actuels.
                AppliquerFiltres();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement des commandes : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Méthode qui filtre la liste principale et met à jour l'affichage.
        private void AppliquerFiltres()
        {
            // On part toujours de la liste complète pour ne pas perdre de données.
            IEnumerable<Commande> commandesFiltrees = toutesLesCommandes;

            // 1. On filtre par le nom du client (insensible à la casse).
            if (!string.IsNullOrEmpty(txtFiltreClient.Text))
            {
                commandesFiltrees = commandesFiltrees.Where(c =>
                    c.NomClient.IndexOf(txtFiltreClient.Text, StringComparison.OrdinalIgnoreCase) >= 0);
            }

            // 2. On filtre pour masquer les commandes déjà retirées.
            if (chkMasquerRecuperees.IsChecked == true)
            {
                commandesFiltrees = commandesFiltrees.Where(c => !c.Retiree);
            }

            // On met à jour la source de données du DataGrid avec la nouvelle liste filtrée.
            dataGridCommandes.ItemsSource = commandesFiltrees.ToList();
        }

        // Un seul gestionnaire d'événement pour tous les contrôles de filtre.
        private void Filtres_Changed(object sender, RoutedEventArgs e)
        {
            // Si les données sont chargées, on applique les filtres.
            if (toutesLesCommandes != null)
            {
                AppliquerFiltres();
            }
        }

        // Logique du bouton "Marquer comme Retirée".
        private void BtnMarquerRecuperee_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridCommandes.SelectedItem is Commande commandeSelectionnee)
            {
                if (commandeSelectionnee.Retiree)
                {
                    MessageBox.Show("Cette commande est déjà marquée comme retirée.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                var resultat = MessageBox.Show($"Confirmer que la commande n°{commandeSelectionnee.NumCommande} a été récupérée ?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (resultat == MessageBoxResult.Yes)
                {
                    try
                    {
                        // 1. On met à jour la base de données directement d'ici.
                        string query = "UPDATE commande SET retiree = TRUE, payee = TRUE WHERE num_commande = @id";
                        using (var cmd = new NpgsqlCommand(query))
                        {
                            cmd.Parameters.AddWithValue("@id", commandeSelectionnee.NumCommande);
                            DataAccess.Instance.ExecuteSet(cmd);
                        }

                        // 2. C'est l'étape clé : on recharge TOUT pour voir le changement.
                        ChargerDonnees();

                        MessageBox.Show("La commande a bien été marquée comme retirée.", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Erreur lors de la mise à jour de la commande : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner une commande.", "Aucune sélection", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}

