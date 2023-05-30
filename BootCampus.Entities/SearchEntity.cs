using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BootCampus.Entities
{
    public class SearchEntity
    {
        #region Search
        /// <summary>
        /// 게시글 상태
        /// </summary>
        public int STATE { get; set; }
        /// <summary>
        /// 검색조건
        /// </summary>
        public int SEARCH_TYPE { get; set; }
        /// <summary>
        /// 검색어
        /// </summary>
        public string SEARCH_WORD { get; set; }
        /// <summary>
        /// 페이지
        /// </summary>
        public int PAGE { get; set; }
        #endregion
    }
}
