using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BootCampus.Entities
{
    public class NoticeEntity
    {

        #region Notice
        /// <summary>
        /// 게시글 번호
        /// </summary>
        public int BOARD_SEQ { get; set; }
        /// <summary>
        /// 상태
        /// </summary>
        public string STATE { get; set; }
        /// <summary>
        /// 제목
        /// </summary>
        public string TITLE { get; set; }
        /// <summary>
        /// 내용
        /// </summary>
        public string CONTENTS { get; set; }

        /// <summary>
        /// 파일명
        /// </summary>
        public string FILE_NAME { get; set; }
        /// <summary>
        /// 파일
        /// </summary>
        public string FILE { get; set; }
        /// <summary>
        /// 사용자 아이디
        /// </summary>
        public string USER_NAME { get; set; }
        /// <summary>
        /// 등록날짜
        /// </summary>
        public string WRITE_DATE { get; set; }
        /// <summary>
        /// 조회수
        /// </summary>
        public int VIEW_COUNT { get; set; }
        /// <summary>
        /// 게시글의 댓글 수
        /// </summary>
        public int REPLY_COUNT { get; set; }
        #endregion

    }
}
