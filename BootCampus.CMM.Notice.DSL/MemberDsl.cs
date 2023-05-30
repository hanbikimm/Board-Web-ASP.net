using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace BootCampus.CMM.Notice.DSL
{
    public class MemberDsl
    {
        private const string _connectionString = @"server=DESKTOP-6L322N3;database=BOOTCAMPUS;pwd=mssql;user id=sa;Pooling=True;Min Pool Size=10;Max Pool Size=150;Load Balance Timeout=360;Connection Lifetime=360;Connect Timeout=30;Enlist=True;TrustServerCertificate=True";

        #region
        /// <summary>
        /// 유저 정보에 맞는 데이터를 가져와 반환해줌
        /// </summary>
        /// <param name="id"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public DataTable SelectMember(string id, string password)
        {
            DataTable user = new DataTable();
            using(SqlConnection conn = new SqlConnection(_connectionString))
            {
                try
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("dbo.UP_BOOTCAMPUS_USER_R", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@USER_ID", id);
                    cmd.Parameters.AddWithValue("@USER_PW", password);

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(user);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }
            }
            return user;
        }
        #endregion
    }
}
