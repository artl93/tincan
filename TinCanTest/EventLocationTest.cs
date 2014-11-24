using TinCan;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AudioLibraryTests
{
    
    
    /// <summary>
    ///This is a test class for EventLocationTest and is intended
    ///to contain all EventLocationTest Unit Tests
    ///</summary>
    [TestClass()]
    public class EventLocationTest
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
        ///A test for CompareTo
        ///</summary>
        [TestMethod()]
        public void CompareToBeatTest()
        {
            EventLocation target = new EventLocation() { Beat = 1, Div = 0.0f, Measure = 0 }; 
            EventLocation other = new EventLocation() { Beat = 1, Div = 0.0f, Measure = 0 };
            int expected = 0; 
            int actual;
            actual = target.CompareTo(other);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for CompareTo
        ///</summary>
        [TestMethod()]
        public void CompareToMeasureTest()
        {
            EventLocation target = new EventLocation() { Beat = 0, Div = 0.0f, Measure = 1 };
            EventLocation other = new EventLocation() { Beat = 0, Div = 0.0f, Measure = 1 };
            int expected = 0;
            int actual;
            actual = target.CompareTo(other);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for CompareTo
        ///</summary>
        [TestMethod()]
        public void CompareToDivTest()
        {
            EventLocation target = new EventLocation() { Beat = 1, Div = 0.1f, Measure = 0 };
            EventLocation other = new EventLocation() { Beat = 1, Div = 0.1f, Measure = 0 };
            int expected = 0;
            int actual;
            actual = target.CompareTo(other);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for CompareTo
        ///</summary>
        [TestMethod()]
        public void CompareToBeatGreaterTest()
        {
            EventLocation target = new EventLocation() { Beat = 1, Div = 0.0f, Measure = 0 };
            EventLocation other = new EventLocation() { Beat = 0, Div = 0.0f, Measure = 0 };
            int expected = 1;
            int actual;
            actual = target.CompareTo(other);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for CompareTo
        ///</summary>
        [TestMethod()]
        public void CompareToBeatLessTest()
        {
            EventLocation target = new EventLocation() { Beat = 2, Div = 0.0f, Measure = 0 };
            EventLocation other = new EventLocation() { Beat = 1, Div = 0.0f, Measure = 0 };
            int expected = 1;
            int actual;
            actual = target.CompareTo(other);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for CompareTo
        ///</summary>
        [TestMethod()]
        public void CompareToMeasureGreaterTest()
        {
            EventLocation target = new EventLocation() { Beat = 0, Div = 0.0f, Measure = 1 };
            EventLocation other = new EventLocation() { Beat = 0, Div = 0.0f, Measure = 0 };
            int expected = 1;
            int actual;
            actual = target.CompareTo(other);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for CompareTo
        ///</summary>
        [TestMethod()]
        public void CompareToDivGreaterTest()
        {
            EventLocation target = new EventLocation() { Beat = 1, Div = 0.1f, Measure = 0 };
            EventLocation other = new EventLocation() { Beat = 1, Div = 0.0f, Measure = 0 };
            int expected = 1;
            int actual;
            actual = target.CompareTo(other);
            Assert.AreEqual(expected, actual);
        }


        /// <summary>
        ///A test for AddBeats
        ///</summary>
        [TestMethod()]
        public void AddBeatsWholeTest()
        {
            EventLocation target = new EventLocation(); // TODO: Initialize to an appropriate value
            double spanBeats = 1.0D; // TODO: Initialize to an appropriate value
            int beatsPerMeasure = 4; // TODO: Initialize to an appropriate value
            target.AddBeats(spanBeats, beatsPerMeasure);
            int expectedBeats = 1;
            Assert.AreEqual(expectedBeats, target.Beat);
        }


        /// <summary>
        ///A test for AddBeats
        ///</summary>
        [TestMethod()]
        public void AddBeatsPartialTest()
        {
            EventLocation target = new EventLocation(); // TODO: Initialize to an appropriate value
            double spanBeats = 0.5D; // TODO: Initialize to an appropriate value
            int beatsPerMeasure = 4; // TODO: Initialize to an appropriate value
            target.Div = 0.5F;
            target.AddBeats(spanBeats, beatsPerMeasure);
            int expectedBeats = 1;
            double expectedRemainder = 0.0D;
            Assert.AreEqual(target.Beat, expectedBeats);
            Assert.AreEqual(target.Div, expectedRemainder);
        }

        /// <summary>
        ///A test for AddBeats
        ///</summary>
        [TestMethod()]
        public void AddBeatsPartialWithRemainderTest()
        {
            EventLocation target = new EventLocation(); 
            double spanBeats = 0.5D; 
            int beatsPerMeasure = 4; 
            target.Div = 0.6F;
            target.AddBeats(spanBeats, beatsPerMeasure);
            int expectedBeats = 1;
            double expectedRemainder = 0.1D;
            Assert.AreEqual(target.Beat, expectedBeats);
            Assert.IsTrue(target.Div >= expectedRemainder);
        }
    }
}
