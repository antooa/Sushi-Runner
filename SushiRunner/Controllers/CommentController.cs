using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SushiRunner.Data.Entities;
using SushiRunner.Services.Dto;
using SushiRunner.Services.Interfaces;
using SushiRunner.ViewModels;
using SushiRunner.ViewModels.Home;

namespace SushiRunner.Controllers
{
    public class CommentController:Controller
    {
        private readonly ICommentService _commentService;
        private readonly IMapper _mapper;

        public CommentController(ICommentService service, IMapper mapper)
        {
            _mapper = mapper;
            _commentService = service;
        }

        public IActionResult Index()
        {
            var dtos = _commentService.GetList();

            var comments = dtos.Select(dto => _mapper.Map<CommentDTO, CommentModel>(dto)).ToList();
            return View(comments);
        }
        
        [HttpPost]
        public IActionResult Create([Bind("Message", "ProductId")] CommentModel commentModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var commentDto = _mapper.Map<CommentModel, CommentDTO>(commentModel);
                    _commentService.Create(commentDto);
                }
                catch (Exception e)
                {
                    return View(e.Message);
                }

                return RedirectToAction("Index","Home");
            }

            return RedirectToAction("Index","Home");
        }

        
//        public IEnumerable<CommentModel> GetByProductId(long id)
//        {
////            var dtos = _commentService.GetListByProductId(2);
//            var comments = dtos.Select(dto => _mapper.Map<CommentDTO, CommentModel>(dto)).ToList();
//            return comments;
//        }
    }
}