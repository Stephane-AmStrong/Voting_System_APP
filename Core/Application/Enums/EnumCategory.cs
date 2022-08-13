using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Application.Enums
{
    public enum EnumCategory
    {
        [Description("Secretary")]
        Secretary = 1,

        [Description("Vice President")]
        VicePresident = 2,

        [Description("President")]
        President = 3,
    }
}
