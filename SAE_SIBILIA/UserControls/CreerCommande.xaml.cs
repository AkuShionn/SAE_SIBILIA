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
            // On vérifie deux choses :
            // 1. Qu'un plat est bien sélectionné dans la liste (`comboBoxPlats.SelectedItem is Plat plat`).
            // 2. Que le texte dans le champ quantité est bien un nombre entier (`int.TryParse`).
            if (comboBoxPlats.SelectedItem is Plat plat && int.TryParse(txtQuantite.Text, out int quantite))
            {
                // Si les conditions sont bonnes, on ajoute un nouvel objet Plat à notre "panier" (la liste platsCommandes).
                platsCommandes.Add(new Plat
                {
                    NomPlat = plat.NomPlat,
                    NbPersonnes = quantite, // On utilise NbPersonnes pour stocker la quantité désirée.
                    PrixUnitaire = plat.PrixUnitaire * quantite, // On calcule le prix total pour cette ligne (prix du plat * quantité).
                    NumPlat = plat.NumPlat // On conserve l'ID original du plat.
                });

                // On appelle une autre méthode pour rafraîchir l'affichage de la liste et du total.
                MettreAJourListeEtTotal();
            }
        }
        private void MettreAJourListeEtTotal()
        {
            // Astuce pour forcer la ListBox à se mettre à jour : on vide sa source de données avant de la réassigner.
            listPlatsCommandes.ItemsSource = null;
            listPlatsCommandes.ItemsSource = platsCommandes;

            // On calcule la somme de tous les prix des plats dans le panier.
            // La fonction .Sum() vient de LINQ.
            double total = platsCommandes.Sum(p => p.PrixUnitaire);

            // On met à jour le TextBlock du total en le formatant en devise (ex: "12,34 €").
            txtTotal.Text = total.ToString("C2");
        }
        private void ValiderCommande_Click(object sender, RoutedEventArgs e)
        {
            // On vérifie que le client, la date, et au moins un plat ont été ajoutés.
            if (clientIdSelectionne == null || dateRetrait.SelectedDate == null || !platsCommandes.Any())
            {
                MessageBox.Show("Veuillez sélectionner un client, une date de retrait et ajouter au moins un plat.", "Erreur de validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return; // On arrête l'exécution de la méthode.
            }

            // On ouvre une nouvelle connexion à la base de données.
            // Le 'using' garantit que la connexion sera fermée même en cas d'erreur.
            using var conn = new NpgsqlConnection(DataAccess.connectionString);
            conn.Open();

            // On démarre une transaction. C'est une sécurité : soit TOUT est enregistré, soit RIEN n'est enregistré.
            // Si une erreur survient au milieu, tout est annulé (rollback).
            using var transaction = conn.BeginTransaction();
            try
            {
                // 1. On insère la commande principale dans la table 'commande'.
                var insertCmd = new NpgsqlCommand(@"
                    INSERT INTO commande (numclient, datecommande, dateretraitprevue, payee, retiree, prixtotal)
                    VALUES (@numclient, CURRENT_DATE, @dateretrait, FALSE, FALSE, @prixtotal)
                    RETURNING numcommande;", conn); // RETURNING permet de récupérer l'ID qui vient d'être créé.

                // On assigne les valeurs aux paramètres pour éviter les injections SQL.
                insertCmd.Parameters.AddWithValue("@numclient", clientIdSelectionne.Value);
                insertCmd.Parameters.AddWithValue("@dateretrait", dateRetrait.SelectedDate.Value);
                insertCmd.Parameters.AddWithValue("@prixtotal", platsCommandes.Sum(p => p.PrixUnitaire));

                // On exécute la commande et on récupère le nouvel ID de la commande.
                int numCommande = (int)insertCmd.ExecuteScalar();

                // 2. On boucle sur chaque plat du panier pour les insérer dans la table de liaison.
                foreach (var platCmd in platsCommandes)
                {
                    var insertPlat = new NpgsqlCommand(@"
                        INSERT INTO platcommande (numcommande, numplat, quantite)
                        VALUES (@numcommande, @numplat, @quantite);", conn);

                    insertPlat.Parameters.AddWithValue("@numcommande", numCommande); // L'ID de la commande qu'on vient de créer.
                    insertPlat.Parameters.AddWithValue("@numplat", platCmd.NumPlat); // L'ID du plat.
                    insertPlat.Parameters.AddWithValue("@quantite", platCmd.NbPersonnes); // La quantité pour ce plat.

                    insertPlat.ExecuteNonQuery();
                }

                // Si on arrive ici sans erreur, toutes les insertions ont réussi. On valide la transaction.
                transaction.Commit();

                MessageBox.Show("Commande enregistrée avec succès !", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);

                // On nettoie le formulaire pour la prochaine commande.
                platsCommandes.Clear();
                MettreAJourListeEtTotal();
                // (On pourrait aussi vider les autres champs ici : client, date, etc.)
            }
            catch (Exception ex)
            {
                // Si une erreur est survenue, on annule tout ce qui a été fait dans la transaction.
                transaction.Rollback();
                MessageBox.Show("Erreur lors de l'enregistrement de la commande :\n" + ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
