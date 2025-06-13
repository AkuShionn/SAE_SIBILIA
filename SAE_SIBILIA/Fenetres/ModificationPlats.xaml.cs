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
    /// Logique d'interaction pour ModificationPlats.xaml
    /// </summary>
    public partial class ModificationPlats : Window
    {
        private Plat platAModifier;

        public ModificationPlats(Plat plat)
        {
            InitializeComponent();
            this.platAModifier = plat;
            this.DataContext = this.platAModifier; // Le binding se fait automatiquement
            LoadAndSetComboBoxes();
        }

        private void LoadAndSetComboBoxes()
        {
            // --- Catégories ---
            var categories = DataAccess.Instance.GetCategories();
            comboBox_Categorie.ItemsSource = categories;
            comboBox_Categorie.DisplayMemberPath = "NomCategorie";
            // Pré-sélectionner la catégorie actuelle du plat
            comboBox_Categorie.SelectedItem = categories.FirstOrDefault(c => c.NumCategorie == platAModifier.Categorie.NumCategorie);

            // --- Sous-catégories (dépend de la catégorie) ---
            if (comboBox_Categorie.SelectedItem is Categorie selectedCategorie)
            {
                var sousCategories = DataAccess.Instance.GetSousCategoriesByCategorie(selectedCategorie.NumCategorie);
                comboBox_SousCategorie.ItemsSource = sousCategories;
                comboBox_SousCategorie.DisplayMemberPath = "NomSousCategorie";
                // Pré-sélectionner la sous-catégorie actuelle
                comboBox_SousCategorie.SelectedItem = sousCategories.FirstOrDefault(sc => sc.NumSousCategorie == platAModifier.SousCategorie.NumSousCategorie);
            }

            // --- Périodes ---
            var periodes = DataAccess.Instance.GetPeriodes();
            comboBox_Periode.ItemsSource = periodes;
            comboBox_Periode.DisplayMemberPath = "LibellePeriode";
            // Pré-sélectionner la période actuelle
            comboBox_Periode.SelectedItem = periodes.FirstOrDefault(p => p.NumPeriode == platAModifier.Disponiple.NumPeriode);
        }

        private void ComboBox_Categorie_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Mettre à jour la liste des sous-catégories quand la catégorie change
            if (comboBox_Categorie.SelectedItem is Categorie selectedCategorie)
            {
                comboBox_SousCategorie.ItemsSource = DataAccess.Instance.GetSousCategoriesByCategorie(selectedCategorie.NumCategorie);
                comboBox_SousCategorie.DisplayMemberPath = "NomSousCategorie";
            }
        }

        private void BtnEnregistrer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Le binding TwoWay met à jour les champs simples (Nom, Prix, etc.)
                // Mais il faut mettre à jour les objets complexes (liés aux ComboBox) manuellement.
                if (comboBox_Categorie.SelectedItem is Categorie newCat) platAModifier.Categorie = newCat;
                if (comboBox_SousCategorie.SelectedItem is SousCategorie newSousCat) platAModifier.SousCategorie = newSousCat;
                if (comboBox_Periode.SelectedItem is Periode newPeriode) platAModifier.Disponiple = newPeriode;

                // On appelle la méthode Update de l'objet Plat
                platAModifier.Update();

                MessageBox.Show("Plat mis à jour avec succès !", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                this.DialogResult = true; // Indique le succès
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la mise à jour : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnAnnuler_Click(object sender, RoutedEventArgs e)
        {
            // On ferme simplement la fenêtre. Le DialogResult sera null, indiquant une annulation.
            this.Close();
        }
    }
}
