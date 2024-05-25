using TryBets.Bets.DTO;
using TryBets.Bets.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace TryBets.Bets.Repository;

public class BetRepository : IBetRepository
{
    protected readonly ITryBetsContext _context;
    public BetRepository(ITryBetsContext context)
    {
        _context = context;
    }

    public BetDTOResponse Post(BetDTORequest betRequest, string email)
    {
        var match = _context.Matches.FirstOrDefault(m => m.MatchId == betRequest.MatchId);
        var team = _context.Teams.FirstOrDefault(t => t.TeamId == betRequest.TeamId);
        if (match == null) throw new Exception("Match not founded");
        if (match.MatchFinished) throw new Exception("Match finished");
        if (team == null) throw new Exception("Team not founded");
        if (team.TeamId != match.MatchTeamAId && team.TeamId != match.MatchTeamBId)
            throw new Exception("Team is not in this match");
        var user = _context.Users.First(u => u.Email == email);
        Bet formatBet = new Bet
        {
            UserId = user.UserId,
            MatchId = betRequest.MatchId,
            TeamId = betRequest.TeamId,
            BetValue = betRequest.BetValue
        };
        var newBet = _context.Bets.Add(formatBet).Entity;
        _context.SaveChanges();
        return new BetDTOResponse {
            BetId = newBet.BetId,
            MatchId = newBet.MatchId,
            TeamId = newBet.TeamId,
            BetValue = newBet.BetValue,
            MatchDate = match.MatchDate,
            TeamName = team.TeamName,
            Email = email,
        };
    }
    public BetDTOResponse Get(int BetId, string email)
    {
        var bet = _context.Bets.Include(b => b.Team).Include(b => b.Match).FirstOrDefault(b => b.BetId == BetId);
        var user = _context.Users.First(u => u.Email == email);
        if (bet == null) throw new Exception("Bet not founded");
        if (bet!.UserId != user.UserId) throw new Exception("The user don't have this bet");
        return new BetDTOResponse {
            BetId = bet.BetId,
            MatchId = bet.MatchId,
            TeamId = bet.TeamId,
            BetValue = bet.BetValue,
            MatchDate = bet.Match!.MatchDate,
            TeamName = bet.Team!.TeamName,
            Email = email,
        };
    }
}