using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiYi.Demo.Api.Models
{
    [Serializable, ProtoContract]
    public class DiYiMsg
    {

        [ProtoMember(1)]
        public string Msgid
        {
            get; set;
        }
        [ProtoMember(2)]
        public string Code
        {
            get; set;
        }
        [ProtoMember(3)]
        public string Method
        {
            get; set;
        }
        [ProtoMember(4)]
        public string CellStatus
        {
            get; set;
        }
       
    }


  
}