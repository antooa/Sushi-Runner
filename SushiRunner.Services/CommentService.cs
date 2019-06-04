using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using SushiRunner.Data.Entities;
using SushiRunner.Data.Repositories;
using SushiRunner.Services.Dto;
using SushiRunner.Services.Interfaces;

namespace SushiRunner.Services
{
    public class CommentService:ICommentService
    {
        private readonly IRepository<Comment, long> _repository;
        private readonly IMapper _mapper;
        private bool _disposed;
        
        public CommentService(IRepository<Comment, long> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        
        public void Create(CommentDTO commentDto)
        {
            var comment = _mapper.Map<CommentDTO, Comment>(commentDto);
            _repository.Create(comment);
            _repository.Save();
        }
        public IEnumerable<CommentDTO> GetList()
        {
            var comments = _repository.GetList();
            return comments.Select(comment => _mapper.Map<Comment, CommentDTO>(comment)).ToList();
        }

//        public IEnumerable<CommentDTO> GetListByProductId(long id)
//        {
//            var comments = _repository.Search(c => c.ProductId == id);
//            
//            return comments.Select(comment => _mapper.Map<Comment, CommentDTO>(comment)).ToList();
//        }
        public CommentDTO Get(long id)
        {
            var comment = _repository.Get(id);
            return _mapper.Map<Comment, CommentDTO>(comment);
        }

        public void Update(CommentDTO commentDto)
        {
            var comment = _mapper.Map<CommentDTO, Comment>(commentDto);
            _repository.Update(comment);
            _repository.Save();
        }

        public void Delete(long id)
        {
            _repository.Delete(id);
            _repository.Save();
        }

        public virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _repository.Dispose();
                }
            }

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}