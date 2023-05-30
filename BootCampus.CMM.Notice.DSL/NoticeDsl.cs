using BootCampus.Entities;
using BootCampus.Models;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace BootCampus.CMM.Notice.DSL
{
    public class NoticeDsl
    {
        private const string _connectionString = @"server=DESKTOP-6L322N3;database=BOOTCAMPUS;pwd=mssql;user id=sa;Pooling=True;Min Pool Size=10;Max Pool Size=150;Load Balance Timeout=360;Connection Lifetime=360;Connect Timeout=30;Enlist=True;TrustServerCertificate=True";

        #region State List 조회
        public DataTable SelectStateList()
        {
            DataTable stateTable = new DataTable();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("dbo.UP_BOOTCAMPUS_STATE_R", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                    dataAdapter.Fill(stateTable);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }
            }

            return stateTable;
        }
        #endregion

        #region Notice State 수정
        public bool UpdateState(NoticeModel request)
        {
            bool result = false;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("dbo.UP_BOOTCAMPUS_BOARD_STATE_U", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BOARD_SEQ", request.notice.BOARD_SEQ);
                    cmd.Parameters.AddWithValue("@STATE", request.notice.STATE);

                    cmd.ExecuteNonQuery();
                    result = true;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }
            }

            return result;
        }
        #endregion

        #region 엑셀 다운로드
        /// <summary>
        /// 만약 조건이 있다면 조건 파라미터값을 넣어줌
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public DataTable SelectNoticeListForExcel(SearchEntity request)
        {
            DataTable noticeTable = new DataTable();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("dbo.UP_BOOTCAMPUS_BOARD_FOR_EXCEL_L", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    // 조건이 있는 경우
                    if (CheckCondition(request))
                    {
                        cmd.Parameters.AddWithValue("@STATE", GetState(request.STATE));

                        // 검색어가 있는 경우
                        if (request.SEARCH_TYPE != 0 && request.SEARCH_WORD != null)
                        {
                            cmd.Parameters.AddWithValue("@SEARCH_TYPE", GetType(request.SEARCH_TYPE));
                            cmd.Parameters.AddWithValue("@SEARCH_WORD", request.SEARCH_WORD);
                        }

                        // 페이지 이동의 경우
                        if (request.PAGE > 1)
                        {
                            cmd.Parameters.AddWithValue("@page", request.PAGE);
                        }
                    }

                    SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                    dataAdapter.Fill(noticeTable);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }

            }

            return noticeTable;

        }
        #endregion

        #region Notice List 조회
        private bool CheckCondition(SearchEntity condition)
        {
            bool result = true;
            if(condition.STATE == 0 && 
                condition.SEARCH_TYPE == 0 && 
                string.IsNullOrEmpty(condition.SEARCH_WORD) &&
                condition.PAGE == 1)
            {
                result = false;
            }

            return result;
        }

        private string GetType(int type)
        {
            string strType = null;
            switch (type)
            {
                case 1:
                    strType = "제목";
                    break;
                case 2:
                    strType = "내용";
                    break ;
                case 3:
                    strType = "번호";
                    break;
                case 4:
                    strType = "작성자";
                    break;
            }
            return strType;
        }

        private string GetState(int state)
        {
            string strState = null;
            switch (state)
            {
                case 1:
                    strState = "오픈";
                    break;
                case 2:
                    strState = "접수됨";
                    break;
                case 3:
                    strState = "진행중";
                    break;
                case 4:
                    strState = "완료됨";
                    break;
            }
            return strState;
        }

        /// <summary>
        /// 만약 조건이 있다면 조건 파라미터값을 넣어주고,
        /// 페이지가 1보다 크다면 페이지 파라미터값을 넣어줌
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public DataSet SelectNoticeList(NoticeModel request)
        {
            DataSet noticeListSet = new DataSet();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("dbo.UP_BOOTCAMPUS_BOARD_L", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    // 조건이 있는 경우
                    if(CheckCondition(request.condition))
                    {
                        cmd.Parameters.AddWithValue("@STATE", GetState(request.condition.STATE));
  
                        // 검색어가 있는 경우
                        if (request.condition.SEARCH_TYPE != 0 && request.condition.SEARCH_WORD != null)
                        {
                            cmd.Parameters.AddWithValue("@SEARCH_TYPE", GetType(request.condition.SEARCH_TYPE));
                            cmd.Parameters.AddWithValue("@SEARCH_WORD", request.condition.SEARCH_WORD);
                        }

                        // 페이지 이동의 경우
                        if (request.condition.PAGE > 1)
                        {
                            cmd.Parameters.AddWithValue("@page", request.condition.PAGE);
                        }
                    }
                    

                    SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                    dataAdapter.Fill(noticeListSet);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }

            }

            return noticeListSet;
        }
        #endregion

        #region 조회수 증가
        /// <summary>
        /// id값을 가져와 조회수를 증가해줌
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool UpdateViewCount(int boardId)
        {
            bool result = false;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("dbo.UP_BOOTCAMPUS_BOARD_VIEW_COUNT_U", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BOARD_SEQ", boardId);

                    cmd.ExecuteNonQuery();
                    result = true;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }
            }
            return result;
        }
        #endregion

        #region Notice Detail 조회
        /// <summary>
        /// 해당 id값의 상세조회 데이터를 반환함
        /// </summary>
        /// <param name="boardId"></param>
        /// <returns></returns>
        public DataSet SelectNoticeDetail(int boardId)
        {
            DataSet detailSet = new DataSet();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("dbo.UP_BOOTCAMPUS_BOARD_R", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BOARD_SEQ", boardId);

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(detailSet);

                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }
            }

            return detailSet;
        }
        #endregion


        #region Notice 생성
        /// <summary>
        /// CONTENTS와 FILE은 필수조건이 아니기 때문에, 구분 후 파라미터값 넣어줌
        /// </summary>
        /// <param name="notice"></param>
        /// <returns></returns>
        public bool InsertNotice(NoticeEntity request)
        {
            bool result = false;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("dbo.UP_BOOTCAMPUS_BOARD_C", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@STATE", "오픈");
                    cmd.Parameters.AddWithValue("@TITLE", request.TITLE);
                    if (request.CONTENTS == null)
                    {
                        request.CONTENTS = "";
                    }
                    cmd.Parameters.AddWithValue("@CONTENTS", request.CONTENTS);
                    cmd.Parameters.AddWithValue("@USER_NAME", request.USER_NAME);
                    if (!string.IsNullOrEmpty(request.FILE_NAME))
                    {
                        cmd.Parameters.AddWithValue("@FILE_NAME", request.FILE_NAME);
                        cmd.Parameters.AddWithValue("@FILE", request.FILE);
                    }

                    cmd.ExecuteNonQuery();
                    result = true;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }
            }

            return result;
        }
        #endregion

        #region Notice 수정
        /// <summary>
        /// CONTENTS와 FILE은 필수조건이 아니기 때문에, 구분 후 파라미터값 넣어줌
        /// </summary>
        /// <param name="notice"></param>
        /// <returns></returns>
        public bool UpdateNotice(NoticeEntity requset)
        {
            bool result = false;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("dbo.UP_BOOTCAMPUS_BOARD_U", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BOARD_SEQ", requset.BOARD_SEQ);
                    cmd.Parameters.AddWithValue("@TITLE", requset.TITLE);
                    cmd.Parameters.AddWithValue("@CONTENTS", requset.CONTENTS);
                    if (!string.IsNullOrEmpty(requset.FILE_NAME))
                    {
                        cmd.Parameters.AddWithValue("@FILE_NAME", requset.FILE_NAME);
                        cmd.Parameters.AddWithValue("@FILE", requset.FILE);
                    }


                    cmd.ExecuteNonQuery();
                    result = true;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }
            }

            return result;
        }
        #endregion

        #region Notice 삭제
        /// <summary>
        /// id값에 해당하는 데이터를 삭제해줌
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteNotice(int boardId)
        {
            bool result = false;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("dbo.UP_BOOTCAMPUS_BOARD_D", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BOARD_SEQ", boardId);

                    cmd.ExecuteNonQuery();
                    result = true;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }
            }

            return result;
        }
        #endregion
    }
}