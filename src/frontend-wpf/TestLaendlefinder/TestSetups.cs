using System;
using Laendlefinder.Classes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestLaendlefinder
{
    /**
     * @class TestSetups
     * @brief Enthält Tests für die Setups der Klassen Event, Location, Type, RegRequest und FavRequest.
     * Diese Klasse testet das Setzen und Auslesen von Eigenschaften in den jeweiligen Klassen.
     */
    [TestClass]
    public sealed class TestSetups
    {
        /**
         * Testet das Setzen und Auslesen aller Eigenschaften eines Event-Objekts.
         * Erwartet, dass alle gesetzten Werte korrekt ausgelesen werden können.
         */
        [TestMethod]
        public void Event_Property_Set_And_Get()
        {
            var ev = new Event
            {
                eid = 42,
                name = "Testevent",
                date = new DateTime(2024, 6, 1),
                time = new TimeSpan(14, 0, 0),
                description = "Beschreibung",
                picture = "bild.jpg",
                type = 3,
                Location = new Location { lid = 1, name = "Ort" }
            };

            Assert.AreEqual(42, ev.eid);
            Assert.AreEqual("Testevent", ev.name);
            Assert.AreEqual(new DateTime(2024, 6, 1), ev.date);
            Assert.AreEqual(new TimeSpan(14, 0, 0), ev.time);
            Assert.AreEqual("Beschreibung", ev.description);
            Assert.AreEqual("bild.jpg", ev.picture);
            Assert.AreEqual(3, ev.type);
            Assert.IsNotNull(ev.Location);
            Assert.AreEqual("Ort", ev.Location.name);
        }

        /**
         * Testet das Setzen und Auslesen aller Eigenschaften eines Location-Objekts.
         * Erwartet, dass alle gesetzten Werte korrekt ausgelesen werden können.
         */
        [TestMethod]
        public void Location_Property_Set_And_Get()
        {
            var loc = new Location
            {
                lid = 7,
                name = "Testlocation",
                address = "Teststraße 1",
                latitude = 47.2,
                longitude = 9.6,
                picture = "loc.jpg"
            };

            Assert.AreEqual(7, loc.lid);
            Assert.AreEqual("Testlocation", loc.name);
            Assert.AreEqual("Teststraße 1", loc.address);
            Assert.AreEqual(47.2, loc.latitude);
            Assert.AreEqual(9.6, loc.longitude);
            Assert.AreEqual("loc.jpg", loc.picture);
        }

        /**
         * Testet das Setzen und Auslesen aller Eigenschaften eines Type-Objekts.
         * Erwartet, dass alle gesetzten Werte korrekt ausgelesen werden können.
         */
        [TestMethod]
        public void Type_Property_Set_And_Get()
        {
            var t = new Laendlefinder.Classes.Type
            {
                tid = 5,
                type = "Konzert"
            };

            Assert.AreEqual(5, t.tid);
            Assert.AreEqual("Konzert", t.type);
        }

        /**
         * Testet das Setzen und Auslesen aller Eigenschaften eines RegRequest-Objekts.
         * Erwartet, dass alle gesetzten Werte korrekt ausgelesen werden können.
         */
        [TestMethod]
        public void RegRequest_Property_Set_And_Get()
        {
            var req = new RegRequest
            {
                firstname = "Max",
                lastname = "Mustermann",
                email = "max@test.com",
                passwort = "geheim"
            };

            Assert.AreEqual("Max", req.firstname);
            Assert.AreEqual("Mustermann", req.lastname);
            Assert.AreEqual("max@test.com", req.email);
            Assert.AreEqual("geheim", req.passwort);
        }

        /**
         * Testet das Setzen und Auslesen aller Eigenschaften eines FavRequest-Objekts.
         * Erwartet, dass alle gesetzten Werte korrekt ausgelesen werden können.
         */
        [TestMethod]
        public void FavRequest_Property_Set_And_Get()
        {
            var fav = new FavRequest
            {
                uid = 11,
                eid = 22
            };

            Assert.AreEqual(11, fav.uid);
            Assert.AreEqual(22, fav.eid);
        }

        /**
         * Testet die Standardwerte eines neu erstellten Event-Objekts.
         * Erwartet, dass alle Werte auf ihren Standardwerten stehen.
         */
        [TestMethod]
        public void Event_Null_And_Default_Values()
        {
            var ev = new Event();
            Assert.AreEqual(0, ev.eid);
            Assert.IsNull(ev.name);
            Assert.AreEqual(default(DateTime), ev.date);
            Assert.AreEqual(default(TimeSpan), ev.time);
            Assert.IsNull(ev.description);
            Assert.IsNull(ev.picture);
            Assert.AreEqual(0, ev.type);
            Assert.IsNull(ev.Location);
        }
    }
}