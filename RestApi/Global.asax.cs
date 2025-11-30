using RestApi.Data;
using RestApi.Handlers;
using System.Data.Entity;
using System.Web.Http;

namespace RestApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            // ✅ Web API 라우팅 등록
            GlobalConfiguration.Configure(WebApiConfig.Register);

            // ✅ JWT 검증 핸들러 등록
            GlobalConfiguration.Configuration.MessageHandlers.Add(new JwtValidationHandler());

            // ✅ Entity Framework 초기화기 등록
            Database.SetInitializer(new AuthDbContextInitializer(new System.Data.Entity.DbModelBuilder()));

            // ✅ 강제로 DB 초기화 실행 → App_Data/AuthDb.sqlite 생성 및 테이블/인덱스 반영
            using (var db = new AuthDbContext())
            {
                db.Database.Initialize(force: true);
            }
        }
    }
}
