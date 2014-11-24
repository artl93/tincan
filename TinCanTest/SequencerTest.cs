using TinCan;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace AudioLibraryTests
{
    
    
    /// <summary>
    ///This is a test class for MusicSequencerTest and is intended
    ///to contain all MusicSequencerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SequencerTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for Tempo
        ///</summary>
        [TestMethod()]
        public void TempoPositionTest41k()
        {
            const int sampleRate = 44100;
            Sequencer seq = new Sequencer(sampleRate, 120.0F);
            EventLocation expectedStart = new EventLocation();
            Assert.AreEqual(seq.CurrentPosition, expectedStart);
            seq.Start();
            seq.UpdatePosition(0, sampleRate);
            expectedStart.Beat = 2;
            Assert.AreEqual(seq.CurrentPosition, expectedStart);
        }


        /// <summary>
        ///A test for Tempo
        ///</summary>
        [TestMethod()]
        public void TempoPositionTest48k()
        {
            const int sampleRate = 48000;
            Sequencer seq = new Sequencer(sampleRate, 120.0F);
            EventLocation expectedStart = new EventLocation();
            Assert.AreEqual(seq.CurrentPosition, expectedStart);
            seq.Start();
            seq.UpdatePosition(0, sampleRate);
            expectedStart.Beat = 2;
            Assert.AreEqual(seq.CurrentPosition, expectedStart);
        }

        /// <summary>
        ///A test for Tempo
        ///</summary>
        [TestMethod()]
        public void TempoPositionTest96k()
        {
            const int sampleRate = 96000;
            Sequencer seq = new Sequencer(sampleRate, 120.0F);
            EventLocation expectedStart = new EventLocation();
            Assert.AreEqual(seq.CurrentPosition, expectedStart);
            seq.Start();
            seq.UpdatePosition(0, sampleRate);
            expectedStart.Beat = 2;
            Assert.AreEqual(seq.CurrentPosition, expectedStart);
        }
        
       
        /// <summary>
        ///A test for AddTrack
        ///</summary>
        [TestMethod()]
        public void AddTrackTest()
        {
            const int sampleRate = 96000;
            const double tempo = 120.0D;
            Sequencer target = new Sequencer(sampleRate, tempo); // TODO: Initialize to an appropriate value
            SequencerTrack track = SequencerTrack.Create(null, 4, 2); 
            string name = "Test"; 
            target.AddTrack(name, track);
        }

        /// <summary>
        ///A test for AddEvent
        ///</summary>
        [TestMethod()]
        public void AddEventWithoutAddingTrackTest()
        {
            const int sampleRate = 96000;
            const double tempo = 120.0D;
            Sequencer target = new Sequencer(sampleRate, tempo); // TODO: Initialize to an appropriate value
            SequencerTrack track = SequencerTrack.Create(null, 4, 2); 
            EventLocation location = new EventLocation(); // TODO: Initialize to an appropriate value
            track.AddEvent(location, 0);            
        }

        /// <summary>
        ///A test for AddEvent
        ///</summary>
        [TestMethod()]
        public void AddEventWithTrackTest()
        {
            const int sampleRate = 96000;
            const double tempo = 120.0D;
            Sequencer target = new Sequencer(sampleRate, tempo); // TODO: Initialize to an appropriate value
            SequencerTrack track = SequencerTrack.Create(null, 4, 2);
            EventLocation location = new EventLocation(); // TODO: Initialize to an appropriate value
            string name = "Test";
            target.AddTrack(name, track);
            track.AddEvent(location, 0);
        }


        private void AddEightQuarterNotesToTrack(SequencerTrack track)
        {
            for (int meas = 0; meas < 2; meas++)
            {
                for (int beat = 0; beat < 4; beat++)
                {
                    var loc = new EventLocation();
                    loc.Beat = beat;
                    loc.Measure = meas;
                    track.AddEvent(loc, 0);
                }
            }
        }


        private void Add16EighthNotesToTrack(SequencerTrack track)
        {
            var loc = new EventLocation();
            for (int i = 0; i < 16; i++)
            {
                track.AddEvent(loc, 0);
                loc.AddBeats(0.5D, 4);
            }
        }

        public void TestEighthNotes(int sampleRate, double tempo, int frameSize)
        {
            double samplesPerNote = ((sampleRate * 60.0) / (tempo * 2));
            int count = 0;
            Sequencer seq = new Sequencer(sampleRate, tempo); // TODO: Initialize to an appropriate value
            MockNotePlayer player = new MockNotePlayer(
                delegate(AudioEventInfo info)
                {
                    int expectedSample = (int)(count * samplesPerNote);
                    int actualSample = (info.frame * frameSize) + info.sampleOffset;
                    Assert.AreEqual(expectedSample, actualSample);
                    count++;
                });
            SequencerTrack track = SequencerTrack.Create(player, 4, 2);
            string name = "Test";
            seq.AddTrack(name, track);
            Add16EighthNotesToTrack(track);
            seq.Start();


            int countOfSamplesNeeded = (int)(samplesPerNote * 16);
            int samplesRendered = 0;
            int frame = 0;

            while (samplesRendered < countOfSamplesNeeded)
            {
                seq.UpdatePosition(frame, frameSize);
                frame++;
                samplesRendered += frameSize;
            }

        }

        public void TestQuarterNotes(int sampleRate, double tempo, int frameSize)
        {
            double samplesPerBeat = ((sampleRate * 60.0) / tempo);
            int count = 0;
            Sequencer seq = new Sequencer(sampleRate, tempo); // TODO: Initialize to an appropriate value
            MockNotePlayer player = new MockNotePlayer(
                delegate(AudioEventInfo info) 
                {
                    int expectedSample = (int) Math.Round(count * samplesPerBeat, 0, MidpointRounding.AwayFromZero);
                    int actualSample = (info.frame * frameSize) + info.sampleOffset;
                    Assert.AreEqual(expectedSample, actualSample);
                    count++;
                });
            SequencerTrack track = SequencerTrack.Create(player, 4, 2);
            string name = "Test";
            seq.AddTrack(name, track);
            AddEightQuarterNotesToTrack(track);

            int countOfSamplesNeeded = (int)(samplesPerBeat * 8);
            int samplesRendered = 0;
            int frame = 0;
            seq.Start();


            while (samplesRendered < countOfSamplesNeeded)
            {
                seq.UpdatePosition(frame, frameSize);
                frame++;
                samplesRendered += frameSize;
            }

        }

        [TestMethod()]
        public void RenderQuarterNotesAt96k120bpm256bytesTest()
        {
            TestQuarterNotes(96000, 120, 256);
        }

        [TestMethod()]
        public void RenderQuarterNotesAt41k120bpm256bytesTest()
        {
            TestQuarterNotes(44100, 120, 256);
        }

        [TestMethod()]
        public void RenderQuarterNotesAt41k91bpm256bytesTest()
        {
            TestQuarterNotes(44100, 91, 256);
        }

        [TestMethod()]
        public void RenderEighthNotesAt41k120bpm256bytesTest()
        {
            TestEighthNotes(44100, 120, 256);
        }

        
        class MockNotePlayer : IInstrument
        {
            private Action<AudioEventInfo> _onPlayCallback;

            public MockNotePlayer(Action<AudioEventInfo> onPlayCallback)
            {
                _onPlayCallback = onPlayCallback;
            }
            public void Play(AudioEventInfo eventInfo)
            {
                _onPlayCallback(eventInfo);
            }


            public void SetBlockSize(int channels, int sampleRate)
            {
                throw new NotImplementedException();
            }

            public bool WriteToOutput(short[] outData, int sampleOffset, int sampleCount, int frame)
            {
                throw new NotImplementedException();
            }
        }
    }
}
