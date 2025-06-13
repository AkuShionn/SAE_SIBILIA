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
    /// Logique d'interaction pour CommandeDuJour.xaml
    /// </summary>
    public partial class CommandeDuJour : UserControl
    {
        public CommandeDuJour()
        {
            InitializeComponent();
        }

        private void btnHistoriqueCommandes_Click(object sender, RoutedEventArgs e)
        {
            Historique historique = new Historique();
            historique.ShowDialog();
        }
    }
}
