using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using JustenkaBot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JustenkaBot.commands
{

    public class TeamCommands : BaseCommandModule
    {
        private Dictionary<ulong, List<string>> teamMembers = new Dictionary<ulong, List<string>>();

        [Command("addmembers")]
        public async Task AddMembers(CommandContext ctx, params string[] memberNames)
        {
            
            if (!teamMembers.TryGetValue(ctx.Guild.Id, out var members))
            {
                members = new List<string>();
                teamMembers[ctx.Guild.Id] = members;
            }

            members.AddRange(memberNames);

            await ctx.Channel.SendMessageAsync($"Added {string.Join(", ", memberNames)} to the list of team members.");
        }



        [Command("clearteams")]
        public async Task ClearTeam(CommandContext ctx)
        {
            teamMembers.Remove(ctx.Guild.Id);
            await ctx.Channel.SendMessageAsync("Cleared the list of team members.");
        }


       
        [Command("generateteams")]
        public async Task GenerateTeams(CommandContext ctx)
        {
            //take shuffle split
            if (teamMembers.TryGetValue(ctx.Guild.Id, out var members))
            {
                
                var random = new Random();
                var shuffledMembers = members.OrderBy(x => random.Next()).ToList();

                
                var team1 = shuffledMembers.Take(shuffledMembers.Count / 2);
                var team2 = shuffledMembers.Skip(shuffledMembers.Count / 2);

                
                await ctx.Channel.SendMessageAsync($"Team 1: {string.Join(", ", team1)}");
                await ctx.Channel.SendMessageAsync($"Team 2: {string.Join(", ", team2)}");
            }
            else
            {
                
                await ctx.Channel.SendMessageAsync("No team members have been added. Use the !addmember command to add members.");
            }
        }



        [Command("removemembers")]
        public async Task RemoveMembers(CommandContext ctx, params string[] memberNames)
        {
            if (!teamMembers.TryGetValue(ctx.Guild.Id, out var members) || members.Count == 0)
            {
                await ctx.Channel.SendMessageAsync("No team members have been added.");
                return;
            }

            var removedMembers = new List<string>();

            foreach (var memberName in memberNames)
            {
                if (members.Remove(memberName))
                {
                    removedMembers.Add(memberName);
                }
            }

            if (removedMembers.Count > 0)
            {
                await ctx.Channel.SendMessageAsync($"Removed {string.Join(", ", removedMembers)} from the list of team members.");
            }
            else
            {
                await ctx.Channel.SendMessageAsync("No members were removed. Make sure you provided valid member names.");
            }
        }

        [Command("showmembers")]
        public async Task ShowMembers(CommandContext ctx)
        {
            if (!teamMembers.TryGetValue(ctx.Guild.Id, out var members) || members.Count == 0)
            {
                await ctx.Channel.SendMessageAsync("No members have been added.");
                return;
            }

            var membersList = string.Join(", ", members);
            await ctx.Channel.SendMessageAsync($"Current members in the list: {membersList}");
        }

    }

}
