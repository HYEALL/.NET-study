using System;

namespace MES.Models
{
    /// <summary>
    /// 생산지시 Model (PRODUCTION_ORDER 매핑)
    /// </summary>
    public class ProductionOrderModel
    {
        public string ProdOrderId       { get; set; }
        public string ItemId            { get; set; }
        public string ItemName          { get; set; }
        public string ItemType          { get; set; }
        public string UnitCode          { get; set; }
        public decimal OrderQty         { get; set; }
        public DateTime PlanStartDate   { get; set; }
        public DateTime PlanEndDate     { get; set; }
        public string Priority          { get; set; }   // HIGH / NORMAL / LOW
        public string PriorityNm        { get; set; }   // 긴급 / 보통 / 낮음
        public string Status            { get; set; }   // WAIT / RUN / DONE / CANCEL / HOLD
        public string StatusNm          { get; set; }   // 대기 / 진행중 / 완료 / 취소 / 보류
        public string CustomerOrderId   { get; set; }
        public string Remark            { get; set; }
        public string CreatedBy         { get; set; }
        public DateTime? CreatedAt      { get; set; }
        public string UpdatedBy         { get; set; }
        public DateTime? UpdatedAt      { get; set; }

        // 집계 필드
        public int  TotalLotCnt         { get; set; }
        public int  DoneLotCnt          { get; set; }
        public decimal GoodQty          { get; set; }
        public decimal NgQty            { get; set; }

        // UI 표시용 계산 속성
        public decimal AchieveRate =>
            OrderQty > 0 ? Math.Round((GoodQty / OrderQty) * 100, 1) : 0;

        public string LotProgress =>
            $"{DoneLotCnt} / {TotalLotCnt}";
    }

    /// <summary>
    /// 검색 조건 Model
    /// </summary>
    public class ProductionOrderSearchModel
    {
        public string   ItemId          { get; set; }
        public string   Status          { get; set; }
        public string   Priority        { get; set; }
        public DateTime? PlanStartFrom  { get; set; }
        public DateTime? PlanStartTo    { get; set; }
    }

    /// <summary>
    /// 품목 콤보 Model
    /// </summary>
    public class ItemComboModel
    {
        public string ItemId        { get; set; }
        public string ItemName      { get; set; }
        public string DisplayName   { get; set; }
        public string UnitCode      { get; set; }
        public string ItemType      { get; set; }
    }
}
