using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Timers;

namespace Smart.Core.Computer
{
    /// <summary>
    /// 
    /// </summary>
    public class MemoryMonitor : Monitor
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="interval"></param>
        public MemoryMonitor(double interval) : base(interval)
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnElapsed(object sender, ElapsedEventArgs e)
        {
            base.OnElapsed(sender, e);
        }

        /// <summary>
        /// 获得物理内存
        /// </summary>
        /// <returns></returns>
        private long GetPhysicalMemory()
        {
            var mc = new ManagementClass("Win32_ComputerSystem");
            var moc = mc.GetInstances();
            foreach (var mo in moc)
            {
                if (mo["TotalPhysicalMemory"] != null)
                {
                    return long.Parse(mo["TotalPhysicalMemory"].ToString());
                }
            }
            return 0;
        }

        /// <summary>
        /// 获取可用内存
        /// </summary>
        /// <returns></returns>
        private long GetAvailableMemory()
        {
            long availablebytes = 0;
            //ManagementObjectSearcher mos = new ManagementObjectSearcher("SELECT * FROM Win32_PerfRawData_PerfOS_Memory");
            //foreach (ManagementObject mo in mos.Get())
            //{
            //    availablebytes = long.Parse(mo["Availablebytes"].ToString());
            //}
            var mos = new ManagementClass("Win32_OperatingSystem");
            foreach (var mo in mos.GetInstances())
            {
                if (mo["FreePhysicalMemory"] != null)
                {
                    availablebytes = 1024 * long.Parse(mo["FreePhysicalMemory"].ToString());
                    return availablebytes;
                }
            }
            return availablebytes;
        }
    }
}
