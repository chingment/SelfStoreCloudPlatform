using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Lumos.Common
{
    public class ImageUpload
    {
        public string OriginalPath
        {
            get;
            set;
        }

        public int OriginalWidth
        {
            get;
            set;
        }

        public int OriginalHeight
        {
            get;
            set;
        }

        public string BigPath
        {
            get;
            set;
        }

        public int BigWidth
        {
            get;
            set;
        }

        public int BigHeight
        {
            get;
            set;
        }

        public string SmallPath
        {
            get;
            set;
        }

        public int SmallWidth
        {
            get;
            set;
        }

        public int SmallHeight
        {
            get;
            set;
        }



        public bool Save(HttpPostedFileBase upload, string domain, string savefolder, string oldsavepath)
        {
            string saveFolder = savefolder;
            string oldSavePath = oldsavepath;
            string imageSign = "M";

            int[] bigImgSize = new int[2] { 500, 500 };
            int[] smallImgSize = new int[2] { 100, 100 };
            try
            {
                #region 检查是否为空
                if (upload == null)
                    return false;


                if (upload.ContentLength == 0)
                    return false;

                if (saveFolder == null)
                    return false;


                if (saveFolder.Trim() == "")
                    return false;

                #endregion


                HttpPostedFileBase file_upload = upload;
                string extension = System.IO.Path.GetExtension(file_upload.FileName).ToLower();

                string yyyyMMddhhmmssfff = DateTime.Now.ToString("yyyyMMddhhmmssfff");
                string originalNewfilename = imageSign + yyyyMMddhhmmssfff + "O" + extension;//原图片文件名称
                string bigNewfilename = imageSign + yyyyMMddhhmmssfff + "B" + extension;//大图片文件名称
                string smallNewfilename = imageSign + yyyyMMddhhmmssfff + "S" + extension;//小图片文件名称

                string originalSavePath = string.Format("{0}/{1}", savefolder, originalNewfilename);
                string bigSavePath = string.Format("{0}/{1}", savefolder, bigNewfilename);
                string smallSavePath = string.Format("{0}/{1}", savefolder, smallNewfilename);

                string serverOriginalSavePath = System.Web.HttpContext.Current.Server.MapPath("~/") + originalSavePath;
                string serverBigSavePath = System.Web.HttpContext.Current.Server.MapPath("~/") + bigSavePath;
                string serverSmallSavePath = System.Web.HttpContext.Current.Server.MapPath("~/") + smallSavePath;

                DirectoryInfo dir = new DirectoryInfo(System.Web.HttpContext.Current.Server.MapPath(savefolder) + "/");
                if (!dir.Exists)
                {
                    dir.Create();
                }

                file_upload.SaveAs(serverOriginalSavePath);

                System.Drawing.Image originalImage = System.Drawing.Image.FromFile(serverOriginalSavePath);
                this.OriginalPath = domain + originalSavePath;
                this.OriginalWidth = originalImage.Width;
                this.OriginalHeight = originalImage.Height;

                if (GreateMiniImageModel(serverOriginalSavePath, serverBigSavePath, bigImgSize[0], bigImgSize[1]))
                {
                    this.BigPath = domain + bigSavePath;
                    this.BigWidth = bigImgSize[0];
                    this.BigHeight = bigImgSize[1];
                }
                if (GreateMiniImageModel(serverOriginalSavePath, serverSmallSavePath, smallImgSize[0], smallImgSize[1]))
                {
                    this.SmallPath = domain + smallSavePath;
                    this.SmallWidth = smallImgSize[0];
                    this.SmallHeight = smallImgSize[1];
                }

                return true;
            }
            catch
            {
                return false;
            }
        }


        public bool Save(HttpPostedFile upload, string domain, string savefolder, string oldsavepath)
        {
            string saveFolder = savefolder;
            string oldSavePath = oldsavepath;
            string imageSign = "M";

            int[] bigImgSize = new int[2] { 500, 500 };
            int[] smallImgSize = new int[2] { 100, 100 };
            try
            {
                #region 检查是否为空
                if (upload == null)
                    return false;


                if (upload.ContentLength == 0)
                    return false;

                if (saveFolder == null)
                    return false;


                if (saveFolder.Trim() == "")
                    return false;

                #endregion


                HttpPostedFile file_upload = upload;
                string extension = System.IO.Path.GetExtension(file_upload.FileName).ToLower();

                string yyyyMMddhhmmssfff = DateTime.Now.ToString("yyyyMMddhhmmssfff");
                string originalNewfilename = imageSign + yyyyMMddhhmmssfff + "O" + extension;//原图片文件名称
                string bigNewfilename = imageSign + yyyyMMddhhmmssfff + "B" + extension;//大图片文件名称
                string smallNewfilename = imageSign + yyyyMMddhhmmssfff + "S" + extension;//小图片文件名称

                string originalSavePath = string.Format("{0}/{1}", savefolder, originalNewfilename);
                string bigSavePath = string.Format("{0}/{1}", savefolder, bigNewfilename);
                string smallSavePath = string.Format("{0}/{1}", savefolder, smallNewfilename);

                string serverOriginalSavePath = System.Web.HttpContext.Current.Server.MapPath("~/") + originalSavePath;
                string serverBigSavePath = System.Web.HttpContext.Current.Server.MapPath("~/") + bigSavePath;
                string serverSmallSavePath = System.Web.HttpContext.Current.Server.MapPath("~/") + smallSavePath;

                DirectoryInfo dir = new DirectoryInfo(System.Web.HttpContext.Current.Server.MapPath(savefolder) + "/");
                if (!dir.Exists)
                {
                    dir.Create();
                }

                file_upload.SaveAs(serverOriginalSavePath);

                System.Drawing.Image originalImage = System.Drawing.Image.FromFile(serverOriginalSavePath);
                this.OriginalPath = domain+originalSavePath;
                this.OriginalWidth = originalImage.Width;
                this.OriginalHeight = originalImage.Height;

                if (GreateMiniImageModel(serverOriginalSavePath, serverBigSavePath, bigImgSize[0], bigImgSize[1]))
                {
                    this.BigPath = domain + bigSavePath;
                    this.BigWidth = bigImgSize[0];
                    this.BigHeight = bigImgSize[1];
                }
                if (GreateMiniImageModel(serverOriginalSavePath, serverSmallSavePath, smallImgSize[0], smallImgSize[1]))
                {
                    this.SmallPath = domain + smallSavePath;
                    this.SmallWidth = smallImgSize[0];
                    this.SmallHeight = smallImgSize[1];
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        private bool GreateMiniImageModel(string originalpath, string savepath, int tWidth, int tHeight)
        {
            try
            {
                System.Drawing.Image SImage = System.Drawing.Image.FromFile(originalpath);
                int imgWidth = SImage.Width;
                int imgHeight = SImage.Height;
                int newWidth = 0;
                int newHeight = 0;
                if (imgWidth > tWidth)
                {
                    newWidth = tWidth;
                    newHeight = tWidth * imgHeight / imgWidth;
                    if (newHeight > tHeight)
                    {
                        newWidth = tHeight * newWidth / newHeight;
                        newHeight = tHeight;
                    }
                }
                else if (imgHeight > tHeight)
                {
                    newHeight = tHeight;
                    newWidth = tHeight * imgWidth / imgHeight;
                    if (newWidth > tWidth)
                    {
                        newHeight = tWidth * newHeight / newWidth;
                        newWidth = tWidth;
                    }
                }
                else
                {
                    newWidth = imgWidth;
                    newHeight = imgHeight;
                }
                Bitmap b = new Bitmap(SImage, newWidth, newHeight);

                b.Save(savepath);
                b.Dispose();
                SImage.Dispose();

                return true;
            }
            catch
            {
                return false;
            }

        }

        private bool DelFile(string fileName)
        {
            bool isflag = false;
            string delPath = System.Web.HttpContext.Current.Server.MapPath("~/");
            string delPicPath = string.Empty;
            int deleteCount = 0;
            try
            {
                delPicPath = string.Format("{0}/{1}", delPath, fileName);
                if (System.IO.File.Exists(delPicPath))//如果该文件存在，则删除
                {
                    deleteCount++;
                    System.IO.File.Delete(delPicPath);
                    isflag = true;
                }
            }
            catch
            {
                if (deleteCount < 100)//为避免因资源被占用删除不了数据   所以在此循环100次
                {
                    DelFile(fileName);
                }
            }
            return isflag;
        }


        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

    }
}
