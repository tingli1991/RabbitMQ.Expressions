using log4net;
using Newtonsoft.Json;
using RabbitMQ.Expressions;
using RabbitMQ.Expressions.Service;

namespace RabbitMQ.Plugin.SmsHandler
{
    /// <summary>
    /// 短信处理
    /// </summary>
    public class SmsRpcHandler : RpcHandler
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(SmsRpcHandler));

        /// <summary>
        /// 处理方法
        /// </summary>
        /// <param name="messageBody"></param>
        /// <returns></returns>
        public override ResultModel Handler(dynamic messageBody)
        {
            _log.Info($"短信队列消费者执行成功，请求参数：{JsonConvert.SerializeObject(messageBody)}");
            return new ResultModel() { Success = true };
        }
    }
}