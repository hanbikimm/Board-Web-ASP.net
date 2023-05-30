using BootCampus.CMM.Notice.DSL;
using BootCampus.Entities;
using BootCampus.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Transactions;


namespace BootCampus.CMM.Notice.BSL
{
    public class NoticeBsl
    {
        NoticeDsl noticeDsl = new NoticeDsl();
        CommentDsl commentDsl = new CommentDsl();

        #region State List 조회
        /// <summary>
        /// 프론트 Select에 사용할 상태 목록을 가져옴
        /// </summary>
        /// <returns></returns>
        public List<SelectEntity> GetStateList()
        {
            List<SelectEntity> result = new List<SelectEntity>();

            DataTable stateTable = noticeDsl.SelectStateList();
            foreach (DataRow data in stateTable.Rows)
            {
                SelectEntity state = new SelectEntity();
                state.VALUE = Convert.ToInt32(data["STATE_SEQ"]);
                state.TEXT = data["STATE_TYPE"].ToString();

                result.Add(state);
            }

            return result;
        }
        #endregion

        #region State 수정
        /// <summary>
        /// 상태값을 수정하고, 그에 맞는 실행 결과를 반환해줌
        /// </summary>
        /// <param name="notice"></param>
        /// <returns></returns>
        public ResultEntity ModifyState(NoticeModel request)
        {
            ResultEntity result = new ResultEntity();

            bool updateResult = noticeDsl.UpdateState(request);
            result.Success = updateResult;
            if (updateResult == true)
                result.Message = "게시글 상태 수정 성공";
            else
                result.Message = "게시글 상태 수정 실패";

            return result;
        }
        #endregion

        #region 엑셀 다운로드
        /// <summary>
        /// 페이징 처리되지 않은, 검색 조건에 맞는 전체 목록을 조회해 데이터를 가져옴
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public DataTable DownloadExcel(SearchEntity request)
        {
            //List<NoticeEntity> result = new List<NoticeEntity>();

            DataTable noticeTable = noticeDsl.SelectNoticeListForExcel(request);

            
            return noticeTable;

            
            //if (result.Rows.Count > 0)
            //{

            //    foreach (DataRow notice in noticeTable.Rows)
            //    {
            //        NoticeEntity noticeEntity = new NoticeEntity();

            //        noticeEntity.BOARD_SEQ = Convert.ToInt32(notice["BOARD_SEQ"]);
            //        noticeEntity.STATE = notice["STATE"].ToString();
            //        noticeEntity.TITLE = notice["TITLE"].ToString();
            //        noticeEntity.USER_NAME = notice["USER_NAME"].ToString();
            //        noticeEntity.WRITE_DATE = Convert.ToDateTime(notice["WRITE_DATE"]).ToString("yyyy-MM-dd");
            //        noticeEntity.VIEW_COUNT = Convert.ToInt32(notice["VIEW_COUNT"]);
            //        noticeEntity.REPLY_COUNT = Convert.ToInt32(notice["REPLY_COUNT"]);

            //        result.Add(noticeEntity);
            //    }
            //}

            //return result;
        }
        #endregion

        #region Notice List 조회
        /// <summary>
        /// 검색 조건에 맞는 리스트를 가져와 검색조건, 페이지, 게시글 목록 정보를 가져옴
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public NoticeModel GetNoticeList(NoticeModel request)
        {
            NoticeModel response = new NoticeModel();

            DataSet noticeListSet = noticeDsl.SelectNoticeList(request);
            DataTable noticeTable = noticeListSet.Tables[0];
            if (noticeTable.Rows.Count > 0)
            {
                response.result.Success = true;
                response.result.Message = "게시글 목록 조회 성공";

                response.condition = request.condition;
                response.TOTAL_PAGE = Convert.ToInt32(noticeListSet.Tables[1].Rows[0]["TOTAL_PAGE"]);

                // 페이지 계산
                int nowPage = response.condition.PAGE;
                int totalPage = response.TOTAL_PAGE;
                response.PAGES = new List<decimal>();

                //페이징에 보여줄 페이지 수
                var showPage = 5;
                //시작될 페이지 번호
                var startPage = Math.Truncate((decimal)((nowPage - 1) / showPage)) * showPage + 1;
                var checkRestPage = totalPage - (startPage - 1);
                //페이징에 보여줄 실제 페이지 수
                var pageCount = checkRestPage < showPage ? checkRestPage : showPage;

                List<decimal> pageList = new List<decimal>();
                for (var i = 0; i < pageCount; i++)
                {
                    response.PAGES.Add(i + startPage);
                }

                // 게시글 목록
                foreach (DataRow data in noticeTable.Rows)
                {
                    NoticeEntity notice = new NoticeEntity();


                    notice.BOARD_SEQ = Convert.ToInt32(data["BOARD_SEQ"]);
                    notice.STATE = data["STATE"].ToString();
                    notice.TITLE = data["TITLE"].ToString();
                    notice.USER_NAME = data["USER_NAME"].ToString();
                    notice.WRITE_DATE = Convert.ToDateTime(data["WRITE_DATE"]).ToString("yyyy-MM-dd");
                    notice.VIEW_COUNT = Convert.ToInt32(data["VIEW_COUNT"]);
                    notice.REPLY_COUNT = Convert.ToInt32(data["REPLY_COUNT"]);

                    response.noticeList.Add(notice);
                }
            }
            else
            {
                response.result.Success = false;
                response.result.Message = "게시글 목록이 없거나 조회 실패";
                response.PAGES = new List<decimal> { 1 };
            }

            return response;
        }
        #endregion

        #region 조회수 증가
        /// <summary>
        /// 조회수를 증가시키고, 그에 맞는 실행 결과를 반환해줌
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ResultEntity PlusViewCount(int boardId)
        {
            ResultEntity response = new ResultEntity();

            bool updateResult = noticeDsl.UpdateViewCount(boardId);

            response.Success = updateResult;
            if (updateResult == true)
            {
                response.Message = "조회수 증가 성공";
            }
            else
            {
                response.Message = "조회수 증가 실패";
            }

            return response;
        }
        #endregion

        #region Notice Detail 조회
        /// <summary>
        /// notice의 id값을 토대로 상세조회(게시글, 댓글 목록)함
        /// 데이터가 있는지 DataTable.Rows.Count로 확인함
        /// </summary>
        /// <param name="boardId"></param>
        /// <returns></returns>
        public NoticeModel GetNoticeDetail(int boardId)
        {
            NoticeModel response = new NoticeModel();

            DataSet detailSet = noticeDsl.SelectNoticeDetail(boardId);
            DataTable noticeTable = detailSet.Tables[0];
            if (noticeTable.Rows.Count > 0)
            {

                response.result.Success = true;
                response.result.Message = "게시글 상세 조회 성공";

                DataRow notice = noticeTable.Rows[0];
                response.notice.BOARD_SEQ = Convert.ToInt32(notice["BOARD_SEQ"]);
                response.notice.TITLE = notice["TITLE"].ToString();
                response.notice.STATE = notice["STATE"].ToString();
                response.notice.USER_NAME = notice["USER_NAME"].ToString();
                response.notice.CONTENTS = notice["CONTENTS"].ToString();
                response.notice.WRITE_DATE = Convert.ToDateTime(notice["WRITE_DATE"]).ToString("yyyy-MM-dd");
                response.notice.VIEW_COUNT = Convert.ToInt32(notice["VIEW_COUNT"]);
                response.notice.FILE_NAME = notice["FILE_NAME"].ToString();
                response.notice.FILE = notice["FILE"].ToString();

                DataTable commentTable = detailSet.Tables[1];
                if (commentTable.Rows.Count > 0)
                {
                    foreach (DataRow data in commentTable.Rows)
                    {
                        CommentEntity comment = new CommentEntity();

                        comment.REPLY_SEQ = Convert.ToInt32(data["REPLY_SEQ"]);
                        if (!string.IsNullOrEmpty(data["PARENT_SEQ"].ToString()))
                        {
                            comment.PARENT_SEQ = Convert.ToInt32(data["PARENT_SEQ"]);
                        }
                        comment.BOARD_SEQ = Convert.ToInt32(data["BOARD_SEQ"]);
                        comment.REPLY_CONTENTS = data["REPLY_CONTENTS"].ToString();
                        comment.USER_NAME = data["USER_NAME"].ToString();
                        comment.CREATE_DATE = Convert.ToDateTime(data["CREATE_DATE"]).ToString("yyyy-MM-dd HH:mm");
                        comment.LEVEL = Convert.ToInt32(data["LEVEL"]);

                        response.commentList.Add(comment);
                    }
                }
            }
            else
            {
                response.result.Success = false;
                response.result.Message = "게시글 상세 조회 실패";
            }

            return response;
        }
        #endregion



        #region 게시글 생성
        /// <summary>
        /// 게시글 생성의 성공 여부에 따라 데이터를 담고 결과를 반환함
        /// </summary>
        /// <param name="notice"></param>
        /// <returns></returns>
        public ResultEntity SaveNotice(NoticeEntity request)
        {
            ResultEntity result = new ResultEntity();

            bool insertResult = noticeDsl.InsertNotice(request);
            result.Success = insertResult;
            if (insertResult == true)
            {
                result.Message = "게시글 생성 성공";
            }
            else
            {
                result.Message = "게시글 생성 실패";
            }

            return result;
        }
        #endregion

        #region 게시글 수정
        /// <summary>
        /// 게시글 수정의 성공 여부에 따라 데이터를 담고 결과를 반환함
        /// </summary>
        /// <param name="notice"></param>
        /// <returns></returns>
        public ResultEntity ModifyNotice(NoticeEntity requset)
        {
            ResultEntity result = new ResultEntity();

            bool updateResult = noticeDsl.UpdateNotice(requset);
            result.Success = updateResult;
            if (updateResult == true)
            {
                result.Message = "게시글 수정 성공";
            }
            else
            {
                result.Message = "게시글 수정 실패";
            }

            return result;
        }
        #endregion

        #region 게시글 삭제 (transaction)
        /// <summary>
        /// 게시글 삭제의 성공 여부에 따라 데이터를 담고 결과를 반환함
        /// DB FK 조건에 따라 해당 게시글의 댓글들 먼저 삭제하고, 게시글을 삭제함
        /// 댓글, 게시글 삭제에 모두 성공하면 그에 맞는 결과값을 반환함
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ResultEntity RemoveNotice(int boardId)
        {
            ResultEntity response = new ResultEntity();

            bool commentResult = false;
            bool noticeResult = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
            {
                commentResult = commentDsl.DeleteAllComment(boardId);
                noticeResult = noticeDsl.DeleteNotice(boardId);
                if (noticeResult == true && commentResult == true)
                {
                    response.Success = true;
                    response.Message = "게시글 삭제 성공";
                }
                else
                {
                    response.Success = false;
                    response.Message = "게시글 삭제 실패";
                }
                scope.Complete();
            }
            return response;
        }
        #endregion
    }
}