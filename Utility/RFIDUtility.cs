using GDotnet.Reader.Api.DAL;
using GDotnet.Reader.Api.Protocol.Gx;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using XpertApp2.DB;


namespace XpertApp2.Utility
{
    public class RFIDUtility
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public string Read_RFID()
        {
            var result = "";
            try
            {
                GClient clientConn = new GClient();
                eConnectionAttemptEventStatusType status;
                // clientConn.OpenTcp("192.168.1.168:8160", 3000, out status)
                //clientConn.OpenSerial("COM16:115200", 3000, out status)
                if (clientConn.OpenSerial($"{DB_Base.rfid_com}:{DB_Base.rfid_baudrate}", 3000, out status))
                {
                    // subscribe to event
                    clientConn.OnEncapedTagEpcLog += new delegateEncapedTagEpcLog(OnEncapedTagEpcLog);
                    clientConn.OnEncapedTagEpcOver += new delegateEncapedTagEpcOver(OnEncapedTagEpcOver);

                    // 2 antenna read Inventory, EPC & TID
                    EncapedLogBaseEpcInfo tag = ReadSingleTag(clientConn, (uint)(eAntennaNo._1 | eAntennaNo._2), 5000);
                    if (null != tag)
                    {
                        result = tag.logBaseEpcInfo.ToString();
                    }
                }
                else
                {
                    Console.WriteLine("Connect failure.");
                }
            }
            catch (Exception ex)
            {

                log.Error(ex.Message);
                MessageBox.Show(ex.Message, "Error");
            }

            return result;
        }
        public static void Read_RFID(string readername)
        {
            GClient clientConn = new GClient();
            eConnectionAttemptEventStatusType status;
            // clientConn.OpenTcp("192.168.1.168:8160", 3000, out status)
            //clientConn.OpenSerial("COM16:115200", 3000, out status)
            if (clientConn.OpenSerial(readername, 3000, out status))
            {
                // subscribe to event
                clientConn.OnEncapedTagEpcLog += new delegateEncapedTagEpcLog(OnEncapedTagEpcLog);
                clientConn.OnEncapedTagEpcOver += new delegateEncapedTagEpcOver(OnEncapedTagEpcOver);


                Console.WriteLine("Enter any character to start reading the tag.");
                Console.ReadKey();
                Console.WriteLine("Reading....");
                // 2 antenna read Inventory, EPC & TID
                EncapedLogBaseEpcInfo tag = ReadSingleTag(clientConn, (uint)(eAntennaNo._1 | eAntennaNo._2), 5000);
                if (null != tag)
                {
                    MessageBox.Show(tag.logBaseEpcInfo.ToString());
                    Console.WriteLine(tag.logBaseEpcInfo.ToString());
                }
                // do sth.

                Console.ReadKey();

            }
            else
            {
                Console.WriteLine("Connect failure.");
            }
            Console.ReadKey();
        }

        public void Compare_RFID(string rfid1, string rfid2)
        {
            try
            {
                EventDB eventDB = new EventDB();
                ContentDB contentDB = new ContentDB();

                // 将字符串拆分为数组，使用中文逗号“，”作为分隔符
                string[] array1 = rfid1.Split('，');
                string[] array2 = rfid2.Split('，');

                // 找出string1中有但string2中没有的元素
                var borrow_arr = array1.Except(array2);

                // 找出string2中有但string1中没有的元素
                var return_arr = array2.Except(array1);
                DB_Base.borrowlist = new System.Collections.ObjectModel.ObservableCollection<string>();
                string borrowlist_str = "";
                string returnlist_str = "";
                string[] tomail = new string[] { DB_Base.CurrentUser.Email };
                var Subject = "";
                var msg = "";
                foreach (var item in borrow_arr)
                {
                    eventDB.InsertBorrowRecords(item, DB_Base.CurrentUser.UserName);
                    contentDB.UpdateContent_on_hand(item, DB_Base.CurrentUser.UserName);//update content on hand
                    DB_Base.borrowlist.Add(contentDB.GetContent_rfid(item));
                    DBUtility.verifyBorrow(item);//verify if user is allowed to borrow this item
                }

                foreach (var item in return_arr)
                {
                    eventDB.UpdateBorrowRecords_return(item, DB_Base.CurrentUser.UserName);
                    contentDB.UpdateContent_on_hand(item, "");
                    DB_Base.returnlist.Add(contentDB.GetContent_rfid(item));
                }

                if (DB_Base.borrowlist.Count() > 0)
                {
                    Subject = " borrwo";
                    foreach (var item in DB_Base.borrowlist)
                    {
                        borrowlist_str += item + ",";
                    }
                    msg += $"{DB_Base.CurrentUser.UserName} take {borrowlist_str} out on {DateTime.Now}.</br>";
                }

                if (DB_Base.returnlist.Count() > 0)
                {
                    Subject += " return";

                    foreach (var item in DB_Base.returnlist)
                    {
                        returnlist_str += item + ",";
                    }
                    msg += $"{DB_Base.CurrentUser.UserName} return {returnlist_str} out on {DateTime.Now}.</br>";

                }
                if (!string.IsNullOrEmpty(msg))
                {
                    EmailUtility.SendEmail(msg, Subject + "item", tomail);
                }
            }
            catch (Exception ex)
            {

                log.Error(ex.Message);
                MessageBox.Show(ex.Message, "Error");
            }
            
        }

        private static object waitReadSingle = new object();
        private static EncapedLogBaseEpcInfo waitTag = null;
        public static EncapedLogBaseEpcInfo ReadSingleTag(GClient clientConn, uint antEnable, int timeout)
        {
            waitTag = null;
            try
            {
                
                MsgBaseStop msgBaseStop = new MsgBaseStop();
                clientConn.SendUnsynMsg(msgBaseStop);
                MsgBaseInventoryEpc msgBaseInventoryEpc = new MsgBaseInventoryEpc();
                msgBaseInventoryEpc.AntennaEnable = antEnable;
                msgBaseInventoryEpc.InventoryMode = (byte)eInventoryMode.Inventory;
                msgBaseInventoryEpc.ReadTid = new ParamEpcReadTid();                // tid参数
                msgBaseInventoryEpc.ReadTid.Mode = (byte)eParamTidMode.Auto;
                msgBaseInventoryEpc.ReadTid.Len = 6;
                clientConn.SendUnsynMsg(msgBaseInventoryEpc);
                try
                {
                    lock (waitReadSingle)
                    {
                        if (null == waitTag)
                        {
                            Monitor.Wait(waitReadSingle, timeout);
                        }
                    }
                }
                catch { }
                msgBaseStop = new MsgBaseStop();
                clientConn.SendUnsynMsg(msgBaseStop);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                MessageBox.Show(ex.Message, "Error");
            }
            

            return waitTag;
        }

        #region API事件

        public static void OnEncapedTagEpcLog(EncapedLogBaseEpcInfo msg)
        {
            // Any blocking inside the callback will affect the normal use of the API !
            // 回调里面的任何阻塞或者效率过低，都会影响API的正常使用 !
            if (null != msg && 0 == msg.logBaseEpcInfo.Result)
            {
                waitTag = msg;
                try
                {
                    lock (waitReadSingle)
                    {
                        Monitor.Pulse(waitReadSingle);
                    }
                }
                catch { }
            }
        }

        public static void OnEncapedTagEpcOver(EncapedLogBaseEpcOver msg)
        {
            if (null != msg)
            {
                Console.WriteLine("Epc log over.");
            }
        }

        #endregion
    }
}
