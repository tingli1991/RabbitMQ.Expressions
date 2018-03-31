using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Expressions.ConsoleTest
{
    class Program
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(Program));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            _log.Info("服务启动");
            var section = ConfigurationManager.GetSection("rabbitmqSettings");
            HandlerManager.Instance.OnStart();
            _log.Info("服务启动完成");
            Console.ReadLine();
        }
    }
}
