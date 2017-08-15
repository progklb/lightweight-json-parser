using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonReader
{
    public static class JsonExamples
    {
        #region CONSTANTS
        public const string SIMPLE_KVP_OBJECT =
@"{
    ""id"": ""Dwu85P9SOIk"",
    ""created_at"": ""2016 - 05 - 03T11:00:28 - 04:00"",
    ""updated_at"": ""2016 - 07 - 10T11:00:01 - 05:00"",
    ""width"": 2448,
    ""height"": 3264,
    ""color"": ""#6E633A"",
    ""downloads"": 1345,
    ""likes"": 24,
    ""liked_by_user"": false,
    ""description"": ""A man drinking a coffee, 'elegantly'."",
    'remarks': 'Fantastic photo!'
}";

        public const string SIMPLE_MIXED_EMPTY_OBJECT =
@"{
    ""id"": ""Dwu85P9SOIk"",
    ""exif"":{}
}";

        public const string SIMPLE_MIXED_OBJECT =
@"{
    ""id"": ""Dwu85P9SOIk"",
    ""downloads"": 1345,
    ""likes"": 24,
    ""liked_by_user"": false,
    ""description"": ""A man drinking a coffee."",
    ""exif"": {
            ""make"": ""Canon"",
            ""model"": ""Canon EOS 40D"",
            ""exposure_time"": ""0.011111111111111112"",
            ""aperture"": ""4.970854"",
            ""focal_length"": ""37"",
            ""iso"": 100
            }
}";

        public const string SIMPLE_KVP_ARRAY =
@"[{
    ""id"": ""Dwu85P9SOIk""
    },
    {
    ""id"": ""uAKJ6874Ffy""
    }
]}";
        public const string SIMPLE_MIXED_ARRAY =
@"{[ 
    {""exif"": {
        ""make"": ""Canon"",
        ""model"": ""Canon EOS 5D"",
        ""exposure_time"": ""0.011111111111111112"",
        ""aperture"": ""4.970854"",
        ""focal_length"": ""270"",
        ""iso"": 400
        }
    },
    {""exif"": {
        ""make"": ""Canon"",
        ""model"": ""Canon EOS 40D"",
        ""exposure_time"": ""0.011111111111111112"",
        ""aperture"": ""4.970854"",
        ""focal_length"": ""37"",
        ""iso"": 100
        }
    }
]}";

        public const string COMPLEX_MIXED_OBJECT =
@"{
    ""id"": ""Dwu85P9SOIk"",
    ""created_at"": ""2016 - 05 - 03T11:00:28 - 04:00"",
    ""updated_at"": ""2016 - 07 - 10T11:00:01 - 05:00"",
    ""width"": 2448,
    ""height"": 3264,
    ""color"": ""#6E633A"",
    ""downloads"": 1345,
    ""likes"": 24,
    ""liked_by_user"": false,
    ""description"": ""A man drinking a coffee."",
    ""exif"": {
        ""make"": ""Canon"",
        ""model"": ""Canon EOS 40D"",
        ""exposure_time"": ""0.011111111111111112"",
        ""aperture"": ""4.970854"",
        ""focal_length"": ""37"",
        ""iso"": 100
        },
    ""location"": {
        ""city"": ""Montreal"",
        ""country"": ""Canada"",
        ""position"": {
            ""latitude"": 45.4732984,
            ""longitude"": -73.6384879
            }
        },
    ""categories"": [
        {
            ""id"": 4,
            ""title"": ""Nature"",
            ""photo_count"": 24783,
            ""links"": {
            ""self"": ""https://api.unsplash.com/categories/4"",
            ""photos"": ""https://api.unsplash.com/categories/4/photos""
        },
        {
            ""id"": 5,
            ""title"": ""Home"",
            ""photo_count"": 6581,
            ""links"": {
            ""self"": ""https://api.unsplash.com/categories/5"",
            ""photos"": ""https://api.unsplash.com/categories/5/photos""
        }
    ]
}";


        public const string COMPLEX_DIFF_TYPES_OBJECT =
@"{
    ""string"": ""This is a string with 'quotes' and stuff."",
    ""date"": ""2016 - 07 - 10T11:00:01 - 05:00"",
    ""boolean"": true,
    ""null"" : null,
    ""object_empty"" : {},
    ""object_single"" : { ""key"" : ""value"" },
    ""object_double"" : { ""key1"" : ""value1"", ""key2"" : ""value2"" },
    ""int"": 123,
    ""frac_1"": 123.123,
    ""frac_2"": .123,
    ""exp_1"": 1e5,
    ""exp_frac_1"": 1.24e5,
    ""exp_frac_2"": 1.24e+5,
    ""exp_frac_3"": 1.24e-5,
    ""exp_frac_4"": 1.24E+5,
    ""signed_int"": -1,
    ""signed_frac_1"": -1.24,
    ""signed_frac_2"": +1.24,
    ""signed_exp_frac_2"": -1.24e+5
}";

        public const string INVALID_OBJECT = 
@" ({}";
        #endregion
    }
}
