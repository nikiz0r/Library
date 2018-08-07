using System;
using System.Collections;
using System.Collections.Generic;
using AutoMapper;
using Library.API2.Entities;
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
        public IActionResult AuthorCollection([FromBody] IEnumerable<AuthorForCreationDto> authorCollection)
        {
            if (authorCollection == null) return BadRequest();

            var authorEntities = Mapper.Map<IEnumerable<Author>>(authorCollection);

            foreach (var author in authorEntities)
                _libraryRepository.AddAuthor(author);

            if (!_libraryRepository.Save()) throw new Exception("Creating author collection failed on save.");

            return new OkResult();
        }
    }
}