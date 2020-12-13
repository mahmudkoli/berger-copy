﻿namespace BergerMsfaApi.Models.PainterRegistration
{
    public class PainterCompanyMTDValueModel
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public decimal Value { get; set; }
        public float CountInPercent { get; set; }
        public float CumelativeInPercent { get; set; }

    }
}
