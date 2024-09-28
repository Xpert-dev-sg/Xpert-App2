using GDotnet.Reader.Api.DAL;
using GDotnet.Reader.Api.Protocol.Gx;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using XpertApp2.DB;
using XpertApp2.Views;



namespace XpertApp2.Utility
{
    public class RFIDUtility
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static List<string> IDs;
        public static string Read_RFID()
        {
            IDs = new List<string>();
            var result = "";
            try
            {
                GClient clientConn = new GClient();
                eConnectionAttemptEventStatusType status;
                // clientConn.OpenTcp("192.168.1.168:8160", 3000, out status)
                //clientConn.OpenSerial("COM16:115200", 3000, out status)
                if (clientConn.OpenTcp("192.168.1.250:8160", 5000, out status))//clientConn.OpenSerial($"{DB_Base.rfid_com}:{DB_Base.rfid_baudrate}", 3000, out status))
                {
                    log.Info("RFID connection:192.168.1.250:8160");
                    // subscribe to event
                    clientConn.OnEncapedTagEpcLog += new delegateEncapedTagEpcLog(OnEncapedTagEpcLog);
                    clientConn.OnEncapedTagEpcOver += new delegateEncapedTagEpcOver(OnEncapedTagEpcOver);
                    // stop command, idle state
                    MsgBaseStop msgBaseStop = new MsgBaseStop();
                    clientConn.SendSynMsg(msgBaseStop);

                    // Power configuration, set the power of the 4 antennas as 30dBm
                    MsgBaseSetPower msgBaseSetPower = new MsgBaseSetPower();
                    msgBaseSetPower.DicPower = new Dictionary<byte, byte>()
                                                                         {
                                                                         {1, 30},
                                                                         {2, 30},
                                                                         {3, 30},
                                                                         {4, 30}
                                                                         };
                    clientConn.SendSynMsg(msgBaseSetPower);

                    // 2 antenna read Inventory, EPC & TID
                    EncapedLogBaseEpcInfo tag = ReadSingleTag(clientConn, (uint)(eAntennaNo._1 | eAntennaNo._2 | eAntennaNo._3 | eAntennaNo._4), 5000);
                    if (null != tag)
                    {

                        //result = tag.logBaseEpcInfo.ToString();
                        result = string.Join(";", IDs.Distinct());
                        log.Info($"get RFID:{result}");
                    }
                }
                else
                {
                    log.Error("Connect failure.");
                }
            }
            catch (Exception ex)
            {

                log.Error(ex.Message);
                MessageBox.Show(ex.Message, "Error");
            }

            return result;
        }


        public static string Read_RFID(uint antenable)
        {
            IDs = new List<string>();
            var result = "";
            try
            {
                GClient clientConn = new GClient();
                eConnectionAttemptEventStatusType status;
                // clientConn.OpenTcp("192.168.1.168:8160", 3000, out status)
                //clientConn.OpenSerial("COM16:115200", 3000, out status)
                if (clientConn.OpenTcp("192.168.1.250:8160", 3000, out status))
                {
                    // subscribe to event
                    clientConn.OnEncapedTagEpcLog += new delegateEncapedTagEpcLog(OnEncapedTagEpcLog);
                    //clientConn.OnEncapedTagEpcOver += new delegateEncapedTagEpcOver(OnEncapedTagEpcOver);
                    // stop command, idle state
                    MsgBaseStop msgBaseStop = new MsgBaseStop();
                    clientConn.SendSynMsg(msgBaseStop);

                    // Power configuration, set the power of the 4 antennas as 30dBm
                    MsgBaseSetPower msgBaseSetPower = new MsgBaseSetPower();
                    msgBaseSetPower.DicPower = new Dictionary<byte, byte>()
                                                                         {
                                                                         {1, 10},
                                                                         {2, 10},
                                                                         {3, 10},
                                                                         {4, 10}
                                                                         };
                    clientConn.SendSynMsg(msgBaseSetPower);


                    // 2 antenna read Inventory, EPC & TID
                    EncapedLogBaseEpcInfo tag = ReadSingleTag(clientConn, antenable, 3000);
                    if (null != tag)
                    {
                        result = string.Join(";", IDs.Distinct());
                        log.Info($"get RFID:{result}");
                    }

                }
                else
                {
                    log.Error("Connect failure.");
                }
               
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                MessageBox.Show(ex.Message, "Error");
            }

            return result;
        }

        //public void Compare_RFID(string rfid1, string rfid2)
        //{
        //    try
        //    {
        //        log.Info($"after close:{string.Join(",", rfid1)}");
        //        log.Info($"before ope:{string.Join(",", rfid2)}");
        //        EventDB eventDB = new EventDB();
        //        ContentDB contentDB = new ContentDB();

        //        // 将字符串拆分为数组，使用中文逗号“，”作为分隔符
        //        string[] array1 = rfid1.Split(';');
        //        string[] array2 = rfid2.Split(';');

        //        // 找出string1中有但string2中没有的元素
        //        var borrow_arr = array1.Except(array2);

        //        // 找出string2中有但string1中没有的元素
        //        var return_arr = array2.Except(array1);

        //        DB_Base.borrowlist = null;// new System.Collections.ObjectModel.ObservableCollection<Items>();
        //        DB_Base.returnlist = null;// new System.Collections.ObjectModel.ObservableCollection<Items>();

        //        string borrowlist_str = "";
        //        string returnlist_str = "";
        //        string[] tomail = new string[] { DB_Base.borrowUser.Email };
        //        var Subject = "";
        //        var msg = "";
        //        string b = "";
        //        string r = "";
        //        foreach (var item in borrow_arr)
        //        {
        //            eventDB.InsertBorrowRecords(item, DB_Base.borrowUser.UserName);
        //            contentDB.UpdateContent_on_hand(item, DB_Base.borrowUser.UserName);//update content on hand
        //            Items items = new Items();
        //            items.Item_Name = contentDB.GetContent_rfid(item);
        //            DB_Base.borrowlist.Add(items);
        //            //DBUtility.verifyBorrow(item);//verify if user is allowed to borrow this item
        //            b += item + ",";
        //        }

        //        foreach (var item in return_arr)
        //        {
        //            eventDB.UpdateBorrowRecords_return(item, DB_Base.borrowUser.UserName);
        //            contentDB.UpdateContent_on_hand(item, "");
        //            Items items = new Items();
        //            items.Item_Name = contentDB.GetContent_rfid(item);
        //            DB_Base.returnlist.Add(items);
        //            r += item + ",";
        //        }
        //        log.Info($"borrow:{string.Join(",", b)}");
        //        log.Info($"return:{string.Join(",", r)}");
        //        if (DB_Base.borrowlist.Count() > 0)
        //        {
        //            Subject = " borrwo";
        //            foreach (var item in DB_Base.borrowlist)
        //            {
        //                borrowlist_str += item.Item_Name + ",";
        //            }
        //            msg += $"{DB_Base.borrowUser.UserName} take {borrowlist_str} out on {DateTime.Now}.</br>";
        //        }

        //        if (DB_Base.returnlist.Count() > 0)
        //        {
        //            Subject += " return";

        //            foreach (var item in DB_Base.returnlist)
        //            {
        //                returnlist_str += item.Item_Name + ",";
        //            }
        //            msg += $"{DB_Base.borrowUser.UserName} return {returnlist_str} out on {DateTime.Now}.</br>";

        //        }
        //        log.Info($"borrow:{string.Join(",", borrowlist_str)}");
        //        log.Info($"return:{string.Join(",", returnlist_str)}");
        //        if (!string.IsNullOrEmpty(msg))
        //        {
        //            //show form list
        //            NotePopup notePopup = new NotePopup();
        //            notePopup.Show();
        //            EmailUtility.SendEmail(msg, Subject + "item", tomail);
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        log.Error(ex.Message);
        //        MessageBox.Show(ex.Message, "Error");
        //    }

        //}

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
                msgBaseInventoryEpc.AntennaEnable = (ushort)(eAntennaNo._1 | eAntennaNo._2 | eAntennaNo._3 | eAntennaNo._4);
                msgBaseInventoryEpc.InventoryMode = (byte)eInventoryMode.Inventory;
                msgBaseInventoryEpc.ReadTid = new ParamEpcReadTid();                // tid参数
                msgBaseInventoryEpc.ReadTid.Mode = (byte)eParamTidMode.Auto;
                msgBaseInventoryEpc.ReadTid.Len = 6;
                clientConn.SendUnsynMsg(msgBaseInventoryEpc);

                //lock (waitReadSingle)
                //{
                //    if (null == waitTag)
                //    {
                //        Monitor.Wait(waitReadSingle, timeout);
                //    }
                //}
                Thread.Sleep(500);

                msgBaseStop = new MsgBaseStop();
                clientConn.SendUnsynMsg(msgBaseStop);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                MessageBox.Show(ex.Message, "Error");
            }
            finally
            {
                clientConn.Close();
            }


            return waitTag;
        }

        #region API事件

        public static void OnEncapedTagEpcLog(EncapedLogBaseEpcInfo msg)
        {
            //TestControl testControl = new TestControl();
            // Any blocking inside the callback will affect the normal use of the API !
            // 回调里面的任何阻塞或者效率过低，都会影响API的正常使用 !
            if (null != msg && 0 == msg.logBaseEpcInfo.Result)
            {
                //RFID rFID = new RFID();
                //rFID.ID = msg.logBaseEpcInfo.ToString();
                IDs.Add(msg.logBaseEpcInfo.ToString());
                waitTag = msg;
                //MessageBox.Show(msg.logBaseEpcInfo.ToString());
                //TestControl.RFIDs.Add(rFID);

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
