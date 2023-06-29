using Newtonsoft.Json;
using static punkOptimise.Models.Enums;

namespace punkOptimise.Models
{
    public class PunkOptimiseResponse
    {
        public PunkOptimiseResponse(string message = "", ResultType resultType = ResultType.Success)
        {
            Message = message;
            ResultType = resultType;
        }

        [JsonProperty(PropertyName = "message")]
        public string Message { get; private set; }

        [JsonProperty(PropertyName = "resultType")]
        public Enums.ResultType ResultType { get; set; }
    }
}
