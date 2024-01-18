using DSharpPlus.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace JustenkaBot
{
    public class GifHandler
    {
        private static readonly Dictionary<string, string> KeywordToGifUrlMap = new Dictionary<string, string>
        {
            { "dog", "https://tenor.com/view/dog-dawg-wut-what-wtf-gif-9118700\r\n" },
            { "buggati", "https://tenor.com/view/andrew-tate-bugatti-what-color-smoking-top-g-gif-26393811\r\n" },
            { "trunk monkey", "https://tenor.com/view/car-press-button-trunk-monkey-open-hatch-gif-15867111\r\n" },
            { "ape", "https://tenor.com/view/monkey-lick-gif-19649491\r\n" },

            { "example", "https://tenor.com/view/andrew-tate-bugatti-what-color-smoking-top-g-gif-26393811\r\n" },

            
        };

        public static async Task HandleGifAsync(MessageCreateEventArgs e)
        {
            foreach (var mapping in KeywordToGifUrlMap)
            {
                if (e.Message.Content.ToLower().Contains(mapping.Key.ToLower()))
                {
                    await e.Channel.SendMessageAsync($"{mapping.Value}");
                    return; 
                }
            }
        }
    }
}




