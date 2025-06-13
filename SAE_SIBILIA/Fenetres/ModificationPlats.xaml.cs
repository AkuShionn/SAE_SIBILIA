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
            this.DataContext = this.platAModifier;

            // Logique pour charger les ComboBox (similaire à ta fenêtre d'ajout)
            // et pré-sélectionner les bonnes valeurs
            LoadComboBoxes();
        }

        private void LoadComboBoxes()
        {
            // Charger toutes les catégories
            comboBox_Categorie.ItemsSource = DataAccess.Instance.GetCategories();
            comboBox_Categorie.DisplayMemberPath = "NomCategorie";
            // On pré-sélectionne la catégorie actuelle du plat
            comboBox_Categorie.Text = platAModifier.NomCategorie;

            // ... (Faire de même pour Sous-catégorie et Période)
        }

        private void BtnEnregistrerModifs_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Mettre à jour les objets complexes si l'utilisateur a changé les ComboBox
                platAModifier.Categorie = (Categorie)comboBox_Categorie.SelectedItem ?? platAModifier.Categorie;
                // ... (Faire de même pour SousCategorie et Disponiple)

                platAModifier.Update();
                MessageBox.Show("Plat mis à jour avec succès !", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                this.DialogResult = true; // Indiquer le succès pour rafraîchir la liste principale
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la mise à jour du plat : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
