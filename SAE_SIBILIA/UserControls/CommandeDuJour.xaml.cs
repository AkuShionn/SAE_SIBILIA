using Npgsql;
using SAE_SIBILIA.Classes;
using SAE_SIBILIA.Fenetres;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SAE_SIBILIA.UserControls
{
    public partial class CommandeDuJour : UserControl
    {
        private List<Commande> toutesLesCommandes;

        public CommandeDuJour()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ChargerDonnees();
        }

        public void ChargerDonnees()
        {
            try
            {
                toutesLesCommandes = Commande.FindCommandesDuJour();
                AppliquerFiltres();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement des commandes : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public void RechargerCommandes()
        {
            ChargerDonnees();
        }

        private void AppliquerFiltres()
        {
            IEnumerable<Commande> commandesFiltrees = toutesLesCommandes;

            if (!string.IsNullOrEmpty(txtFiltreClient.Text))
            {
                commandesFiltrees = commandesFiltrees.Where(c =>
                    c.NomClient.IndexOf(txtFiltreClient.Text, StringComparison.OrdinalIgnoreCase) >= 0);
            }

            if (chkMasquerRecuperees.IsChecked == true)
            {
                commandesFiltrees = commandesFiltrees.Where(c => !c.Retiree);
            }

            dataGridCommandes.ItemsSource = commandesFiltrees.ToList();
        }

        private void Filtres_Changed(object sender, RoutedEventArgs e)
        {
            if (toutesLesCommandes != null)
            {
                AppliquerFiltres();
            }
        }

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
                        string query = "UPDATE commande SET retiree = TRUE, payee = TRUE WHERE num_commande = @id";
                        using (var cmd = new NpgsqlCommand(query))
                        {
                            cmd.Parameters.AddWithValue("@id", commandeSelectionnee.NumCommande);
                            DataAccess.Instance.ExecuteSet(cmd);
                        }

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

        private void BtnHistorique_Click(object sender, RoutedEventArgs e)
        {
            Historique historique = new Historique();
            historique.ShowDialog();
        }
       
    }
}
