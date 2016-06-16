using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpdMtpLib;
using WpdMtpLib.DeviceProperty;

namespace ThetaUsbController.Model
{
    public class Theta
    {
        /// <summary>
        /// 露出プログラム
        /// </summary>
        public ExposureProgramMode Program
        {
            get
            {
                ExposureProgramMode mode = ExposureProgramMode.NormalProgram;
                if (IsConnected)
                {
                    MtpResponse res = mtp.Execute(MtpOperationCode.GetDevicePropValue, new uint[1] { (uint)MtpDevicePropCode.ExposureProgramMode }, null);
                    mode = (ExposureProgramMode)BitConverter.ToUInt16(res.Data, 0);
                }
                return mode;
            }

            set
            {
                if (IsConnected)
                {
                    mtp.Execute(MtpOperationCode.SetDevicePropValue, new uint[1] { (uint)MtpDevicePropCode.ExposureProgramMode }, BitConverter.GetBytes(((ushort)value)));
                }
            }
        }



        /// <summary>
        /// 露出値
        /// </summary>
        public int Ev
        {
            get
            {
                int val = 0;
                if (IsConnected)
                {
                    MtpResponse res = mtp.Execute(MtpOperationCode.GetDevicePropValue, new uint[1] { (uint)MtpDevicePropCode.ExposureBiasCompensation }, null);
                    val = (int)BitConverter.ToInt16(res.Data, 0);
                }
                return val;
            }

            set
            {
                if (IsConnected)
                {
                    mtp.Execute(MtpOperationCode.SetDevicePropValue, new uint[1] { (uint)MtpDevicePropCode.ExposureBiasCompensation }, BitConverter.GetBytes(((short)value)));
                }
            }
        }

        /// <summary>
        /// 接続状態
        /// </summary>
        public bool IsConnected { get; private set; }

        /// <summary>
        /// ストレージID
        /// </summary>
        private uint storageId = 0;

        /// <summary>
        /// MTPオブジェクト
        /// </summary>
        private MtpCommand mtp;

        /// <summary>
        /// シングルトン
        /// </summary>
        private static Theta instance;

        /// <summary>
        /// シングルトンにする
        /// </summary>
        private Theta()
        {
            IsConnected = false;
            mtp = new MtpCommand();
        }

        /// <summary>
        /// インスタンス取得
        /// </summary>
        /// <returns></returns>
        public static Theta GetInstance()
        {
            if (instance == null)
            {
                instance = new Theta();
            }
            return instance;
        }

        /// <summary>
        /// Thetaに接続する
        /// </summary>
        /// <returns></returns>
        public bool Connect()
        {
            // 接続済みならtrueを返して終了
            if (IsConnected) { return true; }

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
            IsConnected = true;

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
        public void Disconnect()
        {
            // 接続していなければ終了
            if (!IsConnected) { return; }

            // 切断
            mtp.Close();
            IsConnected = false;

            return;
        }

        /// <summary>
        /// レリーズ押下
        /// </summary>
        /// <returns></returns>
        public bool Release()
        {
            if (IsConnected)
            {
                mtp.Execute(MtpOperationCode.InitiateCapture, new uint[2] { 0, 0 }, null);
            }
            return true;
        }
    }
}
