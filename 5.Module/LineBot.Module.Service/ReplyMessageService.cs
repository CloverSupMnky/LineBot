using LineBot.Asset.Model.AppSetting;
using LineBot.Asset.Model.LineBot;
using LineBot.Module.Interface;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LineBot.Module.Service
{
    public class ReplyMessageService : IReplyMessageService
    {
        private readonly IOptions<LineBotSetting> lineBotSetting;

        public ReplyMessageService(IOptions<LineBotSetting> lineBotSetting) 
        {
            this.lineBotSetting = lineBotSetting;
        }

        public async Task ReplyTextMessage(string text)
        {
            HttpClient httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders
                .Add("Authorization", $"Bearer {this.lineBotSetting.Value.AccesstoKen}");

            var baseUrl = @"https://api.line.me/v2/bot/message";

            var reqUrl = $"{baseUrl}/multicast";

            var req = new
            {
                to = new string[] {this.lineBotSetting.Value.UserId},
                messages = new object[] 
                {
                    new { type = "text",text = text }
                }
            };

            var reqJson = JsonConvert.SerializeObject(
                req,
                Formatting.None,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });

            var httpReqMsg = new HttpRequestMessage(HttpMethod.Post, reqUrl);

            // Content Type
            httpReqMsg.Content = 
                new StringContent(reqJson, Encoding.UTF8, "application/json");

            var resp = await httpClient.SendAsync(httpReqMsg);
        }
    }
}
