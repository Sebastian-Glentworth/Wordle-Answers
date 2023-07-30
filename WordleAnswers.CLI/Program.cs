// See https://aka.ms/new-console-template for more information

using System.Globalization;
using System.Reflection;
using CsvHelper;
using WordleAnswers.Core;

Console.WriteLine("Hello, start typing a word to see if it exists.");

using var stream = Assembly.Load("WordleAnswers.Core")
    .GetManifestResourceStream("WordleAnswers.Core.answers.csv");
using var textReader = new StreamReader(stream ?? throw new ApplicationException("Cannot load answers"));
using var csv = new CsvReader(textReader, CultureInfo.InvariantCulture);
var records = csv.GetRecords<Answer>().ToArray();

var exit = false;
var searchTerm = string.Empty;
do
{
    var key = Console.ReadKey();
    Console.Clear();
    
    if(key.Key == ConsoleKey.Backspace || (key.Key >= ConsoleKey.A && key.Key <= ConsoleKey.Z))
    {
        if (key.Key == ConsoleKey.Backspace)
        {
            searchTerm = searchTerm.Length > 0
                ? searchTerm = searchTerm.Substring(0, searchTerm.Length - 1)
                : searchTerm;
        }

        if (key.Key >= ConsoleKey.A && key.Key <= ConsoleKey.Z)
        {
            searchTerm = (new string(searchTerm.ToCharArray().Append(key.KeyChar).ToArray())).ToUpperInvariant();
        }
        
        Console.WriteLine($"search term: {searchTerm}");
        Console.WriteLine("---------------RESULTS---------------");

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var limit = 5;
            var candidates = records
                .Where(r => r.Letters.StartsWith(searchTerm));
            foreach (var rec in candidates.Take(limit))
            {
                var addString = string.IsNullOrWhiteSpace(rec.AdditionalInformation) 
                    ? string.Empty 
                    : $", AdditionalInformation {rec.AdditionalInformation}";
                Console.WriteLine($"{rec.Letters} - {rec.Date}{addString}");
            }

            if (candidates.Count() > limit)
            {
                Console.WriteLine($"Showing {limit} of {candidates.Count()} records");
            }

            if (!candidates.Any())
            {
                Console.WriteLine("No Results");
            }
        }
    }

    if (key.Key == ConsoleKey.Escape)
    {
        exit = true;
    }
} while (!exit);

