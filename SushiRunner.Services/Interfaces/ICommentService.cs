using SushiRunner.Services.Dto;
using System.Collections.Generic;

namespace SushiRunner.Services.Interfaces
{
    public interface ICommentService: ICrudService<CommentDTO, long>
    {
//        IEnumerable<CommentDTO> GetListByProductId(long id);
    }
}