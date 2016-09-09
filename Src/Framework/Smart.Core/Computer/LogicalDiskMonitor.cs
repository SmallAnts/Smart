using Smart.Core.Extensions;
using System.Collections.Generic;
using System.IO;
using System.Management;
using System.Timers;

namespace Smart.Core.Computer
{
    /// <summary>
    /// 
    /// </summary>
    public class LogicalDiskMonitor : Monitor
    {
        private const long MB = 1048576; //  1024 * 1024 = 1048576

        /// <summary>
        /// 
        /// </summary>
        public DriveInfo[] LogicalDisks { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public LogicalDiskMonitor(double interval = 1000) : base(interval)
        {
            LogicalDisks = new DriveInfo[] { };

            #region Win32_LogicalDisk
            //var diskClass = new ManagementClass("Win32_LogicalDisk");
            //var disks = diskClass.GetInstances();
            //foreach (var disk in disks)
            //{
            //    var diskInfo = new LogicalDiskInfo();
            //    diskInfo.Name = disk["Name"].ToString();
            //    diskInfo.Description = disk["Description"].ToString();
            //    if (disk["Size"].ToString().AsLong() > 0)
            //    {
            //        diskInfo.TotalSpace = disk["Size"].ToString().AsLong() / MB;
            //        diskInfo.FreeSpace = disk["FreeSpace"].ToString().AsLong() / MB;
            //    }
            //}
            #endregion
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnElapsed(object sender, ElapsedEventArgs e)
        {
            LogicalDisks = DriveInfo.GetDrives();
        }
    }

    //public class LogicalDiskInfo
    //{
    //    /// <summary>
    //    /// 磁盘名称
    //    /// </summary>
    //    public string Name { get; set; }
    //    /// <summary>
    //    /// 磁盘描述
    //    /// </summary>
    //    public string Description { get; set; }
    //    /// <summary>
    //    /// 磁盘总容量
    //    /// </summary>
    //    public long TotalSpace { get; set; }
    //    /// <summary>
    //    /// 磁盘可用空间
    //    /// </summary>
    //    public long FreeSpace { get; set; }
    //    /// <summary>
    //    /// 磁盘已用空间
    //    /// </summary>
    //    public long UsedSpace { get { return TotalSpace - FreeSpace; } }
    //}
}

