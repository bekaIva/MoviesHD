//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.IO;
//using System.Linq;
//using System.Net.Http;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;
//using MoviesAll.Droid;
//using MoviesAll.Model;
//using Android;
//using Android.App;
//using Android.Content;
//using Android.OS;
//using Android.Runtime;
//using Android.Views;
//using Android.Widget;
//[assembly: Xamarin.Forms.Dependency(typeof(DownloadFileDependencyService))]
//namespace MoviesAll.Droid
//{
//    public class DownloadFileDependencyService : IDownloader
//    {
//        public DownloadFileDependencyService()
//        {
//        }


//        public DownloadItem PrepareDownload(string url, string fileName, string referer = null)
//        {
//            DownloadItem d = new DownloadItem() { Name = fileName };
//            d.StartDownload = () =>
//            {
//                var token = d.tokenSource.Token;
//                return Task.Run(async () =>
//                {
//                    try
//                    {
//                        d.IsDownloading = true;
//                        if (GrantPermision())
//                        {
//                            var downloadFolder = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads);
//                            var filePath = Path.Combine(downloadFolder.AbsolutePath, fileName);
//                            FileStream fs = new FileStream(filePath, FileMode.CreateNew, FileAccess.Write);
//                            try
//                            {
//                                var test = this;

//                                HttpClient client = new HttpClient();
//                                client.DefaultRequestHeaders.CacheControl = new System.Net.Http.Headers.CacheControlHeaderValue() { NoCache = true };
//                                client.DefaultRequestHeaders.Add("User-Agent", Strings.UserAgent);
//                                if (referer?.Length > 0)
//                                {
//                                    if (client.DefaultRequestHeaders.Contains("Referer")) client.DefaultRequestHeaders.Remove("Referer");
//                                    client.DefaultRequestHeaders.Add("Referer", referer);
//                                }
//                                var stream = await client.GetStreamAsync(url);
//                                d.TotalSize = stream.Length;
//                                byte[] bufer = new byte[4096];
//                                var date = DateTime.Now;
//                                double startTime = GetCurrentTime();
//                                double bytesReaded = 0;
//                                int readed = await stream.ReadAsync(bufer, 0, bufer.Length, token);
//                                d.DownloadStarted?.Invoke("Download started to: " + filePath);
//                                bytesReaded = readed;
//                                while (readed > 0)
//                                {
//                                    while (d.IsPaused)
//                                    {
//                                        await Task.Delay(500);
//                                        token.ThrowIfCancellationRequested();
//                                    }
//                                    double t = (GetCurrentTime());
//                                    double dif = t - startTime;
//                                    if (dif > 1000)
//                                    {
//                                        startTime = t;
//                                        d.BytesInSecond = (bytesReaded / dif) * 1000;
//                                        startTime = GetCurrentTime();
//                                        d.BytesDownloaded += (long)bytesReaded;
//                                        bytesReaded = 0;
//                                    }
//                                    token.ThrowIfCancellationRequested();
//                                    await fs.WriteAsync(bufer, 0, readed);
//                                    bufer = new byte[4096];
//                                    readed = await stream.ReadAsync(bufer, 0, bufer.Length, token);
//                                    bytesReaded += readed;
//                                }
//                                d.BytesDownloaded += (long)bytesReaded;
//                                if (d.TotalSize != d.BytesDownloaded)
//                                {
//                                    throw new Exception("Download interrupted.");
//                                }
//                                fs.Close();
//                            }


//                            catch (Exception e)
//                            {
//                                fs.Close();
//                                throw e;
//                            }
//                        }
//                        else
//                        {
//                            throw new Exception("No access!");
//                        }
//                    }
//                    catch (Exception e)
//                    {
//                        try
//                        {

//                        }
//                        catch { }
//                        throw e;
//                    }
//                    finally
//                    {
//                        d.IsDownloading = false;
//                    }

//                    //string filePath = Path.Combine((Android.OS.Environment.ExternalStorageDirectory).AbsolutePath,Android.OS.Environment.DirectoryDownloads);
//                    //var file = File.Create(filePath);
//                });
//            };

//            return d;

//        }

//        private static long GetCurrentTime()
//        {
//            return (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds;
//        }

//        bool GrantPermision()
//        {
//            if (MainActivity.Activity.CheckSelfPermission(Manifest.Permission.WriteExternalStorage) != Android.Content.PM.Permission.Granted)
//            {
//                var permissions = new string[] { Manifest.Permission.ReadExternalStorage, Manifest.Permission.WriteExternalStorage };
//                MainActivity.Activity.RequestPermissions(permissions, 77);
//                return false;
//            }
//            else return true;
//        }
//    }
//}