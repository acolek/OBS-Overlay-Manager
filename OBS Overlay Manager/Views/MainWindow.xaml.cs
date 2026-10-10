using System.ComponentModel;
using System.Windows;
using OBS_Overlay_Manager.Models;
using OBS_Overlay_Manager.ViewModels;

namespace OBS_Overlay_Manager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainWindowViewModel vm;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = vm = new MainWindowViewModel(Profile.LoadProfiles(), new PNGComposite());
        }

        private void On_Close(object sender, CancelEventArgs e)
        {
            vm.SaveJSON();
        }
    }
}