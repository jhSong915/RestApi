using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RestApi.Models
{
    public class House
    {
        public string MRN { get; set; }              // 적하목록 관리번호
        public string ForwarderCode { get; set; }    // 포워더 세관 부호
        public string MasterBLNo { get; set; }       // Master B/L 번호
        public string HouseBLNo { get; set; }        // House B/L 번호
              // ✅ 송화인 / 수화인 / 통지처 정보 (1:1 관계)
        public HousePartyInfo Shipper { get; set; }
        public HousePartyInfo Consignee { get; set; }
        public HousePartyInfo Notify { get; set; }
        public string VesselName { get; set; }       // 선박명
        public string VoyageNo { get; set; }         // 항차
        public string PortOfLoading { get; set; }    // 선적항
        public string PortOfDischarge { get; set; }  // 양륙항
        public string Mark { get; set; }             // 마크
        public string Description { get; set; }      // 디스크립션
        public string HSCode { get; set; }           // HS 코드
              // ✅ 소수점 3자리까지 표시
        [Column(TypeName = "decimal(18,3)")]
        public float GrossWeight { get; set; }     // 총중량 (KG)
              // ✅ 소수점 3자리까지 표시
        [Column(TypeName = "decimal(18,3)")]
        public decimal Measurement { get; set; }     // 용적 (CBM)
        public int Packages { get; set; }            // 포장 수량
        public string PKG_UNIT { get; set; }         // 포장 단위
        public string BLType { get; set; }           // 비엘 구분 (D, C, E, B)

              // ✅ 면장 정보 (1:N 관계)
        public List<HouseLicenseInfo> License { get; set; }
               // ✅ 컨테이너 정보 (1:N 관계)
        public List<HouseContainerInfo> Containers { get; set; }

    }
}