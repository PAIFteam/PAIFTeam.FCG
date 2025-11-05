using Newtonsoft.Json;

namespace PAIFGAMES.FCG.Api.Response
{
    public class Result
    {
        [JsonProperty("succeeded")]
        public bool Succeeded { get; set; }

        [JsonProperty("errors")]
        public MessageError[] Errors { get; set; }

        [JsonProperty("data")]
        public dynamic Data { get; set; }
    }
}