using System;
using System.IO;
using System.Linq;
using Laendlefinder.Classes;
using Laendlefinder.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestLaendlefinder
{
    /**
     * @class TestEventCollection
     * @brief Enthält Tests für die EventCollection-Klasse.
     * Diese Klasse testet die Funktionen der EventCollection.
     */
    [TestClass]
    public sealed class TestEventCollection
    {
        [TestMethod]
        /**
         * Testet die Suche nach einem Event anhand des Namens.
         * Erwartet, dass das Event gefunden wird.
         */
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
        /**
         * Testet die Suche nach einem Event anhand eines nicht vorhandenen Textes.
         * Erwartet, dass das Event nicht gefunden wird.
         */
        public void Search_ByUnknownTerm_ReturnsEmpty()
        {
            var collection = new EventCollection
            {
                new Event { eid = 1, name = "Alpha", date = DateTime.Now, Location = new Location() }
            };

            var result = collection.Search("NichtVorhanden");
            Assert.AreEqual(0, result.Count);
        }
        
        /**
        * Testet, dass null-Events bei der Suche ignoriert werden.
        * Erwartet ein leeres Ergebnis.
        */
        [TestMethod]
        public void Search_NullEvent_Ignored()
        {
            var collection = new EventCollection();
            collection.Add(null);

            var result = collection.Search("irgendwas");
            Assert.AreEqual(0, result.Count);
        }

        /**
        * Testet das Filtern nach Datum.
        * Erwartet, dass nur das Event mit dem passenden Datum gefunden wird.
        */
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
        
        /**
         * Testet das Filtern nach einem leeren EventCollection.
         * Erwartet ein leeres Ergebnis.
         */
        [TestMethod]
        public void FilterByDateRange_EmptyCollection_ReturnsEmpty()
        {
            var collection = new EventCollection();
            var result = collection.FilterByDateRange(DateTime.Now, DateTime.Now.AddDays(1));
            Assert.AreEqual(0, result.Count);
        }

        /**
         * Testet die Serialisierung einer leeren EventCollection.
         * Erwartet, dass die Datei leer bleibt.
         */
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

        /**
         * Testet das Filtern nach einem Datumsbereich.
         * Erwartet, dass alle Events im Bereich gefunden werden.
         */
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

        /**
         * Testet die Serialisierung einer EventCollection mit Events.
         * Erwartet, dass die Datei die Eventdaten enthält.
         */
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