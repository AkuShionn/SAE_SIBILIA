using System;
using SAE_SIBILIA.Classes;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Npgsql;  // Nécessaire pour utiliser PostgreSQL via Npgsql

namespace SAE_SIBILIA.Fenetres
{
    public partial class Connexion : Window
    {
        // Chaîne de connexion à votre base de données PostgreSQL
        private readonly string ConnectionString = "Host=srv-peda-new;Port=5433;Username=alvesdjo;Password=NgRpEr;Database=sibilia;Options='-c search_path=sibilia_shema'";
        public ObservableCollection<Employe> LesEmployes { get; set; }
        public Employe EmployeConnecte { get; set; }

        public Connexion()
        {
            InitializeComponent();
            //this.Closing += FermetureDeLaConnexion;
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            // Ouvrir MainWindow
            if (string.IsNullOrWhiteSpace(usernameTextBox.Text) || string.IsNullOrWhiteSpace(passwordBox.Password))
            {
                MessageBox.Show("Veuillez entrer un nom d'utilisateur et un mot de passe.");
                return;
            }

            try
            {
                new DataAccess(usernameTextBox.Text, passwordBox.Password);
                Employe user = Employe.FindByLoginAndPassword(usernameTextBox.Text, passwordBox.Password);

                if (user is null)
                {
                    MessageBox.Show("Mot de passe ou login incorrect, réessayez.");
                }
                else
                {
                    EmployeConnecte = user;
                    DialogResult = true;
                    ChargeData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Échec de la connexion à la base de données. Vérifiez vos identifiants.");
            }
        }
            public void ChargeData()
            {
                try
                {
                    List<Employe> employes = new Employe().FindAll();
                    LesEmployes = new ObservableCollection<Employe>(employes);
                    this.DataContext = this;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Problème lors de la récupération des données,veuillez consulter votre admin");
                    LogError.Log(ex, "Erreur SQL");
                    Application.Current.Shutdown();
                }
            }
        /*
        private void FermetureDeLaConnexion(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this.DialogResult != true)
            {
                Application.Current.Shutdown();
            }
        }
        */
        private void ButtQuitterApp(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
