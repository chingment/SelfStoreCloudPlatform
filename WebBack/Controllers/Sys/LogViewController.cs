using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Configuration;
using System.IO;
using System.Web.Mvc;
using WebBack.Models.Sys.LogView;

namespace WebBack.Controllers.Sys
{
    public class LogViewController : OwnBaseController
    {
        protected FileInfo[] Files;
        protected DirectoryInfo[] Dirs;
        protected String Parent = "";

        private static String CURRENT_DOWNLOADER = null;
        private void ListFiles()
        {
            var rootDir = Directory.CreateDirectory(getBaseDir() + Parent);
            this.Dirs = rootDir.GetDirectories();
            this.Files = rootDir.GetFiles();
        }

        private void DownloadFile()
        {
            if (CURRENT_DOWNLOADER != null)
            {
                throw new Exception(string.Format("System at the same time only allow a person to download the log file, the current {0} is downloading the file, please try again later.", getCurrentUserName()));
            }
            CURRENT_DOWNLOADER = getCurrentUserName();
            try
            {
                var file = Request["File"];
                var filePath = getBaseDir() + Parent + "\\" + file;
                var tempZipFilePath = ZipFile(file, filePath);

                Response.ContentType = "application/zip";
                Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", file + ".zip"));
                var fileStream = new FileStream(tempZipFilePath, FileMode.Open, FileAccess.Read);
                fileStream.CopyTo(Response.OutputStream);
                fileStream.Close();
                Response.OutputStream.Close();
                System.IO.File.Delete(tempZipFilePath);
            }
            finally
            {
                CURRENT_DOWNLOADER = null;
            }

            Response.End();

        }

        private string ZipFile(string file, string filePath)
        {
            var tempFilePath = string.Format("{0}\\tmp\\{1}.{2}.{3}", getBaseDir(), getCurrentUserName(), file,
                                             DateTime.Now.ToString("yyyyMMddHHmmss"));
            var tempZipFilePath = tempFilePath + ".zip";
            System.IO.File.Copy(filePath, tempFilePath);

            var fileStream = new FileStream(tempFilePath, FileMode.Open, FileAccess.Read);
            var zipStream = new ZipOutputStream(new FileStream(tempZipFilePath, FileMode.Create, FileAccess.Write));

            try
            {
                var entry = new ZipEntry(file);
                zipStream.PutNextEntry(entry);
                byte[] buffer = new byte[4096];
                StreamUtils.Copy(fileStream, zipStream, buffer);
            }
            finally
            {
                zipStream.Close();
                fileStream.Close();
            }
            System.IO.File.Delete(tempFilePath);
            return tempZipFilePath;
        }

        private String getCurrentUserName()
        {
            return User.Identity.Name;
        }

        private String getBaseDir()
        {
            String baseDir = Server.MapPath(@"/Log-data");


            if (string.IsNullOrWhiteSpace(baseDir))
            {
                throw new Exception("Missing app_LogPath parameter definition in Web.config ");
            }
            return baseDir;
        }

        public ActionResult List()
        {

            Parent = Request["Parent"];
            if (Parent == null)
            {
                Parent = "";
            }
            if (Request["File"] == null)
            {
                ListFiles();
            }
            else
            {
                DownloadFile();
            }
            ListModel model = new ListModel();
            model.Dirs = this.Dirs;
            model.Files = this.Files;
            model.Parent = this.Parent;
            return View(model);
        }


    }
}