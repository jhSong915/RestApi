namespace RestApi.Queries
{
    public static class HouseQueries
    {
        public static string GetHouseList =>
            @"SELECT HOUSE_ID, BL_NO, CUSTOMS_CODE, HOUSE_NAME, CREATED_DATE 
              FROM HOUSES WHERE BL_NO = :BL_NO";
    }
}
