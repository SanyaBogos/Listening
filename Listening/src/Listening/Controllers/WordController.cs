using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Listening.Aspect;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Cors;
using Microsoft.AspNet.Mvc;
using MongoDB.Bson;
using WebListening.Models;
using WebListening.Repositories;
using WebListening.Services;

namespace WebListening.Controllers
{
    //[EnableCors("AllowAll")]
    [ExceptionHandle]
    [Authorize]
    [Route("api/[controller]")]
    public class WordController : Controller
    {
        private readonly TextService _textService;
        private readonly IRepository<Text> _textRepository;

        public WordController(IRepository<Text> repository, TextService textService)
        {
            _textRepository = repository;
            _textService = textService;
        }

        [HttpGet("wordsInParagraphs/{id}")]
        public JsonResult GetWordsInParagraphs(string id)
        {
            Response.StatusCode = (int)HttpStatusCode.OK;
            return Json(_textRepository.Get(id));
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public JsonResult GetWordsCountInParagraphs(string id)
        {
            var wordsInParagraphs = _textRepository.Get(id).WordsInParagraphs;
            var wordsCounts = _textService.GetWordCounts(wordsInParagraphs);
            Response.StatusCode = (int)HttpStatusCode.OK;
            return Json(wordsCounts);
        }

        [AllowAnonymous]
        [HttpGet("letter/{id}/{paragraphIndex}/{wordIndex}/{symbolIndex}")]
        public JsonResult GetLetter(string id, int paragraphIndex, int wordIndex, int symbolIndex)
        {
            Response.StatusCode = (int)HttpStatusCode.OK;
            return Json(_textRepository.Get(id)
                            .WordsInParagraphs[paragraphIndex]
                            [wordIndex][symbolIndex]);
        }

        [AllowAnonymous]
        [HttpGet("wordCorrectness/{id}/{paragraphIndex}/{wordIndex}/{value}")]
        public JsonResult GetWordCorrectness(string id, int paragraphIndex, int wordIndex, string value)
        {
            value = value.Replace("`", "'");
            Response.StatusCode = (int)HttpStatusCode.OK;
            return Json(value.Equals(_textRepository.Get(id)
                        .WordsInParagraphs[paragraphIndex][wordIndex]));
        }

        [AllowAnonymous]
        // TODO: remake this using http://stackoverflow.com/questions/9981330/how-to-pass-an-array-of-integers-to-a-asp-net-web-api-rest-service
        [HttpPost("wordsForCheck/{id}")]
        public JsonResult PostCheckWords(string id, [FromBody]string[] words)
        {
            var correctWordLocatorsDtoList = _textService.CheckWordsCorrectness(id, words);
            Response.StatusCode = (int)HttpStatusCode.OK;
            return Json(correctWordLocatorsDtoList);
        }
    }
}
