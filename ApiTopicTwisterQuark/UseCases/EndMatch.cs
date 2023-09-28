using ApiTopicTwisterQuark.Entities;
using ApiTopicTwisterQuark.Entities.DTOs;
using ApiTopicTwisterQuark.Repositories.Interfaces;
using ApiTopicTwisterQuark.Services;
using ApiTopicTwisterQuark.Utils;

namespace ApiTopicTwisterQuark.UseCases
{
    public class EndMatch
    {
        private IMatchRepository matchRepository;
        private IRoundRepository roundRepository;
        private IPlayerRepository playerRepository;
        private INotificationService notificationService;
        private int point = 1;
        private AllForGameNotificationsIDs allForGameNotificationsIDs = new AllForGameNotificationsIDs();

        public EndMatch(IMatchRepository matchRepository,IRoundRepository roundRepository,IPlayerRepository playerRepository, INotificationService notificationService)
        {
            this.matchRepository = matchRepository;
            this.roundRepository = roundRepository;
            this.playerRepository = playerRepository;
            this.notificationService = notificationService;
        }

        public async Task<bool> IsEndOfMatch(string matchID)
        {
            Match match = await matchRepository.Get(matchID);
            match.Rounds = await roundRepository.GetRoundsFromMatchID(matchID);
            return match.AreRoundsOver(); 
        }
        public async Task EndThisMatch(string matchID)
        {
            Match match = await matchRepository.Get(matchID);
            match.Rounds = await roundRepository.GetRoundsFromMatchID(matchID);
            if (match.IsOver()) return;
            match.SetOver();
            await matchRepository.Update(match);
            await AddVictoryPointsToWinnerOfThisMatch(matchID);
        }


        public async Task AddVictoryPointsToWinnerOfThisMatch(string matchID)
        {
            Match match = await matchRepository.Get(matchID);
            if (IsThereWinnerInMatch(match))
            {
                Player player = await playerRepository.Get(match.WinnerID);
                player.AddVictoryPoints(point);
                await playerRepository.Update(player);
                await notificationService.AddPlayerForGameNotification(player.ID, allForGameNotificationsIDs.playerStatsChange);
            }
        }

        public async Task<MatchResultDTO> ReturnMatchResultForPlayer(string matchID, string playerID)
        {
            Match match = await matchRepository.Get(matchID);
            match.Rounds = await roundRepository.GetRoundsFromMatchID(matchID);
            string rivalID = match.GiveMeRival(playerID);
            return CreateMatchResult(match, playerID, rivalID);
        }
       

        private MatchResultDTO  CreateMatchResult(Match match, string playerID, string rivalID)
        {
            MatchResultDTO matchResult = new MatchResultDTO(
                match.ID,playerID, rivalID,
                match.GetPlayerRoundsWon(playerID),
                match.GetPlayerRoundsWon(rivalID),
                match.IsPlayerTheWinner(playerID),
                match.Tie
            );

            return matchResult;
        }

        private bool IsThereWinnerInMatch(Match match)
        {
            return match.WinnerID != "";
        }



    }
}