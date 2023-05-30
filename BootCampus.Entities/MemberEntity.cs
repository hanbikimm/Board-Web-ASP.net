using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BootCampus.Entities
{
    public class MemberEntity
    {
        /// <summary>
        /// 사용자 아이디
        /// </summary>
        public string USER_ID { get; set; }
        /// <summary>
        /// 사용자 비밀번호
        /// </summary>
        public string USER_PW { get; set; }
        /// <summary>
        /// 사용자 이름
        /// </summary>
        public string USER_NAME { get; set; }
    }
}
