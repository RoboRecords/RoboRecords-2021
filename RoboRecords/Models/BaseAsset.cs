/*
 * Asset.cs: The base model for any asset managed by the AssetManager
 * Copyright (C) 2022, Refrag <Refragg>
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * See the 'LICENSE' file for more details.
 */

using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Dynamic;
using System.Text.Json;
using System.Text.Json.Serialization;
using SixLabors.ImageSharp;

namespace RoboRecords.Models;

public class BaseAsset
{
    [JsonPropertyName("id")]
    public int DbId { get; set; }
    public string Name { get; init; }
    public string FileExtension { get; set; }
    
    // Note: we don't want this to be included in the database, for some reason, it gets included without even being in the model in the DbContext
    [NotMapped]
    public virtual string Path { get; }
    
    [NotMapped]
    [JsonConverter(typeof(LazyJsonConverter))]
    public Lazy<IImageInfo> ImageInfo { get; }

    public BaseAsset()
    {
        ImageInfo = new Lazy<IImageInfo>(LoadImageInfo);
    }
    
    public BaseAsset(string name, string fileExtension)
    {
        Name = name;
        FileExtension = fileExtension;
        ImageInfo = new Lazy<IImageInfo>(LoadImageInfo);
    }

    private IImageInfo LoadImageInfo()
    {
        return Image.Identify(FileManager.GetAbsolutePath(GetFullPath()));
    }

    public override string ToString()
    {
        return $"Asset: ID: {DbId}, Name: {Name}, Ext: {FileExtension}, Path: {GetFullPath()}";
    }
    
    public string ToStringWithImageInfo()
    {
        return $"Asset: ID: {DbId}, Name: {Name}, FileExtension: {FileExtension}, Path: {GetFullPath()}\nImage: Width: {ImageInfo.Value.Width}, Height: {ImageInfo.Value.Height}";
    }
    
    private string GetFullPath()
    {
        return System.IO.Path.Combine(FileManager.NewAssetsDirectoryName, Path, $"{DbId.ToString()}.{FileExtension}");
    }
}

public class LazyJsonConverter : JsonConverter<Lazy<IImageInfo>>
{
    public override Lazy<IImageInfo>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotImplementedException(); //Note: This doesn't seem to get called for whatever reason
    }

    public override void Write(Utf8JsonWriter writer, Lazy<IImageInfo> value, JsonSerializerOptions options)
    {
        IImageInfo imageInfo = value.Value;
        writer.WriteStartObject();
        writer.WriteNumber("width", imageInfo.Width);
        writer.WriteNumber("height", imageInfo.Height);
        writer.WriteEndObject();
    }
}