namespace RabbitMQ.Expressions
{
    /// <summary>
    /// 返回基础数据
    /// </summary>
    public class ResultModel
    {
        /// <summary>
        /// 业务处理结果
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 错误码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 错误消息
        /// </summary>
        public string Message { get; set; }
    }

    /// <summary>
    /// 返回基础数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ResultModel<T> : ResultModel
    {
        /// <summary>
        /// 业务数据
        /// </summary>
        public T Data { get; set; }
    }
}