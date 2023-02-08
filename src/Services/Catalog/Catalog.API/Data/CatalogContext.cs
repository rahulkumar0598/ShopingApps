﻿using Catalog.API.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Catalog.API.Data
{
    public class CatalogContext : ICatalogContext
    {
        public CatalogContext(IConfiguration configuration)
        {
            //Connection to the MongoDb
            var client = new MongoClient(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            //Creating Database in MongoDb
            var database = client.GetDatabase(configuration.GetValue<string>("DatabaseSettings:DatabaseName"));
            Products = database.GetCollection<Product>(configuration.GetValue<string>("DatabaseSettings:CollectionName"));
            CatalogContextSeed.SeedData(Products);
        }

        //Product collection property for mongoDb
        public IMongoCollection<Product> Products { get; }
    }
}
