using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Google;

namespace GoogleCalenderTest
{
    [TestClass]
    public class GoogleCalenderTestUnitTest
    {
        [TestMethod]
        public void TestToday()
        {
            // arrange
            int expectedValue = 1;
            Enums.Period enumToTest = Enums.Period.Today;
            string enumName = Enum.GetName(enumToTest.GetType(), enumToTest);

            // act  
            int actualValue =
                Google.PeriodCalc.GetDateTime(enumToTest, DayOfWeek.Monday, new DateTime(2018, 1, 1)).Day;

            // assert 
            Assert.AreEqual(expectedValue, actualValue, string.Format("{0} period calculation is invalid.", enumName));
        }

        [TestMethod]
        public void TestTomorrow()
        {
            // arrange
            int expectedValue = 2;
            Enums.Period enumToTest = Enums.Period.Tomorrow;
            string enumName = Enum.GetName(enumToTest.GetType(), enumToTest);

            // act  
            int actualValue =
                Google.PeriodCalc.GetDateTime(enumToTest, DayOfWeek.Monday, new DateTime(2018, 1, 1)).Day;

            // assert 
            Assert.AreEqual(expectedValue, actualValue, string.Format("{0} period calculation is invalid.", enumName));
        }

        [TestMethod]
        public void TestDayAfterTomorrow()
        {
            // arrange
            int expectedValue = 3;
            Enums.Period enumToTest = Enums.Period.DayAfterTomorrow;
            string enumName = Enum.GetName(enumToTest.GetType(), enumToTest);

            // act  
            int actualValue =
                Google.PeriodCalc.GetDateTime(enumToTest, DayOfWeek.Monday, new DateTime(2018, 1, 1)).Day;

            // assert 
            Assert.AreEqual(expectedValue, actualValue, string.Format("{0} period calculation is invalid.", enumName));
        }


        /// <summary>
        /// Checks dates and days with specified enum values
        /// </summary>
        [TestMethod]
        public void TestDays()
        {
            // arrange
            DayOfWeek expectedValue;
            DateTime testDate = new DateTime(2017, 12, 31);
            int expectedValue2 = 0;
            if (expectedValue2 == 0) expectedValue2 = 7; // Sundays date is 7

            foreach (DayOfWeek day in Enum.GetValues(typeof(DayOfWeek)))
            {
                // arrange 
                expectedValue = day;
                
                Enums.Period enumToTest = (Enums.Period)day;
                string enumName = Enum.GetName(enumToTest.GetType(), enumToTest);

                // act  
                DayOfWeek actualValue =
                    Google.PeriodCalc.GetDateTime(enumToTest, DayOfWeek.Monday, testDate).DayOfWeek;
                int actualValue2 =
                    Google.PeriodCalc.GetDateTime(enumToTest, DayOfWeek.Monday, testDate).Day;
                
                // assert 
                Assert.AreEqual(expectedValue, actualValue, string.Format("{0} period calculation is invalid.", enumName));
                Assert.AreEqual(expectedValue2, actualValue2, string.Format("{0} period calculation is invalid.", "Day"));

                if (expectedValue2 == 7) expectedValue2 = 0;
                expectedValue2++; // Increase date each day starting monday, where Monday is the 1st.
            }
        }

        [TestMethod]
        public void TestNextWeek()
        {
            // arrange
            int expectedValue = 8;
            Enums.Period enumToTest = Enums.Period.NextWeek;
            string enumName = Enum.GetName(enumToTest.GetType(), enumToTest);

            // act  
            int actualValue =
                Google.PeriodCalc.GetDateTime(enumToTest, DayOfWeek.Monday, new DateTime(2018, 1, 1)).Day;

            // assert 
            Assert.AreEqual(expectedValue, actualValue, string.Format("{0} period calculation is invalid.", enumName));
        }

        [TestMethod]
        public void TestNextMonth()
        {
            // arrange
            int expectedValue = 2;
            Enums.Period enumToTest = Enums.Period.NextMonth;
            string enumName = Enum.GetName(enumToTest.GetType(), enumToTest);

            // act  
            int actualValue =
                Google.PeriodCalc.GetDateTime(enumToTest, DayOfWeek.Monday, new DateTime(2018, 1, 1)).Month;

            // assert 
            Assert.AreEqual(expectedValue, actualValue, string.Format("{0} period calculation is invalid.", enumName));
        }

        [TestMethod]
        public void TestNextYear()
        {
            // arrange
            int expectedValue = 2019;
            Enums.Period enumToTest = Enums.Period.NextYear;
            string enumName = Enum.GetName(enumToTest.GetType(), enumToTest);

            // act  
            int actualValue =
                Google.PeriodCalc.GetDateTime(enumToTest, DayOfWeek.Monday, new DateTime(2018, 1, 1)).Year;

            // assert 
            Assert.AreEqual(expectedValue, actualValue, string.Format("{0} period calculation is invalid.", enumName));
        }
    }
}
