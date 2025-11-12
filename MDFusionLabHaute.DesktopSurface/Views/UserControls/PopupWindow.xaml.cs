using MDFusionLabHaute.DesktopSurface.LanguageResources;
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
using MDFusionLabHaute.DesktopSurface.Views.Pages;

namespace MDFusionLabHaute.DesktopSurface.Views.UserControls
{
    /// <summary>
    /// Interaction logic for PopupWindow.xaml
    /// </summary>
    public partial class PopupWindow : Window
    {
        public PopupWindow()
        {
            InitializeComponent();

            txtNextDispenseMessage.Content = LocalizedString.GetLocalizedValue<string>("NEXT DISPENSE MESSAGE", CustomDispense._selectedLanguageResource);
        }

        private void btnContinue_Click(object sender, RoutedEventArgs e)
        {
            // Set the DialogResult to true when the button is clicked
            DialogResult = true;
        }
    }
}
