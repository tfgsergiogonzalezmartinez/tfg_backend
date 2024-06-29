using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend_tfg.interfaces;
using backend_tfg.repositorios;
using Microsoft.AspNetCore.SignalR;

public class WebChat : Hub
{
    private static ConcurrentDictionary<string, string> UsuariosConectados = new ConcurrentDictionary<string, string>();
    private readonly IChatRepositorio _chatRepositorio;

    public WebChat(IChatRepositorio chatRepositorio){
        _chatRepositorio = chatRepositorio;

    }

     public override async Task OnConnectedAsync()
    {
        var userName = Context.GetHttpContext().Request.Query["user"].ToString();
        var estado = UsuariosConectados.AddOrUpdate(userName, Context.ConnectionId, (key, oldValue) => Context.ConnectionId);
        await Clients.All.SendAsync("UserConnected", userName);
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        UsuariosConectados.TryRemove(Context.ConnectionId, out var userName);

        await Clients.All.SendAsync("UserDisconnected", userName);
        await base.OnDisconnectedAsync(exception);
    }

    public async Task JoinGroup(string groupName, string userName)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
    }

    public async Task LeaveGroup(string groupName, string userName)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
    }

    public async Task SendMessage(NewMessage message)
    {

        await SendDirectMessage(message.destinatario, message);
    }
    public async Task SendDirectMessage(string userId, NewMessage message)
    {
        if (UsuariosConectados.TryGetValue(userId, out var connectionId))
        {
            await Clients.Client(connectionId).SendAsync("mensajePrivado", message);
            var chat = await this._chatRepositorio.postMessageUsers(message);
        }
    }
}

public record NewMessage(string usuario, string mensaje, string destinatario);
