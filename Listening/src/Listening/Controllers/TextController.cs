using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Cors;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Net.Http.Headers;
using MongoDB.Bson;
using WebListening.Exceptions;
using WebListening.Models;
using WebListening.Repositories;
using WebListening.Services;
using AutoMapper;
using Listening.Aspect;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace WebListening.Controllers
{
    [EnableCors("AllowAll")]
    [ExceptionHandle]
    [Authorize(Roles = "admin")]
    [Route("api/[controller]")]
    public class TextController : Controller
    {
        private IRepository<Text> _textRepository;
        private readonly TextService _textService;

        public TextController(IRepository<Text> repository, TextService textService)
        {
            _textRepository = repository;
            _textService = textService;
        }

        [AllowAnonymous]
        [HttpGet]
        public IEnumerable<TextDescriptionDto> GetAllTextsDescription()
        {
            return Mapper.Map<IQueryable<Text>, IEnumerable<TextDescriptionDto>>(_textRepository.GetAll());
        }

        [HttpGet("{textId}")]
        public TextDto GetText(string textId)
        {
            TextDto textDto = _textService.GenerateTextDtoById(textId);
            return textDto;
        }

        [HttpPost]
        public string PostText([FromBody]TextDto textDto)
        {
            Text text = _textService.GenerateTextByDto(textDto);
            _textRepository.Insert(text);
            return $"Success post {textDto.Title}";
        }


        [HttpPut("{id}")]
        public string PutText(string id, [FromBody]TextDto textDto)
        {
            Text text = _textService.GenerateTextByDto(textDto);

            _textRepository.Update(text);
            return $"Success put {textDto.Title}";
        }

        [HttpDelete("{id}")]
        public void DeleteText(string id)
        {
            _textRepository.Delete(id);
        }
    }
}
