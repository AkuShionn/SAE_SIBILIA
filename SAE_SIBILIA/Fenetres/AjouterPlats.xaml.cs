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
        private List<Categorie> categories;
        private List<SousCategorie> sousCategories;
        private List<Periode> periodes;

        public AjouterPlats()
        {
            InitializeComponent();
            Loaded += Charge_AjouterPlats;
        }

        private void Charge_AjouterPlats(object sender, RoutedEventArgs e)
        {
            categories = DataAccess.Instance.GetCategories();
            periodes = DataAccess.Instance.GetPeriodes();

            comboBox_Categorie.ItemsSource = categories;
            comboBox_Categorie.DisplayMemberPath = "NomCategorie";
            comboBox_Categorie.SelectedValuePath = "NumCategorie";
            comboBox_Categorie.SelectionChanged += ComboBox_Categorie;

            comboBox_Periode.ItemsSource = periodes;
            comboBox_Periode.DisplayMemberPath = "LibellePeriode";
            comboBox_Periode.SelectedValuePath = "NumPeriode";
        }

        private void ComboBox_Categorie(object sender, SelectionChangedEventArgs e)
        {
            if (comboBox_Categorie.SelectedItem is Categorie selectedCategorie)
            {
                sousCategories = DataAccess.Instance.SousCategoriesParCategorie(selectedCategorie.NumCategorie);
                comboBox_SousCategorie.ItemsSource = sousCategories;
                comboBox_SousCategorie.DisplayMemberPath = "NomSousCategorie";
                comboBox_SousCategorie.SelectedValuePath = "NumSousCategorie";
            }
        }

        private void buttCreerPlat_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (comboBox_Categorie.SelectedItem is Categorie &&
                    comboBox_SousCategorie.SelectedItem is SousCategorie sousCat &&
                    comboBox_Periode.SelectedItem is Periode periode)
                {
                    Plat plat = new Plat
                    {
                        NomPlat = txtBoxNomPlat.Text.Trim(),
                        NbPersonnes = int.Parse(txtBoxNbPersonnes.Text),
                        PrixUnitaire = double.Parse(txtBoxPrixUnitaire.Text),
                        DelaiPreparation = int.Parse(txtBoxTempsPreparation.Text),
                        SousCategorie = sousCat,
                        Disponiple = periode
                    };

                    int result = DataAccess.Instance.AjouterPlat(plat);
                    if (result > 0)
                    {
                        MessageBox.Show("Plat ajouté avec succès !", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Erreur lors de l’ajout du plat.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Tous les champs doivent être remplis correctement.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur : " + ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

      
    }



}
    

