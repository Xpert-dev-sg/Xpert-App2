using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Markup;
using XpertApp2.DB;
using XpertApp2.Views;

namespace XpertApp2.Utility
{

    public class DoorUtility
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public SerialPort ComDevice = new SerialPort();
        public void OpenDoor()
        {
            try
            {

                byte[] senddoor1Data = strToHexByte("574B4C590901820B88");
                byte[] senddoor2Data = strToHexByte("574B4C590901820281");
                byte[] senddoor3Data = strToHexByte("574B4C5908018686");
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


                ComDevice.PortName = comlist;
                ComDevice.BaudRate = Convert.ToInt32(baudrate);
                ComDevice.Parity = (Parity)Convert.ToInt32(parity);
                ComDevice.DataBits = Convert.ToInt32(databits);
                ComDevice.StopBits = (StopBits)Convert.ToInt32(stopbits);

                
                ComDevice.Open();
                
                if (ComDevice.IsOpen)
                {   
                    log.Info($"connection:{ComDevice.PortName};{ComDevice.BaudRate};{ComDevice.Parity};{ComDevice.DataBits};{ComDevice.StopBits};ComDevice.IsOpen:{ComDevice.IsOpen}; send data:574B4C5908018686");
                    //ComDevice.Write(senddoor1Data, 0, senddoor1Data.Length);
                    //Thread.Sleep(1000);
                    //ComDevice.Write(senddoor2Data, 0, senddoor2Data.Length);
                    //Thread.Sleep(1000);
                    ComDevice.Write(senddoor3Data, 0, senddoor3Data.Length);
                    Thread.Sleep(1000);
                }


            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                MessageBox.Show(ex.Message, "Error");
                //return;
            }
            finally
            {
                if (ComDevice.IsOpen)
                {
                    ComDevice.Close();
                }
            }





        }


        public void OpenDoor_test(string c)
        {
            try
            {
                ComDevice.DataReceived += new SerialDataReceivedEventHandler(Com_DataReceived);//绑定事件

                byte[] senddoor1Data = strToHexByte(c);

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


                ComDevice.PortName = comlist;
                ComDevice.BaudRate = Convert.ToInt32(baudrate);
                ComDevice.Parity = (Parity)Convert.ToInt32(parity);
                ComDevice.DataBits = Convert.ToInt32(databits);
                ComDevice.StopBits = (StopBits)Convert.ToInt32(stopbits);
                AdminPage.reults.Add($"{ComDevice.PortName};{ComDevice.BaudRate};{ComDevice.Parity};{ComDevice.DataBits};{ComDevice.StopBits}");
                ComDevice.Open();
                
                if (ComDevice.IsOpen)
                {
                    //MessageBox.Show("ComDevice.IsOpen: " + ComDevice.IsOpen);
                    AdminPage.reults.Add($"ComDevice.IsOpen:{ComDevice.IsOpen}");
                    AdminPage.reults.Add($"send:{c}");
                    log.Info($"connection:{ComDevice.PortName};{ComDevice.BaudRate};{ComDevice.Parity};{ComDevice.DataBits};{ComDevice.StopBits};ComDevice.IsOpen:{ComDevice.IsOpen}; send data:{c}");
                    ComDevice.Write(senddoor1Data, 0, senddoor1Data.Length);
                    Thread.Sleep(2000);
                }



            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                MessageBox.Show(ex.Message, "Error");
                //return;
            }
            finally
            {
                if (ComDevice.IsOpen)
                {
                    ComDevice.Close();
                }
            }





        }

        private byte[] strToHexByte(string hexString)
        {
            //hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0) hexString += " ";
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2).Replace(" ", ""), 16);
            return returnBytes;
        }

        public bool IsOPen()
        {
            return ComDevice.IsOpen;
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
        private void Com_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                byte[] ReDatas = new byte[ComDevice.BytesToRead];
                ComDevice.Read(ReDatas, 0, ReDatas.Length);//读取数据
                                                           //MessageBox.Show("ReDatas: " + ReDatas);
                                                           //this.AddData(ReDatas);//输出数据

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < ReDatas.Length; i++)
                {
                    sb.AppendFormat("{0:x2}" + " ", ReDatas[i]);
                }
                string result = sb.ToString().ToUpper();

                //TestControl.reults.Add(result);
                log.Info(result);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                log.Error(ex.Message);

            }

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
            try
            {
                this.door = door;
                this.door.DoorStateChanged += OnDoorStateChanged;

                // 定时器用于定期检查门的状态
                doorCheckTimer = new System.Timers.Timer(1000); // 每秒检查一次
                doorCheckTimer.Elapsed += CheckDoorStatus;
                doorCheckTimer.AutoReset = true;
                doorCheckTimer.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                log.Error(ex.Message);
            }

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
            try
            {
                if (DB_Base.IsDoorOpen == false)
                {
                    DB_Base.IsDoorOpen = true;
                    log.Info("Monitoring Started...");
                    // 实现监控逻辑
                    DoorUtility doorUtility = new DoorUtility();
                    doorUtility.LogDoor(true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                log.Error(ex.Message);
            }

        }

        private void StopMonitoring()
        {
            try
            {
                if (DB_Base.IsDoorOpen == true)
                {
                    DB_Base.IsDoorOpen = false;
                    log.Info("Monitoring Stopped...");
                    // 实现停止监控逻辑
                    DoorUtility doorUtility = new DoorUtility();
                    doorUtility.LogDoor(false);

                    RFIDUtility rFIDUtility = new RFIDUtility();
                    DB_Base.RFIDList_c = rFIDUtility.Read_RFID();
                    rFIDUtility.Compare_RFID(DB_Base.RFIDList_c, DB_Base.RFIDList_o);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                log.Error(ex.Message);
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
