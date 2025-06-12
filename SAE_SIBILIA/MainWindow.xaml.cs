using SAE_SIBILIA.Classes;
using SAE_SIBILIA.Fenetres;
using SAE_SIBILIA.UserControls;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SAE_SIBILIA
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Employe employeConnecte;
        public MainWindow()
        {
            InitializeComponent();
            Login();
            ChargeUserControl();
        }
        private void Employe()
        {
            Main.Content = new PageAcceuil();
        }
        private void Login()
        {
            Connexion co = new Connexion();
            if (co.ShowDialog() == true)
            {
                employeConnecte = co.EmployeConnecte;
            }
        }
        private void ChargeUserControl()
        {
            Main.Content = null;
            Employe();
        }

        private void ButtonClient_Click(object sender, RoutedEventArgs e)
        {
            DataGridClient dgclientControl = new DataGridClient();
            Menu.Content = dgclientControl;
        }

        private void ButtonAcceuil_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            Menu.Content = null;

        }

        private void ButtonNouvelleCommande_Click(object sender, RoutedEventArgs e)
        {
            CreerCommande commmandeControl = new CreerCommande();
            Menu.Content = commmandeControl;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CreerCommande commmandeControl = new CreerCommande();
            Menu.Content = commmandeControl;
        }

        private void ButtClickGestionPlat(object sender, RoutedEventArgs e)
        {
           DataGridGestionPlats gestionPlatsControl = new DataGridGestionPlats();
           Menu.Content = gestionPlatsControl;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
          
                CommandeDuJour commandeDuJ = new CommandeDuJour();
                Menu.Content = commandeDuJ;
           

        }
    }
}