﻿using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace ZFCTAPI.Core.Helpers
{
    public class JsonsHelper:JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            List<KeyValuePair<string, object>> list = value as List<KeyValuePair<string, object>>;
            writer.WriteStartArray();
            foreach (var item in list)
            {
                writer.WriteStartObject();
                writer.WritePropertyName(item.Key);
                writer.WriteValue(item.Value);
                writer.WriteEndObject();
            }
            writer.WriteEndArray();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(List<KeyValuePair<string, object>>);
        }
    }
}
