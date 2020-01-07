﻿// <auto-generated />
//
// To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
//
//    using Spitzer.Models;
//
//    var imageAssets = ImageAssets.FromJson(jsonString);

namespace Spitzer.Models.Images
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public class ImageAssets
    {
        public static List<Uri> FromJson(string json) => JsonConvert.DeserializeObject<List<Uri>>(json, NasaMedia.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this List<Uri> self) => JsonConvert.SerializeObject(self, NasaMedia.Converter.Settings);
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
