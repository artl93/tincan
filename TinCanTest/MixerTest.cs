using TinCan;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AudioLibraryTests
{
    
    
    /// <summary>
    ///This is a test class for AudioComponentContainerTest and is intended
    ///to contain all AudioComponentContainerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class MixerTest
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


        internal virtual Mixer CreateAudioComponentContainer()
        {
            // TODO: Instantiate an appropriate concrete class.
            var target = Mixer.Create();
            target.AddComponent("sine generator1", WaveformGenerator.Create(Waveform.Sine));
            target.AddComponent("sine generator2", WaveformGenerator.Create(Waveform.Sine));
            return target;
        }

        /// <summary>
        ///A test for WriteToOutput
        ///</summary>
        [TestMethod()]
        public void WriteToOutputSummingSineTest()
        {
            const int sampleRate = 44100;
            const int channels = 2;
            var target = Mixer.Create();
            target.SetBlockSize(channels, sampleRate);
            var generator1 = WaveformGenerator.Create(Waveform.Nothing);
            var generator2 = WaveformGenerator.Create(Waveform.Sine);
            generator2.Play = generator1.Play = true;
            target.AddComponent("sine generator1", generator1);
            target.AddComponent("sine generator2", generator2);
            var expectedResultGenerator = WaveformGenerator.Create(Waveform.Sine);
            expectedResultGenerator.SetBlockSize(channels, sampleRate);
            expectedResultGenerator.Play = generator2.Play = generator1.Play = true;

            var expectedData = new short[256 * channels];
            expectedResultGenerator.WriteToOutput(expectedData, 0, expectedData.Length, 0);

            var outData = new short[256 * channels]; // TODO: Initialize to an appropriate value
            int offset = 0; // TODO: Initialize to an appropriate value
            int length = outData.Length; // TODO: Initialize to an appropriate value
            int frame = 0; // TODO: Initialize to an appropriate value
            target.WriteToOutput(outData, offset, length, frame);

            for (int i = 0; i < (expectedData.Length); i+=sizeof(short))
            {
                var expected = expectedData[i];
                var actual = outData[i];
                Assert.IsTrue(expected == actual, string.Format("expected = {0}; actual = {1}", expected, actual));
            }
        }
    }
}
