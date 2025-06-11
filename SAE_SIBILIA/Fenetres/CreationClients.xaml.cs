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
using System.Windows.Shapes;

namespace SAE_SIBILIA.Fenetres
{
    /// <summary>
    /// Logique d'interaction pour CreationClients.xaml
    /// </summary>
    public partial class CreationClients : Window
    {
        public CreationClients()
        {
            InitializeComponent();
        }

        private void buttValiderClient_Click(object sender, RoutedEventArgs e)
        {
            // Récupérer les valeurs des TextBox
            string nom = txtBoxNomClient.Text;
            string prenom = txtBoxPrenomClient.Text;
            string tel = txtBoxTelClient.Text;
            string adresseRue = txtBoxAdresseClient.Text;
            string adresseCP = txtBoxCPClient.Text;
            string adresseVille = txtBoxVille.Text;

            // Créer un nouvel objet Client avec les données du formulaire
            Client nouveauClient = new Client(nom, prenom, tel, adresseRue, adresseCP, adresseVille);

            try
            {
                // Insérer le client dans la base de données
                // Utilise une méthode d'insertion dans DataAccess, ici tu peux avoir une méthode comme InsertClient()
                int result = DataAccess.Instance.AjouterClient(nouveauClient);

                // Vérifier si l'insertion a réussi
                if (result > 0)
                {
                    MessageBox.Show("Client créé avec succès !", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();  // Fermer la fenêtre après la création
                }
                else
                {
                    MessageBox.Show("Erreur lors de la création du client", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                // En cas d'erreur, afficher un message
                MessageBox.Show("Erreur : " + ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
