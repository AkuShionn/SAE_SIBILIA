using System;
using System.Windows;
using System.Windows.Controls;
using Npgsql;  // Nécessaire pour utiliser PostgreSQL via Npgsql

namespace SAE_SIBILIA.Fenetres
{
    public partial class Connexion : Window
    {
        // Chaîne de connexion à votre base de données PostgreSQL
        private readonly string ConnectionString = "Host=srv-peda-new;Port=5433;Username=alvesdjo;Password=NgRpEr;Database=sibilia;Options='-c search_path=sibilia_shema'";

        public Connexion()
        {
            InitializeComponent();
        }

        /* // Méthode pour vérifier l'utilisateur dans la base de données
         private bool AuthenticateUser(string username, string password, out string userRole)
         {
             userRole = string.Empty;  // Le rôle de l'utilisateur

             try
             {
                 // Connexion à la base de données
                 using (var connection = new NpgsqlConnection(ConnectionString))
                 {
                     connection.Open();

                     // Requête SQL pour vérifier l'utilisateur et récupérer le rôle
                     string query = "SELECT role FROM users WHERE username = @username AND password = @password";
                     using (var cmd = new NpgsqlCommand(query, connection))
                     {
                         // Paramétrer les valeurs de la requête
                         cmd.Parameters.AddWithValue("username", username);
                         cmd.Parameters.AddWithValue("password", password);

                         // Exécuter la requête et récupérer les résultats
                         using (var reader = cmd.ExecuteReader())
                         {
                             if (reader.HasRows) // Si l'utilisateur existe dans la base
                             {
                                 reader.Read();  // Lire les données
                                 userRole = reader.GetString(0); // Récupérer le rôle
                                 return true;  // Authentification réussie
                             }
                         }
                     }
                 }
             }
             catch (Exception ex)
             {
                 MessageBox.Show("Erreur de connexion à la base de données: " + ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
             }

             return false;  // L'utilisateur n'a pas été trouvé ou une erreur s'est produite
         }

         // L'événement Click pour le bouton "Se connecter"
         private void LoginButton_Click(object sender, RoutedEventArgs e)
         {
             // Récupérer les valeurs des champs de saisie
             string username = usernameTextBox.Text;
             string password = passwordBox.Password;

             // Vérifier l'utilisateur dans la base de données
             if (AuthenticateUser(username, password, out string userRole))
             {
                 // Ouvrir MainWindow en fonction du rôle de l'utilisateur
                 if (userRole == "admin")
                 {
                     MessageBox.Show("Bienvenue Administrateur !", "Connexion réussie", MessageBoxButton.OK, MessageBoxImage.Information);
                     // Si c'est un admin, ouvrir MainWindow
                     MainWindow mainWindow = new MainWindow();
                     mainWindow.Show();
                 }
                 else if (userRole == "employee")
                 {
                     MessageBox.Show("Bienvenue Employé !", "Connexion réussie", MessageBoxButton.OK, MessageBoxImage.Information);
                     // Si c'est un employé, ouvrir MainWindow (ou autre fenêtre)
                     MainWindow mainWindow = new MainWindow();
                     mainWindow.Show();
                 }

                 // Fermer la fenêtre de connexion
                 this.Close();
             }
             else
             {
                 // Si les identifiants sont incorrects
                 MessageBox.Show("Identifiant ou mot de passe incorrect", "Erreur de connexion", MessageBoxButton.OK, MessageBoxImage.Error);
             }*/

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            // Ouvrir MainWindow
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();  // Afficher MainWindow

            // Fermer la fenêtre de connexion
            this.Close();
        }
    }
}
