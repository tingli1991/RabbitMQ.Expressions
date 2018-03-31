using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Expressions.Configuration;
using RabbitMQ.Expressions.Interface;
using System;

namespace RabbitMQ.Expressions.Service
{
    /// <summary>
    /// 基础处理业务
    /// </summary>
    public abstract class BaseHandler : IHandler
    {
        /// <summary>
        /// 连接工厂
        /// </summary>
        protected ConnectionFactory _factory;

        /// <summary>
        /// 消息返回客户端
        /// </summary>  
        protected EventingBasicConsumer _basicConsumer;

        /// <summary>
        /// 队列名称
        /// </summary>
        public string QueueName { get; set; }

        /// <summary>
        /// 消息连接通道
        /// </summary>
        public IModel Channel { get; set; }

        /// <summary>
        /// 消息连接
        /// </summary>
        public IConnection Connection { get; set; }

        /// <summary>
        /// 异常处理事件
        /// </summary>
        public event OnExceptionHandler Exception;

        /// <summary>
        /// 基础连接字符串
        /// </summary>
        public ConnectionStringsElement ConnectionStrings { get; set; }

        /// <summary>
        /// 服务端配置列表
        /// </summary>
        public ServiceConfigurationElement ServiceConfiguration { get; set; }
        
        /// <summary>
        /// 异常处理类
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="ex"></param>
        public virtual void OnException(object sender, Exception ex)
        {
            if (Exception != null)
                Exception(sender, ex);
        }
    }
}