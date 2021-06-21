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
using RoboRecords.Models;

namespace RoboRecords.Services
{
    public class DatabaseService
    {
        private const string GamesCollection = "Games";
        private readonly IConfiguration _configuration;
        private readonly string _defaultDatabase;
        private MongoClient _databaseClient;
        private IMongoDatabase _gamesDatabase;
        
        public DatabaseService(IConfiguration configuration)
        {
            _configuration = configuration;
            _defaultDatabase = "games";
            _databaseClient = new MongoClient(_configuration["DbConnectionString"]);
            _gamesDatabase = _databaseClient.GetDatabase(_defaultDatabase);
        }

        public List<RoboGame> GetGames()
        {
            return GetCollectionElements<RoboGame>(GamesCollection);
        }

        public void AddGame(RoboGame roboGame)
        {
            GetCollection<RoboGame>(GamesCollection).InsertOne(roboGame);
        }

        public List<T> GetCollectionElements<T>(string collectionName)
        {
            return GetCollectionElements(collectionName, FilterDefinition<T>.Empty);
        }
        
        public List<T> GetCollectionElements<T>(string collectionName, FilterDefinition<T> filter)
        {
            return GetCollection<T>(collectionName).Find(filter).ToList();
        }
        
        private IMongoCollection<T> GetCollection<T>(string collectionName)
        {
            return _gamesDatabase.GetCollection<T>(collectionName);
        }
    }
}
