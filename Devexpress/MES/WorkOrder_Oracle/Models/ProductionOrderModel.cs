// Models/WorkOrderModel.cs
using System;

namespace MES.Models
{
    public class ProductionOrderSearchModel
    {
        public string WorkOrderNo { get; set; }  // 작업오더번호
        public DateTime? WipOrderFixedDate { get; set; }  // 생산오더생성일
        public string WorkOrderStatusDesc { get; set; }  // 상태명
        public string WorkOrderStatusLcode { get; set; }  // 상태코드
        public string OrderNo { get; set; }  // 오더번호
        public string OrderLineNo { get; set; }  // 오더라인번호
        public string BomVersion { get; set; }  // BOM버전
        public string BomItemCode { get; set; }  // BOM품목코드
        public string BomItemDescription { get; set; }  // BOM품목명
        public string BomDesignNo { get; set; }  // 도면번호
        public string WorkOrderTypeLcode { get; set; }  // 오더유형코드
        public decimal WorkOrderId { get; set; }  // 작업오더ID
        public decimal PcsPerPnlQty { get; set; }  // 합수
        public string WorkOrderTypeDesc { get; set; }  // 오더유형명
        public decimal WorkOrderQty { get; set; }  // 생산지시량
        public decimal InputShortageQty { get; set; }  // 부족적용량
        public decimal ApplyNoverFgOnhandQty { get; set; }  // 재고적용량
        public decimal SalesOrderNetQty { get; set; }  // 순수주량
        public decimal WorkOrderNetQty { get; set; }  // 순투입량
        public decimal LossJobInputQty { get; set; }  // 로스량
        public decimal LossJobInputRate { get; set; }  // 불량율
        public decimal StdInputUomQty { get; set; }  // 실투입량PCS
        public decimal StdInputPnlQty { get; set; }  // 실투입량PNL
        public decimal PnlPerJobQty { get; set; }  // PNL/Lot
        public decimal IncludedInDeviations { get; set; }  // 산입편차
        public decimal BomItemId { get; set; }  // BOM품목ID
        public string WeekNum { get; set; }  // 주차
        public string PnlSize { get; set; }  // PNL사이즈
        public string Remark { get; set; }  // 비고
        public decimal LotQty { get; set; }  // LOT수량
        public string ProvideDivisionLcode { get; set; }  // 공급구분코드
        public string ProvideDivisionDesc { get; set; }  // 공급구분명
        public decimal PnlPerSheetQty { get; set; }  // PNL/SHEET
        public decimal TotalSheetQty { get; set; }  // 총SHEET수량
        public string ManagedComment { get; set; }  // 관리코멘트
    }

    // 검색조건 Model
    public class WorkOrderSearchModel
    {
        public decimal SobId { get; set; }  // 회사 ID
        public decimal OrgId { get; set; }  // 조직 ID
        public decimal OrderLineId { get; set; }  // 오더라인 ID
    }
}