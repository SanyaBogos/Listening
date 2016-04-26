using System.Collections.Generic;
using System.Linq;
using System.Net;
using FluentAssertions;
using Microsoft.AspNet.Http;
using MongoDB.Bson;
using Moq;
using WebListening.Controllers;
using WebListening.Models;
using WebListening.Repositories;
using WebListening.Services;
using Xunit;

namespace Tests
{
    public class WordControllerSpecs
    {
        private Mock<HttpResponse> _responseMock;
        private Mock<IRepository<Text>> _repoMock;
        private WordController _wordController;
        private Text[] _texts;

        public WordControllerSpecs()
        {
            BuildTexts();

            _repoMock = new Mock<IRepository<Text>>();
            _responseMock = new Mock<HttpResponse>();
            var httpContextMock = new Mock<HttpContext>();

            httpContextMock.Setup(x => x.Response).Returns(_responseMock.Object);
            _wordController = new WordController(_repoMock.Object, new TextService(_repoMock.Object));
            _wordController.ActionContext.HttpContext = httpContextMock.Object;
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void Should_Return_Correct_Letter_Count_In_Paragraphs_For_GetWordsCountInParagraphs(int textndexer)
        {
            _repoMock.Setup(x => x.GetAll()).Returns(_texts.AsQueryable());
            var expectedResults = BuildExpectedCountInParagraphDictionaryResult();
            var id = _texts[textndexer].TextId;

            var actual = _wordController.GetWordsCountInParagraphs(id.ToString()).Value as string[][];

            _responseMock.VerifySet(x => x.StatusCode = (int)HttpStatusCode.OK);
            actual.ShouldBeEquivalentTo<string[][]>(expectedResults[id]);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void Should_Return_Invalid_Operation_When_No_Texts_Returned_From_Repo_For_GetWordsCountInParagraphs(int textndexer)
        {
            _repoMock.Setup(x => x.GetAll()).Returns(new Text[0].AsQueryable());
            var id = _texts[textndexer].TextId;

            var actual = _wordController.GetWordsCountInParagraphs(id.ToString()).Value as string;

            _responseMock.VerifySet(x => x.StatusCode = (int)HttpStatusCode.BadRequest);
            actual.ShouldBeEquivalentTo("Invalid operation");
        }

        [Theory]
        [InlineData(0, 0, 0, 0, 'T')]
        [InlineData(0, 0, 2, 3, 'e')]
        [InlineData(0, 0, 7, 3, 'r')]
        [InlineData(1, 0, 2, 1, 'e')]
        [InlineData(2, 2, 5, 3, 'w')]
        [InlineData(2, 3, 5, 2, 'k')]
        [InlineData(2, 3, 1, 1, 'o')]
        public void Should_Return_Correct_Letter_For_GetLetter(int textndexer, int paragraphIndex, int wordIndex, int symbolIndex, char result)
        {
            _repoMock.Setup(x => x.GetAll()).Returns(_texts.AsQueryable());
            var id = _texts[textndexer].TextId;

            var actual = (char)_wordController.GetLetter(id.ToString(), paragraphIndex, wordIndex, symbolIndex).Value;

            _responseMock.VerifySet(x => x.StatusCode = (int)HttpStatusCode.OK);
            actual.ShouldBeEquivalentTo(result);
        }

        [Theory]
        [InlineData(0, 0, 0, 0)]
        [InlineData(1, 0, 0, 0)]
        [InlineData(2, 0, 0, 0)]
        public void Should_Return_InvalidOperation_When_No_Texts_Returned_From_Repo_For_GetLetter(int textndexer, int paragraphIndex, int wordIndex, int symbolIndex)
        {
            _repoMock.Setup(x => x.GetAll()).Returns(new Text[0].AsQueryable());
            var id = _texts[textndexer].TextId;

            var actual = _wordController.GetLetter(id.ToString(), paragraphIndex, wordIndex, symbolIndex).Value as string;

            _responseMock.VerifySet(x => x.StatusCode = (int)HttpStatusCode.BadRequest);
            actual.ShouldBeEquivalentTo("Invalid operation");
        }

        [Theory]
        [InlineData(1, 100, 0, 0)]
        [InlineData(2, 0, 100, 0)]
        [InlineData(2, 0, 0, 100)]
        public void Should_Return_InvalidOperation_When_Text_Indexer_Out_Of_Range_For_GetLetter(int textndexer, int paragraphIndex, int wordIndex, int symbolIndex)
        {
            _repoMock.Setup(x => x.GetAll()).Returns(_texts.AsQueryable());
            var id = _texts[textndexer].TextId;

            var actual = _wordController.GetLetter(id.ToString(), paragraphIndex, wordIndex, symbolIndex).Value as string;

            _responseMock.VerifySet(x => x.StatusCode = (int)HttpStatusCode.BadRequest);
            actual.ShouldBeEquivalentTo("Invalid operation");
        }

        [Theory]
        [InlineData("asd", 0, 0, 0)]
        [InlineData("dfgdfg", 0, 0, 0)]
        [InlineData("avbnvbnsd", 0, 0, 0)]
        public void Should_Return_InvalidOperation_When_Text_Id_Is_Incorrect_For_GetLetter(string textndexer, int paragraphIndex, int wordIndex, int symbolIndex)
        {
            _repoMock.Setup(x => x.GetAll()).Returns(_texts.AsQueryable());

            var actual = _wordController.GetLetter(textndexer, paragraphIndex, wordIndex, symbolIndex).Value as string;

            _responseMock.VerifySet(x => x.StatusCode = (int)HttpStatusCode.BadRequest);
            actual.ShouldBeEquivalentTo("Invalid operation");
        }

        [Theory]
        [InlineData(0, 0, 0, "There", true)]
        [InlineData(0, 0, 2, "moments", true)]
        [InlineData(0, 0, 7, "your", true)]
        [InlineData(1, 0, 2, "me", true)]
        [InlineData(2, 2, 5, "know", true)]
        [InlineData(2, 3, 5, "make", true)]
        [InlineData(2, 3, 6, "this", true)]
        [InlineData(0, 0, 0, "dfgfd", false)]
        [InlineData(0, 0, 0, "dfkjhgjkfdghjdf", false)]
        [InlineData(0, 0, 2, "mofents", false)]
        [InlineData(0, 0, 2, "momdfgdfgents", false)]
        [InlineData(0, 0, 7, "yoir", false)]
        [InlineData(0, 0, 7, "yosaaaaur", false)]
        [InlineData(1, 0, 2, "ma", false)]
        [InlineData(1, 0, 2, "mesdf", false)]
        [InlineData(2, 2, 5, "knew", false)]
        [InlineData(2, 2, 5, "kndddddow", false)]
        [InlineData(2, 3, 5, "makesdfgfdg", false)]
        [InlineData(2, 3, 5, "maks", false)]
        [InlineData(2, 3, 6, "tais", false)]
        [InlineData(2, 3, 6, "thfghfghgfhis", false)]
        public void Should_Return_Correct_Bool_Value_For_GetWordCorrectness(int textndexer, int paragraphIndex, int wordIndex, string value, bool result)
        {
            _repoMock.Setup(x => x.GetAll()).Returns(_texts.AsQueryable());
            var id = _texts[textndexer].TextId;

            var actual = (bool)_wordController.GetWordCorrectness(id.ToString(), paragraphIndex, wordIndex, value).Value;

            _responseMock.VerifySet(x => x.StatusCode = (int)HttpStatusCode.OK);
            actual.ShouldBeEquivalentTo(result);
        }

        [Theory]
        [InlineData("asd", 0, 0, 0)]
        [InlineData("dfgdfg", 0, 0, 0)]
        [InlineData("avbnvbnsd", 0, 0, 0)]
        public void Should_Return_InvalidOperation_When_Text_Id_Is_Incorrect_For_GetWordCorrectness(string textndexer, int paragraphIndex, int wordIndex, string value)
        {
            _repoMock.Setup(x => x.GetAll()).Returns(_texts.AsQueryable());

            var actual = _wordController.GetWordCorrectness(textndexer, paragraphIndex, wordIndex, value).Value as string;

            _responseMock.VerifySet(x => x.StatusCode = (int)HttpStatusCode.BadRequest);
            actual.ShouldBeEquivalentTo("Invalid operation");
        }

        private void BuildTexts()
        {
            //var xxx = new { One = "asd" };
            _texts = new Text[] {
                new Text() {
                    TextId = ObjectId.GenerateNewId(),
                    Title="Ronald Reagan",
                    SubTitle="",
                    WordsInParagraphs=new string[][] { new string[] { "There", "are", "moments", "you", "never", "erase", "from", "your", "memory", "ever", ".", "I", "didn’t", "know", "I", "was", "shot", ".", "In", "fact", ",", "I", "was", "still", "asking", ",", "what", "was", "that", "noise", ",", "I", "thought", "it", "was", "firecrackers", ".", "And", "the", "next", "thing", "I", "knew", "Jerry", ",", "secret", "service", ",", "simply", "grabbed", "me", "here", "and", "threw", "me", "into", "the", "car", "and", "then", "he", "dived", "in", "on", "top", "of", "me", ".", "And", "it", "was", "only", "then", "that", "I", "felt", "a", "paralysing", "pain", "and", "I", "learned", "that", "the", "bullet", "had", "hit", "me", "up", "here", ".", "When", "I", "walked", "in", "they", "were", "just", "concluding", "a", "meeting", "in", "the", "hospital", "of", "all", "the", "doctors", "associated", "with", "the", "hospital", ".", "So", "when", "I", "saw", "all", "those", "doctors", "surround", "me", ",", "I", "said", "I", "hoped", "they", "were", "all", "republicans", "." } },
                    AudioName="shot1.mp3"
                },
                new Text() {
                    TextId = ObjectId.GenerateNewId(),
                    Title="The Economic Prospect",
                    SubTitle="Eddie George, Governor of Bank of England, Hertfordshire University, 14 February 1999",
                    WordsInParagraphs=new string[][] {
                        new string[] { "So", "let", "me", "begin", "with", "what", "it", "is", "that", "we", "are", "trying", "to", "do", "." },
                        new string[] { "In", "one", "sense", "that's", "very", "easy", "to", "explain", "these", "days", ".", "The", "previous", "Conservative", "Government", "had", "already", "in", "1992", "defined", "the", "objective", "of", "monetary", "policy", "in", "terms", "of", "a", "target", "rate", "for", "retail", "price", "inflation", ".", "The", "new", "Labour", "Government", "immediately", "on", "assuming", "office", "similarly", "defined", "the", "objective", "in", "terms", "of", "an", "inflation", "target", ",", "but", "they", "went", "an", "important", "step", "further", ".", "As", "soon", "as", "Gordon", "Brown", "became", "Chancellor", "of", "the", "Exchequer", ",", "in", "May", "1997", ",", "he", "announced", "that", "he", "would", "no", "longer", "exercise", "his", "powers", "to", "set", "short-term", "interest", "rates", ",", "which", "is", "at", "the", "heart", "of", "monetary", "policy", ",", "but", "instead", "he", "would", "set", "the", "inflation", "target", ",", "and", "then", "he", "would", "delegate", "the", "achievement", "of", "that", "target", "to", "a", "new", "Monetary", "Policy", "Committee", "to", "be", "established", "in", "the", "Bank", "of", "England", "." },
                        new string[] { "This", "position", "was", "subsequently", "formalised", "in", "the", "new", "Bank", "of", "England", "Act", "which", "came", "into", "effect", "in", "June", "last", "year", ".", "And", "that", "Act", "now", "defines", "in", "Statute", "the", "Bank's", "objective", "as", "\"the", "maintenance", "of", "price", "stability", ",", "and", "subject", "to", "that", ",", "to", "support", "the", "Government's", "economic", "policy", ",", "including", "its", "objectives", "for", "growth", "and", "employment\"", "." }
                    },
                    AudioName="eddiegeorge.mp3"
                },
                new Text() {
                    TextId=ObjectId.GenerateNewId(),
                    Title="Barack Obama's West Point speech on Afghanistan - 1/12/09",
                    SubTitle="",
                    WordsInParagraphs=new string[][] {
                        new string[] { "As","commander-in-chief",",","I","have","determined","that","it","is","in","our","vital","national","interest","to","send","an","additional","30,000","US","troops","to","Afghanistan",".","After","18","months",",","our","troops","will","begin","to","come","home",".","These","are","the","resources","that","we","need","to","seize","the","initiative",",","while","building","the","Afghan","capacity","that","can","allow","for","a","responsible","transition","of","our","forces","out","of","Afghanistan","."},
                        new string[] { "I","do","not","make","this","decision","lightly",".","I","opposed","the","war","in","Iraq","precisely","because","I","believe","that","we","must","exercise","restraint","in","the","use","of","military","force",",","and","always","consider","the","long-term","consequences","of","our","actions",".","We","have","been","at","war","now","for","eight","years",",","at","enormous","cost","in","lives","and","resources",".","Years","of","debate","over","Iraq","and","terrorism","have","left","our","unity","on","national","security","issues","in","tatters",",","and","created","a","highly","polarised","and","partisan","backdrop","for","this","effort",".","And","having","just","experienced","the","worst","economic","crisis","since","the","Great","Depression",",","the","American","people","are","understandably","focused","on","rebuilding","our","economy","and","putting","people","to","work","here","at","home","."},
                        new string[] { "Most", "of", "all", ",", "I", "know", "that", "this", "decision", "asks", "even", "more", "of", "you-", "a", "military", "that", ",", "along", "with", "your", "families", ",", "has", "already", "borne", "the", "heaviest", "of", "all", "burdens", ".", "As", "president", ",", "I", "have", "signed", "a", "letter", "of", "condolence", "to", "the", "family", "of", "each", "American", "who", "gives", "their", "life", "in", "these", "wars", ".", "I", "have", "read", "the", "letters", "from", "the", "parents", "and", "spouses", "of", "those", "who", "deployed", ".", "I", "have", "visited", "our", "courageous", "wounded", "warriors", "at", "Walter", "Reed", ".", "I", "have", "travelled", "to", "Dover", "to", "meet", "the", "flag-draped", "caskets", "of", "18", "Americans", "returning", "home", "to", "their", "final", "resting", "place", ".", "I", "see", "first-hand", "the", "terrible", "wages", "of", "war", ".", "If", "I", "did", "not", "think", "that", "the", "security", "of", "the", "United", "States", "and", "the", "safety", "of", "the", "American", "people", "were", "at", "stake", "in", "Afghanistan", ",", "I", "would", "gladly", "order", "every", "single", "one", "of", "our", "troops", "home", "tomorrow", "." },
                        new string[] { "So", "no-", "I", "do", "not", "make", "this", "decision", "lightly", ".", "I", "make", "this", "decision", "because", "I", "am", "convinced", "that", "our", "security", "is", "at", "stake", "in", "Afghanistan", "and", "Pakistan", "." }
                    },
                    AudioName="obama.mp3"
                },
            };
        }

        private Dictionary<ObjectId, string[][]> BuildExpectedCountInParagraphDictionaryResult()
        {
            var expArrays = BuildExpectedCountsInParagraphArrays();
            var expectedResults = new Dictionary<ObjectId, string[][]>();

            foreach (var text in _texts)
                expectedResults.Add(text.TextId, expArrays.Dequeue());

            return expectedResults;
        }

        private Queue<string[][]> BuildExpectedCountsInParagraphArrays()
        {
            var expArrays = new Queue<string[][]>();
            expArrays.Enqueue(new string[][] { new string[] { "5", "3", "7", "3", "5", "5", "4", "4", "6", "4", ".", "1", "6", "4", "1", "3", "4", ".", "2", "4", ",", "1", "3", "5", "6", ",", "4", "3", "4", "5", ",", "1", "7", "2", "3", "12", ".", "3", "3", "4", "5", "1", "4", "5", ",", "6", "7", ",", "6", "7", "2", "4", "3", "5", "2", "4", "3", "3", "3", "4", "2", "5", "2", "2", "3", "2", "2", ".", "3", "2", "3", "4", "4", "4", "1", "4", "1", "10", "4", "3", "1", "7", "4", "3", "6", "3", "3", "2", "2", "4", ".", "4", "1", "6", "2", "4", "4", "4", "10", "1", "7", "2", "3", "8", "2", "3", "3", "7", "10", "4", "3", "8", ".", "2", "4", "1", "3", "3", "5", "7", "8", "2", ",", "1", "4", "1", "5", "4", "4", "3", "11", "." } });
            expArrays.Enqueue(new string[][] {
                new string[] {"2", "3", "2", "5", "4", "4", "2", "2", "4", "2", "3", "6", "2", "2", "." },
                new string[] {"2","3","5","6","4","4","2","7","5","4",".","3","8","12","10","3","7","2","4","7","3","9","2","8","6","2","5","2","1","6","4","3","6","5","9",".","3","3","6","10","11","2","8","6","9","7","3","9","2","5","2","2","9","6",",","3","4","4","2","9","4","7",".","2","4","2","6","5","6","10","2","3","9",",","2","3","4",",","2","9","4","2","5","2","6","8","3","6","2","3","10","8","5",",","5","2","2","3","5","2","8","6",",","3","7","2","5","3","3","9","6",",","3","4","2","5","8","3","11","2","4","6","2","1","3","8","6","9","2","2","11","2","3","4","2","7","." },
                new string[] { "4", "8", "3", "12", "10", "2", "3", "3", "4", "2", "7", "3", "5", "4", "4", "6", "2", "4", "4", "4", ".", "3", "4", "3", "3", "7", "2", "7", "3", "6", "9", "2", "4", "11", "2", "5", "9", ",", "3", "7", "2", "4", ",", "2", "7", "3", "12", "8", "6", ",", "9", "3", "10", "3", "6", "3", "11", "." }
            });
            expArrays.Enqueue(new string[][] {
                new string[] {"2","18",",","1","4","10","4","2","2","2","3","5","8","8","2","4","2","10","6","2","6","2","11",".","5","2","6",",","3","6","4","5","2","4","4",".","5","3","3","9","4","2","4","2","5","3","10",",","5","8","3","6","8","4","3","5","3","1","11","10","2","3","6","3","2","11","." },
                new string[] {"1","2","3","4","4","8","7",".","1","7","3","3","2","4","9","7","1","7","4","2","4","8","9","2","3","3","2","8","5",",","3","6","8","3","9","12","2","3","7",".","2","4","4","2","3","3","3","5","5",",","2","8","4","2","5","3","9",".","5","2","6","4","4","3","9","4","4","3","5","2","8","8","6","2","7",",","3","7","1","6","9","3","8","8","3","4","6",".","3","6","4","11","3","5","8","6","5","3","5","10",",","3","8","6","3","14","7","2","10","3","7","3","7","6","2","4","4","2","4","."},
                new string[] { "4", "2", "3", ",", "1", "4", "4", "4", "8", "4", "4", "4", "2", "4", "1", "8", "4", ",", "5", "4", "4", "8", ",", "3", "7", "5", "3", "8", "2", "3", "7", ".", "2", "9", ",", "1", "4", "6", "1", "6", "2", "10", "2", "3", "6", "2", "4", "8", "3", "5", "5", "4", "2", "5", "4", ".", "1", "4", "4", "3", "7", "4", "3", "7", "3", "7", "2", "5", "3", "8", ".", "1", "4", "7", "3", "10", "7", "8", "2", "6", "4", ".", "1", "4", "9", "2", "5", "2", "4", "3", "11", "7", "2", "2", "9", "9", "4", "2", "5", "5", "7", "5", ".", "1", "3", "10", "3", "8", "5", "2", "3", ".", "2", "1", "3", "3", "5", "4", "3", "8", "2", "3", "6", "6", "3", "3", "6", "2", "3", "8", "6", "4", "2", "5", "2", "11", ",", "1", "5", "6", "5", "5", "6", "3", "2", "3", "6", "4", "8", "." },
                new string[] { "2", "3", "1", "2", "3", "4", "4", "8", "7", ".", "1", "4", "4", "8", "7", "1", "2", "9", "4", "3", "8", "2", "2", "5", "2", "11", "3", "8", "." }
            });
            return expArrays;
        }
    }
}
