using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RestApi.Models
{
    public class HouseContainerInfo
    {
        public string ContainerNo { get; set; }      // 컨테이너 번호
        public string ContainerType { get; set; }    // 컨테이너 타입 (20GP, 40HQ 등)
        public string HSCode { get; set; }           // HS 코드
        public string SealNo1 { get; set; }          // 봉인 번호 1
        public string SealNo2 { get; set; }          // 봉인 번호 2
        public string SealNo3 { get; set; }          // 봉인 번호 3
        [Column(TypeName = "decimal(18,3)")]
        public decimal ContainerWeight { get; set; } // 컨테이너 중량 ✅ 소수점 3자리까지
        public DangerousGoodsInfo DangerousGoods { get; set; }

    }
}