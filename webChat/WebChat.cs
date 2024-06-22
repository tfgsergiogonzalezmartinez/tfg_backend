using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

public class WebChat : Hub
{
    public async Task JoinGroup(string groupName, string userName)
    {
        Console.WriteLine($"JoinGroup called with groupName: {groupName}, userName: {userName}");
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        await Clients.Group(groupName).SendAsync("NewUser", $"{userName} entró al canal");
    }

    public async Task LeaveGroup(string groupName, string userName)
    {
        Console.WriteLine($"LeaveGroup called with groupName: {groupName}, userName: {userName}");
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        await Clients.Group(groupName).SendAsync("LeftUser", $"{userName} salió del canal");
    }

    public async Task SendMessage(NewMessage message)
    {
        Console.WriteLine($"SendMessage called with message: {message.Mensaje}, userName: {message.Usuario}, groupName: {message.Grupo}");
        await Clients.Group(message.Grupo).SendAsync("NewMessage", message);
    }
}

public record NewMessage(string Usuario, string Mensaje, string Grupo);

