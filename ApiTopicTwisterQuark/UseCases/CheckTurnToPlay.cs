using ApiTopicTwisterQuark.Entities;
using ApiTopicTwisterQuark.Entities.DTOs;
using ApiTopicTwisterQuark.Repositories;
using ApiTopicTwisterQuark.Repositories.Interfaces;

namespace ApiTopicTwisterQuark.UseCases
{
    public class CheckTurnToPlay
    {
        private IMatchRepository matchRepository;
        private IRoundRepository roundRepository;
        private ITurnRepository turnRepository;

        public CheckTurnToPlay(IMatchRepository matchRepository, IRoundRepository roundRepository, ITurnRepository turnRepository)
        {
            this.matchRepository = matchRepository;
            this.roundRepository = roundRepository;
            this.turnRepository = turnRepository;
        }

        public async Task<bool> IsPlayerTurn(string matchID, string playerID)
        {
            Match match = await matchRepository.Get(matchID);
            Turn actualTurn = await GetActualTurnFromMatch(match);
            return actualTurn.IsPlayer(playerID);
        }
        

        public async  Task<DataApplication> GetDataToStartTurn(string matchID)
        {
            Match match = await matchRepository.Get(matchID);
            Turn actualTurn = await GetActualTurnFromMatch(match);
            return new DataApplication(matchID, actualTurn.RoundID, actualTurn.ID, match.GetNumberOfRounds(), actualTurn.PlayerID); 
        }


        private async Task<Turn> GetActualTurnFromMatch(Match match)
        {
            match.Rounds = await roundRepository.GetRoundsFromMatchID(match.ID);
            Round actualRound = match.GetActualRound();
            actualRound.Turns = await turnRepository.GetTurnsFromMatchIDAndRoundID(match.ID, actualRound.ID);
            Turn actualTurn = actualRound.GetActualTurn();
            return actualTurn;
        }


    } 
}

