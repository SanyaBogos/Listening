using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Http;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebListening.Models
{
    public class Text
    {
        [BsonId]
        public ObjectId TextId { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string[][] WordsInParagraphs { get; set; }
        public string AudioName { get; set; }
    }

    public class TextDto
    {
        public string TextId { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Text { get; set; }
        public string AudioName { get; set; }
    }

    public class TextDescriptionDto
    {
        public ObjectId TextId { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string AudioName { get; set; }
    }

    //public class LetterLocatorDto : WordLocatorDto
    //{
    //    public int LetterIndex { get; set; }
    //}

    //public class WordLocatorDto
    //{
    //    public string TextId { get; set; }
    //    public int ParagraphIndex { get; set; }
    //    public int WordIndex { get; set; }
    //}

    public class CorrectWordLocatorsDto
    {
        public string Word { get; set; }
        public WordLocatorDto[] Locators { get; set; }
    }

    public class WordLocatorDto
    {
        public int ParagraphIndex { get; set; }
        public int WordIndex { get; set; }
    }
}
