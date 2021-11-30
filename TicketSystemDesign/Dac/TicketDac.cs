using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace TicketSystemDesign
{
    public class TicketDac
    {
        private static string connectString = new ConfigurationBuilder().SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                                                                 .AddJsonFile("appsettings.json")
                                                                 .Build().GetConnectionString("TicketDB");

        /// <summary>
        /// Using Dapper to get user information by username
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public static Models.UserInfo GetUserInfo(string username)
        {
            string strsql = @"  select Top 1 [userid]
                                            ,[username]
                                            ,[status]
                                            ,[pwd]
                                            ,[salt]
                                from [ticketsystem].[dbo].[UserInfo]
                                where [username] = @username
                             ";

            try
            {
                using (var conn = new SqlConnection(connectString))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("username", username, System.Data.DbType.String);

                    var result = conn.QueryFirstOrDefault<Models.UserInfo>(strsql, parameters);
                    return result;
                }
            }
            catch (Exception ex)
            {
                ErrorMsgHelper.WriteLog("Error occur in connect to db for get user information or sql query , the error message on the below: \n\t" + ex.Message.ToString());
                return null;
            }
        }

        /// <summary>
        /// 查詢卡片列表
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<TicketInfoModel> GetTicketList()
        {
            var sql = @" select [Summary]
                                ,[Description]
                                ,[Severity]
                                ,[Priority]
                                , TT.[TicketId]
                                , TT.[TicketTypeId]
                                , TT.[DateTime]
                                , UI.[UserName]
                                , TT.[UserId]
                                ,[Resolved]
	                            , TP.TicketType
                        from[TicketSystem].[dbo].[TicketTable] as TT
                            inner join TicketProp as TP
                                on TT.TicketTypeId = TP.TicketTypeId
                            inner join UserInfo as UI
                                on TT.UserId = UI.UserId
                        order by TT.[TicketId] desc ";

            try
            {
                using (var conn = new SqlConnection(connectString))
                {
                    var result = conn.Query<TicketInfoModel>(sql);
                    return result;
                }
            }
            catch (Exception ex)
            {
                ErrorMsgHelper.WriteLog("Error occur in connect to db for get ticket data or sql query , the error message on the below: \n\t" + ex.Message.ToString());
                return null;
            }
        }
    }
}
