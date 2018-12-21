using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiYi.Demo.EntityDto.Extend
{
    /// <summary>
    /// 数据库表
    /// </summary>
    public class DbTableAttribute : Attribute
    {
        public string Title { get; }

        public DbTableAttribute() { }

        public DbTableAttribute(string title)
        {
            Title = title;
        }
    }
}
