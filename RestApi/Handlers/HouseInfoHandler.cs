using RestApi.Models;
using RestApi.Data;
using RestApi.Queries;
using System;
using System.Collections.Generic;

namespace RestApi.Handlers
{
    public class HouseInfoHandler
    {
        private readonly OracleDbContext _dbContext;

        // 생성자에서 DbContext 주입
        public HouseInfoHandler(OracleDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<House> GetHouseList(string blNo, string customsCode)
        {
            var parameters = new Dictionary<string, object>
            {
                { "BL_NO", blNo },
                { "CUSTOMS_CODE", customsCode }
            };

            return _dbContext.ExecuteList(HouseQueries.GetHouseList, parameters, reader =>
            {
                return new House
                {
                                    // 실제 DB 컬럼에 맞게 추가 매핑
                };
            });
        }
    }
}
