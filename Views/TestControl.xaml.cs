using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
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
using System.Xml.Linq;
using XpertApp2.DB;
using XpertApp2.Utility;

namespace XpertApp2.Views
{
    /// <summary>
    /// Interaction logic for TestControl.xaml
    /// </summary>
    public partial class TestControl : Page
    {
        
        public static ObservableCollection<string> reults;
        public TestControl()
        {
            InitializeComponent();
            reults = new ObservableCollection<string>(); 
            lbResults.ItemsSource = reults;
        }


        private void btnReadRFID_Click(object sender, RoutedEventArgs e)
        {
            
            RFIDUtility rFIDUtility = new RFIDUtility();
            string s = rFIDUtility.Read_RFID();
            

            reults.Add(s);
        }

        private void btnOPenDoor_left_Click(object sender, RoutedEventArgs e)
        {
            string s = "574B4C590901820281";
            reults.Add($"send {s}");
            DoorUtility doorUtility = new DoorUtility();
            doorUtility.OpenDoor_test(s);
            
        }

        private void btnOPenDoor_all_Click(object sender, RoutedEventArgs e)
        {
            string s = "574B4C5908018686";
            reults.Add($"send {s}");
            DoorUtility doorUtility = new DoorUtility();
            doorUtility.OpenDoor_test(s);
        }

        private void btnOPenDoor_right_Click(object sender, RoutedEventArgs e)
        {
            string s = "574B4C590901820B88";
            reults.Add($"send {s}");
            DoorUtility doorUtility = new DoorUtility();
            doorUtility.OpenDoor_test(s);
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

       

        private void btnClearRFID_Click(object sender, RoutedEventArgs e)
        {
            //RFIDs.Clear();
            reults.Clear();

        }
    }

    
}
