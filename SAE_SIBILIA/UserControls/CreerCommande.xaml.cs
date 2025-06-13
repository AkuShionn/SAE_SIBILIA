using Npgsql;
using SAE_SIBILIA.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SAE_SIBILIA.UserControls
{
    public partial class CreerCommande : UserControl
    {
        private List<Plat> platsCommandes = new();
        private List<Plat> tousLesPlats = new();
        private int? clientIdSelectionne = null;

        public CreerCommande()
        {
            InitializeComponent();
            ChargerPlats();
        }

        private void ChargerPlats()
        {
            Plat plat = new Plat();
            tousLesPlats = plat.FindAll();

            comboBoxPlats.ItemsSource = tousLesPlats;
            comboBoxPlats.DisplayMemberPath = "NomPlat";
            comboBoxPlats.SelectedValuePath = "NumPlat";
        }

        private void AjouterPlat_Click(object sender, RoutedEventArgs e)
        {
            if (comboBoxPlats.SelectedItem is Plat plat && int.TryParse(txtQuantite.Text, out int quantite))
            {
                platsCommandes.Add(new Plat
                {
                    NomPlat = plat.NomPlat,
                    NbPersonnes = quantite,
                    PrixUnitaire = plat.PrixUnitaire * quantite,
                    NumPlat = plat.NumPlat
                });
                MettreAJourListeEtTotal();
            }
        }

        private void MettreAJourListeEtTotal()
        {
            listPlatsCommandes.ItemsSource = null;
            listPlatsCommandes.ItemsSource = platsCommandes;

            double total = platsCommandes.Sum(p => p.PrixUnitaire);
            txtTotal.Text = total.ToString("C2");
        }

        private void ValiderCommande_Click(object sender, RoutedEventArgs e)
        {
            if (clientIdSelectionne == null || dateRetrait.SelectedDate == null || !platsCommandes.Any())
            {
                MessageBox.Show("Veuillez remplir tous les champs et ajouter au moins un plat.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            using var conn = new NpgsqlConnection(DataAccess.connectionString);
            conn.Open();

            using var transaction = conn.BeginTransaction();
            try
            {
                var insertCmd = new NpgsqlCommand(@"
                    INSERT INTO commande (numclient, datecommande, dateretraitprevue, payee, retiree, prixtotal)
                    VALUES (@numclient, CURRENT_DATE, @dateretrait, FALSE, FALSE, @prixtotal)
                    RETURNING numcommande;", conn);

                insertCmd.Parameters.AddWithValue("@numclient", clientIdSelectionne);
                insertCmd.Parameters.AddWithValue("@dateretrait", dateRetrait.SelectedDate.Value);
                insertCmd.Parameters.AddWithValue("@prixtotal", platsCommandes.Sum(p => p.PrixUnitaire));

                int numCommande = (int)insertCmd.ExecuteScalar();

                foreach (var platCmd in platsCommandes)
                {
                    var insertPlat = new NpgsqlCommand(@"
                        INSERT INTO platcommande (numcommande, numplat, quantite)
                        VALUES (@numcommande, @numplat, @quantite);", conn);

                    insertPlat.Parameters.AddWithValue("@numcommande", numCommande);
                    insertPlat.Parameters.AddWithValue("@numplat", platCmd.NumPlat);
                    insertPlat.Parameters.AddWithValue("@quantite", platCmd.NbPersonnes);

                    insertPlat.ExecuteNonQuery();
                }

                transaction.Commit();

                // ➕ Si la commande est prévue pour aujourd’hui, on met à jour CommandeDuJour
                if (dateRetrait.SelectedDate.Value.Date == DateTime.Today)
                {
                    var parentWindow = Window.GetWindow(this);
                    if (parentWindow != null)
                    {
                        var commandeDuJour = FindChild<CommandeDuJour>(parentWindow);
                        if (commandeDuJour != null)
                        {
                            commandeDuJour.RechargerCommandes();
                        }
                    }
                }

                MessageBox.Show("Commande enregistrée avec succès !", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                platsCommandes.Clear();
                MettreAJourListeEtTotal();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                MessageBox.Show("Erreur lors de l'enregistrement de la commande :\n" + ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RechercherClient()
        {
            string nom = txtNomClient.Text.Trim();
            string tel = txtTelClient.Text.Trim();

            using var conn = new NpgsqlConnection(DataAccess.connectionString);
            conn.Open();

            var cmd = new NpgsqlCommand("SELECT numclient FROM client WHERE nomclient ILIKE @nom OR telephone ILIKE @tel LIMIT 1", conn);
            cmd.Parameters.AddWithValue("@nom", "%" + nom + "%");
            cmd.Parameters.AddWithValue("@tel", "%" + tel + "%");

            var result = cmd.ExecuteScalar();
            clientIdSelectionne = result != null ? (int?)result : null;
        }

        // 🔍 Fonction utilitaire pour retrouver un contrôle enfant
        public static T? FindChild<T>(DependencyObject parent) where T : DependencyObject
        {
            if (parent == null) return null;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);

                if (child is T correctlyTyped)
                    return correctlyTyped;

                var result = FindChild<T>(child);
                if (result != null)
                    return result;
            }

            return null;
        }
    }
}
