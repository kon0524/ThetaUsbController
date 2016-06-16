using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpdMtpLib;

namespace ThetaUsbController.Model
{
    public static class Theta
    {
        /// <summary>
        /// 露出値
        /// </summary>
        public static int Ev
        {
            get
            {
                int val = 0;
                if (isConnected)
                {
                    MtpResponse res = mtp.Execute(MtpOperationCode.GetDevicePropValue, new uint[1] { (uint)MtpDevicePropCode.ExposureBiasCompensation }, null);
                    val = (int)BitConverter.ToInt16(res.Data, 0);
                }
                return val;
            }

            set
            {
                if (isConnected)
                {
                    mtp.Execute(MtpOperationCode.SetDevicePropValue, new uint[1] { (uint)MtpDevicePropCode.ExposureBiasCompensation }, BitConverter.GetBytes(((short)value)));
                }
            }
        }

        /// <summary>
        /// 接続状態
        /// </summary>
        private static bool isConnected = false;

        /// <summary>
        /// ストレージID
        /// </summary>
        private static uint storageId = 0;

        /// <summary>
        /// MTPオブジェクト
        /// </summary>
        private static MtpCommand mtp = new MtpCommand();

        /// <summary>
        /// Thetaに接続する
        /// </summary>
        /// <returns></returns>
        public static bool Connect()
        {
            // 接続済みならtrueを返して終了
            if (isConnected) { return true; }

            string thetaId = null;
            string[] ids = mtp.GetDeviceIds();
            foreach (string id in ids)
            {
                int vid = id.IndexOf("vid_05ca");
                int pid = id.IndexOf("pid_0366");
                if (vid != -1 && pid != -1)
                {
                    thetaId = id;
                    break;
                }
            }

            // Thetaがなければfalseを返す
            if (thetaId == null) { return false; }

            // Thetaに接続する
            mtp.Open(thetaId);
            isConnected = true;

            // 静止画撮影モードにする
            MtpResponse res;
            res = mtp.Execute(MtpOperationCode.SetDevicePropValue, new uint[1] { (uint)MtpDevicePropCode.StillCaptureMode }, BitConverter.GetBytes(((ushort)0x0001)));

            // 撮影モードをオートにする
            res = mtp.Execute(MtpOperationCode.SetDevicePropValue, new uint[1] { (uint)MtpDevicePropCode.ExposureProgramMode }, BitConverter.GetBytes(((ushort)0x0002)));

            // ストレージIDを取得する
            res = mtp.Execute(MtpOperationCode.GetStorageIDs, null, null);
            storageId = MtpData.GetUInt32Array(res)[0];

            return true;
        }

        /// <summary>
        /// Thetaから切断する
        /// </summary>
        public static void Disconnect()
        {
            // 接続していなければ終了
            if (!isConnected) { return; }

            // 切断
            mtp.Close();
            isConnected = false;

            return;
        }

        /// <summary>
        /// レリーズ押下
        /// </summary>
        /// <returns></returns>
        public static bool Release()
        {
            if (isConnected)
            {
                mtp.Execute(MtpOperationCode.InitiateCapture, new uint[2] { 0, 0 }, null);
            }
            return true;
        }
    }
}
