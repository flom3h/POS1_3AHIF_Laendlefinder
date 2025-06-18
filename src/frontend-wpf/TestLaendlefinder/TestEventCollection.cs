using System;
using System.IO;
using System.Linq;
using Laendlefinder.Classes;
using Laendlefinder.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestLaendlefinder
{
    [TestClass]
    public sealed class TestEventCollection
    {
        [TestMethod]
        public void Search_ByName_FindsEvent()
        {
            var collection = new EventCollection
            {
                new Event { eid = 1, name = "TestEvent", date = DateTime.Now, Location = new Location() }
            };

            var result = collection.Search("TestEvent");
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("TestEvent", result.First().name);
        }
        
        [TestMethod]
        public void Search_ByUnknownTerm_ReturnsEmpty()
        {
            var collection = new EventCollection
            {
                new Event { eid = 1, name = "Alpha", date = DateTime.Now, Location = new Location() }
            };

            var result = collection.Search("NichtVorhanden");
            Assert.AreEqual(0, result.Count);
        }
        
        public void Search_NullEvent_Ignored()
        {
            var collection = new EventCollection();
            collection.Add(null); // absichtlich null

            var result = collection.Search("irgendwas");
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void FilterByDate_FindsCorrectEvent()
        {
            var date = new DateTime(2024, 6, 1);
            var collection = new EventCollection
            {
                new Event { eid = 2, name = "Event1", date = date, Location = new Location() },
                new Event { eid = 3, name = "Event2", date = date.AddDays(1), Location = new Location() }
            };

            var result = collection.FilterByDate(date);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Event1", result.First().name);
        }
        
        [TestMethod]
        public void FilterByDateRange_EmptyCollection_ReturnsEmpty()
        {
            var collection = new EventCollection();
            var result = collection.FilterByDateRange(DateTime.Now, DateTime.Now.AddDays(1));
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void Serialize_EmptyCollection_CreatesEmptyFile()
        {
            var tempFile = Path.GetTempFileName();
            try
            {
                var collection = new EventCollection();
                collection.Serialize(tempFile);

                var lines = File.ReadAllLines(tempFile);
                Assert.AreEqual(0, lines.Length);
            }
            finally
            {
                File.Delete(tempFile);
            }
        }

        [TestMethod]
        public void FilterByDateRange_FindsEventsInRange()
        {
            var start = new DateTime(2024, 6, 1);
            var end = new DateTime(2024, 6, 3);
            var collection = new EventCollection
            {
                new Event { eid = 4, name = "A", date = start, Location = new Location() },
                new Event { eid = 5, name = "B", date = start.AddDays(2), Location = new Location() },
                new Event { eid = 6, name = "C", date = end.AddDays(1), Location = new Location() }
            };

            var result = collection.FilterByDateRange(start, end);
            Assert.AreEqual(2, result.Count);
        }

        [TestMethod]
        public void Serialize_WritesFile()
        {
            var tempFile = Path.GetTempFileName();
            try
            {
                var collection = new EventCollection
                {
                    new Event { eid = 7, name = "SerialEvent", date = DateTime.Now, Location = new Location() }
                };
                collection.Serialize(tempFile);

                var lines = File.ReadAllLines(tempFile);
                Assert.IsTrue(lines.Any(l => l.Contains("SerialEvent")));
            }
            finally
            {
                File.Delete(tempFile);
            }
        }
    }
}