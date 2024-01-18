using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using JustenkaBot;
using JustenkaBot.commands;
using JustenkaBot.Confing;

internal class Program
{
    private static DiscordClient Client { get; set; }
    private static CommandsNextExtension Commands { get; set; }

    static async Task Main(string[] args)
    {
        // Read token and prefix IS JSON 
        var jsonReader = new JSONReader();
        await jsonReader.ReadJSON();

        // Botui butinos komandos 
        var discordConfig = new DiscordConfiguration()
        {
            Intents = DiscordIntents.All,
            Token = jsonReader.token,
            TokenType = TokenType.Bot,
            AutoReconnect = true
        };

        // Discord client instance (butinas)
        Client = new DiscordClient(discordConfig);

        
        Client.Ready += Client_Ready;

        // Del anti-spam
        Client.MessageCreated += OnMessageCreated;

        // Boto permissionai (Nustatyti internto discord paneli)
        var commandsConfig = new CommandsNextConfiguration()
        {
            StringPrefixes = new string[] { jsonReader.prefix },
            EnableMentionPrefix = true,
            EnableDms = true,
            EnableDefaultHelp = true
        };

        Commands = Client.UseCommandsNext(commandsConfig);
        Commands.RegisterCommands<TeamCommands>();

        //  Settina bota, kad perma online butu
        await Client.ConnectAsync();
        await Task.Delay(-1);
    }



    // Consoles langas
    private static Task Client_Ready(DiscordClient sender, DSharpPlus.EventArgs.ReadyEventArgs args)
    {
        Console.WriteLine($"Bot is ready as: {sender.CurrentUser.Username}");
        return Task.CompletedTask;
    }


    private static async Task OnMessageCreated(DiscordClient client, MessageCreateEventArgs e)
    {
        if (e.Author.IsBot)
            return;

        var antispam = new Antispam();
        await Antispam.HandleMessageAsync(client, e);
        await GifHandler.HandleGifAsync(e);
    }


   
}
