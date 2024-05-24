namespace TryBets.Matches.DTO;
public class MatchDTOResponse
{
    public int MatchId { get; set; }
    public DateTime MatchDate { get; set; }
    public int MatchTeamAId { get; set; }
    public int MatchTeamBId { get; set; }
    public string? TeamAName { get; set; }
    public string? TeamBName { get; set; }
    public decimal MatchTeamAOdds { get; set;}
    public decimal MatchTeamBOdds { get; set;}
    public bool MatchFinished { get; set;}
    public int? MatchWinnerId { get; set; }
}