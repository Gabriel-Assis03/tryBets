using TryBets.Odds.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Globalization;

namespace TryBets.Odds.Repository;

public class OddRepository : IOddRepository
{
    protected readonly ITryBetsContext _context;
    public OddRepository(ITryBetsContext context)
    {
        _context = context;
    }

    public Match Patch(int MatchId, int TeamId, string BetValue)
    {
        string BetValueConverted = BetValue.Replace(',', '.');
        decimal BetValueDecimal = Decimal.Parse(BetValueConverted, CultureInfo.InvariantCulture);
        var match = _context.Matches.FirstOrDefault(m => m.MatchId == MatchId);
        if (match == null) throw new Exception("Match not founded");
        // if (match.MatchFinished) throw new Exception("Match finished");
        if (TeamId != match.MatchTeamAId && TeamId != match.MatchTeamBId)
            throw new Exception("Team is not in this match");
        if (TeamId == match.MatchTeamAId) match.MatchTeamAValue += BetValueDecimal;
        if (TeamId == match.MatchTeamBId) match.MatchTeamBValue += BetValueDecimal;
        _context.Matches.Update(match);
        _context.SaveChanges();
        return match;
    }
}