using GDotnet.Reader.Api.Protocol.Gx;
using System.IO.Ports;
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
using XpertTest1.RFID;

namespace XpertTest1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SerialPort ComDevice = new SerialPort();
        public MainWindow()
        {
            InitializeComponent();
            //ComDevice.DataReceived += new SerialDataReceivedEventHandler(Com_DataReceived);//绑定事件
        }

        private void Open_Door_Button_Click(object sender, RoutedEventArgs e)
        {


            string comlist = txtport.Text;
            string baudrate = txtbaudrate.Text;
            string parity = txtparity.Text;
            string databits = txtdatabits.Text;
            string stopbits = txtstopbits.Text;

            if (string.IsNullOrEmpty(comlist))
            {
                MessageBox.Show("没有发现串口,请检查线路！");
                return;
            }

            if (ComDevice.IsOpen == false)
            {
                ComDevice.PortName = comlist;
                ComDevice.BaudRate = Convert.ToInt32(baudrate);
                ComDevice.Parity = (Parity)Convert.ToInt32(parity);
                ComDevice.DataBits = Convert.ToInt32(databits);
                ComDevice.StopBits = (StopBits)Convert.ToInt32(stopbits);
                try
                {
                    ComDevice.Open();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error");
                    return;
                }
                btnOpen.Content = "Close door";

            }
            else
            {
                try
                {
                    ComDevice.Close();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error");
                }
                btnOpen.Content = "Open door";
            }
        }


        private void Read_RFID_Button_Click(object sender, RoutedEventArgs e)
        {
            string readername = $"{txtport.Text}:{txtbaudrate2.Text}";

            XpertTest1.RFID.RFID.Read_RFID(readername);
        }


        //private void Com_DataReceived(object sender, SerialDataReceivedEventArgs e)
        //{
        //    byte[] ReDatas = new byte[ComDevice.BytesToRead];
        //    ComDevice.Read(ReDatas, 0, ReDatas.Length);//读取数据
        //    this.AddData(ReDatas);//输出数据
        //}

        ///// <summary>
        ///// 添加数据
        ///// </summary>
        ///// <param name="data">字节数组</param>
        //public void AddData(byte[] data)
        //{
        //    if (rbtnHex.Checked)
        //    {
        //        StringBuilder sb = new StringBuilder();
        //        for (int i = 0; i < data.Length; i++)
        //        {
        //            sb.AppendFormat("{0:x2}" + " ", data[i]);
        //        }
        //        AddContent(sb.ToString().ToUpper());
        //    }
        //    else if (rbtnASCII.Checked)
        //    {
        //        AddContent(new ASCIIEncoding().GetString(data));
        //    }
        //    else if (rbtnUTF8.Checked)
        //    {
        //        AddContent(new UTF8Encoding().GetString(data));
        //    }
        //    else if (rbtnUnicode.Checked)
        //    {
        //        AddContent(new UnicodeEncoding().GetString(data));
        //    }
        //    else
        //    { }

        //    lblRevCount.Invoke(new MethodInvoker(delegate
        //    {
        //        lblRevCount.Text = (int.Parse(lblRevCount.Text) + data.Length).ToString();
        //    }));
        //}


    }
}