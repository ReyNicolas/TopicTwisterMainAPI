using ApiTopicTwisterQuark.Entities;
using ApiTopicTwisterQuark.Entities.DTOs;
using ApiTopicTwisterQuark.Repositories;
using ApiTopicTwisterQuark.Repositories.Interfaces;

namespace ApiTopicTwisterQuark.UseCases
{
    public class StartTurn
    {
        private IMatchRepository matchRepository;
        private IRoundRepository roundRepository;
        private ITurnRepository turnRepository;

        public StartTurn(IMatchRepository matchRepository, IRoundRepository roundRepository,
            ITurnRepository turnRepository)
        {
            this.matchRepository = matchRepository;
            this.roundRepository = roundRepository;
            this.turnRepository = turnRepository;
        }

        public async Task<StartTurnDTO> GetStartTurnData(string matchID,int roundID,string playerID)
        {
            Match match;            
            match = await matchRepository.Get(matchID);               
            match.Rounds = await roundRepository.GetRoundsFromMatchID(matchID);
            Round round;           
            round = await roundRepository.Get(matchID, roundID);
            round.Turns = await turnRepository.GetTurnsFromMatchIDAndRoundID(matchID, roundID);
            Turn turn;            
            turn = round.GetTurnOfPlayerID(playerID);           
            
            StartTurnDTO startTurnDTO= new StartTurnDTO(roundID,match.GetNumberOfRounds(),turn.Time,round.Categories,round.Letter);

            return startTurnDTO;
        }
    }
}