using CsvHelper.Configuration.Attributes;

namespace WordleAnswers.Core;

public class Answer
{
    [Index(0)]
    public DateOnly Date { get; set; }
    [Index(1)]
    public string Letters { get; set; } = string.Empty;
    [Index(2)]
    public string AdditionalInformation { get; set; } = string.Empty;
}