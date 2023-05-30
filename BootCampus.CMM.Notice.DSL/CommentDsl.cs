using BootCampus.Entities;
using BootCampus.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BootCampus.CMM.Notice.DSL
{
    public class CommentDsl
    {
        private const string _connectionString = @"server=DESKTOP-6L322N3;database=BOOTCAMPUS;pwd=mssql;user id=sa;Pooling=True;Min Pool Size=10;Max Pool Size=150;Load Balance Timeout=360;Connection Lifetime=360;Connect Timeout=30;Enlist=True;TrustServerCertificate=True";

        #region 댓글 생성
        /// <summary>
        /// 댓글 INSERT 쿼리를 실행하고, 실행 성공 여부를 bool로 반환함
        /// </summary>
        /// <param name="comment"></param>
        /// <returns></returns>
        public bool InsertComment(CommentEntity request)
        {
            bool result = false;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("dbo.UP_BOOTCAMPUS_REPLY_C", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PARENT_SEQ", request.PARENT_SEQ);
                    cmd.Parameters.AddWithValue("@BOARD_SEQ", request.BOARD_SEQ);
                    cmd.Parameters.AddWithValue("@REPLY_CONTENTS", request.REPLY_CONTENTS);
                    cmd.Parameters.AddWithValue("@USER_NAME", request.USER_NAME);


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

        #region 댓글 수정
        /// <summary>
        /// 댓글 UPDATE 쿼리를 실행하고, 실행 성공 여부를 bool로 반환함
        /// </summary>
        /// <param name="comment"></param>
        /// <returns></returns>
        public bool UpdateComment(CommentEntity request)
        {
            bool result = false;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("dbo.UP_BOOTCAMPUS_REPLY_U", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@REPLY_SEQ", request.REPLY_SEQ);
                    cmd.Parameters.AddWithValue("@REPLY_CONTENTS", request.REPLY_CONTENTS);

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

        #region 댓글 삭제
        /// <summary>
        /// 댓글 DELETE 쿼리를 실행하고, 실행 성공 여부를 bool로 반환함
        /// </summary>
        /// <param name="commentId"></param>
        /// <returns></returns>
        public bool DeleteComment(int commentId)
        {
            bool result = false;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("dbo.UP_BOOTCAMPUS_REPLY_D", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@REPLY_SEQ", commentId);

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

        #region 댓글 전체 삭제
        /// <summary>
        /// 댓글 DELETE 쿼리를 실행하고, 실행 성공 여부를 bool로 반환함
        /// </summary>
        /// <param name="boardID"></param>
        /// <returns></returns>
        public bool DeleteAllComment(int boardID)
        {
            bool result = false;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("dbo.UP_BOOTCAMPUS_REPLY_D2", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BOARD_SEQ", boardID);

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
