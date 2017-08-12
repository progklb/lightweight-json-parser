using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonReader
{
    public static class JsonExamples
    {
        public const string SIMPLE_OBJECT =
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
    ""description"": ""A man drinking a coffee.""
}";

        public const string SIMPLE_ARRAY =
@"[{
    ""id"": ""Dwu85P9SOIk""
    },
    {
    ""id"": ""uAKJ6874Ffy""
    }
]}";

        public const string COMPLEX_OBJECT =
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

        public const string INVALID_JSON = @" ({}";
    }
}
