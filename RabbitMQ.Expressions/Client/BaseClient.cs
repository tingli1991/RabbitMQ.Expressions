using RabbitMQ.Client;
using RabbitMQ.Expressions.Configuration;
using RabbitMQ.Expressions.Interface;
using System;
using System.Configuration;

namespace RabbitMQ.Expressions.Client
{
    /// <summary>
    /// Rpc-Client
    /// </summary>
    public abstract class BaseClient : IClient, IDisposable
    {
        /// <summary>
        /// 连接工厂
        /// </summary>
        protected ConnectionFactory _factory;

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
        /// 客户端端配置列表
        /// </summary>
        public ClientConfigurationElement ClientConfiguration { get; set; }

        /// <summary>
        /// 对象锁
        /// </summary>
        protected static object _lock = new object();

        /// <summary>
        /// 自定义配置文件节点名称
        /// </summary>
        protected const string SECTION_NAME = "rabbitmqSettings";

        /// <summary>
        /// 配置信息
        /// </summary>
        protected static readonly RabbitMQConfigurationSection configSection = (RabbitMQConfigurationSection)ConfigurationManager.GetSection(SECTION_NAME);

        /// <summary>
        /// 异常处理类
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="ex"></param>
        public virtual void OnException(object sender, Exception ex)
        {
            Exception?.Invoke(sender, ex);
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public BaseClient()
        {
            try
            {
                if (configSection == null)
                {
                    throw new TypeInitializationException("rabbitmqSettings", null);
                }

                if (configSection.Clients == null || configSection.Clients.Count <= 0)
                {
                    throw new TypeInitializationException("rabbitmqSettings->clients", null);
                }

                //绑定连接字符串
                ConnectionStrings = configSection.ConnectionStrings ?? throw new TypeInitializationException("rabbitmqSettings->connectionStrings", null);

                //绑定队列名称
                var type = this.GetType();
                ClientConfigurationElement clientConfiguration = null;
                foreach (ClientConfigurationElement client in configSection.Clients)
                {
                    if (client.NameSpace.Equals(type.Namespace) && client.ClassName.Equals(type.Name))
                    {
                        clientConfiguration = client;
                        break;
                    }
                }
                ClientConfiguration = clientConfiguration ?? throw new TypeInitializationException("rabbitmqSettings->clients", null);
                QueueName = ClientConfiguration.Name;
                if (_factory == null)
                {
                    lock (_lock)
                    {
                        if (_factory == null)
                        {
                            _factory = new ConnectionFactory()
                            {
                                AutomaticRecoveryEnabled = true,
                                Port = ConnectionStrings.Port,
                                HostName = ConnectionStrings.Host,
                                Password = ConnectionStrings.Password,
                                UserName = ConnectionStrings.UserName,
                                RequestedHeartbeat = ConnectionStrings.TimeOut
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                OnException(this, ex);
            }
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (Channel != null)
            {
                Channel.Dispose();
                Channel.Close();
                Channel = null;
            }

            if (Connection != null)
            {
                Connection.Dispose();
                Connection.Close();
                Connection = null;
            }
        }

        /// <summary>
        /// 清空资源
        /// </summary>
        public void Clear()
        {
            Dispose();
            _factory = null;
            QueueName = null;
            ConnectionStrings = null;
            ClientConfiguration = null;
        }
    }
}