using LineBot.Asset.Enum;
using LineBot.Asset.Model.AppSetting;
using LineBot.Asset.Model.LineBot;
using LineBot.Entitys.Models;
using LineBot.Module.Interface;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using URF.Core.Abstractions.Trackable;

namespace LineBot.Module.Service
{
    public class ReplyMessageService : IReplyMessageService
    {
        private readonly IOptions<LineBotSetting> lineBotSetting;
        private readonly ITrackableRepository<RentFixedFee> rentFixedFeeRepo;
        private readonly ITrackableRepository<UtilityFee> utilityFeeRepo;
        private readonly ITrackableRepository<Person> personRepo;
        private readonly ITrackableRepository<PersonalLiability> personalLiabilityRepo;

        public ReplyMessageService(
            IOptions<LineBotSetting> lineBotSetting,
            ITrackableRepository<RentFixedFee> rentFixedFeeRepo,
            ITrackableRepository<UtilityFee> utilityFeeRepo,
            ITrackableRepository<Person> personRepo,
            ITrackableRepository<PersonalLiability> personalLiabilityRepo) 
        {
            this.lineBotSetting = lineBotSetting;
            this.rentFixedFeeRepo = rentFixedFeeRepo;
            this.utilityFeeRepo = utilityFeeRepo;
            this.personRepo = personRepo;
            this.personalLiabilityRepo = personalLiabilityRepo;
        }

        /// <summary>
        /// 接收查詢房租訊息
        /// </summary>
        public async Task QueryRent(WebHookEvent req)
        {
            if (req.Events == null || req.Events.Length == 0) return;

            var reqEvent = req.Events.FirstOrDefault();
            if (reqEvent == null || 
                reqEvent.Type != EventType.message || 
                reqEvent.Message == null ||
                reqEvent.Message.Type != MessageType.text ||
                reqEvent.Message.Text != "查詢房租") return;

            // 取得發送內容
            var textMessage = this.GetTextMessages();

            // 發送訊息
            await this.ReplyTextMessage(textMessage,reqEvent.ReplyToken);
        }

        /// <summary>
        /// 取得文字訊息內容
        /// </summary>
        /// <returns></returns>
        private List<TextMessage> GetTextMessages() 
        {
            var commonFees = this.GetCommonFees();

            // 管理費 : xx,xxx + 房租 : xxxx + 水費 : xxx = xx,xxx
            var title = this.GetTitle(commonFees);

            // 取得租屋總人數
            var persons = this.personRepo.Queryable().ToList();

            // 基本: xx,xxx / x = x,xxx
            var subTitle = this.GetSubTitle(commonFees, persons.Count);

            // A: 基本 x,xxx (+/- 人員欠債費用) = x,xxx
            // B: 基本 x,xxx (+/- 人員欠債費用) = x,xxx
            var detail = this.GetPersonDetail(commonFees, persons);

            // 預期組成
            // title    管理費 : xx,xxx + 房租 : xxxx + 水費 : xxx = xx,xxx
            // subTitle 基本: xx,xxx / x = x,xxx
            //          ----------------------------------------
            // detail   A: 基本 x,xxx (+/- 人員欠債費用) = x,xxx
            //          B: 基本 x,xxx (+/- 人員欠債費用) = x,xxx

            StringBuilder sb = new StringBuilder();

            sb.AppendLine(title);
            sb.AppendLine(subTitle);
            sb.AppendLine("----------------------------------------");
            sb.AppendLine(detail);

            return new List<TextMessage>() { new TextMessage { Text = sb.ToString()} };
        }

        /// <summary>
        /// 取得租屋固定+公共費用項目
        /// </summary>
        /// <returns></returns>
        private IEnumerable<CommonFee> GetCommonFees()
        {
            // 取得固定房租資料(房租、管理費等)
            var rentFixedFeeItems = this.rentFixedFeeRepo.Queryable()
                .Select(r => new CommonFee { Name = r.ItemName, Fee = r.Fee });

            // 取得公共費用資料(水費、電費等)
            var utilityFeeItems = this.utilityFeeRepo.Queryable()
                .Where(u => !u.IsClosed)
                .Select(u => new CommonFee { Name = u.ItemName, Fee = u.Fee });

            return rentFixedFeeItems.Concat(utilityFeeItems);
        }

        /// <summary>
        /// 組成 管理費 : xx,xxx + 房租 : xxxx + 水費 : xxx = xx,xxx
        /// </summary>
        private string GetTitle(IEnumerable<CommonFee> commonFees) 
        {
            // 組成項目名稱 + 金額
            var items = commonFees.Select(c => $"{c.Name} : {c.Fee.ToString("N0")}");

            // 取得總金額
            var total = commonFees.Sum(c => c.Fee);

            return $"{string.Join(" + ", items)} = {total.ToString("N0")}";
        }

        /// <summary>
        /// 組成 基本: xx,xxx / x = x,xxx
        /// </summary>
        private string GetSubTitle(IEnumerable<CommonFee> commonFees, int personCount)
        {
            // 取得總金額
            var total = commonFees.Sum(c => c.Fee);

            // 取得均分後的金額
            var average = this.GetAverageFee(commonFees, personCount);

            return $"基本 : {total.ToString("N0")} / {personCount} = {average.ToString("N0")}";
        }

        /// <summary>
        /// 取得平均租金(基本要繳的租金)
        /// </summary>
        private decimal GetAverageFee(IEnumerable<CommonFee> commonFees,int personCount) 
        {
            // 取得總金額
            var total = commonFees.Sum(c => c.Fee);

            // 均分房租，採無條件捨去
            return Math.Floor(total / personCount);
        }

        /// <summary>
        /// 組成個人明細內容(人員相互欠債)
        /// A: 基本 x,xxx (+/- 人員欠債費用) = x,xxx
        /// B: 基本 x,xxx (+/- 人員欠債費用) = x,xxx
        /// </summary>
        /// <returns></returns>
        private string GetPersonDetail(
            IEnumerable<CommonFee> commonFees, List<Person> persons)
        {
            var average = this.GetAverageFee(commonFees,persons.Count);

            // 租屋人員金額字典
            var personFeeDic = persons.ToDictionary(p => p.PersonId, p => average);

            // 租屋人員項目字典
            var personItemDic = persons.ToDictionary(p => p.PersonId, p => $"基本 {average.ToString("N0")}");

            // 取得人員負債表
            var liabilities = this.personalLiabilityRepo.Queryable().ToList();

            foreach (var l in liabilities)
            {
                // 債主減少對應金額(由欠債人付)
                personFeeDic[l.CreditorId] -= l.Fee;

                // 欠債人增加對應金額(債主扣除)
                personFeeDic[l.DebtorId] += l.Fee;

                // 更新項目內容
                // 基本 x,xxx - 人員欠債費用
                personItemDic[l.CreditorId] = 
                    $"{personItemDic[l.CreditorId]} - {l.Fee.ToString("N0")}";

                // 基本 x,xxx + 人員欠債費用
                personItemDic[l.DebtorId] = 
                    $"{personItemDic[l.DebtorId]} + {l.Fee.ToString("N0")}";
            }

            StringBuilder sb = new StringBuilder();

            foreach(var person in persons) 
            {
                // 組成
                // A: 基本 x,xxx (+/- 人員欠債費用) = x,xxx
                sb.AppendLine($"{person.PersonName} : {personItemDic[person.PersonId]} = {personFeeDic[person.PersonId].ToString("N0")}");
            }

            return sb.ToString();
        }

        /// <summary>
        /// 發送 LineBot 文字訊息
        /// </summary>
        private async Task ReplyTextMessage(
            List<TextMessage> messages, string replyToken)
        {
            var req = new
            {
                replyToken = replyToken,
                messages = messages
            };

            var reqJson = JsonConvert.SerializeObject(
                req,
                Formatting.None,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });

            await this.Reply(reqJson);
        }

        /// <summary>
        /// 發送訊息
        /// </summary>
        private async Task Reply(string reqJson)
        {
            var httpReqMsg = new HttpRequestMessage(
                HttpMethod.Post, @"https://api.line.me/v2/bot/message/reply");

            // Content Type
            httpReqMsg.Content =
                new StringContent(reqJson, Encoding.UTF8, "application/json");

            HttpClient httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders
                .Add("Authorization", $"Bearer {this.lineBotSetting.Value.AccesstoKen}");

            var resp = await httpClient.SendAsync(httpReqMsg);
        }
    }
}
