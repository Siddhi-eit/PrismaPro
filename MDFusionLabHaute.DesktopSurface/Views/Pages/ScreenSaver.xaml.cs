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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MDFusionLabHaute.DesktopSurface.Views.Pages
{
    /// <summary>
    /// Interaction logic for ScreenSaver.xaml
    /// </summary>
    public partial class ScreenSaver : Page
    {
        #region Declaration
        private static ScreenSaver _instance;
        public static ScreenSaver Instance
        {
            get { return _instance ?? (_instance = new ScreenSaver()); }
        }
        #endregion
        public ScreenSaver()
        {
            InitializeComponent();

        }
    }
}