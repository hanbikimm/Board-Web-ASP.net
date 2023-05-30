using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BootCampus.Entities
{
    public class CommentEntity
    {
        #region Comment
        /// <summary>
        /// 댓글 번호
        /// </summary>
        public int REPLY_SEQ { get; set; }
        /// <summary>
        /// 부모 댓글 번호
        /// </summary>
        public int PARENT_SEQ { get; set; }
        /// <summary>
        /// 게시글 번호
        /// </summary>
        public int BOARD_SEQ { get; set; }
        /// <summary>
        /// 내용
        /// </summary>
        public string REPLY_CONTENTS { get; set; }
        /// <summary>
        /// 사용자 아이디
        /// </summary>
        public string USER_NAME { get; set; }
        /// <summary>
        /// 등록날짜
        /// </summary>
        public string CREATE_DATE { get; set; }
        /// <summary>
        /// 댓글 Depth
        /// </summary>
        public int LEVEL { get; set; }
        #endregion
    }
}
