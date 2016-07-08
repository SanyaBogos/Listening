using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Cors;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Mvc;
using MongoDB.Bson;
using WebListening.Models;
using WebListening.Repositories;
using WebListening.Services;

namespace WebListening.Controllers
{
    //[EnableCors("AllowAll")]
    [Authorize]
    [Route("api/[controller]")]
    public class WordController : Controller
    {
        private readonly TextService _textService;
        private readonly IRepository<Text> _textRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public WordController(
            IRepository<Text> repository,
            TextService textService,
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _textRepository = repository;
            _textService = textService;
            _context = context;
        }

        [HttpGet("wordsInParagraphs/{id}")]
        public JsonResult GetWordsInParagraphs(string id)
        {
            return Json(_textRepository.GetById(id));
            //return Json(_textService.GetTextById(id));
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public JsonResult GetWordsCountInParagraphs(string id)
        {
            try
            {
                var userId = User.GetUserId();

                if (userId != null)
                {
                    // TODO: read data from DB
                }


                var wordsInParagraphs = _textRepository.GetById(id).WordsInParagraphs;
                var wordsCounts = _textService.GetWordCounts(wordsInParagraphs);
                Response.StatusCode = (int)HttpStatusCode.OK;
                return Json(wordsCounts);

                //Response.StatusCode = (int)HttpStatusCode.OK;
                //var wordsInParagraphs = _textService.GetTextById(id).WordsInParagraphs;
                //var wordsCounts = _textService.GetWordCounts(wordsInParagraphs);
                //return Json(wordsCounts);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Invalid operation");
            }
        }

        [AllowAnonymous]
        [HttpGet("letter/{id}/{paragraphIndex}/{wordIndex}/{symbolIndex}")]
        public JsonResult GetLetter(string id, int paragraphIndex, int wordIndex, int symbolIndex)
        {
            try
            {
                var userId = User.GetUserId();

                if (userId != null)
                {
                    // TODO: read data from DB
                    var userProgress = _context.UserTextProgresses
                                            .Where(x => x.UserId == userId
                                            && x.TextId == id
                                            && x.Started != null);

                    //if (userProgress.e)
                    //{

                    //}

                    //if (!_context.UserTextProgresses.Any(x=>x.))
                    //{

                    //}
                }

                Response.StatusCode = (int)HttpStatusCode.OK;
                return Json(_textRepository.GetById(id)
                                .WordsInParagraphs[paragraphIndex]
                                [wordIndex][symbolIndex]);

                //var letterLocator = new LetterLocatorDto
                //{
                //    TextId = id,
                //    ParagraphIndex = paragraphIndex,
                //    WordIndex = wordIndex,
                //    LetterIndex = symbolIndex
                //};

                //var letter = _textService.GetLetter(letterLocator);
                //return Json(letter);
            }
            catch (Exception)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Invalid operation");
            }
        }

        [AllowAnonymous]
        [HttpGet("wordCorrectness/{id}/{paragraphIndex}/{wordIndex}/{value}")]
        public JsonResult GetWordCorrectness(string id, int paragraphIndex, int wordIndex, string value)
        {
            try
            {
                value = value.Replace("`", "'");
                Response.StatusCode = (int)HttpStatusCode.OK;
                return Json(value.Equals(_textRepository.GetById(id)
                            .WordsInParagraphs[paragraphIndex][wordIndex]));
                //var wordLocator = new WordLocatorDto
                //{
                //    TextId = id,
                //    ParagraphIndex = paragraphIndex,
                //    WordIndex = wordIndex
                //};
                //var isCorrect = _textService.CheckWordCorrectness(wordLocator, value);
                //return Json(isCorrect);
            }
            catch (Exception)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Invalid operation");
            }
        }

        [AllowAnonymous]
        // TODO: remake this using http://stackoverflow.com/questions/9981330/how-to-pass-an-array-of-integers-to-a-asp-net-web-api-rest-service
        [HttpPost("wordsForCheck/{id}")]
        public JsonResult PostCheckWords(string id, [FromBody]string[] words)
        {
            try
            {
                var formattedWords = words.Select(x => x.Replace("`", "'")).ToArray();
                var wordsInParagraphs = _textRepository.GetById(id).WordsInParagraphs;
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

                return Json(correctWordLocatorsDtoList);
            }
            catch (Exception)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Invalid operation");
            }
        }

        //private List<string[]> GetWordCounts(string[][] wordsInParagraphs)
        //{
        //    var specialSymbols = new string[] { ",", ".", "?", ":" };
        //    var wordsCounts = new List<string[]>();

        //    foreach (var hiddenWordsInParagraphs in wordsInParagraphs)
        //    {
        //        var hiddenWordsLengthInText = new List<string>();
        //        foreach (var word in hiddenWordsInParagraphs)
        //            if (specialSymbols.Contains(word))
        //                hiddenWordsLengthInText.Add(word);
        //            else
        //                hiddenWordsLengthInText.Add(word.Length.ToString());
        //        wordsCounts.Add(hiddenWordsLengthInText.ToArray());
        //    }

        //    return wordsCounts;
        //}
    }
}
