using LineBot.Asset.Model.LineBot.Base;

namespace LineBot.Asset.Model.LineBot
{
    public class WebHookEvent
    {
        public string Destination { get; set; }

        public Event[] Events { get; set; }
    }
}
