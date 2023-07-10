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


namespace TagAll_Doors_Windows
{
    /// <summary>
    /// Interaction logic for Window.xaml
    /// </summary>
    public partial class frmTagDrsWndws : Window
    {
        public frmTagDrsWndws()
        {
            InitializeComponent();
        }

        internal bool GetCheckBoxFlrs()
        {
            if (cbxFlrPlans.IsChecked == true)
                return true;

            return false;
        }

        internal bool GetCheckBoxDims()
        {
            if (cbxDimPlans.IsChecked == true)
                return true;

            return false;
        }

        internal bool GetCheckBoxDouble()
        {
            if (cbxDblTag.IsChecked == true)
                return true;

            return false;
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void btnHelp_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
