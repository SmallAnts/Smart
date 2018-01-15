namespace Smart.Core.Infrastructure
{
    /// <summary>
    /// 应在启动时执行任务的接口
    /// </summary>
    public interface IStartupTask 
    {
        /// <summary>
        /// 执行启动任务
        /// </summary>
        void Execute();

        /// <summary>
        /// 获取执行顺序
        /// </summary>
        int Order { get; }
    }
}
