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
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonClient_Click(object sender, RoutedEventArgs e)
        {
            CreationClients clientControl = new CreationClients();
            Menu.Content = clientControl;
        }

        private void ButtonAcceuil_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            Menu.Content = mainWindow;
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
    }
}