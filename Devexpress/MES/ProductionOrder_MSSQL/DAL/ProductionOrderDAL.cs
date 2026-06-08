using System;
using System.Data;
using System.Data.SqlClient;         // Oracle.ManagedDataAccess.Client → SqlClient
using System.Configuration;
using MES.Models;

namespace MES.DAL
{
    /// <summary>
    /// DB 연결 기본 클래스 (MSSQL 버전)
    /// Oracle OracleBaseDAL → MssqlBaseDAL 로 교체
    /// </summary>
    public class MssqlBaseDAL
    {
        // App.config connectionStrings["MES_DB"] 읽기
        protected static string ConnectionString =>
            ConfigurationManager
                .ConnectionStrings["MES_DB"].ConnectionString;

        /// <summary>
        /// SqlConnection 열어서 반환.
        /// 호출 측에서 using으로 반드시 닫을 것.
        /// </summary>
        protected SqlConnection GetConnection()
        {
            try
            {
                var conn = new SqlConnection(ConnectionString);
                conn.Open();
                return conn;
            }
            catch (SqlException ex)
            {
                // 주요 MSSQL 오류코드
                // 18456 : 로그인 실패 (계정/비밀번호)
                //  2    : 서버를 찾을 수 없음 (Data Source 오류)
                // 4060  : 데이터베이스 없음 (Initial Catalog 오류)
                throw new Exception(
                    $"MSSQL 연결 실패 (ErrorCode: {ex.Number})\n{ex.Message}");
            }
        }

        /// <summary>
        /// 저장 프로시저 실행용 SqlCommand 생성
        /// </summary>
        protected SqlCommand CreateCommand(SqlConnection conn, string procName)
        {
            return new SqlCommand(procName, conn)
            {
                CommandType = CommandType.StoredProcedure,
                CommandTimeout = int.Parse(
                    ConfigurationManager.AppSettings["DBTimeout"] ?? "30")
            };
        }

        /// <summary>
        /// null / 빈 문자열 → DBNull 변환 헬퍼
        /// </summary>
        protected object ToDbValue(object val)
        {
            if (val == null) return DBNull.Value;
            if (val is string s && string.IsNullOrWhiteSpace(s)) return DBNull.Value;
            return val;
        }
    }

    /// <summary>
    /// 생산지시 DAL (MSSQL 버전)
    /// Oracle RefCursor 방식 → SqlDataAdapter 방식으로 변경
    /// </summary>
    public class ProductionOrderDAL : MssqlBaseDAL
    {
        /// <summary>
        /// 생산오더 목록 조회
        /// Oracle: WIP_WORK_ORDER_G.WORK_ORDER_SELECT (패키지.프로시저)
        /// MSSQL : dbo.USP_WORK_ORDER_SELECT
        /// </summary>
        public DataTable SelectList(decimal sobId, decimal orgId, decimal orderLineId)
        {
            using (var conn = GetConnection())
            using (var cmd = CreateCommand(conn, "dbo.USP_WORK_ORDER_SELECT"))
            {
                // Oracle: OracleDbType.Decimal  → MSSQL: SqlDbType.Decimal
                cmd.Parameters.Add("@p_sob_id", SqlDbType.Decimal).Value = sobId;
                cmd.Parameters.Add("@p_org_id", SqlDbType.Decimal).Value = orgId;
                cmd.Parameters.Add("@p_order_line_id", SqlDbType.Decimal).Value = orderLineId;

                // Oracle은 RefCursor OUT 파라미터가 필요했지만
                // MSSQL은 SELECT 결과셋을 SqlDataAdapter가 직접 받음
                var adapter = new SqlDataAdapter(cmd);
                var dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }

        /// <summary>
        /// 생산오더 등록 (예시 - 프로시저 생성 후 활성화)
        /// </summary>
        public string Insert(ProductionOrderModel model, string userId)
        {
            using (var conn = GetConnection())
            using (var cmd = CreateCommand(conn, "dbo.USP_PRODUCTION_ORDER_INSERT"))
            {
                cmd.Parameters.Add("@p_sob_id", SqlDbType.Decimal).Value = model.SobId;
                cmd.Parameters.Add("@p_org_id", SqlDbType.Decimal).Value = model.OrgId;
                cmd.Parameters.Add("@p_order_line_id", SqlDbType.Decimal).Value = model.OrderLineId;
                cmd.Parameters.Add("@p_bom_item_code", SqlDbType.NVarChar, 50).Value
                    = ToDbValue(model.BomItemCode);
                cmd.Parameters.Add("@p_work_order_qty", SqlDbType.Decimal).Value = model.WorkOrderQty;
                cmd.Parameters.Add("@p_wip_fixed_date", SqlDbType.DateTime).Value
                    = ToDbValue(model.WipOrderFixedDate);
                cmd.Parameters.Add("@p_user_id", SqlDbType.NVarChar, 50).Value = userId;

                // OUT 파라미터: 채번된 WORK_ORDER_ID 반환
                var outParam = new SqlParameter("@p_work_order_id", SqlDbType.Decimal)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(outParam);

                cmd.ExecuteNonQuery();

                return outParam.Value?.ToString();
            }
        }

        /// <summary>
        /// 생산오더 상태 변경 (예시)
        /// </summary>
        public void UpdateStatus(decimal workOrderId, string statusCode,
                                  string userId, string remark = null)
        {
            using (var conn = GetConnection())
            using (var cmd = CreateCommand(conn, "dbo.USP_WORK_ORDER_STATUS_UPDATE"))
            {
                cmd.Parameters.Add("@p_work_order_id", SqlDbType.Decimal).Value = workOrderId;
                cmd.Parameters.Add("@p_status_lcode", SqlDbType.NVarChar, 20).Value = statusCode;
                cmd.Parameters.Add("@p_user_id", SqlDbType.NVarChar, 50).Value = userId;
                cmd.Parameters.Add("@p_remark", SqlDbType.NVarChar, 500).Value = ToDbValue(remark);

                // MSSQL: 프로시저 내부에서 BEGIN TRAN / COMMIT / ROLLBACK 처리
                // C# 코드는 Oracle과 동일하게 ExecuteNonQuery() 호출
                cmd.ExecuteNonQuery();
            }
        }
    }
}
