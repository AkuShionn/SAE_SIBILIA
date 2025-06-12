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
    /// Logique d'interaction pour AjouterPlats.xaml
    /// </summary>
    public partial class AjouterPlats : Window
    {
        public AjouterPlats()
        {
            InitializeComponent();
            // Charger les catégories dans le ComboBox
            var categories = DataAccess.Instance.GetCategories();
            comboBox_Categorie.ItemsSource = categories;
            comboBox_Categorie.DisplayMemberPath = "NomCategorie";
            comboBox_Categorie.SelectedValuePath = "NumCategorie";

            comboBox_Categorie.SelectionChanged += ComboBox_Categorie_SelectionChanged;

            var periodes = DataAccess.Instance.GetPeriodes();
            comboBox_Periode.ItemsSource = periodes;
            comboBox_Periode.DisplayMemberPath = "LibellePeriode";
            comboBox_Periode.SelectedValuePath = "NumPeriode";

        }
        private void ComboBox_Categorie_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboBox_Categorie.SelectedItem is Categorie selectedCategorie)
            {
                var sousCats = DataAccess.Instance.GetSousCategoriesByCategorie(selectedCategorie.NumCategorie);
                comboBox_SousCategorie.ItemsSource = sousCats;
                comboBox_SousCategorie.DisplayMemberPath = "NomSousCategorie";
                comboBox_SousCategorie.SelectedValuePath = "NumSousCategorie";
            }
        }
        private void ajouterCategorie (object sender, SelectionChangedEventArgs e)
        {
            if (comboBox_Categorie.SelectedItem is Categorie selectedCategorie)
            {
                var sousCats = DataAccess.Instance.GetSousCategoriesByCategorie(selectedCategorie.NumCategorie);
                comboBox_SousCategorie.ItemsSource = sousCats;
                comboBox_SousCategorie.DisplayMemberPath = "NomSousCategorie";
                comboBox_SousCategorie.SelectedValuePath = "NumSousCategorie";
            }
        }

       
        
    }
}
