using Application.Common.Utils;
using NUnit.Framework;

namespace Application.UnitTests.Common.Utils
{
    public class RoundTests
    {
        [Test]
        public void Amount_ToNearestSevenCents_ReturnsCorrectResult()
        {
            // Arrange
            const decimal valueToRound = 0.14147M;
            const decimal stepAmount = 0.07M;
            const decimal expectedResult = 0.21M;

            // Act
            var result = Round.Amount(valueToRound, stepAmount, Round.Type.Up);
            
            // Assert
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void Amount_ToNearestFiveCents_ReturnsCorrectResult()
        {
            // Arrange
            const decimal valueToRound = 0.14147M;
            const decimal stepAmount = 0.05M;
            const decimal expectedResult = 0.15M;

            // Act
            var result = Round.Amount(valueToRound, stepAmount, Round.Type.Up);
            
            // Assert
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void Amount_ToNearestOneCent_ReturnsCorrectResult()
        {
            // Arrange
            const decimal valueToRound = 0.14147M;
            const decimal stepAmount = 0.01M;
            const decimal expectedResult = 0.15M;

            // Act
            var result = Round.Amount(valueToRound, stepAmount, Round.Type.Up);
            
            // Assert
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void Amount_ToNearestFourCents_ReturnsCorrectResult()
        {
            // Arrange
            const decimal valueToRound = 0.14147M;
            const decimal stepAmount = 0.04M;
            const decimal expectedResult = 0.16M;

            // Act
            var result = Round.Amount(valueToRound, stepAmount, Round.Type.Up);
            
            // Assert
            Assert.AreEqual(expectedResult, result);
        }
    }
}
