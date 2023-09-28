namespace ApiTopicTwisterQuark.Entities.DTOs;

public class TurnResultDTO
{
    public List<AnswerDTO> Answers;
    public float TimeLeft;

    public TurnResultDTO(List<AnswerDTO> answers,float timeLeft)
    {
        this.Answers = answers;
        this.TimeLeft = timeLeft;
    }
}
