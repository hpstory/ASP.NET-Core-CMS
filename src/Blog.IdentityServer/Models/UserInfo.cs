using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.IdentityServer.Models
{
    public class UserInfo
    {
        public UserInfo() { }
        public UserInfo(string loginName, string loginPassword)
        {
            LoginName = loginName;
            LoginPassword = loginPassword;
            RealName = LoginName;
            Status = 0;
            CreateTime = DateTime.Now;
            UpdateTime = DateTime.Now;
            LastErrTime = DateTime.Now;
            ErrorCount = 0;
            Name = "";
        }
        /// <summary>
        /// 用户ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 登录账号
        /// </summary>
        public string LoginName { get; set; }
        /// <summary>
        /// 登录密码
        /// </summary>
        public string LoginPassword { get; set; }
        /// <summary>
        /// 真实姓名
        /// </summary>
        public string RealName { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public System.DateTime CreateTime { get; set; } = DateTime.Now;
        /// <summary>
        /// 更新时间
        /// </summary>
        public System.DateTime UpdateTime { get; set; } = DateTime.Now;

        /// <summary>
        ///最后登录时间 
        /// </summary>
        public DateTime LastErrTime { get; set; } = DateTime.Now;

        /// <summary>
        ///错误次数 
        /// </summary>
        public int ErrorCount { get; set; }



        /// <summary>
        /// 登录账号
        /// </summary>
        public string Name { get; set; }

        // 性别
        public int Gender { get; set; } = 0;
        // 年龄
        public int Age { get; set; }
        // 生日
        public DateTime Birth { get; set; } = DateTime.Now;
        // 地址
        public string Address { get; set; }

        public bool IsDeleted { get; set; }
    }
}
