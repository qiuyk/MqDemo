namespace MqSdk.Entity
{
    /// <summary>
    /// * 表示匹配一个单词
    /// # 表示匹配 0个或多个单词
    /// </summary>
    public enum MqEnum
    {
        /// <summary>
        /// 广播模式
        /// 忽略路由键，发送给所有与之绑定的队列或交换机
        /// </summary>
        Fanout,
        /// <summary>
        /// 按规则通知
        /// 绑定的路由键可以使用统配符, 规则: 必须由(.)分割单词列表。
        /// </summary>
        Topic,
        /// <summary>
        /// 单独通知
        /// 投递消息时路由键完全匹配才能正常投递消息
        /// </summary>
        Direct
    }
}
