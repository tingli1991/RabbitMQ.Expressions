using log4net;
using Newtonsoft.Json;
using RabbitMQ.Expressions;
using RabbitMQ.Expressions.Service;

namespace RabbitMQ.Plugin.InvestHandler
{
    /// <summary>
    /// 投资队列服务
    /// </summary>
    public class InvestRpcHandler : RpcHandler
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(InvestRpcHandler));

        /// <summary>
        /// 处理方法
        /// </summary>
        /// <param name="messageBody"></param>
        /// <returns></returns>
        public override ResultModel Handler(dynamic messageBody)
        {
            _log.Info($"投资队列消费者执行成功，请求参数：{JsonConvert.SerializeObject(messageBody)}");
            return new ResultModel<object>() { Success = true, Data = new { NO = "0001" } };
        }
    }
}