using Microsoft.VisualStudio.TestTools.UnitTesting;
using RabbitMQ.Expressions.Client;
using System.Diagnostics;

namespace RabbitMQ.Expressions.UnitTest
{
    /// <summary>
    /// 
    /// </summary>
    [TestClass]
    public class RabbitMQSectionTest
    {
        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void ClientTest()
        {
            using (var client = new RpcClient())
            {
                var messageBody = new { Model = 1 };
                var response = client.Execute(messageBody);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void ClientFromDataTest()
        {
            using (var client = new RpcClient())
            {
                var messageBody = new { Model = 1 };
                var response = client.Execute<object, object>(messageBody);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void BatchClientTest()
        {
            Stopwatch getTime = new Stopwatch();
            getTime.Start();
            using (var client = new RpcClient())
            {
                client.Exception += Client_Exception;
                for (int i = 0; i < 100000; i++)
                {
                    var messageBody = new { Model = 1 };
                    var response = client.Execute(messageBody);
                }
            }
            getTime.Stop();
            var totalSeconds = getTime.ElapsedMilliseconds;
        }

        private void Client_Exception(object sender, System.Exception ex)
        {

        }
    }
}