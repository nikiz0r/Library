﻿using AutoMapper;
using Library.API2.Models;
using Library.API2.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.API2.Controllers
{
    [Route("api/authors/{authorId}/books")]
    public class BooksController : Controller
    {
        private ILibraryRepository _libraryRepository;

        public BooksController(ILibraryRepository libraryRepository)
        {
            _libraryRepository = libraryRepository;
        }

        [HttpGet]
        public IActionResult BooksForAuthor(Guid authorId)
        {
            if (!_libraryRepository.AuthorExists(authorId)) return NotFound();

            var booksForAuthorFromRepo = _libraryRepository.GetBooksForAuthor(authorId);

            var booksForAuthor = Mapper.Map<IEnumerable<BookDto>>(booksForAuthorFromRepo);

            return new OkObjectResult(booksForAuthor);
        }

        [HttpGet("{id}")]
        public IActionResult BooksForAuthor(Guid authorId, Guid id)
        {
            if (!_libraryRepository.AuthorExists(authorId)) return NotFound();

            var bookForAuthorFromRepo = _libraryRepository.GetBookForAuthor(authorId, id);
            if (bookForAuthorFromRepo == null) return NotFound();

            var bookForAuthor = Mapper.Map<BookDto>(bookForAuthorFromRepo);

            return new OkObjectResult(bookForAuthor);
        }
    }
}