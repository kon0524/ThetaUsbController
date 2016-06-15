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
        /// 接続状態
        /// </summary>
        private static bool isConnected = false;

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
    }
}
