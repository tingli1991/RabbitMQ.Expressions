using RabbitMQ.Expressions.Configuration;
using System.Configuration;

namespace RabbitMQ.Expressions
{
    /// <summary>
    /// RabbitMQ自定义配置根节点
    /// </summary>
    public class RabbitMQConfigurationSection : ConfigurationSection
    {
        /// <summary>
        /// 基础连接字符串
        /// </summary>
        [ConfigurationProperty("connectionStrings")]
        public ConnectionStringsElement ConnectionStrings
        {
            get => (ConnectionStringsElement)base["connectionStrings"];
        }

        /// <summary>
        /// 服务端配置列表
        /// </summary>
        [ConfigurationProperty("services", IsDefaultCollection = true)]
        public ServiceConfigurationElementCollection Services
        {
            get => (ServiceConfigurationElementCollection)base["services"];
        }

        /// <summary>
        /// 客户端配置列表
        /// </summary>
        [ConfigurationProperty("clients", IsDefaultCollection = true)]
        public ClientConfigurationElementCollection Clients
        {
            get => (ClientConfigurationElementCollection)base["clients"];
        }
    }
}