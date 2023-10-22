/// <summary>
/// Auto-generated (and adapted) from https://app.quicktype.io.
/// </summary>
namespace MadBox.Exercice
{
    using System;
    using System.Globalization;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    [Serializable]
    public partial class GameSettings
    {
        [JsonProperty("entities")]
        public Entity[] Entities = null;

        [JsonProperty("global")]
        public Global Global = null;

        [JsonProperty("list")]
        public long[] List = null;
    }

    [Serializable]
    public partial class Entity
    {
        [JsonProperty("id")]
        public long Id;

        [JsonProperty("name")]
        public string Name = null;

        [JsonProperty("description")]
        public string Description = null;

        [JsonProperty("number")]
        public long Number;

        [JsonProperty("plainJson")]
        public PlainJson PlainJson = null;

        [JsonProperty("nestedList")]
        public NestedList[] NestedList = null;

        [JsonProperty("nestedObject")]
        public NestedObject NestedObject = null;
    }

    [Serializable]
    public partial class NestedList
    {
        [JsonProperty("id")]
        public long Id;

        [JsonProperty("name")]
        public string Name = null;

        [JsonProperty("nestedObject")]
        public NestedObject NestedObject = null;
    }

    [Serializable]
    public partial class NestedObject
    {
        [JsonProperty("string")]
        public string String = null;

        [JsonProperty("bool")]
        public bool Bool;
    }

    [Serializable]
    public partial class PlainJson
    {
        [JsonProperty("stringKey")]
        public string StringKey = null;

        [JsonProperty("numberKey")]
        public long NumberKey;
    }

    [Serializable]
    public partial class Global
    {
        [JsonProperty("key")]
        public string Key = null;

        [JsonProperty("number")]
        public long Number;

        [JsonProperty("bool")]
        public bool Bool;

        [JsonProperty("string")]
        public string String = null;
    }

    public partial class GameSettings
    {
        public static GameSettings FromJson(string json) => JsonConvert.DeserializeObject<GameSettings>(json, Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this GameSettings self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}
