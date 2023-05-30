using BootCampus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BootCampus.Models
{
    public class NoticeModel
    {
        #region Result
        /// <summary>
        /// 실행 결과
        /// </summary>
        public ResultEntity result { get; set; } = new ResultEntity();
        #endregion

        #region Search
        /// <summary>
        /// 검색 조건
        /// </summary>
        public SearchEntity condition { get; set; } = new SearchEntity();
        #endregion

        #region PAGE
        /// <summary>
        /// 게시글 목록 전체 페이지
        /// </summary>
        public int TOTAL_PAGE { get; set; }
        public List<decimal> PAGES { get; set; }
        #endregion

        #region Notice
        /// <summary>
        /// 게시글 상세
        /// </summary>
        public NoticeEntity notice { get; set; } = new NoticeEntity();

        /// <summary>
        /// 게시글 목록
        /// </summary>
        public List<NoticeEntity> noticeList { get; set; } = new List<NoticeEntity>();
        #endregion

        #region Comment
        /// <summary>
        /// 댓글 상세
        /// </summary>
        public CommentEntity comment { get; set; } = new CommentEntity();
        /// <summary>
        /// 댓글 목록
        /// </summary>
        public List<CommentEntity> commentList { get; set; } = new List<CommentEntity>();
        #endregion
    }
}
