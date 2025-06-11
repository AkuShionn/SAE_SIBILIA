using SAE_SIBILIA.Classes;
using System.Configuration;
using System.Data;
using System.Windows;

namespace SAE_SIBILIA
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public enum Action { Modifier, Créer };
        public App()
        {
            this.Exit += App_Exit;
        }
        private void App_Exit(object sender, ExitEventArgs e)
        {
            DataAccess.Instance.CloseConnection();
        }

    }

}
