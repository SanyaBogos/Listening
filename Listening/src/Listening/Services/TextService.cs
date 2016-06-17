using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using WebListening.Models;
using WebListening.Repositories;
using AutoMapper;

namespace WebListening.Services
{
    public class TextService
    {
        private readonly IRepository<Text> _textRepository;
        private string[] _specialSymbols;
        private char[] _moneySymbols;

        public TextService(IRepository<Text> repository)
        {
            _textRepository = repository;
            _specialSymbols = new string[] { ",", ".", "?", ":", ";", "-", "!" };
            _moneySymbols = new char[] { '$', '£', '€' };
        }

        public string[][] GetWordCounts(string[][] wordsInParagraphs)
        {
            var specialSymbols = Array.ConvertAll(_specialSymbols, item => Convert.ToChar(item));
            var wordsCounts = new List<string[]>();

            foreach (var hiddenWordsInParagraphs in wordsInParagraphs)
            {
                var hiddenWordsLengthInText = new List<string>();
                foreach (var word in hiddenWordsInParagraphs)
                    if (word.All(x => specialSymbols.Contains(x) || _moneySymbols.Contains(x)))
                        hiddenWordsLengthInText.Add(word);
                    else
                        hiddenWordsLengthInText.Add(word.Length.ToString());
                wordsCounts.Add(hiddenWordsLengthInText.ToArray());
            }

            return wordsCounts.ToArray();
        }

        public TextDto GenerateTextDtoById(string textId)
        {
            var text = _textRepository.Get(textId);
            var sb = new StringBuilder();
            foreach (var paragraph in text.WordsInParagraphs)
            {
                foreach (var wordOrSymbol in paragraph)
                    if (_specialSymbols.Except(new string[] { "-" }).Contains(wordOrSymbol))
                        sb.Append($"{wordOrSymbol}");
                    else
                        sb.Append($" {wordOrSymbol}");

                sb.AppendLine();
            }

            var textDto = Mapper.Map<TextDto>(text);
            textDto.Text = sb.ToString();

            return textDto;
        }

        public Text GenerateTextByDto(TextDto textDto)
        {
            var formattedText = textDto.Text.Replace("’", "'").Replace("  ", " ");
            foreach (var symbol in _specialSymbols)
                formattedText = formattedText.Replace($" {symbol}", $"{symbol}");

            var paragraphs = formattedText
                .Split(new string[] { "\r\n", "\n", "\n " }, StringSplitOptions.RemoveEmptyEntries);
            var wordsInParagraphs = new List<string[]>();

            foreach (var paragraph in paragraphs)
            {
                var words = new List<string>();
                foreach (var word in paragraph.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries))
                    DevideWordAndSpecialSymbols(words, word);
                wordsInParagraphs.Add(words.ToArray());
            }

            var text = Mapper.Map<Text>(textDto);
            text.WordsInParagraphs = wordsInParagraphs.ToArray();

            return text;
        }

        public List<CorrectWordLocatorsDto> CheckWordsCorrectness(string id, string[] words)
        {
            var formattedWords = words.Select(x => x.Replace("`", "'")).ToArray();
            var wordsInParagraphs = _textRepository.Get(id).WordsInParagraphs;
            var correctWordLocatorsDtoList = new List<CorrectWordLocatorsDto>();

            foreach (var word in formattedWords)
            {
                var locators = new List<WordLocatorDto>();

                for (int i = 0; i < wordsInParagraphs.Length; i++)
                    for (int j = 0; j < wordsInParagraphs[i].Length; j++)
                        if (word.Equals(wordsInParagraphs[i][j], StringComparison.InvariantCultureIgnoreCase))
                            locators.Add(new WordLocatorDto
                            {
                                ParagraphIndex = i,
                                WordIndex = j,
                                IsCapital = char.IsUpper(wordsInParagraphs[i][j].First())
                            });

                if (locators.Count > 0)
                    correctWordLocatorsDtoList.Add(
                        new CorrectWordLocatorsDto { Locators = locators.ToArray(), Word = word });
            }

            return correctWordLocatorsDtoList;
        }

        private void DevideWordAndSpecialSymbols(List<string> words, string word)
        {
            var specialSymbols = Array.ConvertAll(_specialSymbols, item => Convert.ToChar(item));
            var endIndexer = word.Length;
            var startIndexer = 0;

            if (_moneySymbols.Contains(word.First()))
            {
                words.Add(word.First().ToString());
                startIndexer++;
            }

            while (specialSymbols.Contains(word[endIndexer - 1]))
                endIndexer--;

            if (endIndexer != startIndexer)
                words.Add(word.Substring(startIndexer, endIndexer - startIndexer));

            if (word.Length != endIndexer)
                words.Add(word.Substring(endIndexer, word.Length - endIndexer));
        }
    }
}
