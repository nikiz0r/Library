using AutoMapper;
using Library.API2.Entities;
using Library.API2.Helpers;
using Library.API2.Models;
using Library.API2.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.API2.Controllers
{
    [Route("api/authors")]
    public class AuthorsControllers : Controller
    {
        private ILibraryRepository _libraryRepository;

        public AuthorsControllers(ILibraryRepository libraryRepository)
        {
            _libraryRepository = libraryRepository;
        }

        [HttpGet]
        public IActionResult Authors()
        {
            var authorsFromRepo = _libraryRepository.GetAuthors();

            var authors = Mapper.Map<IEnumerable<AuthorDto>>(authorsFromRepo);
            return new OkObjectResult(authors);
        }

        [HttpGet("{authorId}", Name = "GetAuthor")]
        public IActionResult Authors([FromRoute]Guid authorId)
        {
            var authorFromRepo = _libraryRepository.GetAuthor(authorId);

            if (authorFromRepo == null) return NotFound();

            var author = Mapper.Map<AuthorDto>(authorFromRepo);

            return new OkObjectResult(author);
        }

        [HttpPost]
        public IActionResult Authors([FromBody]AuthorForCreationDto author)
        {
            if (author == null) return BadRequest();

            var authorEntity = Mapper.Map<Author>(author);

            _libraryRepository.AddAuthor(authorEntity);

            if (!_libraryRepository.Save()) throw new Exception("Creating an author failed on save."); //return StatusCode(500, "A problem happened with handling your request.");

            var authorToReturn = Mapper.Map<AuthorDto>(authorEntity);

            return CreatedAtRoute("GetAuthor", new { id = authorToReturn.Id }, authorToReturn);
        }
    }
}
