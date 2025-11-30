using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RestApi.Models
{
    public class HousePartyInfo
    {
        public string PartyType { get; set; }        // 구분 (개인, 법인, 협회, 은행 등)
        public string Name { get; set; }             // 이름 (회사명 또는 개인명)
        public string Address { get; set; }          // 주소
        public string City { get; set; }             // 도시
        public string StateCode { get; set; }        // 주코드
        public string PostalCode { get; set; }       // 우편번호
        public string ContactNumber { get; set; }    // 연락처 (전화번호)
        public string ContactPerson { get; set; }    // 담당자명
        public string Email { get; set; }            // 이메일
        public string AeoNo { get; set; }            // AEO 번호 (Authorized Economic Operator)
        public string PartyTypeNo { get; set; }      // 구분에 따라 생년월일, 사업자등록번호 등
        public string EoriNo { get; set; }           // EORI 번호 (Economic Operator Registration and Identification) EU 세관

    }
}