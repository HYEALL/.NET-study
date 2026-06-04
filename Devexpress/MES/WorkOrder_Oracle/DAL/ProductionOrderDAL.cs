using System;
using System.Collections.Generic;
using System.Data;
//using System.Data.SqlClient;
using MES.Models;
using Oracle.ManagedDataAccess.Client;

namespace MES.DAL
{
    /// <summary>
    /// DB 연결 기본 클래스
    /// </summary>
    public class OracleBaseDAL
    {
        protected static string ConnectionString =>
            System.Configuration.ConfigurationManager
                  .ConnectionStrings["MES_ORA"].ConnectionString;

        protected OracleConnection GetConnection()
        {
            try
            {
                var conn = new OracleConnection(ConnectionString);
                conn.Open();
                return conn;
            }
            catch (OracleException ex)
            {
                // Oracle 전용 예외 처리
                // ORA-12541: TNS 리스너 없음
                // ORA-01017: 계정/비밀번호 오류
                // ORA-12154: TNS 서비스명 오류
                throw new Exception($"Oracle 연결 실패 (ORA-{ex.Number})\n{ex.Message}");
            }
        }

        protected OracleCommand CreateCommand(OracleConnection conn, string procName)
        {
            var cmd = new OracleCommand(procName, conn)
            {
                CommandType = CommandType.StoredProcedure,
                CommandTimeout = 30
            };
            return cmd;
        }

        protected object ToDbValue(object val)
        {
            if (val == null) return DBNull.Value;
            if (val is string s && string.IsNullOrWhiteSpace(s)) return DBNull.Value;
            return val;
        }

    }

    /// <summary>
    /// 생산지시 DAL
    /// </summary>
    public class ProductionOrderOracleDAL : OracleBaseDAL
    {
        public DataTable SelectList(decimal sobId, decimal orgId, decimal orderLineId)
        {
            using (var conn = GetConnection())
            using (var cmd = CreateCommand(conn, "WIP_WORK_ORDER_G.WORK_ORDER_SELECT"))
            {
                var cursor = cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor);
                cursor.Direction = ParameterDirection.Output;

                cmd.Parameters.Add("P_SOB_ID", OracleDbType.Decimal).Value = sobId;
                cmd.Parameters.Add("P_ORG_ID", OracleDbType.Decimal).Value = orgId;
                cmd.Parameters.Add("P_ORDER_LINE_ID", OracleDbType.Decimal).Value = orderLineId;

                var adapter = new OracleDataAdapter(cmd);
                var dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }

        /*
        public string Insert(ProductionOrderModel model, string userId)
        {
            using (var conn = GetConnection())
            using (var cmd = CreateCommand(conn, "USP_PRODUCTION_ORDER_INSERT"))
            {
                cmd.Parameters.Add("p_item_id", OracleDbType.Varchar2, 50).Value
                    = model.ItemId;
                cmd.Parameters.Add("p_order_qty", OracleDbType.Decimal).Value
                    = model.OrderQty;
                cmd.Parameters.Add("p_plan_start_date", OracleDbType.Date).Value
                    = model.PlanStartDate;
                cmd.Parameters.Add("p_user_id", OracleDbType.Varchar2, 50).Value
                    = userId;

                // OUT 파라미터 (채번된 ID 반환)
                var outParam = cmd.Parameters.Add("p_prod_order_id", OracleDbType.Varchar2, 50);
                outParam.Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();

                return outParam.Value?.ToString();
            }
        }
        public void UpdateStatus(string prodOrderId, string status, string userId, string remark = null)
        {
            using (var conn = GetConnection())
            using (var cmd = CreateCommand(conn, "USP_PRODUCTION_ORDER_STATUS_UPDATE"))
            {
                cmd.Parameters.Add("p_prod_order_id", OracleDbType.Varchar2, 50).Value = prodOrderId;
                cmd.Parameters.Add("p_status", OracleDbType.Varchar2, 20).Value = status;
                cmd.Parameters.Add("p_user_id", OracleDbType.Varchar2, 50).Value = userId;
                cmd.Parameters.Add("p_remark", OracleDbType.Varchar2, 500).Value = ToDbValue(remark);

                // [차이10] 트랜잭션 처리
                // MSSQL:  프로시저 안에서 BEGIN TRANSACTION / COMMIT
                // Oracle: 프로시저 안에서 COMMIT / ROLLBACK
                //         C# 코드에서는 동일하게 ExecuteNonQuery() 호출
                cmd.ExecuteNonQuery();
            }
        }
        */
    }
}