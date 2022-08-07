[![NuGet](https://img.shields.io/nuget/v/WTelegramClient.Extensions.Updates)](https://www.nuget.org/packages/WTelegramClient.Extensions.Updates)

# WTelegramClient.Extensions.Updates

### Simple Set of Extensions For Easier Update Handling In [WTelegramClient](https://github.com/wiz0u/WTelegramClient/)



### Examples : 

#### <i> Filter For New Messages From Channel : </i>

```csharp
var client = new Client(/*provide the config..*/);

client.RegisterUpdateType<UpdateNewChannelMessage>((channelMessage, updatesBase) =>
{
    Console.WriteLine("Message In : " + channelMessage.message.Date.ToString("F"));
});
```

#### <i> Or Listen For New Message From Channel <b>AND</b> Deleted Messages : </i>


```csharp
client.RegisterUpdateType<UpdateNewChannelMessage, UpdateDeleteChannelMessages>(update, updatesBase) =>
{
    Console.WriteLine("type of new update is" + p.GetType());
});
```

#### <i> You Can Register a Specific Chat Type With Passing  a `Peer` Type : </i>

```csharp

await client.RegisterChatTypeAsync<PeerChannel>((update, channel) =>
{
    Console.WriteLine($"RegisterChatTypeAsync:PeerChannel: {channel.channel_id} with type : {update.GetType()}");
    return Task.CompletedTask;
});

```
#### <i> Also You Can Search For A Specific `ID` Inside Of Upcoming Updates(Including Update Type) : </i>

```csharp
client.RegisterUpdateWithId<UpdateUserTyping>(121212, (update, upBase) =>
{
    Console.WriteLine($"user is typing : {update.user_id}");
});
```

## TO-DO :

<b>
- Do Something About this cases : https://github.com/MrAliSalehi/WTelegramClient.Extensions.Updates/blob/968bbfac38512d901a413b3c88e6e78de01b1f17/Internal/FilterChatTypeExtension.cs#L178

either Remove them And mark As Unsupported Or Handle Them In Some Way ... 

 - Add More Filters To The Library
 
 - Write Some Automated Tests For Filters.
</b>
