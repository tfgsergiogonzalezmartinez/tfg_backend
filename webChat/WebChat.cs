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
        var estado = UsuariosConectados.AddOrUpdate(userName, Context.ConnectionId, (key, oldValue) => Context.ConnectionId);        await Clients.All.SendAsync("UserConnected", userName);
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        // Remover el usuario de la lista de conectados
        UsuariosConectados.TryRemove(Context.ConnectionId, out var userName);

        await Clients.All.SendAsync("UserDisconnected", userName);
        await base.OnDisconnectedAsync(exception);
    }

    public async Task JoinGroup(string groupName, string userName)
    {
        Console.WriteLine($"JoinGroup called with groupName: {groupName}, userName: {userName}");
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        // await Clients.Group(groupName).SendAsync("NewUser", $"{userName} entró al canal");
    }

    public async Task LeaveGroup(string groupName, string userName)
    {
        Console.WriteLine($"LeaveGroup called with groupName: {groupName}, userName: {userName}");
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        // await Clients.Group(groupName).SendAsync("LeftUser", $"{userName} salió del canal");
    }

    public async Task SendMessage(NewMessage message)
    {
        Console.WriteLine($"SendMessage called with message: {message.mensaje}, userName: {message.usuario}, groupName: {message.grupo}");
        await Clients.Group(message.grupo).SendAsync("NewMessage", message);

        await SendDirectMessage(message.destinatario, message);
        // Notificar al destinatario del mensaje sobre el grupo de chat privado
        // if (!string.IsNullOrEmpty(message.destinatario))
        // {
        //     await Clients.User(message.destinatario).SendAsync("JoinGroup", message.Grupo);
        // }
    }
        // Nuevo método para enviar un mensaje directamente a un usuario
    public async Task SendDirectMessage(string userId, NewMessage message)
    {
        if (UsuariosConectados.TryGetValue(userId, out var connectionId))
        {
            await Clients.Client(connectionId).SendAsync("mensajePrivado", message);
        }
    }
}

public record NewMessage(string usuario, string mensaje, string grupo, string destinatario);
