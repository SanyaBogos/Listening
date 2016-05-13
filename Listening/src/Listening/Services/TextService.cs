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

        public TextService(IRepository<Text> repository)
        {
            _textRepository = repository;
        }

        public string[][] GetWordCounts(string[][] wordsInParagraphs)
        {
            var specialSymbols = new string[] { ",", ".", "?", ":", ";" };
            var wordsCounts = new List<string[]>();

            foreach (var hiddenWordsInParagraphs in wordsInParagraphs)
            {
                var hiddenWordsLengthInText = new List<string>();
                foreach (var word in hiddenWordsInParagraphs)
                    if (specialSymbols.Contains(word))
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
                    if (wordOrSymbol.Equals(",") || wordOrSymbol.Equals("."))
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
            var specialSymbols = new char[] { ',', '.', '?', ':', ';' };
            var paragraphs = textDto.Text.Split(new string[] { "\r\n", "\n", "\n " }, StringSplitOptions.RemoveEmptyEntries);
            var wordsInParagraphs = new List<string[]>();

            foreach (var paragraph in paragraphs)
            {
                var words = new List<string>();
                foreach (var word in paragraph.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries))
                {
                    //if (word.Last() == ',' || word.Last() == '.')
                    if (specialSymbols.Contains(word.Last()))
                    {
                        words.Add(word.Substring(0, word.Length - 1));
                        words.Add(word.Last().ToString());
                    }
                    else
                    {
                        words.Add(word);
                    }
                }
                wordsInParagraphs.Add(words.ToArray());
            }

            var text = Mapper.Map<Text>(textDto);
            text.WordsInParagraphs = wordsInParagraphs.ToArray();

            return text;
        }

        //public char GetLetter(LetterLocatorDto letterLocator)
        //{
        //    return _textRepository.GetById(letterLocator.TextId)
        //                .WordsInParagraphs[letterLocator.ParagraphIndex][letterLocator.WordIndex][letterLocator.LetterIndex];
        //}

        //public bool CheckWordCorrectness(WordLocatorDto wordLocator, string value)
        //{
        //    return value.Equals(_textRepository.GetById(wordLocator.TextId)
        //                    .WordsInParagraphs[wordLocator.ParagraphIndex][wordLocator.WordIndex]);
        //}

        //public Text GetTextById(string id)
        //{
        //    return _textRepository.GetById(id);
        //}
    }
}
