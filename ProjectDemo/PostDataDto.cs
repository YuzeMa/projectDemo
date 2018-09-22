using System;
using Newtonsoft.Json;

namespace ProjectDemo
{
    public class PostDataDto
    {
      
        public string to { get; set; }
        public string body { get; set; }
        //        {
        //   "to":"0416838863",
        //  "validity":"60",
        //  "priority":false,
        //  "notifyURL":"http://YOUR_URL",  
        //  "scheduledDelivery": 5,
        //  "body":"MIssing u so much"
        //}
    }
}
