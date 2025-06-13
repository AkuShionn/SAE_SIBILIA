using Npgsql;
using SAE_SIBILIA.Classes;
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
    /// Logique d'interaction pour CreerCommande.xaml
    /// </summary>
      
        public partial class CreerCommande : UserControl
        {
            private List<PlatCommande> platsCommandes = new List<PlatCommande>();
            private List<Plat> tousLesPlats = new List<Plat>();
            private int? clientIdSelectionne = null;

            public CreerCommande()
            {
                InitializeComponent();
                ChargerPlats();
            }

            private void ChargerPlats()
            {
                using var conn = new NpgsqlConnection(DataAccess.connectionString);
                conn.Open();

                string query = "SELECT numplat, nomplat, prix FROM plat";
                using var cmd = new NpgsqlCommand(query, conn);
                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var plat = new Plat
                    {
                        NumPlat = reader.GetInt32(0),
                        NomPlat = reader.GetString(1),
                        PrixUnitaire = reader.GetDouble(2)
                    };
                    tousLesPlats.Add(plat);
                }
                comboBoxPlats.ItemsSource = tousLesPlats;
                comboBoxPlats.DisplayMemberPath = "NomPlat";
                comboBoxPlats.SelectedValuePath = "NumPlat";
            }

            private void AjouterPlat_Click(object sender, RoutedEventArgs e)
            {
                if (comboBoxPlats.SelectedItem is Plat plat && int.TryParse(txtQuantite.Text, out int quantite))
                {
                    platsCommandes.Add(new PlatCommande
                    {
                        NomPlat = plat.NomPlat,
                        Quantite = quantite,
                        Prix = plat.PrixUnitaire * quantite,
                        NumPlat = plat.NumPlat
                    });
                    MettreAJourListeEtTotal();
                }
            }

            private void MettreAJourListeEtTotal()
            {
                listPlatsCommandes.ItemsSource = null;
                listPlatsCommandes.ItemsSource = platsCommandes;

                double total = platsCommandes.Sum(p => p.Prix);
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
                    // Insertion de la commande
                    var insertCmd = new NpgsqlCommand(@"
                    INSERT INTO commande (numclient, datecommande, dateretraitprevue, payee, retiree, prixtotal)
                    VALUES (@numclient, CURRENT_DATE, @dateretrait, FALSE, FALSE, @prixtotal)
                    RETURNING numcommande;", conn);

                    insertCmd.Parameters.AddWithValue("@numclient", clientIdSelectionne);
                    insertCmd.Parameters.AddWithValue("@dateretrait", dateRetrait.SelectedDate.Value);
                    insertCmd.Parameters.AddWithValue("@prixtotal", platsCommandes.Sum(p => p.Prix));

                    int numCommande = (int)insertCmd.ExecuteScalar();

                    // Insertion des plats
                    foreach (var platCmd in platsCommandes)
                    {
                        var insertPlat = new NpgsqlCommand(@"
                        INSERT INTO platcommande (numcommande, numplat, quantite)
                        VALUES (@numcommande, @numplat, @quantite);", conn);

                        insertPlat.Parameters.AddWithValue("@numcommande", numCommande);
                        insertPlat.Parameters.AddWithValue("@numplat", platCmd.NumPlat);
                        insertPlat.Parameters.AddWithValue("@quantite", platCmd.Quantite);

                        insertPlat.ExecuteNonQuery();
                    }

                    transaction.Commit();
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

            // Exemple : rechercher le client (à relier à un bouton ou événement)
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
        }

        public class PlatCommande
        {
            public int NumPlat { get; set; }
            public string NomPlat { get; set; }
            public int Quantite { get; set; }
            public double Prix { get; set; }
        }
    }

