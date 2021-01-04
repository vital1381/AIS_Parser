using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using AISDecoder.Decoder;
using AISDecoder.Model;
using AISDecoder.Model.Messages;
using NUnit.Framework;

namespace AISDecoder.Test
{
    [TestFixture]
    public class AisDecoderTest
    {
        private byte[] m_nmeaData;

        [OneTimeSetUp]
        public void FixtureSetUp()
        {
            m_nmeaData = TestResources.sample;
        }

        [TearDown]
        public void TearDown()
        {
        }

        [OneTimeTearDown]
        public void FixtureTearDown()
        {
        }

        [SetUp]
        public void Setup()
        { }


        [Test]
        public void TestSentenceFactoryPerformance()
        {
            var factory = new SentenceFactory { IsCrcEnabled = true };
            
            var watch = Stopwatch.StartNew();
            var sentences = factory.ProcessStream(m_nmeaData).ToArray();
            watch.Stop();

            Console.WriteLine(@"Data:{0} bytes", m_nmeaData.Length);
            Console.WriteLine(@"Nmea sentences:{0}", sentences.Length);
            Console.WriteLine(@"Time:{0} ms", watch.ElapsedMilliseconds);
            Console.WriteLine(@"Time per sentence:{0} ms", (double)watch.ElapsedMilliseconds / sentences.Length);

            Assert.LessOrEqual((double)watch.ElapsedMilliseconds / sentences.Length, 1);
        }


        [Test]
        public void TestAisMessageFactoryPerformance()
        {
            var factory = new SentenceFactory { IsCrcEnabled = true };
            var sentences = factory.ProcessStream(m_nmeaData);
            var messageFactory = new CommonAisMessageFactory();
            var skippedMessages = 0;
            var totalMessages = 0;
            var totalSentences = 0;

            var statisticsSupported = new Dictionary<int,int>();
            var statisticsNotSupported = new Dictionary<int, int>();
            var maxTime = 0L;
            var maxTimeType = 0;

            var watch = Stopwatch.StartNew();
            var watchSingle = Stopwatch.StartNew();

            foreach (var sentence in sentences)
            {
                totalSentences++;
                watchSingle.Restart();
                bool fragmented;
                var message1 = messageFactory.CreateAisMessage(sentence, out fragmented);
                watchSingle.Stop();

                if (watchSingle.ElapsedMilliseconds > maxTime)
                {
                    maxTime = watchSingle.ElapsedMilliseconds;
                    maxTimeType = message1 != null ? message1.Type : 0;
                }

                if (fragmented)
                {
                    continue;
                }

                if (message1 is UnsupportedMessage)
                {
                    if (!statisticsNotSupported.ContainsKey(message1.Type))
                    {
                        statisticsNotSupported.Add(message1.Type, 0);
                    }

                    statisticsNotSupported[message1.Type] = ++statisticsNotSupported[message1.Type];
                    skippedMessages++;
                    continue;
                }

                if (!statisticsSupported.ContainsKey(message1.Type))
                {
                    statisticsSupported.Add(message1.Type, 0);
                }

                statisticsSupported[message1.Type] = ++statisticsSupported[message1.Type];
                totalMessages++;
            }
            watch.Stop();

            Console.WriteLine(@"Data:{0} bytes", m_nmeaData.Length);
            Console.WriteLine(@"Nmea sentences:{0}", totalSentences);
           

            Console.WriteLine(@"Ais messages:{0}", totalMessages);
            Console.WriteLine(@"Skipped messages:{0}", skippedMessages);
            Console.WriteLine(@"Processed messages:{0}", totalMessages - skippedMessages);
            Console.WriteLine(@"Time:{0} ms", watch.ElapsedMilliseconds);
            Console.WriteLine(@"Time per processed message:{0} ms", (double)watch.ElapsedMilliseconds / (totalMessages - skippedMessages));
            Console.WriteLine(@"Max time per message:{0}", maxTime);
            Console.WriteLine(@"Max time message type:{0}", maxTimeType);

            Console.WriteLine(@"Supported:");
            foreach (var kv in statisticsSupported)
            {
                Console.WriteLine(@"Type:{0} count:{1}", kv.Key, kv.Value);
            }

            Console.WriteLine(@"Not supported:");
            foreach (var kv in statisticsNotSupported)
            {
                Console.WriteLine(@"Type:{0} count:{1}", kv.Key, kv.Value);
            }
        }

        [Test]
        public void TestSentenceFactory()
        {
            var factory = new SentenceFactory {IsCrcEnabled = true};
            var sourceString = "!AIVDM,1,1,,A,13HOI:0P0000VOHLCnHQKwvL05Ip,0*23";
            var bytes = Encoding.ASCII.GetBytes(sourceString);
            var resultString = factory.CreateSentence(bytes).ToNmeaString();

            Assert.AreEqual(sourceString, resultString);
        }

        [Test]
        public void TestAisMessageFactoryFragments()
        {
            var factory = new SentenceFactory { IsCrcEnabled = true };
            var sourceString = "!AIVDM,1,1,,A,13HOI:0P0000VOHLCnHQKwvL05Ip,0*23";
            var bytes = Encoding.ASCII.GetBytes(sourceString);
            var sentence = factory.CreateSentence(bytes);

            var messageFactory = new CommonAisMessageFactory();
            bool fragmented;
            var message1 = messageFactory.CreateAisMessage(sentence, out fragmented);

            Assert.IsFalse(fragmented);

            var fragment1 = "!AIVDM,2,1,8,A,569r?PP000000000000P4UQDr3737000000000000000040000000000,0*08";
            var fragment2 = "!AIVDM,2,2,8,A,000000000000000,2*2C";

            bytes = Encoding.ASCII.GetBytes(fragment1);
            var sentenceF1 = factory.CreateSentence(bytes);

            bytes = Encoding.ASCII.GetBytes(fragment2);
            var sentenceF2 = factory.CreateSentence(bytes);

            message1 = messageFactory.CreateAisMessage(sentenceF1, out fragmented);
            Assert.IsTrue(fragmented);

            message1 = messageFactory.CreateAisMessage(sentenceF2, out fragmented);
            Assert.IsFalse(fragmented);
        }



        [Test]
        public void TestJsonSerializer1()
        {
            const string sourceString = "!AIVDM,1,1,,A,13HOI:0P0000VOHLCnHQKwvL05Ip,0*23";
            var factory = new SentenceFactory { IsCrcEnabled = true };
            var bytes = Encoding.ASCII.GetBytes(sourceString);
            var sentence = factory.CreateSentence(bytes);
            Assert.NotNull(sentence);

            bool fragmented;
            var messageFactory = new CommonAisMessageFactory();

            var m = messageFactory.CreateAisMessage(sentence, out fragmented) as ClassAPositionReportMessage;
            Assert.NotNull(m);
            var serializer = new DataContractJsonSerializer(m.GetType());
            var stream = new MemoryStream();
            serializer.WriteObject(stream, m);
            var bytesS = stream.ToArray();
            stream.Close();
            var str = Encoding.UTF8.GetString(bytesS, 0, bytesS.Length);

            Assert.NotNull(str);
        }
        [Test]
        public void TestMessageType1()
        {
            const string sourceString = "!AIVDM,1,1,,A,13HOI:0P0000VOHLCnHQKwvL05Ip,0*23";
            var factory = new SentenceFactory { IsCrcEnabled = true };
            var bytes = Encoding.ASCII.GetBytes(sourceString);
            var sentence = factory.CreateSentence(bytes);
            Assert.NotNull(sentence);

            bool fragmented;
            var messageFactory = new CommonAisMessageFactory();

            var m = messageFactory.CreateAisMessage(sentence, out fragmented) as ClassAPositionReportMessage;
            Assert.NotNull(m);
            Assert.AreEqual(1, m.Type);
            Assert.AreEqual(0, m.RepeatIndicator);
            Assert.AreEqual(227006760, m.Mmsi);
            Assert.AreEqual(NavigationStatus.UnderWayUsingEngine, m.NavigationStatus);
            
            //TODO:: Chech this, 128 or -128??
            Assert.AreEqual(128, m.RateOfTurn);
            Assert.AreEqual(0.0, m.SpeedOverGround);
            Assert.AreEqual(false, m.PositionAcuracy);
            Assert.AreEqual(0.13138f, m.Longtitude);
            Assert.AreEqual(49.47558f, m.Latitude);
            Assert.AreEqual(36.7f, m.CourseOverGround);
            Assert.AreEqual(511, m.TrueHeading);
            Assert.AreEqual(14, m.UtcSeconds);
            Assert.AreEqual(ManeuverIndicator.NotAvailable, m.ManeuverIndicator);
            Assert.AreEqual(0, m.Spare);
            Assert.AreEqual(false, m.RaimFlag);
        }
        
        
        [Test]
        public void TestMessageType2()
        {
            const string sourceString = "!AIVDM,1,1,,B,24SaQh500G0Cu7nMErpJ680N0@9C,0*3F";
            var factory = new SentenceFactory { IsCrcEnabled = true };
            var bytes = Encoding.ASCII.GetBytes(sourceString);
            var sentence = factory.CreateSentence(bytes);
            Assert.NotNull(sentence);

            bool fragmented;
            var messageFactory = new CommonAisMessageFactory();

            var m = messageFactory.CreateAisMessage(sentence, out fragmented) as ClassAPositionReportMessage;
            Assert.NotNull(m);
            Assert.AreEqual(2, m.Type);
            Assert.AreEqual(0, m.RepeatIndicator);
            Assert.AreEqual(305816000, m.Mmsi);
            Assert.AreEqual(NavigationStatus.Moored, m.NavigationStatus);
            
            //TODO:: Chech this, 128 or -128??
            Assert.AreEqual(0, m.RateOfTurn);
            Assert.AreEqual(2.3f, m.SpeedOverGround);
            Assert.AreEqual(false, m.PositionAcuracy);
            Assert.AreEqual(4.3592450f, m.Longtitude);
            Assert.AreEqual(51.2797318f, m.Latitude);
            Assert.AreEqual(258.4f, m.CourseOverGround);
            Assert.AreEqual(256, m.TrueHeading);
            Assert.AreEqual(15, m.UtcSeconds);
            Assert.AreEqual(ManeuverIndicator.NotAvailable, m.ManeuverIndicator);
            Assert.AreEqual(0, m.Spare);
            Assert.AreEqual(false, m.RaimFlag);
        }

        [Test]
        public void TestMessageType3()
        {
            const string sourceString = "!AIVDM,1,1,,B,33aDqfhP00PD2OnMDdF@QOvN205A,0*13";
            var factory = new SentenceFactory { IsCrcEnabled = true };
            var bytes = Encoding.ASCII.GetBytes(sourceString);
            var sentence = factory.CreateSentence(bytes);
            Assert.NotNull(sentence);

            bool fragmented;
            var messageFactory = new CommonAisMessageFactory();

            var m = messageFactory.CreateAisMessage(sentence, out fragmented) as ClassAPositionReportMessage;
            Assert.NotNull(m);
            Assert.AreEqual(3, m.Type);
            Assert.AreEqual(0, m.RepeatIndicator);
            Assert.AreEqual(244660667, m.Mmsi);
            Assert.AreEqual((NavigationStatus)0, m.NavigationStatus);
            
            
            //TODO:: Chech this, 128 or -128??
            Assert.AreEqual(128, m.RateOfTurn);
            Assert.AreEqual(0.0, m.SpeedOverGround);
            Assert.AreEqual(true, m.PositionAcuracy);
            Assert.AreEqual(4.3775916f, m.Longtitude);
            Assert.AreEqual(51.246227f, m.Latitude);
            Assert.AreEqual(13.3f, m.CourseOverGround);
            Assert.AreEqual(511, m.TrueHeading);
            Assert.AreEqual(15, m.UtcSeconds);
            Assert.AreEqual((ManeuverIndicator)0, m.ManeuverIndicator);
            Assert.AreEqual(0, m.Spare);
            Assert.AreEqual(true, m.RaimFlag);
        }

        [Test]
        public void TestMessageType4()
        {
            const string sourceString = "!AIVDM,1,1,,B,4025;PAuho;N>0NJbfMRhNA00D3l,0*66";
            var factory = new SentenceFactory { IsCrcEnabled = true };
            var bytes = Encoding.ASCII.GetBytes(sourceString);
            var sentence = factory.CreateSentence(bytes);
            Assert.NotNull(sentence);

            bool fragmented;
            var messageFactory = new CommonAisMessageFactory();

            var m = messageFactory.CreateAisMessage(sentence, out fragmented) as BaseStationReportMessage;
            Assert.NotNull(m);
            Assert.AreEqual(4, m.Type);
            Assert.AreEqual(0, m.RepeatIndicator);
            Assert.AreEqual(002182017, m.Mmsi);
           
            //TODO:: Chech this, 128 or -128??
            Assert.AreEqual(2012, m.Year);
            Assert.AreEqual(3, m.Month);
            Assert.AreEqual(14, m.Day);
            Assert.AreEqual(11, m.Hour);
            Assert.AreEqual(30, m.Minute);
            Assert.AreEqual(14, m.Second);
            Assert.AreEqual(false, m.FixQuality);
            Assert.AreEqual(6.644625f, m.Longtitude);
            Assert.AreEqual(51.63028f, m.Latitude);
            Assert.AreEqual(EpfdFixType.Gps, m.PositionFixType);
            Assert.AreEqual(false, m.RaimFlag);
        }

        [Test]
        public void TestMessageType5()
        {
            const string sourceString1 = "!AIVDM,2,1,8,A,53:REn02>cP`?AUK:20<hU10E:10tTqB222222172aq5B4oA0=j1FKml,0*11";
            const string sourceString2 = "!AIVDM,2,2,8,A,5;j1FH888888880,2*77";
            var factory = new SentenceFactory { IsCrcEnabled = true };

            var bytes = Encoding.ASCII.GetBytes(sourceString1);
            var sentence1 = factory.CreateSentence(bytes);
            Assert.NotNull(sentence1);

            bytes = Encoding.ASCII.GetBytes(sourceString2);
            var sentence2 = factory.CreateSentence(bytes);

            bool fragmented;
            var messageFactory = new CommonAisMessageFactory();

            var m = messageFactory.CreateAisMessage(sentence1, out fragmented) as ClassAStaticAndVoyageDataMesage;
            Assert.IsTrue(fragmented);
            Assert.IsNull(m);

            m = messageFactory.CreateAisMessage(sentence2, out fragmented) as ClassAStaticAndVoyageDataMesage;
            Assert.IsFalse(fragmented);
            Assert.NotNull(m);

            Assert.AreEqual(5, m.Type);
            Assert.AreEqual(0, m.RepeatIndicator);
            Assert.AreEqual(212375000, m.Mmsi);
            Assert.AreEqual(0, m.AisVersion);
            Assert.AreEqual(9350666, m.Imo);
            Assert.AreEqual("C4YV2", m.Callsign);
            Assert.AreEqual("CLIPPER POINT", m.VesselName);
            Assert.AreEqual(ShipType.CargoHazardousCategoryA, m.ShipType);
            Assert.AreEqual(21, m.Bow);
            Assert.AreEqual(121, m.Stern);
            Assert.AreEqual(5, m.Port);
            Assert.AreEqual(18, m.Starboard);
            Assert.AreEqual(EpfdFixType.Gps, m.PositionFixType);
            Assert.AreEqual(3, m.EtaMonth);
            Assert.AreEqual(14, m.EtaDay);
            Assert.AreEqual(17, m.EtaHour);
            Assert.AreEqual(0, m.EtaMinute);
            Assert.AreEqual(5.5f, m.Draught);
            Assert.AreEqual("HEY/WPT/HEY", m.Destination);
            Assert.AreEqual(false, m.DataTerminalReady);
            Assert.AreEqual(false, m.Spare);
        }

        [Test]
        public void TestMessageType18()
        {
            const string sourceString = "!AIVDM,1,1,,B,B43JRq00LhTWc5VejDI>wwWUoP06,0*29";
            var factory = new SentenceFactory { IsCrcEnabled = true };
            var bytes = Encoding.ASCII.GetBytes(sourceString);
            var sentence = factory.CreateSentence(bytes);
            Assert.NotNull(sentence);

            bool fragmented;
            var messageFactory = new CommonAisMessageFactory();

            var m = messageFactory.CreateAisMessage(sentence, out fragmented) as ClassBPositionReportMessage;
            Assert.NotNull(m);
            Assert.AreEqual(18, m.Type);
            Assert.AreEqual(0, m.RepeatIndicator);
            Assert.AreEqual(272016100, m.Mmsi);
            Assert.AreEqual(11.5f, m.SpeedOverGround);
            Assert.AreEqual(false, m.PositionAcuracy);
            Assert.AreEqual(31.9989529f, m.Longtitude);
            Assert.AreEqual(46.94412f, m.Latitude);
            Assert.AreEqual(126.3f, m.CourseOverGround);
            Assert.AreEqual(511, m.TrueHeading);
            Assert.AreEqual(15, m.UtcSeconds);
            Assert.AreEqual(true, m.RaimFlag);
            Assert.AreEqual(CsUnitType.ClassBCarrierSense, m.CsUnit);
            Assert.AreEqual(false, m.DisplayFlag);
            Assert.AreEqual(true, m.DscFlag);
            Assert.AreEqual(true, m.BandFlag);
            Assert.AreEqual(true, m.Message22Flag);
            Assert.AreEqual(false, m.AssignedMode);
        }

        [Test]
        public void TestMessageType19()
        {
            const string sourceString = "!AIVDM,1,1,,B,C69DqeP0Ar8;JH3R6<4O7wWPl@:62L>jcaQgh0000000?104222P,0*32";
            var factory = new SentenceFactory { IsCrcEnabled = true };
            var bytes = Encoding.ASCII.GetBytes(sourceString);
            var sentence = factory.CreateSentence(bytes);
            Assert.NotNull(sentence);

            bool fragmented;
            var messageFactory = new CommonAisMessageFactory();

            var m = messageFactory.CreateAisMessage(sentence, out fragmented) as ClassBExtendedPositionReportMessage;
            Assert.NotNull(m);
            Assert.AreEqual(19, m.Type);
            Assert.AreEqual(0, m.RepeatIndicator);
            Assert.AreEqual(412432822, m.Mmsi);
            Assert.AreEqual(7.1f, m.SpeedOverGround);
            Assert.AreEqual(true, m.PositionAcuracy);
            Assert.AreEqual(118.994423f, m.Longtitude);
            Assert.AreEqual(24.6957874f, m.Latitude);
            Assert.AreEqual(49.7f, m.CourseOverGround);
            Assert.AreEqual(511, m.TrueHeading);
            Assert.AreEqual(15, m.UtcSeconds);
            Assert.AreEqual("ZHECANGYU4078@@@@@@@", m.VesselName);
            Assert.AreEqual(ShipType.Fishing, m.ShipType);
            Assert.AreEqual(16, m.Bow);
            Assert.AreEqual(8, m.Stern);
            Assert.AreEqual(4, m.Port);
            Assert.AreEqual(4, m.Starboard);
            Assert.AreEqual(EpfdFixType.Gps, m.PositionFixType);
            Assert.AreEqual(false, m.RaimFlag);
            Assert.AreEqual(true, m.DataTerminalReady);
            Assert.AreEqual(false, m.AssignedMode);
        }

        [Test]
        public void TestMessageType24PartA()
        {
            const string sourceStringTypeA = "!AIVDM,1,1,,A,H7P<1>1LPU@D8U8A<0000000000,2*6C";
            var factory = new SentenceFactory { IsCrcEnabled = true };
            var bytes = Encoding.ASCII.GetBytes(sourceStringTypeA);
            var sentence = factory.CreateSentence(bytes);
            Assert.NotNull(sentence);

            bool fragmented;
            var messageFactory = new CommonAisMessageFactory();

            var m = messageFactory.CreateAisMessage(sentence, out fragmented) as ClassBStaticAndVoyageDataMesageTypeA;
            Assert.NotNull(m);
            Assert.AreEqual(24, m.Type);
            Assert.AreEqual(0, m.PartNumber);
            Assert.AreEqual(503513400, m.Mmsi);
            Assert.AreEqual(0, m.RepeatIndicator);
            Assert.AreEqual("WHITEBIRDS@@@@@@@@@@", m.VesselName);
        }

        [Test]
        public void TestMessageType24PartB()
        {
            const string sourceStringTypeA = "!AIVDM,1,1,,B,H>DQ@04N6DeihhlPPPPPPP000000,0*E";
            var factory = new SentenceFactory { IsCrcEnabled = true };
            var bytes = Encoding.ASCII.GetBytes(sourceStringTypeA);
            var sentence = factory.CreateSentence(bytes);
            Assert.NotNull(sentence);

            bool fragmented;
            var messageFactory = new CommonAisMessageFactory();

            var m = messageFactory.CreateAisMessage(sentence, out fragmented) as ClassBStaticAndVoyageDataMesageTypeB;
            Assert.NotNull(m);
            Assert.AreEqual(24, m.Type);
            Assert.AreEqual(1, m.PartNumber);
            Assert.AreEqual(961040384, m.Mmsi);
            Assert.AreEqual(0, m.RepeatIndicator);
            Assert.AreEqual(ShipType.Fishing, m.ShipType);
            Assert.AreEqual("FT-", m.VendorId);
            Assert.AreEqual(string.Empty, m.Callsign);
            Assert.AreEqual(0, m.Bow);
            Assert.AreEqual(0, m.Stern);
            Assert.AreEqual(0, m.Port);
            Assert.AreEqual(0, m.Starboard);
        }
    }
}
