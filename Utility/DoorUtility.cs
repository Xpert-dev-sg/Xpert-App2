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

    public class DoorUtility
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SerialPort ComDevice = new SerialPort();
        public void OpenDoor()
        {
            byte[] senddoor1Data = Encoding.ASCII.GetBytes("574B4C590901820B88");
            byte[] senddoor2Data = Encoding.ASCII.GetBytes("574B4C590901820281");
            byte[] senddoor3Data = Encoding.ASCII.GetBytes("574B4C5908018686");
            string comlist = DB_Base.door_com;
            int baudrate = Convert.ToInt32(DB_Base.door_baudrate);
            int parity = Convert.ToInt32(DB_Base.door_parity);
            int databits = Convert.ToInt32(DB_Base.door_databits);
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
                    RFIDUtility rFIDUtility = new RFIDUtility();
                    DB_Base.RFIDList_o = rFIDUtility.Read_RFID();
                    ComDevice.Open();
                    if (ComDevice.IsOpen)
                    {
                        //ComDevice.Write(senddoor1Data, 0, senddoor1Data.Length);
                        //ComDevice.Write(senddoor2Data, 0, senddoor2Data.Length);
                        ComDevice.Write(senddoor3Data, 0, senddoor3Data.Length);
                    }


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

        public void LogDoor(bool is_open)
        {
            var msg = is_open ? "Door is open." : "Door is closed.";

            log.Info(msg);

            EventModel Event = new EventModel();
            Event.Event_Type = "Door event";
            Event.Event_Description = msg;
            Event.User_Id = DB_Base.CurrentUser.UserName;
            Event.Event_datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            Event.CreateBy = DB_Base.CurrentUser.UserName;
            Event.CreateOn = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            EventDB eventDB = new EventDB();
            eventDB.InsertEvent(Event);
        }
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
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
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
            if (DB_Base.IsDoorOpen == false)
            {
                DB_Base.IsDoorOpen = true;
                Console.WriteLine("Monitoring Started...");
                // 实现监控逻辑
                DoorUtility doorUtility = new DoorUtility();
                doorUtility.LogDoor(true);
            }
        }

        private void StopMonitoring()
        {
            if (DB_Base.IsDoorOpen == true)
            {
                DB_Base.IsDoorOpen = false;
                Console.WriteLine("Monitoring Stopped...");
                // 实现停止监控逻辑
                DoorUtility doorUtility = new DoorUtility();
                doorUtility.LogDoor(false);

                RFIDUtility rFIDUtility = new RFIDUtility();
                DB_Base.RFIDList_c = rFIDUtility.Read_RFID();
                rFIDUtility.Compare_RFID(DB_Base.RFIDList_c, DB_Base.RFIDList_o);

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
