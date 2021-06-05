/*
 * RoboUser.cs: the user model as stored in the database
 * Copyright (C) 2021, Refrag <R3FR4G>
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * See the 'LICENSE' file for more details.
 */

using System;
using AspNetCore.Identity.Mongo.Model;
using MongoDB.Bson.Serialization.Attributes;

namespace RoboRecords.Models
{
    public class RoboUser : MongoUser
    {
        public short Discriminator;

        public string UserNameNoDiscrim;
        public RoboUser(string userName, short discriminator) : base(string.Concat(userName, '#', discriminator))
        {
            Discriminator = discriminator;
            UserNameNoDiscrim = userName;
        }
    }
}