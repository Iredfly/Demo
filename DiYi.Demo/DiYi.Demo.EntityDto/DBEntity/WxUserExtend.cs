using DiYi.Demo.EntityDto.Extend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiYi.Demo.EntityDto.DBEntity
{
    [DbTable("user_extend")]
    public class WxUserExtend
    {
        [DbKeyColumn]
        public int Id { get; set; }
        [DbColumn]
        public int UserId { get; set; }
        [DbColumn]
        public int UserType { get; set; }
        [DbColumn]
        public string Mobile { get; set; }
        [DbColumn]
        public int CompanyId { get; set; }
        [DbColumn]
        public string Company { get; set; }
        [DbColumn]
        public string Pwd { get; set; }
        [DbColumn]
        public string RealName { get; set; }
        [DbColumn]
        public string IDNo { get; set; }
        [DbColumn]
        public string Face { get; set; }
        [DbColumn]
        public string Back { get; set; }
        [DbColumn]
        public string Hold { get; set; }
        [DbColumn]
        public int Status { get; set; }
        [DbColumn]
        public DateTime CreateTime { get; set; }
        [DbColumn]
        public bool IsDeleted { get; set; }
    }
}
