# WTelegramClient.Extensions.Updates

[![NuGet](https://img.shields.io/nuget/v/WTelegramClient.Extensions.Updates)](https://www.nuget.org/packages/WTelegramClient.Extensions.Updates)
![Nuget](https://img.shields.io/nuget/dt/WTelegramClient.Extensions.Updates)


Simple Set of Extensions For Easier Update Handling In [WTelegramClient](https://github.com/wiz0u/WTelegramClient/)


# Install

dotnet cli: `dotnet add package WTelegramClient.Extensions.Updates --version 1.1.0`

nuget: `NuGet\Install-Package WTelegramClient.Extensions.Updates -Version 1.1.0`

reference: `<PackageReference Include="WTelegramClient.Extensions.Updates" Version="1.1.0" />`

### Examples : 

#### _Filter For New Messages From Channel :_

```csharp
var client = new Client(/*provide the config..*/);

await client.LoginUserIfNeeded();

client.RegisterUpdateType<UpdateNewChannelMessage>((channelMessage, updatesBase) =>
{
    Console.WriteLine("Message In : " + channelMessage.message.Date.ToString("F"));
});
```

#### _Or Listen For New Message From Channel <b>AND</b> Deleted Messages :_


```csharp
client.RegisterUpdateType<UpdateNewChannelMessage, UpdateDeleteChannelMessages>(update, updatesBase) =>
{
    Console.WriteLine("type of new update is" + p.GetType());
});
```

#### _You Can Register a Specific Chat Type With Passing  a `Peer` Type :_

```csharp

await client.RegisterChatTypeAsync<PeerChannel>((update, channel) =>
{
    Console.WriteLine($"RegisterChatTypeAsync:PeerChannel: {channel.channel_id} with type : {update.GetType()}");
    return Task.CompletedTask;
});

```
#### _Also You Can Search For A Specific `ID` Inside Of Upcoming Updates(Including Update Type) :_

```csharp
client.RegisterUpdateWithId<UpdateUserTyping>(121212, (update, upBase) =>
{
    Console.WriteLine($"user is typing : {update.user_id}");
});
```

### Blocking

currently all of the mentioned methods will NOT block the current thread, so they has to be called at the `startup` of your application and then the flow of your program should be blocked somewhere else.

using the `*Block` overloads of these methods you can block your current thread:

```csharp
// this is NOT going to block the current thread
client.RegisterUpdateType<UpdateNewChannelMessage>(async (messages, upBase) =>
{
    await Task.Delay(2, cancellationToken);
    Console.WriteLine($"new message");
});

//this WILL block the thread so the code will not continue after this point until the cancelation token is requested
client.RegisterUpdateTypeBlocking<UpdateNewChannelMessage>(async (messages, upBase) =>
{
    await Task.Delay(2, cancellationToken);
    Console.WriteLine($"new message");
}, cancellationToken); // the token is also optional, you can skip it
```
