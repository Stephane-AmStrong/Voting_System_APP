using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Application.Enums
{
    public enum EnumRole
    {
        [Description("Voter")]
        Voter = 1,

        [Description("SuperAdmin")]
        SuperAdmin = 2,
    }
}
