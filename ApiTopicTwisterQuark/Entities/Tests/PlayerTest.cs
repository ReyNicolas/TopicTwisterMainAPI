using NUnit.Framework;

namespace ApiTopicTwisterQuark.Entities.Tests
{
    public class PlayerTest
    {
        [Test]
        public void AddVictoryPoints_WhenPointsAreZeroAndAddZeroValue_DontChange()
        {
            string id = "Player";
            string password = "Password";
            int value = 0;
            Player player = new Player(id,0,password);
            var expectedResult = player.VictoryPoints;


            player.AddVictoryPoints(value);
            var result = player.VictoryPoints;

            Assert.AreEqual(expectedResult, result);
        }



        [Test]
        public void AddVictoryPoints_WhenPointsAreZeroAndAddTen_ReturnTen()
        {
            string id = "Topic";
            string password = "Password";
            int value = 10;
            Player player = new Player(id,0,password);


            player.AddVictoryPoints(value);
            var result = player.VictoryPoints;

            Assert.AreEqual(value, result);
        }

        [Test]
        public void AddVictoryPoints_WhenPointsAreNotZeroAndAddAValue_ReturnTheSumOfPointsAndValue()
        {
            string id = "Topic";
            int value = 10;
            string password = "Password";
            Player player = new Player(id,0,password);
            player.AddVictoryPoints(3);
            var expectedResult = value + 3;

            player.AddVictoryPoints(value);
            var result = player.VictoryPoints;

            Assert.AreEqual(expectedResult, result);
        }
    }
}
