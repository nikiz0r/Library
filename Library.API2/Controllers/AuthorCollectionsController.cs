using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Library.API2.Entities;
using Library.API2.Helpers;
using Library.API2.Models;
using Library.API2.Services;
using Microsoft.AspNetCore.Mvc;

namespace Library.API2.Controllers
{
    [Route("api/authorcollection")]
    public class AuthorCollectionController : Controller
    {
        private ILibraryRepository _libraryRepository;

        public AuthorCollectionController(ILibraryRepository libraryRepository)
        {
            _libraryRepository = libraryRepository;
        }

        [HttpPost]
        public IActionResult CreateAuthorCollection([FromBody] IEnumerable<AuthorForCreationDto> authorCollection)
        {
            if (authorCollection == null) return BadRequest();

            var authorEntities = Mapper.Map<IEnumerable<Author>>(authorCollection);

            foreach (var author in authorEntities)
                _libraryRepository.AddAuthor(author);

            if (!_libraryRepository.Save()) throw new Exception("Creating author collection failed on save.");

            var authorCollectionToReturn = Mapper.Map<IEnumerable<AuthorDto>>(authorEntities);
            var idsAsString = string.Join(",", authorCollectionToReturn.Select(a => a.Id));

            return CreatedAtRoute("AuthorCollection", new { ids = idsAsString }, authorCollectionToReturn);
        }

        [HttpGet("({ids})", Name = "GetAuthorCollection")]
        public IActionResult GetAuthorCollection(
            [ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
        {
            if (ids == null) return BadRequest();

            var authorEntities = _libraryRepository.GetAuthors(ids);

            if (ids.Count() != authorEntities.Count()) return NotFound();

            var authorsToReturn = Mapper.Map<IEnumerable<AuthorDto>>(authorEntities);
            return Ok(authorsToReturn);
        }
    }
}