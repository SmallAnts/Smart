using System.Collections.Generic;
using System.Diagnostics;
using System.Timers;

namespace Smart.Core.Computer
{
    /// <summary>
    /// 网络流量监控
    /// <para>var monitor = new MyNetWorkMonitor();</para>
    /// <para>monitor.Start();</para>
    /// </summary>
    public class NetWorkMonitor : Monitor
    {
        private List<NetWorkAdapter> _adaptersList;    // 该计算机的网络适配器列表。
        private List<NetWorkAdapter> _monitoredAdapters;   // 目前控制的网络适配器列表

        /// <summary>
        /// 获取所有的网络适配器列表
        /// </summary>
        public NetWorkAdapter[] Adapters
        {
            get
            {
                return _adaptersList.ToArray();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public NetWorkMonitor(double interval = 1000) : base(interval)
        {
            _adaptersList = new List<NetWorkAdapter>();               //用来保存获取到的计算机的网络适配器列表
            _monitoredAdapters = new List<NetWorkAdapter>();   //运行的有效的网络适配器列表
            GetNetAdapter();                                //列举出安装在该计算机上面的网络适配器
        }

        /// <summary>
        /// 定时刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnElapsed(object sender, ElapsedEventArgs e)
        {
            foreach (var adapter in _monitoredAdapters) //每秒钟遍历有效的网络适配器
            {
                adapter.CaculateAndRefresh();   //刷新上传下载速度                        
            }
        }

        //获取该计算机上的网络适配器
        private void GetNetAdapter()
        {
            var networkInterface = new PerformanceCounterCategory("Network Interface");
            foreach (string instanceName in networkInterface.GetInstanceNames())
            {
                if (instanceName == "MS TCP Loopback interface") continue;
                var netWorkAdapter = new NetWorkAdapter(instanceName);
                _adaptersList.Add(netWorkAdapter);
            }
        }

        /// <summary>
        /// 启动监控
        /// </summary>
        public override void Start()
        {
            if (_adaptersList.Count > 0)
            {
                foreach (var adapter in _adaptersList)
                {
                    if (!_monitoredAdapters.Contains(adapter))
                    {
                        _monitoredAdapters.Add(adapter);
                        adapter.Start();
                    }
                }
                base.Start();
            }
        }

        /// <summary>
        /// 启动监控指定网络适配器
        /// </summary>
        /// <param name="adapter"></param>
        public void Start(NetWorkAdapter adapter)
        {
            if (!_monitoredAdapters.Contains(adapter))
            {
                _monitoredAdapters.Add(adapter);
                adapter.Start(); //该适配器调用自己函数开始工作      
            }
            base.Start();
        }

        /// <summary>
        /// 停止监控
        /// </summary>
        public override void Stop()
        {
            _monitoredAdapters.Clear();
            base.Stop();
        }

        /// <summary>
        /// 停止监控指定网络适配器
        /// </summary>
        /// <param name="adapter"></param>
        public void Stop(NetWorkAdapter adapter)
        {
            if (_monitoredAdapters.Contains(adapter))
                _monitoredAdapters.Remove(adapter);
            if (_monitoredAdapters.Count == 0)
                base.Stop();
        }

    }

    /// <summary>
    /// 一个安装在计算机上的网络适配器，该类可用于获取网络中的流量
    /// </summary>
    public class NetWorkAdapter
    {
        internal PerformanceCounter _performanceDown;    //控制下载速度的流量计数器
        internal PerformanceCounter _performanceUp;        //控制上传速度的流量计数器
        private long _downLoadNetValues1;        //当前的下载速度,字节计算
        private long _downLoadNetValues2;        //一秒前的下载速度,字节计算
        private long _uploadNetValues1;             //当前的上传速度
        private long _uploadNetValues2;             //一秒前的上传速度

        /// <summary>
        /// 此适配器的名字
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// 每秒钟下载速度
        /// </summary>
        public long DownloadSpeed { get; private set; }
        /// <summary>
        /// 每秒钟上传速度
        /// </summary>
        public long UploadSpeed { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public double DownloadSpeedKbps
        {
            get
            {
                return this.DownloadSpeed / 1024.0;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public double UploadSpeedKbps
        {
            get
            {
                return this.UploadSpeed / 1024.0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="computerNetName"></param>
        public NetWorkAdapter(string computerNetName)
        {
            Name = computerNetName;
            // 创建性能计数器。
            _performanceDown = new PerformanceCounter("Network Interface", "Bytes Received/sec", computerNetName);
            _performanceUp = new PerformanceCounter("Network Interface", "Bytes Sent/sec", computerNetName);
            _downLoadNetValues1 = 0;
            _uploadNetValues1 = 0;
            _downLoadNetValues2 = 0;
            _uploadNetValues2 = 0;
        }

        /// <summary>
        /// 该适配器准备控制的方法函数
        /// </summary>
        public void Start()
        {
            _uploadNetValues1 = _performanceUp.NextSample().RawValue;
            _downLoadNetValues1 = _performanceDown.NextSample().RawValue;
        }

        /// <summary>
        /// 计算速度
        /// </summary>
        public void CaculateAndRefresh()
        {
            _downLoadNetValues2 = _performanceDown.NextSample().RawValue;
            _uploadNetValues2 = _performanceUp.NextSample().RawValue;

            DownloadSpeed = _downLoadNetValues2 - _downLoadNetValues1;
            UploadSpeed = _uploadNetValues2 - _uploadNetValues1;

            _downLoadNetValues1 = _downLoadNetValues2;
            _uploadNetValues1 = _uploadNetValues2;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Name;
        }
    }
}
