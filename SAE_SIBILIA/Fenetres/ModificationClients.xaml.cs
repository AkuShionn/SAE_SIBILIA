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
        private Client clientAModifier;
        public ModificationClients(Client client)
        {
            InitializeComponent();

            // On stocke le client passé en paramètre
            this.clientAModifier = client;
            this.DataContext = this.clientAModifier;
        }

        private void buttModifierClient_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                clientAModifier.Update();
                MessageBox.Show("Client mis à jour avec succès !", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la mise à jour du client : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}

