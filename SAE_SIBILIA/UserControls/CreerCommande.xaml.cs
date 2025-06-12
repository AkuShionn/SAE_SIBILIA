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
        public CreerCommande()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            List<Plat> plats = DataAccess.Instance.GetPlats();

            if (plats.Count == 0)
            {
                MessageBox.Show("Aucun plat trouvé en base.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            comboBoxPlats.ItemsSource = plats;
            comboBoxPlats.DisplayMemberPath = "NomPlat";
        }

    }
}
