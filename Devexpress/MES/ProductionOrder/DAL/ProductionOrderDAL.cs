using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using MES.Models;

namespace MES.DAL
{
    /// <summary>
    /// DB 연결 기본 클래스
    /// </summary>
    public class BaseDAL
    {
        // App.config 또는 공통 설정에서 읽어오는 구조 권장
        protected static string ConnectionString =>
            System.Configuration.ConfigurationManager
                  .ConnectionStrings["MES_DB"].ConnectionString;

        protected SqlConnection GetConnection()
        {
            var conn = new SqlConnection(ConnectionString);
            conn.Open();
            return conn;
        }

        protected SqlCommand CreateCommand(SqlConnection conn, string procName)
        {
            var cmd = new SqlCommand(procName, conn)
            {
                CommandType    = CommandType.StoredProcedure,
                CommandTimeout = 30
            };
            return cmd;
        }
    }

    /// <summary>
    /// 생산지시 DAL
    /// </summary>
    public class ProductionOrderDAL : BaseDAL
    {
        // ─────────────────────────────────────────
        // 1. 목록 조회
        // ─────────────────────────────────────────
        public DataTable SelectList(ProductionOrderSearchModel search)
        {
            using (var conn = GetConnection())
            using (var cmd  = CreateCommand(conn, "USP_PRODUCTION_ORDER_SELECT"))
            {
                cmd.Parameters.Add("@p_item_id",         SqlDbType.NVarChar, 50).Value
                    = ToDbValue(search.ItemId);
                cmd.Parameters.Add("@p_status",          SqlDbType.NVarChar, 20).Value
                    = ToDbValue(search.Status);
                cmd.Parameters.Add("@p_priority",        SqlDbType.NVarChar, 10).Value
                    = ToDbValue(search.Priority);
                cmd.Parameters.Add("@p_plan_start_from", SqlDbType.Date).Value
                    = ToDbValue(search.PlanStartFrom);
                cmd.Parameters.Add("@p_plan_start_to",   SqlDbType.Date).Value
                    = ToDbValue(search.PlanStartTo);

                var adapter = new SqlDataAdapter(cmd);
                var dt      = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }

        // ─────────────────────────────────────────
        // 2. 단건 조회
        // ─────────────────────────────────────────
        public ProductionOrderModel SelectOne(string prodOrderId)
        {
            using (var conn = GetConnection())
            using (var cmd  = CreateCommand(conn, "USP_PRODUCTION_ORDER_SELECT_ONE"))
            {
                cmd.Parameters.Add("@p_prod_order_id", SqlDbType.NVarChar, 50).Value = prodOrderId;

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                        return MapReader(reader);
                    return null;
                }
            }
        }

        // ─────────────────────────────────────────
        // 3. 등록
        // ─────────────────────────────────────────
        public string Insert(ProductionOrderModel model, string userId)
        {
            using (var conn = GetConnection())
            using (var cmd  = CreateCommand(conn, "USP_PRODUCTION_ORDER_INSERT"))
            {
                cmd.Parameters.Add("@p_item_id",          SqlDbType.NVarChar, 50).Value  = model.ItemId;
                cmd.Parameters.Add("@p_order_qty",        SqlDbType.Decimal).Value        = model.OrderQty;
                cmd.Parameters.Add("@p_plan_start_date",  SqlDbType.Date).Value           = model.PlanStartDate;
                cmd.Parameters.Add("@p_plan_end_date",    SqlDbType.Date).Value           = model.PlanEndDate;
                cmd.Parameters.Add("@p_priority",         SqlDbType.NVarChar, 10).Value  = model.Priority;
                cmd.Parameters.Add("@p_customer_order_id",SqlDbType.NVarChar, 50).Value  = ToDbValue(model.CustomerOrderId);
                cmd.Parameters.Add("@p_remark",           SqlDbType.NVarChar, 500).Value = ToDbValue(model.Remark);
                cmd.Parameters.Add("@p_user_id",          SqlDbType.NVarChar, 50).Value  = userId;

                // OUTPUT 파라미터
                var outParam = cmd.Parameters.Add("@p_prod_order_id", SqlDbType.NVarChar, 50);
                outParam.Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();

                return outParam.Value?.ToString();
            }
        }

        // ─────────────────────────────────────────
        // 4. 수정
        // ─────────────────────────────────────────
        public void Update(ProductionOrderModel model, string userId)
        {
            using (var conn = GetConnection())
            using (var cmd  = CreateCommand(conn, "USP_PRODUCTION_ORDER_UPDATE"))
            {
                cmd.Parameters.Add("@p_prod_order_id",    SqlDbType.NVarChar, 50).Value  = model.ProdOrderId;
                cmd.Parameters.Add("@p_item_id",          SqlDbType.NVarChar, 50).Value  = model.ItemId;
                cmd.Parameters.Add("@p_order_qty",        SqlDbType.Decimal).Value        = model.OrderQty;
                cmd.Parameters.Add("@p_plan_start_date",  SqlDbType.Date).Value           = model.PlanStartDate;
                cmd.Parameters.Add("@p_plan_end_date",    SqlDbType.Date).Value           = model.PlanEndDate;
                cmd.Parameters.Add("@p_priority",         SqlDbType.NVarChar, 10).Value  = model.Priority;
                cmd.Parameters.Add("@p_customer_order_id",SqlDbType.NVarChar, 50).Value  = ToDbValue(model.CustomerOrderId);
                cmd.Parameters.Add("@p_remark",           SqlDbType.NVarChar, 500).Value = ToDbValue(model.Remark);
                cmd.Parameters.Add("@p_user_id",          SqlDbType.NVarChar, 50).Value  = userId;

                cmd.ExecuteNonQuery();
            }
        }

        // ─────────────────────────────────────────
        // 5. 상태 변경
        // ─────────────────────────────────────────
        public void UpdateStatus(string prodOrderId, string status, string userId, string remark = null)
        {
            using (var conn = GetConnection())
            using (var cmd  = CreateCommand(conn, "USP_PRODUCTION_ORDER_STATUS_UPDATE"))
            {
                cmd.Parameters.Add("@p_prod_order_id", SqlDbType.NVarChar, 50).Value  = prodOrderId;
                cmd.Parameters.Add("@p_status",        SqlDbType.NVarChar, 20).Value  = status;
                cmd.Parameters.Add("@p_user_id",       SqlDbType.NVarChar, 50).Value  = userId;
                cmd.Parameters.Add("@p_remark",        SqlDbType.NVarChar, 500).Value = ToDbValue(remark);

                cmd.ExecuteNonQuery();
            }
        }

        // ─────────────────────────────────────────
        // 6. 품목 콤보 로드
        // ─────────────────────────────────────────
        public List<ItemComboModel> SelectItemCombo(string itemType = null)
        {
            var list = new List<ItemComboModel>();

            using (var conn = GetConnection())
            using (var cmd  = CreateCommand(conn, "USP_ITEM_COMBO_SELECT"))
            {
                cmd.Parameters.Add("@p_item_type", SqlDbType.NVarChar, 20).Value
                    = ToDbValue(itemType);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new ItemComboModel
                        {
                            ItemId      = reader["ITEM_ID"].ToString(),
                            ItemName    = reader["ITEM_NAME"].ToString(),
                            DisplayName = reader["DISPLAY_NAME"].ToString(),
                            UnitCode    = reader["UNIT_CODE"].ToString(),
                            ItemType    = reader["ITEM_TYPE"].ToString()
                        });
                    }
                }
            }
            return list;
        }

        // ─────────────────────────────────────────
        // 내부 헬퍼
        // ─────────────────────────────────────────
        private ProductionOrderModel MapReader(SqlDataReader r)
        {
            return new ProductionOrderModel
            {
                ProdOrderId      = r["PROD_ORDER_ID"].ToString(),
                ItemId           = r["ITEM_ID"].ToString(),
                ItemName         = r["ITEM_NAME"].ToString(),
                ItemType         = r["ITEM_TYPE"].ToString(),
                UnitCode         = r["UNIT_CODE"].ToString(),
                OrderQty         = Convert.ToDecimal(r["ORDER_QTY"]),
                PlanStartDate    = Convert.ToDateTime(r["PLAN_START_DATE"]),
                PlanEndDate      = Convert.ToDateTime(r["PLAN_END_DATE"]),
                Priority         = r["PRIORITY"].ToString(),
                Status           = r["STATUS"].ToString(),
                CustomerOrderId  = r["CUSTOMER_ORDER_ID"].ToString(),
                Remark           = r["REMARK"].ToString(),
                CreatedBy        = r["CREATED_BY"].ToString(),
                CreatedAt        = r["CREATED_AT"] as DateTime?,
                UpdatedBy        = r["UPDATED_BY"].ToString(),
                UpdatedAt        = r["UPDATED_AT"] as DateTime?
            };
        }

        /// <summary>NULL 또는 빈 문자열을 DBNull로 변환</summary>
        private object ToDbValue(object val)
        {
            if (val == null)                         return DBNull.Value;
            if (val is string s && string.IsNullOrWhiteSpace(s)) return DBNull.Value;
            return val;
        }
    }
}
