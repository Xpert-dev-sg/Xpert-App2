using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO.Ports;

namespace XpertTest1.door
{
    
    public  class Door
    {
        private SerialPort ComDevice = new SerialPort();
        public static void OpenDoor(string comlist,string baudrate,string parity,string databits,string stopbits)
        {
            //if (string.IsNullOrEmpty(comlist))
            //{
            //    MessageBox.Show("没有发现串口,请检查线路！");
            //    return;
            //}

            //if (ComDevice.IsOpen == false)
            //{
            //    ComDevice.PortName = comlist;
            //    ComDevice.BaudRate = Convert.ToInt32(baudrate);
            //    ComDevice.Parity = (Parity)Convert.ToInt32(parity);
            //    ComDevice.DataBits = Convert.ToInt32(databits);
            //    ComDevice.StopBits = (StopBits)Convert.ToInt32(stopbits);
            //    try
            //    {
            //        ComDevice.Open();
                    
            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show(ex.Message, "错误");
            //        return;
            //    }
            //    //btnOpen.Text = "关闭串口";
                
            //}
            //else
            //{
            //    try
            //    {
            //        ComDevice.Close();
                   
            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show(ex.Message, "错误");
            //    }
            //    //btnOpen.Text = "打开串口";
            //}

           
        }
    }
}
