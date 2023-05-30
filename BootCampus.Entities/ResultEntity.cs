using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BootCampus.Entities
{
    public class ResultEntity
    {
        #region Result
        /// <summary>
        /// 성공 여부
        /// </summary>
        public bool Success { get; set; }
        /// <summary>
        /// 결과 메세지
        /// </summary>
        public string Message { get; set; }
        #endregion
    }
}
