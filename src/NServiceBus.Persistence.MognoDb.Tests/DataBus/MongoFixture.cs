﻿using System;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Text;
using MongoDB.Driver;
using NServiceBus.Persistence.MongoDB.DataBus;
using NServiceBus.Persistence.MongoDB.Subscriptions;
using NUnit.Framework;

namespace NServiceBus.Persistence.MognoDb.Tests.DataBus
{
    public class MongoFixture
    {
        
        private MongoDatabase _database;
        private MongoClient _client;
        private GridFsDataBus _gridFsDataBus;


        [SetUp]
        public void SetupContext()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["MongoDB"].ConnectionString;

            _client = new MongoClient(connectionString);
            _database = _client.GetServer().GetDatabase("Test_" + DateTime.Now.Ticks.ToString(CultureInfo.InvariantCulture));
            _gridFsDataBus = new GridFsDataBus(_database);
        }

        protected MongoDatabase Database
        {
            get { return _database; }
        }

        protected GridFsDataBus GridFsDataBus
        {
            get { return _gridFsDataBus; }
        }

        protected string Put(string content, TimeSpan timeToLive)
        {
            var byteArray = Encoding.ASCII.GetBytes(content);
            using (var stream = new MemoryStream(byteArray))
            {
                return GridFsDataBus.Put(stream, timeToLive);
            }
        }

        [TearDown]
        public void TeardownContext()
        {
            _database.Drop();
        }
    }
}