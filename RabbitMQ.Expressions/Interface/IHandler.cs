using RabbitMQ.Expressions.Configuration;
using System;

namespace RabbitMQ.Expressions.Interface
{
    /// <summary>
    /// 异常处理委托
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="ex"></param>
    public delegate void OnExceptionHandler(object sender, Exception ex);

    /// <summary>
    /// 服务端
    /// </summary>
    public abstract class IHandler
    {
        /// <summary>
        /// 停止服务
        /// </summary>
        public abstract void OnStop();

        /// <summary>
        /// 重启服务
        /// </summary>
        public abstract void ReStart();

        /// <summary>
        /// 队列处理方法
        /// </summary>
        /// <param name="messageBody">消息内容</param>
        /// <returns></returns>
        public abstract ResultModel Handler(dynamic messageBody);

        /// <summary>
        /// 启动服务
        /// </summary>
        /// <param name="connectionStrings">连接字符串</param>
        /// <param name="serviceConfiguration"></param>
        public abstract void OnStart(ConnectionStringsElement connectionStrings, ServiceConfigurationElement serviceConfiguration);
    }
}