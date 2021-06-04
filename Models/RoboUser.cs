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