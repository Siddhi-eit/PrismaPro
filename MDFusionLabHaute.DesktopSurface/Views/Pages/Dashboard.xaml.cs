using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace MDFusionLabHaute.DesktopSurface.Views.Pages
{
    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard : Page
    {
        public Dashboard()
        {
            InitializeComponent();
        }

        private void CardActionDispense_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            //Dispense Dispense = new Dispense();
            //Dispense.Loader.Visibility = Visibility.Visible;
            //this.NavigationService.Navigate(Dispense);
            //customDispense.Loader.Visibility = Visibility.Visible;
            CustomDispense customDispense = new CustomDispense();
            this.NavigationService.Navigate(customDispense);
            
            //ScreenSaver screenSaver = new ScreenSaver();
            //this.NavigationService.Navigate(screenSaver);
        }

        private void CardActionScanner_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Scanner scanner = new Scanner();
            scanner.Loader.Visibility = Visibility.Visible;
            this.NavigationService.Navigate(scanner);
        }
    }
}