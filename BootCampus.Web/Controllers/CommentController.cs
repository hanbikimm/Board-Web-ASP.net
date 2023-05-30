using BootCampus.CMM.Notice.BSL;
using BootCampus.Entities;
using BootCampus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BootCampus.Web.Controllers
{
    public class CommentController : Controller
    {
        NoticeBsl noticeBsl = new NoticeBsl();
        CommentBsl commentBsl = new CommentBsl();

        #region 댓글 생성
        public JsonResult Create(CommentEntity request)
        {
            request.USER_NAME = this.User.Identity.Name;
            ResultEntity response = commentBsl.SaveComment(request);

            return Json(response);
        }
        #endregion


        #region 댓글 수정
        public JsonResult Update(CommentEntity request)
        {
            ResultEntity response = commentBsl.ModifyComment(request);

            return Json(response);
        }
        #endregion

        #region 댓글 삭제
        public JsonResult Delete(int commentId)
        {
            ResultEntity response = commentBsl.RemoveComment(commentId);

            return Json(response);
        }
        #endregion
    }
}