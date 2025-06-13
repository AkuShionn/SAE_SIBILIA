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
    /// Logique d'interaction pour ModificationClients.xaml
    /// </summary>
    public partial class ModificationClients : Window
    {
        // On garde une référence privée au client que l'on modifie
        private Client clientAModifier;

        // CE CONSTRUCTEUR EST NOUVEAU. Il remplace public ModificationClients()
        // Il accepte un objet Client en paramètre.
        public ModificationClients(Client client)
        {
            InitializeComponent();

            // On stocke le client passé en paramètre
            this.clientAModifier = client;

            // C'EST LA LIGNE LA PLUS IMPORTANTE :
            // On dit à la fenêtre que toutes ses données par défaut (son "contexte")
            // se trouvent dans l'objet 'clientAModifier'.
            this.DataContext = this.clientAModifier;
        }

        private void buttModifierClient_Click(object sender, RoutedEventArgs e)
        {
            // Grâce au Data Binding, l'objet 'clientAModifier' est déjà à jour
            // avec les nouvelles valeurs des TextBox.
            try
            {
                // On appelle simplement la méthode Update de l'objet.
                clientAModifier.Update();

                // On informe l'utilisateur que tout s'est bien passé.
                MessageBox.Show("Client mis à jour avec succès !", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);

                // On ferme la fenêtre de modification.
                this.Close();
            }
            catch (Exception ex)
            {
                // En cas de problème avec la base de données, on affiche une erreur.
                MessageBox.Show($"Erreur lors de la mise à jour du client : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}

