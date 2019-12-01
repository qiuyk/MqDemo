# MqSdk 文档

> 对接方式

- 引入 MqSdk.dll
- 修改配置文件 mqconfig.xml

> 1 对 1

- 发送消息
```
MqBuilder.CreateBuilder()
	 .withType(MqEnum.Direct)
	 .withReceiver("1111111")
	 .withMessage(new MqMessage
	 {
	     SenderID = "XXXXXXX",
	     MessageID = Guid.NewGuid().ToString("N"),
	     MessageBody = "系统将夜晚0点不停服更新",
	     MessageTitle = "系统提醒",
	 })
	 .SendMessage();
```
- 监听消息
```
MqBuilder.CreateBuilder()
         .withType(MqEnum.Direct)
         .withReceiver("1111111")
         .withListening(MqMessage_Received_Received)
         .Listening();

private static void MqMessage_Received_Received(object sender, MqMessage e)
{
    //处理消息
}
```

> 1 对 组

- 发送消息
```
MqBuilder.CreateBuilder()
         .withType(MqEnum.Topic)
         .withRole("soft")
         .withMessage(new MqMessage
         {
             SenderID = "Boss",
             MessageID = Guid.NewGuid().ToString("N"),
             MessageBody = "软件部门所有成员下班厕所见",
             MessageTitle = "老板来信",
         })
         .SendMessage();
```
- 监听消息
```
MqBuilder.CreateBuilder()
         .withType(MqEnum.Topic)
         .withRole("soft")
         .withReceiver("1111111")
         .withListening(MqMessage_Received_Received)
         .Listening();

private static void MqMessage_Received_Received(object sender, MqMessage e)
{
    //处理消息
}
```

> 1 对 All

- 发送消息
```
MqBuilder.CreateBuilder()
         .withType(MqEnum.Fanout)
         .withMessage(new MqMessage
         {
             SenderID = "System",
             MessageID = Guid.NewGuid().ToString("N"),
             MessageBody = "系统将夜晚0点不停服更新",
             MessageTitle = "系统提醒",
         })
         .SendMessage();
```
- 监听消息
```
MqBuilder.CreateBuilder()
         .withType(MqEnum.Fanout)
         .withReceiver("1111111")
         .withListening(MqMessage_Received_Received)
         .Listening();

private static void MqMessage_Received_Received(object sender, MqMessage e)
{
    //处理消息
}
```

> 异步

- 异步发送
```
 MqBuilder.CreateBuilder()
          .withType(MqEnum.Topic)
          .withMessage(new MqMessage
          {
              SenderID = "Boss",
              MessageID = Guid.NewGuid().ToString("N"),
              MessageBody = "软件部门所有成员下班厕所见",
              MessageTitle = "老板来信",
          })
          .withAsyncException(MqException_Received)
          .SendMessageAsync();
```

- 异步监听

```
MqBuilder.CreateBuilder()
         .withType(MqEnum.Topic)
         .withListening(MqMessage_Received)
         .withAsyncException(MqException_Received)
         .SendMessageAsync();
```

- 异步异常处理

```
private static void MqException_Received(object sender, Exception e)
{
    //测方法为异步返回，请注意处理方式
}
```