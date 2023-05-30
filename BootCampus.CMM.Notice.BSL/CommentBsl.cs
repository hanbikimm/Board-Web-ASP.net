using BootCampus.CMM.Notice.DSL;
using BootCampus.Entities;
using BootCampus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BootCampus.CMM.Notice.BSL
{
    public class CommentBsl
    {
        CommentDsl commentDsl = new CommentDsl();

        #region 댓글 생성
        /// <summary>
        /// 댓글 생성의 성공 여부에 따라 데이터를 담고 결과를 반환함
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ResultEntity SaveComment(CommentEntity request)
        {
            ResultEntity response = new ResultEntity();

            bool insertResult = commentDsl.InsertComment(request);
            response.Success = insertResult;
            if (insertResult == true)
            {
                response.Message = "댓글 생성 성공";
            }
            else
            {
                response.Message = "댓글 생성 실패";
            }

            return response;
        }
        #endregion

        #region 댓글 수정
        /// <summary>
        /// 댓글 수정의 성공 여부에 따라 데이터를 담고 결과를 반환함
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>

        public ResultEntity ModifyComment(CommentEntity request)
        {
            ResultEntity response = new ResultEntity();

            bool updateResult = commentDsl.UpdateComment(request);
            response.Success = updateResult;
            if (updateResult == true)
            {
                response.Message = "댓글 수정 성공";
            }
            else
            {
                response.Message = "댓글 수정 실패";
            }

            return response;
        }
        #endregion 

        #region 댓글 삭제
        /// <summary>
        /// 댓글 삭제의 성공 여부에 따라 데이터를 담고 결과를 반환함
        /// </summary>
        /// <param name="requestId"></param>
        /// <returns></returns>
        public ResultEntity RemoveComment(int commentId)
        {
            ResultEntity response = new ResultEntity();

            bool removeResult = commentDsl.DeleteComment(commentId);
            response.Success = removeResult;
            if (removeResult == true)
            {
                response.Message = "댓글 삭제 성공";
            }
            else
            {
                response.Message = "댓글 삭제 실패";
            }

            return response;
        }
        #endregion 
    }
}
