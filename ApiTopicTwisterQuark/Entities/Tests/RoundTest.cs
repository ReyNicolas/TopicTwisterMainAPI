using NUnit.Framework;

namespace ApiTopicTwisterQuark.Entities.Tests
{
    public class RoundTest
    {
        private List<string> categories;
        private string matchID;
        private int id;
        private char letter;
        string winnerID;
        private bool tie;
        private float time;
        
        [SetUp]
        public void Setup()
        {
            categories = new List<string>(){"Category"};
            matchID = "Match";
            id = 1;
            letter = 'A';
            winnerID = "";
            tie = false;
            time = 60;
            
        }

        [Test]
        public void GetActualTurn_ThereIsNoneTurnsOver_ReturnFirst()
        {
            var expectedTurn = new Turn(matchID,id,1,"player1", time, false, 0);
            var turn2 = new Turn(matchID,id,2,"player2", time, false, 0);
            var round = new Round(matchID, id, categories, letter, winnerID, tie, time);
            round.Turns = new List<Turn>() { expectedTurn, turn2 };

            var result = round.GetActualTurn();

            Assert.AreEqual(expectedTurn,result);
        }
        [Test]
        public void GetActualTurn_FirstTurnIsOver_ReturnSecond()
        {
            var turn1 = new Turn(matchID,id,1,"player1", time, true, 3);
            var expectedTurn = new Turn(matchID,id,2,"player2", time, false, 0);
            var round = new Round(matchID, id, categories, letter, winnerID, tie, time);
            round.Turns = new List<Turn>() { turn1, expectedTurn };

            var result = round.GetActualTurn();

            Assert.AreEqual(expectedTurn,result);
        }
        
       [Test]
        public void AreTurnsOver_ThererAreAnyTurnNotOver_ReturnFalse()
        {
            var turn = new Turn(matchID,id,1,"player", time, false, 0);
            var round = new Round(matchID,id, categories, letter, winnerID, tie, time)
            {
                Turns = new List<Turn>() { turn }
            };

            var result = round.AreTurnsOver();
            
            Assert.IsFalse(result);
        }

        [Test]
        public void AreTurnsOver_AllTurnsAreOver_ReturnTrue()
        {
            var turn1 = new Turn(matchID,id,1,"player1", time, true, 3);
            var turn2 = new Turn(matchID,id,2,"player2", time, true, 2);
           
            var round = new Round(matchID,id, categories, letter, winnerID, tie, time)
            {
                Turns = new List<Turn>() { turn1, turn2 }
            };

            var result = round.AreTurnsOver();
            
            Assert.IsTrue(result);
        }

        [Test]
        public void GetTurnOfPlayerID_ReturnTurnOfPlayerID()
        {
            var playerIDToCheck = "player1"; 
            var expectedTurn = new Turn(matchID,id,1,playerIDToCheck, time, true, 3);
            var turn2 = new Turn(matchID,id,2,"player2", time, true, 2);
            
            var round = new Round(matchID,id, categories, letter, winnerID, tie, time)
            {
                Turns = new List<Turn>() { expectedTurn, turn2 }
            };

            var result = round.GetTurnOfPlayerID(playerIDToCheck);
            
            Assert.AreEqual(expectedTurn, result);
        }
        [Test]
        public void GetRivalIDFromTurns_ReturnRivalID()
        {
            var playerID= "player1";
            var rivalIDToCheck = "player2";
            var turn1 = new Turn(matchID,id,1,playerID, time, true, 3);
            var turn2 = new Turn(matchID,id,2,rivalIDToCheck, time, true, 2);
            
            var round = new Round(matchID,id, categories, letter, winnerID, tie, time)
            {
                Turns = new List<Turn>() { turn1, turn2 }
            };

            var result = round.GetRivalIDFromTurns(playerID);
            
            Assert.AreEqual(rivalIDToCheck, result);
        }
        [Test]
        public void SetOver_SetRoundWinnerIDPlayerWithMostCorrectCount()
        {
            var expectedWinnerID= "player1";
            var rivalID = "player2";
            var correctCount1 = 2;
            var correctCount2 = 1;
            Assert.IsTrue(correctCount1>correctCount2);
            var turn1 = new Turn(matchID,id,1,expectedWinnerID, time, true, correctCount1);
            var turn2 = new Turn(matchID,id,2,rivalID, time, true, correctCount2);
            var round = new Round(matchID,id, categories, letter, winnerID, tie, time)
            {
                Turns = new List<Turn>() { turn1, turn2 }
            };
            
            round.SetOver();
            var result = round.WinnerID;
            
            
            Assert.AreEqual(expectedWinnerID,result);
        }
        [Test]
        public void SetOver_SetRoundWinnerIDPlayerWithMostTimeLeftInCaseOfCorrectCountTie()
        {
            var expectedWinnerID= "player1";
            var rivalID = "player2";
            var correctCount1 = 2;
            var correctCount2 = correctCount1;
            var timeLeft1 = time;
            var timeLeft2 = time - 1;
            Assert.IsTrue(timeLeft1>timeLeft2);
            var turn1 = new Turn(matchID,id,1,expectedWinnerID, timeLeft1, true, correctCount1);
            var turn2 = new Turn(matchID,id,2,rivalID, timeLeft2, true, correctCount2);
            var round = new Round(matchID,id, categories, letter, winnerID, tie, time)
            {
                Turns = new List<Turn>() { turn1, turn2 }
            };
            
            round.SetOver();
            var result = round.WinnerID;
            
            
            Assert.AreEqual(expectedWinnerID,result);
        }
        [Test]
        public void SetOver_SetRoundTieWhenPlayersTieInCorrectCountsAndTimeLeft()
        {
            var playerID= "player1";
            var rivalID = "player2";
            var correctCount1 = 2;
            var correctCount2 = correctCount1;
            var timeLeft1 = time;
            var timeLeft2 = timeLeft1;
            var turn1 = new Turn(matchID,id,1,playerID, timeLeft1, true, correctCount1);
            var turn2 = new Turn(matchID,id,2,rivalID, timeLeft2, true, correctCount2);
            var round = new Round(matchID,id, categories, letter, winnerID, tie, time)
            {
                Turns = new List<Turn>() { turn1, turn2 }
            };
            
            round.SetOver();
            var result = round.Tie;
            
            
            Assert.IsTrue(result);
        }
        
        [Test]
        public void IsPlayerTheWinner_RoundWinnerIDIsEqual_ReturnTrue()
        {
            var expectedWinnerID= "player1";
            var rivalID = "player2";
            var correctCount1 = 3;
            var correctCount2 = 2;
            var timeLeft1 = time;
            var timeLeft2 = timeLeft1;
            var turn1 = new Turn(matchID,id,1,expectedWinnerID, timeLeft1, true, correctCount1);
            var turn2 = new Turn(matchID,id,2,rivalID, timeLeft2, true, correctCount2);
            var round = new Round(matchID,id, categories, letter, winnerID, tie, time)
            {
                Turns = new List<Turn>() { turn1, turn2 }
            };
            
            round.SetOver();
            var result = round.IsPlayerTheWinner(expectedWinnerID);
            
            Assert.IsTrue(result);
        }
        [Test]
        public void IsPlayerTheWinner_RoundWinnerIDIsNotEqual_ReturnFalse()
        {
            var playerIDToCheck= "player1";
            var rivalID = "player2";
            var correctCount1 = 2;
            var correctCount2 = 3;
            var timeLeft1 = time;
            var timeLeft2 = timeLeft1;
            var turn1 = new Turn(matchID,id,1,playerIDToCheck, timeLeft1, true, correctCount1);
            var turn2 = new Turn(matchID,id,2,rivalID, timeLeft2, true, correctCount2);
            var round = new Round(matchID,id, categories, letter, winnerID, tie, time)
            {
                Turns = new List<Turn>() { turn1, turn2 }
            };
            
            round.SetOver();
            var result = round.IsPlayerTheWinner(playerIDToCheck);
            
            Assert.IsFalse(result);
        }
        
    }
}
