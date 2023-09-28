using NUnit.Framework;

namespace ApiTopicTwisterQuark.Entities.Tests
{
    public class MatchTest
    {
        private List<string> categories;
        private int id;
        private char letter;
        string winnerID;
        private bool tie;
        private float time;
        
        [SetUp]
        public void Setup()
        {
            categories = new List<string>(){"Category"};
            id = 1;
            letter = 'A';
            winnerID = "";
            tie = false;
            time = 60;
        }
        
        [Test]
        public void GetNumberOfRounds()
        {
            var players = new List<string>() { "player1", "player2" };
            var matchID = "match";

            var round = new List<Round>()
            {
                 new Round(matchID,id, categories, letter, winnerID, tie, time),
                 new Round(matchID,id, categories, letter, winnerID, tie, time)
                
            };
            var expectedNumber = round.Count;

            var match = new Match(matchID, players, "", false)
            {
                Rounds = round
            };
            var result = match.GetNumberOfRounds();
            
            Assert.AreEqual(expectedNumber, result);
        }

        [Test]
        public void GetActualRound_FirstRoundIsNotOver_ReturnFirst()
        {
            var players = new List<string>() { "player1", "player2" };
            var matchID = "Match";
            var round1 = new Round(matchID,id, categories, letter, winnerID, tie, time);
            var turnRound1 = new Turn(matchID,round1.ID,1,"player1", time, false, 0);
            round1.Turns = new List<Turn>() { turnRound1 };
         
            var round2 = new Round(matchID,2, categories, letter, winnerID, tie, time);
            
            var expectedRound = round1;

            var match = new Match(matchID, players, "", false)
            {
                Rounds = new List<Round>(){round1,round2}
            };
            
            var result = match.GetActualRound();
            
            Assert.AreEqual(expectedRound, result);          
        }
        
        [Test]
        public void GetActualRound_FirstRoundIsOverAndSecondNot_ReturnSecond()
        {
            var players = new List<string>() { "player1" };
            var matchID = "Match";
            var round1 = new Round(matchID,id, categories, letter, winnerID, tie, time);
            var turnRound1 = new Turn(matchID,round1.ID,1,"player1", time, true, 2);
            round1.Turns = new List<Turn>() { turnRound1 };
            round1.SetOver();
            
            
            
           var round2 = new Round(matchID,2, categories, letter, winnerID, tie, time);
            var turnRound2 = new Turn(matchID,round2.ID,1,"player1", time, false, 0);
            round2.Turns = new List<Turn>() { turnRound2 };
            var expectedRound = round2;

            var match = new Match("match", players, "", false)
            {
                Rounds = new List<Round>(){round1,round2}
            };
           
            //Act
            var result = match.GetActualRound();
            //Arrange
            Assert.AreEqual(expectedRound, result);          
        }

        [Test]
        public void AreRoundsOver_NotAllRoundsAreOver_ReturnFalse()
        {
            var players = new List<string>() { "player1", "player2" };
            var matchID = "Match";
            var round1 = new Round(matchID,id, categories, letter, winnerID, tie, time);
            var turnRound1 = new Turn(matchID,round1.ID,1,"player1", time, true, 2);
            round1.Turns = new List<Turn>() { turnRound1 };
            round1.SetOver();
            
            var round2 = new Round(matchID,2, categories, letter, winnerID, tie, time);
            var turnRound2 = new Turn(matchID,round2.ID,1,"player1", time, false, 0);
            round2.Turns = new List<Turn>() { turnRound2 };
            

            var match = new Match("match", players, "", false)
            {
                Rounds = new List<Round>(){round1,round2}
            };
           
            //Act
            var result = match.AreRoundsOver();
            //Arrange
            Assert.IsFalse(result);          
        }
        [Test]
        public void AreRoundsOver_AllRoundsAreOver_ReturnTrue()
        {
            var matchID = "Match";
            var players = new List<string>() { "player1", "player2" };
            var round1 = new Round(matchID,id, categories, letter, winnerID, tie, time);
            var turnRound1 = new Turn(matchID,round1.ID,1,"player1", time, true, 2);
            round1.Turns = new List<Turn>() { turnRound1 };
            round1.SetOver();
            
            var round2 = new Round(matchID,2, categories, letter, winnerID, tie, time);
            var turnRound2 = new Turn(matchID,round2.ID,1,"player1", time, true, 2);
            round2.Turns = new List<Turn>() { turnRound2 };
            round2.SetOver();

            
            
            var match = new Match("match", players, "", false)
            {
                Rounds = new List<Round>(){round1,round2}
            };
           
            //Act
            var result = match.AreRoundsOver();
            //Arrange
            Assert.IsTrue(result);          
        }
    }
}
