using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Expressions.Core;
using System;

namespace RabbitMQ.Expressions.Client
{
    /// <summary>
    /// Rpc客户端
    /// </summary>
    public class RpcClient : BaseClient
    {
        /// <summary>
        /// 执行方法
        /// </summary>
        /// <param name="messageBody"></param>
        /// <returns></returns>
        public override ResultModel Execute<T>(T messageBody)
        {
            var result = new ResultModel();
            try
            {
                Execute(messageBody, eventArgs =>
                {
                    var bodyBytes = eventArgs.Body;
                    result = bodyBytes.ToObject<ResultModel>();
                });
            }
            catch (Exception ex)
            {
                OnException(this, ex);
            }
            return result;
        }

        /// <summary>
        /// 执行方法
        /// </summary>
        /// <typeparam name="R"></typeparam>
        /// <param name="messageBody"></param>
        /// <returns></returns>
        public override ResultModel<R> Execute<T, R>(T messageBody)
        {
            var result = new ResultModel<R>();
            try
            {
                Execute(messageBody, eventArgs =>
                {
                    var bodyBytes = eventArgs.Body;
                    result = bodyBytes.ToObject<ResultModel<R>>();
                });
            }
            catch (Exception ex)
            {
                OnException(this, ex);
            }
            return result;
        }

        /// <summary>
        /// 执行方法
        /// </summary>
        /// <typeparam name="R"></typeparam>
        /// <param name="messageBody"></param>
        /// <param name="callBack">请求回调处理方法</param>
        /// <returns></returns>
        private void Execute<T>(T messageBody, Action<BasicDeliverEventArgs> callBack)
        {
            try
            {
                //创建连接、通道
                Connection = _factory.CreateConnection();
                Channel = Connection.CreateModel();

                var replyQueue = Channel.QueueDeclare();
                var consumer = new QueueingBasicConsumer(Channel);
                Channel.BasicConsume(queue: replyQueue.QueueName, autoAck: true, consumer: consumer);

                var correlationId = $"{Guid.NewGuid()}".Replace("-", "");
                var properties = Channel.CreateBasicProperties();
                properties.ReplyTo = replyQueue.QueueName;
                properties.CorrelationId = correlationId;
                properties.DeliveryMode = ClientConfiguration.Durable ? (byte)2 : (byte)1;

                var messageBodyButys = messageBody.ToBytes();
                Channel.BasicPublish(exchange: string.Empty, routingKey: QueueName, basicProperties: properties, body: messageBodyButys);
                DateTime date = DateTime.Now;
                var millisecondsTimeout = ConnectionStrings.TimeOut * 1000;//超时毫秒数
                if (consumer.Queue.Dequeue(millisecondsTimeout, out BasicDeliverEventArgs eventArgs))
                {
                    if (eventArgs.BasicProperties.CorrelationId == correlationId)
                    {
                        callBack(eventArgs);
                        return;
                    }
                }

                var diff = DateTime.Now - date;
                if (eventArgs == null || diff.TotalMilliseconds > millisecondsTimeout)
                {
                    OnException(this, new Exception($"队列：{QueueName},等待：{diff.TotalMilliseconds}毫秒后超时！！！"));
                    return;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //释放资源
                Dispose();
            }
        }

        /// <summary>
        /// 析构函数
        /// </summary>
        ~RpcClient()
        {
            //清空资源
            Clear();
        }
    }
}