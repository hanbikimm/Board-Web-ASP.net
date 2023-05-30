using BootCampus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BootCampus.Models
{
    public class MemberModel
    {
        #region Result
        /// <summary>
        /// 실행 결과
        /// </summary>
        public ResultEntity result { get; set; } = new ResultEntity();
        #endregion
        #region Member
        /// <summary>
        /// 회원
        /// </summary>
        public MemberEntity member { get; set; } = new MemberEntity();
        #endregion
    }
}
