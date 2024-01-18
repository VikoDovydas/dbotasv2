using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.EventArgs;

public class Antispam
{
    private const int SpamThreshold = 20;
    private const int CooldownTimeSeconds = 30;

    private static Dictionary<ulong, List<DateTime>> userMessageTimestamps = new Dictionary<ulong, List<DateTime>>();
    private static Dictionary<ulong, DateTime> lastWarningTimestamp = new Dictionary<ulong, DateTime>();

    public static async Task HandleMessageAsync(DiscordClient client, MessageCreateEventArgs e)
    {
        if (e.Author.IsBot)
            return;

        ulong userId = e.Author.Id;

        // Jei nera userio dar liste sukuriam jam entry
        if (!userMessageTimestamps.TryGetValue(userId, out var timestamps))
        {
            timestamps = new List<DateTime>();
            userMessageTimestamps[userId] = timestamps;
        }

        // isremovint stampus, kad nespamintu botas nonstop
        var currentTime = DateTime.Now;
        timestamps.RemoveAll(timestamp => (currentTime - timestamp).TotalSeconds >= CooldownTimeSeconds);

        timestamps.Add(currentTime);

        // Checkinam userio spam ir cooldownu s
        if (timestamps.Count > SpamThreshold)
        {
            
            if (!lastWarningTimestamp.TryGetValue(userId, out var lastWarningTime) || (currentTime - lastWarningTime).TotalSeconds >= CooldownTimeSeconds)
            {
                await e.Channel.SendMessageAsync($"{e.Author.Mention}, STOP SPAM");

                lastWarningTimestamp[userId] = currentTime;
            }
        }
    }
}
