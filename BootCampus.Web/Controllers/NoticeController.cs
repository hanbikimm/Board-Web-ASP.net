using BootCampus.CMM.Notice.BSL;
using BootCampus.Entities;
using BootCampus.Models;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Web.Mvc;
using System.Web.Routing;
using ClosedXML.Excel;
using System.IO;
using System;

namespace BootCampus.Web.Controllers
{
    public class NoticeController : Controller
    {
        NoticeBsl noticeBsl = new NoticeBsl();

        #region 상태 수정
        public JsonResult State(NoticeModel request)
        {
            ResultEntity response = noticeBsl.ModifyState(request);

            return Json(response);
        }
        #endregion

        #region 엑셀 다운로드
        public FileResult ExcelDownLoad(SearchEntity request)
        {
            DataTable noticeTable = noticeBsl.DownloadExcel(request);

            string fileName = $"NoticeList_{DateTime.Now.ToString("yyMMddHHmmss")}.xlsx";
            XLWorkbook workbook = new XLWorkbook();
            workbook.AddWorksheet(noticeTable, "Notice");
            using (MemoryStream stream = new MemoryStream())
            {
                workbook.SaveAs(stream);
                return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
        }
        #endregion


        #region 목록 조회
        [Authorize]
        public ActionResult NoticeList(NoticeModel request)
        {
            // 상태 세팅
            List<SelectEntity> stateList = noticeBsl.GetStateList();
            ViewBag.stateList = stateList;

            //검색 조건 세팅
            List<SelectEntity> typeList = new List<SelectEntity>();
            typeList.Add(new SelectEntity() { TEXT = "", VALUE = 0 });
            typeList.Add(new SelectEntity() { TEXT = "제목", VALUE = 1 });
            typeList.Add(new SelectEntity() { TEXT = "내용", VALUE = 2 });
            typeList.Add(new SelectEntity() { TEXT = "번호", VALUE = 3 });
            typeList.Add(new SelectEntity() { TEXT = "작성자", VALUE = 4 });
            ViewBag.typeList = typeList;
            
            request.condition.SEARCH_WORD = string.IsNullOrEmpty(request.condition.SEARCH_WORD) ? "" : request.condition.SEARCH_WORD;
            request.condition.PAGE = request.condition.PAGE > 1 ? request.condition.PAGE : 1;

            NoticeModel response = noticeBsl.GetNoticeList(request);

            return View(response);
        }
        #endregion

        #region 조회수 증가
        public ActionResult ViewCount(int boardId)
        {
            ResultEntity response = noticeBsl.PlusViewCount(boardId);

            return Json(response);
        }
        #endregion

        #region 상세 조회
        [Authorize]
        public ActionResult NoticeDetail(int boardId)
        {
            // 상태 세팅
            List<SelectEntity> stateList = noticeBsl.GetStateList();
            ViewBag.stateList = stateList;

            NoticeModel response = noticeBsl.GetNoticeDetail(boardId);

            return View(response);
        }
        #endregion

        #region 게시글 생성/수정 화면 로드
        [Authorize]
        public ActionResult NoticeHandle(int boardID)
        {
            NoticeModel response = new NoticeModel();
            if (boardID > 0)
            {
                response = noticeBsl.GetNoticeDetail(boardID);
                string title = response.notice.TITLE;
                response.notice.TITLE = title.Substring(title.IndexOf(" ")+1);
                ViewBag.viewType = "수정";
                ViewBag.message = "수정하시겠습니까?";
                ViewBag.date = response.notice.WRITE_DATE;
                
            }
            else
            {
                ViewBag.viewType = "등록";
                ViewBag.message = "저장하시겠습니까?";
                ViewBag.date = DateTime.Now.ToString("yyyy-MM-dd");
            }
            return View(response);
        }
        #endregion

        #region 게시글 생성
        public JsonResult Create(NoticeEntity request)
        {
            request.USER_NAME = this.User.Identity.Name;
            ResultEntity response = noticeBsl.SaveNotice(request);

            return Json(response);
        }
        #endregion


        #region 게시글 수정
        public JsonResult Update(NoticeEntity requset)
        {
            ResultEntity response = noticeBsl.ModifyNotice(requset);

            return Json(response);
        }
        #endregion

        #region 게시글 삭제
        public JsonResult Delete(int boardId)
        {
            ResultEntity response = noticeBsl.RemoveNotice(boardId);

            return Json(response);
        }
        #endregion
    }
}