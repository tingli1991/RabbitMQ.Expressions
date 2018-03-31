using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Expressions.Configuration;
using RabbitMQ.Expressions.Core;
using System;

namespace RabbitMQ.Expressions.Service
{
    /// <summary>
    /// RabbitMQ服务
    /// </summary>
    public abstract class RpcHandler : BaseHandler
    {
        /// <summary>
        /// 重启服务
        /// </summary>
        public override void ReStart()
        {
            //停止服务
            OnStop();

            //启动服务
            OnStart(ConnectionStrings, ServiceConfiguration);
        }

        /// <summary>
        /// 停止服务
        /// </summary>
        public override void OnStop()
        {
            //退订事件
            _basicConsumer.Received -= BasicConsumer_Received;//接受消息

            //销毁资源
            Dispose();
        }

        /// <summary>
        /// 启动服务
        /// </summary>
        /// <param name="connectionStrings">连接字符串</param>
        /// <param name="serviceConfiguration"></param>
        public override void OnStart(ConnectionStringsElement connectionStrings, ServiceConfigurationElement serviceConfiguration)
        {
            try
            {
                ConnectionStrings = connectionStrings ?? throw new TypeInitializationException("rabbitmqSettings->connectionStrings", null);
                _factory = new ConnectionFactory()
                {
                    Port = connectionStrings.Port,
                    AutomaticRecoveryEnabled = true,
                    HostName = connectionStrings.Host,
                    Password = connectionStrings.Password,
                    UserName = connectionStrings.UserName,
                    RequestedHeartbeat = connectionStrings.TimeOut
                };
                OnStart(serviceConfiguration);
            }
            catch (Exception ex)
            {
                //执行异常处理事件
                OnException(this, ex);
            }
        }

        /// <summary>
        /// 启动服务
        /// </summary>
        /// <param name="serviceConfiguration"></param>
        private void OnStart(ServiceConfigurationElement serviceConfiguration)
        {
            try
            {
                ServiceConfiguration = serviceConfiguration ?? throw new TypeInitializationException("rabbitmqSettings->services", null);
                QueueName = ServiceConfiguration.Name;
                Connection = _factory.CreateConnection();
                Channel = Connection.CreateModel();

                Channel.QueueDeclare(queue: serviceConfiguration.Name, durable: serviceConfiguration.Durable, exclusive: false, autoDelete: false, arguments: null);
                Channel.BasicQos(0, serviceConfiguration.PrefetchCount, false);

                //消息接受事件
                _basicConsumer = new EventingBasicConsumer(Channel);
                Channel.BasicConsume(queue: serviceConfiguration.Name, autoAck: false, consumer: _basicConsumer);
                _basicConsumer.Received += BasicConsumer_Received;
            }
            catch (Exception ex)
            {
                //执行异常处理事件
                OnException(this, ex);
            }
        }

        /// <summary>
        /// 接受
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BasicConsumer_Received(object sender, BasicDeliverEventArgs e)
        {
            try
            {
                var body = e.Body;
                var replyProperties = Channel.CreateBasicProperties();

                var properties = e.BasicProperties;
                replyProperties.CorrelationId = properties.CorrelationId;
                Channel.BasicAck(deliveryTag: e.DeliveryTag, multiple: false);

                var messageBody = body.ToObject();
                var response = Handler(messageBody);
                var responseBytes = response.ToBytes();
                Channel.BasicPublish(exchange: string.Empty, routingKey: properties.ReplyTo, basicProperties: replyProperties, body: responseBytes);
            }
            catch (Exception ex)
            {
                //执行异常处理事件
                OnException(this, ex);
            }
        }

        /// <summary>
        /// 析构函数
        /// </summary>
        ~RpcHandler()
        {
            //释放资源
            Dispose();
        }

        /// <summary>
        /// 销毁函数
        /// </summary>
        public void Dispose()
        {
            if (Channel != null)
            {
                Channel.Dispose();
                Channel.Close();
            }

            if (Connection != null)
            {
                Connection.Dispose();
                Connection.Close();
            }
        }
    }
}