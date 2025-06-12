using SAE_SIBILIA.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using SAE_SIBILIA.Fenetres;

namespace SAE_SIBILIA.UserControls
{
    /// <summary>
    /// Logique d'interaction pour DataGridGestionPlats.xaml
    /// </summary>
    public partial class DataGridGestionPlats : UserControl
    {
        public ObservableCollection<Plat> lesPlats { get; set; }
        public DataGridGestionPlats()
        {
            InitializeComponent();
            ChargePlats();
            dgPlats.Items.Filter = RechercheMotClefPlat;
        }

        private void ChargePlats()
        {
            try
            {
                List<Plat> plats = new Plat().FindAll();
                lesPlats = new ObservableCollection<Plat>(plats);
                this.DataContext = this;
            }
            catch (Exception ex)
            {
                LogError.Log(ex, "Erreur SQL lors du chargement des plats");
                MessageBox.Show("Problème lors de récupération des données", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();
            }


        }

        private bool RechercheMotClefPlat(object obj)
        {
            if (string.IsNullOrEmpty(searchBox.Text))
                return true;

            Plat plat = obj as Plat;
            return plat != null && plat.NomPlat.Contains(searchBox.Text, StringComparison.OrdinalIgnoreCase);
        }

        private void Refresh_Plats(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(dgPlats.ItemsSource).Refresh();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            AjouterPlats fenetreAjout = new AjouterPlats();
            fenetreAjout.ShowDialog();
        }
    }
}
