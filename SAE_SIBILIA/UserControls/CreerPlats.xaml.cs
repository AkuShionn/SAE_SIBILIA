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
    /// Logique d'interaction pour CreerPlats.xaml
    /// </summary>
    public partial class CreerPlats : UserControl
    {
        private List<Categorie> categories;
        private List<Periode> periodes;

        public CreerPlats()
        {
            InitializeComponent();
            Loaded += CreerPlats_Loaded;
        }

        private void CreerPlats_Loaded(object sender, RoutedEventArgs e)
        {
            categories = DataAccess.Instance.GetCategories();
            comboBox_Categorie.ItemsSource = categories;
            comboBox_Categorie.DisplayMemberPath = "NomCategorie";
            comboBox_Categorie.SelectedValuePath = "NumCategorie";
            comboBox_Categorie.SelectionChanged += ComboBox_Categorie_SelectionChanged;

            periodes = DataAccess.Instance.GetPeriodes();
        }

        private void ComboBox_Categorie_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboBox_Categorie.SelectedItem is Categorie cat)
            {
                var sousCats = DataAccess.Instance.GetSousCategoriesByCategorie(cat.NumCategorie);
                comboBox_SousCategorie.ItemsSource = sousCats;
                comboBox_SousCategorie.DisplayMemberPath = "NomSousCategorie";
                comboBox_SousCategorie.SelectedValuePath = "NumSousCategorie";
            }
        }

        private void buttCreerPlat_Click(object sender, RoutedEventArgs e)
        {
            if (comboBox_Categorie.SelectedItem is Categorie &&
                comboBox_SousCategorie.SelectedItem is SousCategorie sousCat &&
                !string.IsNullOrWhiteSpace(txtBoxPeriode.Text))
            {
                var periode = periodes.FirstOrDefault(p =>
                    p.LibellePeriode.Equals(txtBoxPeriode.Text.Trim(), StringComparison.OrdinalIgnoreCase));

                if (periode == null)
                {
                    MessageBox.Show("Période inconnue.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Créer l'objet Plat
                var plat = new Plat
                {
                    NomPlat = txtBoxNomPlat.Text.Trim(),
                    NbPersonnes = int.Parse(txtBoxNbPersonnes.Text),
                    DelaiPreparation = int.Parse(txtBoxTempsPreparation.Text),
                    SousCategorie = sousCat,
                    Disponiple = periode
                };

                int res = DataAccess.Instance.AjouterPlat(plat);

                if (res > 0)
                {
                    MessageBox.Show("Plat ajouté avec succès !");
                    ClearForm();
                }
                else
                {
                    MessageBox.Show("Échec de l’ajout du plat.");
                }
            }
            else
            {
                MessageBox.Show("Tous les champs sont obligatoires.");
            }
        }

        private void ClearForm()
        {
            txtBoxNomPlat.Clear();
            txtBoxNbPersonnes.Clear();
            txtBoxTempsPreparation.Clear();
            txtBoxPeriode.Clear();
            comboBox_Categorie.SelectedIndex = -1;
            comboBox_SousCategorie.ItemsSource = null;
        }

      
    }

}
