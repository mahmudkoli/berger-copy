using System;
using System.Collections.Generic;
using System.Text;

namespace Berger.Odata.Model
{
    public class CustomerOccasionDataRootModel
    {
        public string kunnr { get; set; }
        public string name_d { get; set; }
        public string name_sm { get; set; }
        public string vtweg { get; set; }
        public string shop_no { get; set; }
        public string market_name { get; set; }
        public string house_num1 { get; set; }
        public string street { get; set; }
        public string vill { get; set; }
        public string post_office { get; set; }
        public string post_code1 { get; set; }
        public string werks { get; set; }
        public string police_s { get; set; }
        public string city2 { get; set; }
        public string regio { get; set; }
        public string tel_number1 { get; set; }
        public string tel_number2 { get; set; }
        public string tel_number3 { get; set; }
        public string tel_number4 { get; set; }
        public string botype { get; set; }
        public string kyconper { get; set; }
        public string tel_number5 { get; set; }
        public string ktokd { get; set; }
        public string succ_name { get; set; }
        public string rel_succ { get; set; }
        public string owbdat { get; set; }
        public string nid { get; set; }
        public string pass_no { get; set; }
        public string bl_group { get; set; }
        public string parge { get; set; }
        public string reg { get; set; }
        public string education { get; set; }
        public string famst { get; set; }
        public string spart { get; set; }
        public string manvdat { get; set; }
        public string spouse_name { get; set; }
        public string dob_spouse { get; set; }
        public string hobby { get; set; }
        public string favorit_tv { get; set; }
        public string child1_name { get; set; }
        public string child1_dob { get; set; }
        public string child1_occu { get; set; }
        public string child1_educ { get; set; }
        public string child2_name { get; set; }
        public string vkbur { get; set; }
        public string child2_dob { get; set; }
        public string child2_occu { get; set; }
        public string child2_educ { get; set; }
        public string child3_name { get; set; }
        public string child3_dob { get; set; }
        public string child3_occu { get; set; }
        public string child3_educ { get; set; }
        public string rpmkr { get; set; }
        public string regiogroup { get; set; }
        public string busty { get; set; }
        public string name1 { get; set; }

        public CustomerOccasionDataModel ToModel()
        {
            var model = new CustomerOccasionDataModel();
            //model.Client = this.MANDT;
            model.Customer = this.kunnr;
            model.Plant = this.werks;
            model.AccountGroup = this.ktokd;
            model.Division = this.spart;
            model.SalesOffice = this.vkbur;
            model.RegionalMarket = this.rpmkr;
            model.StructureGroup = this.regiogroup;
            model.BusinessStartingYY = this.busty;
            model.Name = this.name1;
            model.ShopOwnerName = this.name_d;
            model.ShopManagerName = this.name_sm;
            model.DistrChannel = this.vtweg;
            model.ShopNo = this.shop_no;
            model.MarketName = this.market_name;
            model.HouseNumber = this.house_num1;
            model.Street = this.street;
            model.VillageArea = this.vill;
            model.PostOffice = this.post_office;
            model.PostalCode = this.post_code1;
            model.PoliceStation = this.police_s;
            model.District = this.city2;
            model.Region = this.regio;
            model.MobilePhone1 = this.tel_number1;
            model.MobilePhone2 = this.tel_number2;
            model.LandPhone = this.tel_number3;
            model.ShopManagersMobile = this.tel_number4;
            model.BusinessOwnershipType = this.botype;
            model.KeyContactPersonInfo = this.kyconper;
            model.KeyContactPersonMobile = this.tel_number5;
            model.SuccessorsName = this.succ_name;
            model.RelationshipWithSucc = this.rel_succ;
            model.DOB = this.owbdat;
            model.NationalID = this.nid;
            model.PassportNo = this.pass_no;
            model.BloodGroup = this.bl_group;
            model.Gender = this.parge;
            model.Religion = this.reg;
            model.Education = this.education;
            model.MaritalStatus = this.famst;
            model.manvdat = this.manvdat;
            model.NameOfSpouse = this.spouse_name;
            model.SpouseDOB = this.dob_spouse;
            model.Hobby = this.hobby;
            model.FavoriteTVChannel = this.favorit_tv;
            model.FirstChildName = this.child1_name;
            model.FirstChildDOB = this.child1_dob;
            model.FirstChildOccupation = this.child1_occu;
            model.FirstChildEducation = this.child1_educ;
            model.SecondChildName = this.child2_name;
            model.SecondChildDOB = this.child2_dob;
            model.SecondChildOccupation = this.child2_occu;
            model.SecondChildEducation = this.child2_educ;
            model.ThirdChildName = this.child3_name;
            model.ThirdChildDOB = this.child3_dob;
            model.ThirdChildOccupation = this.child3_occu;
            model.ThirdChildEducation = this.child3_educ;
            return model;
        }
    }
}
