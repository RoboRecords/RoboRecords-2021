/*
 * DatabaseService.cs: the service to access the database
 * Copyright (C) 2021, Lemin <Leminn> and Refrag <R3FR4G>
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * See the 'LICENSE' file for more details.
 */

using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace RoboRecords.Services
{
    public class DatabaseService
    {
        private readonly IConfiguration _configuration;
        private readonly string _defaultDatabase;
        private MongoClient _databaseClient;
        private IMongoDatabase _database;
        
        public DatabaseService(IConfiguration configuration)
        {
            _configuration = configuration;
            _defaultDatabase = _configuration["DbDefaultDb"];
            _databaseClient = new MongoClient(_configuration["DbConnectionString"]);
            _database = _databaseClient.GetDatabase(_defaultDatabase);
        }

        private IMongoCollection<T> GetCollection<T>(string collectionName)
        {
            return _database.GetCollection<T>(collectionName);
        }

        public List<T> GetCollectionElements<T>(string collectionName)
        {
            return GetCollectionElements(collectionName, FilterDefinition<T>.Empty);
        }
        
        public List<T> GetCollectionElements<T>(string collectionName, FilterDefinition<T> filter)
        {
            return GetCollection<T>(collectionName).Find(filter).ToList();
        }
    }
}