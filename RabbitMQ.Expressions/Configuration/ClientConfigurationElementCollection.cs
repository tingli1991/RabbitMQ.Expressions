using System.Configuration;

namespace RabbitMQ.Expressions.Configuration
{
    /// <summary>
    /// 队列配置集合(服务端)
    /// </summary>
    [ConfigurationCollection(typeof(ClientConfigurationElementCollection), AddItemName = "client")]
    public class ClientConfigurationElementCollection : ConfigurationElementCollection
    {
        /// <summary>
        /// 创建新的队列配置节点
        /// </summary>
        /// <returns></returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new ClientConfigurationElement();
        }

        /// <summary>
        /// 获取队列配置Key
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ClientConfigurationElement)element).Name;
        }

        /// <summary>
        /// 根据配置key索引相应配置
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public new ClientConfigurationElement this[string name]
        {
            get
            {
                return (ClientConfigurationElement)BaseGet(name);
            }
        }
    }
}