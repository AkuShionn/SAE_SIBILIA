using SAE_SIBILIA.Fenetres;
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
    /// Logique d'interaction pour PageAcceuil.xaml
    /// </summary>
    public partial class PageAcceuil : UserControl
    {
        public PageAcceuil()
        {
            InitializeComponent();
        }

        private void ButtonClient_Click(object sender, RoutedEventArgs e)
        {
            DataGridClient dgclientControl = new DataGridClient();
            Menu.Content = dgclientControl;
        }

        private void ButtonAcceuil_Click(object sender, RoutedEventArgs e)
        {
            Menu.Content = null;
        }

        private void ButtonNouvelleCommande_Click(object sender, RoutedEventArgs e)
        {
            CreerCommande commmandeControl = new CreerCommande();
            Menu.Content = commmandeControl;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CreerCommande commmandeControl = new CreerCommande();
            Menu.Content = commmandeControl;
        }

        private void ButtClickGestionPlat(object sender, RoutedEventArgs e)
        {
            DataGridGestionPlats gestionPlatsControl = new DataGridGestionPlats();
            Menu.Content = gestionPlatsControl;
        }

        private void ButtCommandeDuJour(object sender, RoutedEventArgs e)
        {

            CommandeDuJour commandeDuJ = new CommandeDuJour();
            Menu.Content = commandeDuJ;


        }

        private void ButtDeconnexion(object sender, RoutedEventArgs e)
        {
            Connexion fenetreConnexion = new Connexion();
            fenetreConnexion.Show();

            // On trouve la fenêtre qui contient ce UserControl (la fenêtre d'accueil) et on la FERME.
            Window fenetreActuelle = Window.GetWindow(this);
            if (fenetreActuelle != null)
            {
                fenetreActuelle.Close();
            }
        }
    }
}
