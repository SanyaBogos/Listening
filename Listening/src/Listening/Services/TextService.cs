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

        public TextService(IRepository<Text> repository)
        {
            _textRepository = repository;
            _specialSymbols = new string[] { ",", ".", "?", ":", ";", "-", "!" };
        }

        public string[][] GetWordCounts(string[][] wordsInParagraphs)
        {
            var specialSymbols = Array.ConvertAll(_specialSymbols, item => Convert.ToChar(item));
            var wordsCounts = new List<string[]>();

            foreach (var hiddenWordsInParagraphs in wordsInParagraphs)
            {
                var hiddenWordsLengthInText = new List<string>();
                foreach (var word in hiddenWordsInParagraphs)
                    if (word.All(x => specialSymbols.Contains(x)))
                        hiddenWordsLengthInText.Add(word);
                    else
                        hiddenWordsLengthInText.Add(word.Length.ToString());
                wordsCounts.Add(hiddenWordsLengthInText.ToArray());
            }

            return wordsCounts.ToArray();
        }

        public TextDto GenerateTextDtoById(string textId)
        {
            var text = _textRepository.GetById(textId);
            var sb = new StringBuilder();
            foreach (var paragraph in text.WordsInParagraphs)
            {
                foreach (var wordOrSymbol in paragraph)
                    if (_specialSymbols.Contains(wordOrSymbol))
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
            var specialSymbols = Array.ConvertAll(_specialSymbols, item => Convert.ToChar(item));
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
                    DevideWordAndSpecialSymbols(specialSymbols, words, word);
                wordsInParagraphs.Add(words.ToArray());
            }

            var text = Mapper.Map<Text>(textDto);
            text.WordsInParagraphs = wordsInParagraphs.ToArray();

            return text;
        }

        private void DevideWordAndSpecialSymbols(char[] specialSymbols, List<string> words, string word)
        {
            var indexer = word.Length;
            while (specialSymbols.Contains(word[indexer - 1]))
                indexer--;

            if (indexer != 0)
                words.Add(word.Substring(0, indexer));

            if (word.Length != indexer)
                words.Add(word.Substring(indexer, word.Length - indexer));
        }
    }
}
