using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using BootCampus.CMM.Notice.BSL;
using BootCampus.Models;

namespace BootCampus.Web.Controllers
{
    public class MemberController : Controller
    {
        MemberBsl memberBsl = new MemberBsl();

        #region 로그인
        public ActionResult Login(MemberModel request,string returnUrl)
        {
            
            // 아이디 비밀번호 둘다 입력된 경우
            if(!string.IsNullOrEmpty(request.member.USER_ID) && !string.IsNullOrEmpty(request.member.USER_PW))
            {
                MemberModel response = memberBsl.CheckUser(request);
                // 로그인 성공
                if (response.result.Success)
                {
                    FormsAuthentication.SetAuthCookie(response.member.USER_NAME, false);
                    if (TempData["returnUrl"] != null)
                    {
                        string url = TempData["returnUrl"].ToString();
                        TempData["returnUrl"] = null;
                        return Redirect(url);
                    }
                    return RedirectToAction("NoticeList", "Notice");
                }
                // 로그인 실패
                else
                {
                    ModelState.AddModelError("LoginError", "[로그인 실패] 아이디와 비밀번호를 확인해주세요.");
                    return View();
                }
            }
            else
            {
                // 아이디와 비밀번호 둘 다 입력되지 않은 경우(초기 상태)
                if(string.IsNullOrEmpty(request.member.USER_ID) && string.IsNullOrEmpty(request.member.USER_PW))
                {
                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        TempData["returnUrl"] = returnUrl;
                    }
                }
                else
                {
                    ModelState.AddModelError("LoginError", "[정보 미입력] 아이디와 비밀번호 모두 입력해주세요.");
                }

                return View();
            }
            

            
        }
        #endregion

        #region 로그아웃
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Member");
        }
        #endregion
    }
}