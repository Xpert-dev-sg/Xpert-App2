using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace XpertApp2.Views
{
    /// <summary>
    /// Interaction logic for Page2.xaml
    /// </summary>
    public partial class AccessPage : Page
    {
        public AccessPage()
        {
            InitializeComponent();

            #region  创建计时器
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(15);
            timer.Tick += Timer_Tick;
            timer.Start();
            #endregion
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // 回到主线程
            this.NavigationService.Navigate(new MenuPage());

            // 停止计时器
            DispatcherTimer timer = sender as DispatcherTimer;
            if (timer != null)
            {
                timer.Stop();
                timer.Tick -= Timer_Tick;
            }
        }

        
    }
}
