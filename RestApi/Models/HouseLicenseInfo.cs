using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RestApi.Models
{
    public class HouseLicenseInfo
    {
        /// <summary>
        /// 면장번호 (화물관리번호)
        /// 세관에서 관리하는 고유 번호
        /// </summary>
        public string LicenseNo { get; set; }

        /// <summary>
        /// 면장수량
        /// 해당 면장에 포함된 화물의 총 수량
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// 면장중량
        /// 해당 면장에 포함된 화물의 총 중량 (소수점 3자리까지)
        /// </summary>
        [Column(TypeName = "decimal(18,3)")]
        public decimal Weight { get; set; }

        /// <summary>
        /// 분할여부
        /// 화물이 분할되어 신고되는지 여부 (Y/N)
        /// </summary>
        public bool IsSplit { get; set; }

        /// <summary>
        /// 분할개수
        /// 분할된 경우 몇 개로 나뉘었는지 표시
        /// </summary>
        public int SplitCount { get; set; }

        /// <summary>
        /// 동시포장코드
        /// 여러 화물이 동시에 포장된 경우 코드값
        /// </summary>
        public string CoPackingCode { get; set; }

        /// <summary>
        /// 동시포장 개수
        /// 동시포장된 화물의 개수
        /// </summary>
        public int CoPackingCount { get; set; }

    }
}