using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Smart.Core.Computer
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class Monitor
    {
        private Timer _monitorTimer;    // 定时器

        /// <summary>
        /// 
        /// </summary>
        public bool IsRuning { get; private set; }

        /// <summary>
        /// 定时器刷新间隔时间（单位：毫秒），默认值1000
        /// </summary>
        public double Interval
        {
            get { return _monitorTimer.Interval; }
            set { _monitorTimer.Interval = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="interval"></param>
        public Monitor(double interval = 1000)
        {
            _monitorTimer = new Timer(interval);
            _monitorTimer.Elapsed += OnElapsed;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnElapsed(object sender, ElapsedEventArgs e)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Start()
        {
            _monitorTimer.Enabled = true;
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Stop()
        {
            _monitorTimer.Enabled = false;
        }
    }
}
