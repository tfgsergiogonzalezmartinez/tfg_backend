using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using backend_tfg.interfaces;
using backend_tfg.repositorios;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver.Core.Authentication;

public class WebChat : Hub
{
    private static ConcurrentDictionary<string, string> UsuariosConectados = new ConcurrentDictionary<string, string>();
    private readonly IChatRepositorio _chatRepositorio;
    private IConfiguration _config;

    public WebChat(IChatRepositorio chatRepositorio, IConfiguration config)
    {
        _chatRepositorio = chatRepositorio;
        _config = config;

    }

    public override async Task OnConnectedAsync()
    {
        var userName = Context.GetHttpContext().Request.Query["user"].ToString();
        var token = Context.GetHttpContext().Request.Query["token"].ToString();
        if (userName is not null && token is not null && ValidateToken(token))
        {
            var estado = UsuariosConectados.AddOrUpdate(userName, Context.ConnectionId, (key, oldValue) => Context.ConnectionId); await Clients.All.SendAsync("UserConnected", userName);
            await base.OnConnectedAsync();
        }
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        // Remover el usuario de la lista de conectados
        UsuariosConectados.TryRemove(Context.ConnectionId, out var userName);

        await Clients.All.SendAsync("UserDisconnected", userName);
        await base.OnDisconnectedAsync(exception);
    }

    public async Task onEnviarMensajeDirecto(NewMessage message)
    {
        if (UsuariosConectados.TryGetValue(message.destinatario, out var connectionId))
        {
            await Clients.Client(connectionId).SendAsync("mensajePrivado", message);
            var chat = await this._chatRepositorio.postMessageUsers(message);
        }
    }

    private bool ValidateToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_config["JWT_SECRET"]);

        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _config["JWT_ISSUER"],
                ValidAudience = _config["JWT_AUDIENCE"],
                IssuerSigningKey = new SymmetricSecurityKey(key)
            }, out SecurityToken validatedToken);

            return true;
        }
        catch
        {
            // Token inválido
            return false;
        }
    }
}

public record NewMessage(string usuario, string mensaje, string destinatario);
