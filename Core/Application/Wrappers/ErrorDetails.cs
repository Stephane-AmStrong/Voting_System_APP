using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Wrappers
{
    //[JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class ErrorDetails
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        #pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        public List<string>? Errors { get; set; }
    }
}
