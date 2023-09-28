using ApiTopicTwisterQuark.Entities;
using ApiTopicTwisterQuark.Entities.DTOs;
using ApiTopicTwisterQuark.Repositories;
using ApiTopicTwisterQuark.Repositories.Interfaces;

namespace ApiTopicTwisterQuark.UseCases
{
    public class GetMatchesToPlayInfo
    {
        private IMatchRepository matchRepository;
        private IRoundRepository roundRepository;
        private ITurnRepository turnRepository;
        public GetMatchesToPlayInfo(IMatchRepository matchRepository,IRoundRepository roundRepository, ITurnRepository turnRepository)
        {
            this.matchRepository = matchRepository;
            this.roundRepository = roundRepository;
            this.turnRepository = turnRepository;
        }

        public async Task<List<MatchInfoDTO>> Execute(string playerID)
        {
            var playerMatchesActives = await GetMatchesNotOverForThisPlayer(playerID);

            return await GenerateMatchInfosForPlayerID(playerMatchesActives,playerID);
        }

        private async Task<List<Match>> GetMatchesNotOverForThisPlayer(string playerID)
        {
            return (await matchRepository.GetMatches()).Where(
                match => match.HasThisPlayerID(playerID) && !match.IsOver()).Reverse().ToList();
        }

        private async Task<List<MatchInfoDTO>> GenerateMatchInfosForPlayerID(List<Match> matches,string playerID)
        {
            List<MatchInfoDTO> matchesInfos= new List<MatchInfoDTO>();
            foreach (var match in matches)
            {
                MatchInfoDTO matchInfo = await CreateMatchInfo(match, playerID);
                
               if(matchInfo!=null) matchesInfos.Add(matchInfo);
            }

            return matchesInfos;
        }      
        

        private async Task<MatchInfoDTO?> CreateMatchInfo(Match match,string playerID)
        {
            match.Rounds = await roundRepository.GetRoundsFromMatchID(match.ID);
            var actualRound = match.GetActualRound();
            actualRound.Turns = await turnRepository.GetTurnsFromMatchIDAndRoundID(match.ID, actualRound.ID);    
            var actualTurn = actualRound.GetActualTurn();
            var rivalID = match.GiveMeRival(playerID);
            return new MatchInfoDTO(match.ID, rivalID,match.GetPlayerRoundsWon(playerID),match.GetPlayerRoundsWon(rivalID),actualRound.ID, actualTurn.IsPlayer(playerID));
        }
    }
}