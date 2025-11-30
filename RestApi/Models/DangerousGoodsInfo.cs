using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RestApi.Models
{
    public class DangerousGoodsInfo
    {
        /// <summary>
        /// IMO 위험물 클래스 (IMDG Code Class)
        /// 예: 1 = 폭발물, 2 = 가스, 3 = 인화성 액체, 4 = 인화성 고체 등
        /// </summary>
        public string IMDGClass { get; set; }

        /// <summary>
        /// UN 번호 (United Nations Number)
        /// 국제적으로 지정된 위험물 고유 번호
        /// 예: 1203 = Gasoline, 1090 = Acetone
        /// </summary>
        public string UNNumber { get; set; }

        /// <summary>
        /// 포장 그룹 (Packing Group)
        /// 위험물의 위험 정도를 구분하는 그룹
        /// I = 고위험, II = 중위험, III = 저위험
        /// </summary>
        public string PackingGroup { get; set; }

        /// <summary>
        /// 플래시 포인트 (Flash Point)
        /// 인화성 액체의 최소 발화 온도
        /// 예: -40°C (휘발유)
        /// </summary>
        public string FlashPoint { get; set; }
    }
}