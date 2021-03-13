using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FintechCalculator.DataModels
{
    public class Prs
    {
        public int product_id { get; set; }
        public int user_id { get; set; }
        public int parent_id { get; set; }
        public int makler_id { get; set; }
        public bool has_logo { get; set; }
        public string makler_name { get; set; }
        public int loc_id { get; set; }
        public string street_address { get; set; }
        public double yard_size { get; set; }
        public int yard_size_type_id { get; set; }
        public int submission_id { get; set; }
        public int adtype_id { get; set; }
        public int product_type_id { get; set; }
        public decimal price { get; set; }
        public string photo { get; set; }
        public int photo_ver { get; set; }
        public int photos_count { get; set; }
        public decimal area_size_value { get; set; }
        public int currency_id { get; set; }
        public DateTime order_date { get; set; }
        public int price_type_id { get; set; }
        public int vip { get; set; }
        public int color { get; set; }
        public int estate_type_id { get; set; }
        public decimal area_size { get; set; }
        public int area_size_type_id { get; set; }
        public string comment { get; set; }
        public double map_lat { get; set; }
        public double map_lon { get; set; }
        public string l_living { get; set; }
        public int special_persons { get; set; }
        public double rooms { get; set; }
        public int bedrooms { get; set; }
        public int floor { get; set; }
        public int parking_id { get; set; }
        public bool canalization { get; set; }
        public bool water { get; set; }
        public bool road { get; set; }
        public bool electricity { get; set; }
        public int owner_type_id { get; set; }
        public int osm_id { get; set; }
        public JObject name_json { get; set; }
        public JObject pathway_json { get; set; }
        public int homeselfie { get; set; }
        public JObject seo_title_json { get; set; }
        public JObject seo_name_json { get; set; }
    }
}
