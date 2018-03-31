using System;

namespace RabbitMQ.Expressions.Interface
{
    /// <summary>
    /// 客户端
    /// </summary>
    public abstract class IClient
    {
        /// <summary>
        /// 执行方法
        /// </summary>
        /// <param name="messageBody"></param>
        /// <returns></returns>
        public abstract ResultModel Execute<T>(T messageBody);

        /// <summary>
        /// 执行方法
        /// </summary>
        /// <typeparam name="T">请求对象</typeparam>
        /// <typeparam name="R">返回对象</typeparam>
        /// <param name="messageBody">请求内容</param>
        /// <returns></returns>
        public abstract ResultModel<R> Execute<T, R>(T messageBody);
    }
}