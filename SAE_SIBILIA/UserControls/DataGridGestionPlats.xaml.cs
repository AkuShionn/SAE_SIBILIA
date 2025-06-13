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
            if (string.IsNullOrEmpty(textMotClefPlats.Text))
                return true;

            Plat plat = obj as Plat;
            return plat != null && plat.NomPlat.Contains(textMotClefPlats.Text, StringComparison.OrdinalIgnoreCase);
        }

        private void Refresh_Plats(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(dgPlats.ItemsSource).Refresh();
        }

        private void ButAjouterPlat(object sender, RoutedEventArgs e)
        {
            AjouterPlats fenetreAjouterPlats = new AjouterPlats();
            fenetreAjouterPlats.ShowDialog();
        }

        private void SupprimerPlats(object sender, RoutedEventArgs e)
        {
            if (dgPlats.SelectedItem == null)
            {
                MessageBox.Show("Veuillez sélectionner un plat", "Alerte", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                Plat platSelectionne = (Plat)dgPlats.SelectedItem;

                if (MessageBox.Show(
                    $"Attention, ce plat sera définitivement supprimé. Désirez-vous tout de même supprimer {platSelectionne.NomPlat} ?",
                    "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    try
                    {
                        platSelectionne.Delete(); // supprime de la BDD

                        lesPlats.Remove(platSelectionne); // supprime de la liste bindée

                        MessageBox.Show("Plat supprimé.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Erreur lors de la suppression du plat : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Plat non supprimé", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }

        }



    }
}
