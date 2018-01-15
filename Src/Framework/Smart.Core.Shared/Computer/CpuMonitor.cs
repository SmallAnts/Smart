using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Smart.Core.Computer
{
    /// <summary>
    /// 
    /// </summary>
    public class CpuMonitor : Monitor
    {
        private PerformanceCounter _cpuPerformanceCounter;   //CPU计数器
        /// <summary>
        /// 
        /// </summary>
        public float CpuUsage
        {
            get
            {
                return _cpuPerformanceCounter != null ? _cpuPerformanceCounter.NextValue() : 0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="interval"></param>
        public CpuMonitor(double interval = 1000) : base(interval)
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnElapsed(object sender, ElapsedEventArgs e)
        {
            _cpuPerformanceCounter.NextValue();
        }
        /// <summary>
        /// 
        /// </summary>
        public override void Start()
        {
            //初始化CPU计数器
            _cpuPerformanceCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            _cpuPerformanceCounter.MachineName = ".";
            _cpuPerformanceCounter.NextValue();
            base.Start();
        }
    }
}
