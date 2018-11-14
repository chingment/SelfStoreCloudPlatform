using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;


namespace Lumos.Entity
{
    [ComplexType]
    public class ImgSet
    {
        public string ImgUrl { get; set; }
        public bool IsMain { get; set; }
        public string Name { get; set; }
        public int Priority { set; get; }
        public static string GetMain(string jsonStr)
        {
            string imgUrl = "";
            try
            {
                List<ImgSet> d = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ImgSet>>(jsonStr);

                if (d != null)
                {
                    var d1 = d.Where(m => m.IsMain == true).FirstOrDefault();
                    if (d1 != null)
                    {
                        imgUrl = d1.ImgUrl;
                    }
                }

            }
            catch (Exception ex)
            {
                LogUtil.Error("解释ImgSet Json 错误", ex);
            }

            return imgUrl;
        }

        public static string GetMain(List<ImgSet> imgs)
        {
            string imgUrl = "";
            try
            {

                var d1 = imgs.Where(m => m.IsMain == true).FirstOrDefault();
                if (d1 != null)
                {
                    imgUrl = d1.ImgUrl;
                }


            }
            catch (Exception ex)
            {
                LogUtil.Error("解释ImgSet Json 错误", ex);
            }

            return imgUrl;
        }
    }
}
