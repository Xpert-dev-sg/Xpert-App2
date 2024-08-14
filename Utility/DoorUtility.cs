using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using XpertApp2.DB;

namespace XpertApp2.Utility
{
    public  class DoorUtility
    {
        private SerialPort ComDevice = new SerialPort();
        public void OpenDoor()
        {
            
            
            string comlist = "COM";
            int baudrate = 9600, parity = 8, databits = 1;
            StopBits stopbits = (StopBits)Convert.ToInt32(1);
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
                
            }


        }

        //get list from RFID before open door
    }

    public class Door
    {
        public event EventHandler<DoorEventArgs> DoorStateChanged;

        private bool isDoorOpen;
        public bool IsDoorOpen
        {
            get => isDoorOpen;
            set
            {
                if (isDoorOpen != value)
                {
                    isDoorOpen = value;
                    OnDoorStateChanged(new DoorEventArgs(isDoorOpen));
                }
            }
        }

        protected virtual void OnDoorStateChanged(DoorEventArgs e)
        {
            DoorStateChanged?.Invoke(this, e);
        }
    }

    public class DoorMonitor
    {
        private readonly Door door;
        private System.Timers.Timer doorCheckTimer;
        private SerialPort ComDevice = new SerialPort();

        public DoorMonitor(Door door)
        {
            this.door = door;
            this.door.DoorStateChanged += OnDoorStateChanged;

            // 定时器用于定期检查门的状态
            doorCheckTimer = new System.Timers.Timer(1000); // 每秒检查一次
            doorCheckTimer.Elapsed += CheckDoorStatus;
            doorCheckTimer.AutoReset = true;
            doorCheckTimer.Start();
        }

        private void CheckDoorStatus(object sender, ElapsedEventArgs e)
        {
            // 这里可以使用传感器、外部API等实时检查门状态
            // 假设这里通过传感器获取门的实际状态：
            bool currentDoorStatus = GetActualDoorStatus();

            if (door.IsDoorOpen != currentDoorStatus)
            {
                door.IsDoorOpen = currentDoorStatus; // 触发状态变化事件
            }
        }

        private bool GetActualDoorStatus()
        {
            return ComDevice.IsOpen;
            //// 这个方法应该返回门的实际状态
            //// 在此模拟门的状态：随机开关门
            //Random rand = new Random();
            //return rand.Next(0, 2) == 0; // 随机返回 true（开）或 false（关）
        }

        private void OnDoorStateChanged(object sender, DoorEventArgs e)
        {
            if (e.IsDoorOpen)
            {
                StartMonitoring();
            }
            else
            {
                StopMonitoring();
                
            }
        }

        private void StartMonitoring()
        {
            if(DB_Base.IsDoorOpen==false)
            {
                DB_Base.IsDoorOpen = true;
                Console.WriteLine("Monitoring Started...");
                // 实现监控逻辑
                //logdown open door
            }
        }

        private void StopMonitoring()
        {
            if (DB_Base.IsDoorOpen == true)
            {
                DB_Base.IsDoorOpen = false;
                Console.WriteLine("Monitoring Stopped...");
                // 实现停止监控逻辑
                //log down close door
                //get list from RFID
                //compare list create borrow record and return record
                //update database
                //access page to show the result
            }
        }

       
    }

    public class DoorEventArgs : EventArgs
    {
        public bool IsDoorOpen { get; }

        public DoorEventArgs(bool isDoorOpen)
        {
            IsDoorOpen = isDoorOpen;
        }
    }
}
