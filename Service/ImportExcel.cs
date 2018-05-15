using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using LinqToExcel;
using Trace3.Models;

namespace Trace3.Service
{
    public class ImportDataHelper
    {
        /// <summary>
        /// 檢查匯入的 Excel 資料.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="importOutbound">The import zip codes.</param>
        /// <returns></returns>
        public CheckResult CheckImportData(
            string fileName,
            List<OutboundData> importOutbound)
        {
            var result = new CheckResult();

            var targetFile = new FileInfo(fileName);

            if (!targetFile.Exists)
            {
                result.ID = Guid.NewGuid();
                result.Success = false;
                result.ErrorCount = 0;
                result.ErrorMessage = "匯入的資料檔案不存在";
                return result;
            }

            var excelFile = new ExcelQueryFactory(fileName);

            //欄位對映
            excelFile.AddMapping<OutboundData>(x => x.OrderID, "OrderID");
            excelFile.AddMapping<OutboundData>(x => x.Lisence, "Lisence");
            excelFile.AddMapping<OutboundData>(x => x.Declaration, "Declaration");
            excelFile.AddMapping<OutboundData>(x => x.Amount, "Amount");
            

            //SheetName
            var excelContent = excelFile.Worksheet<OutboundData>("臺灣郵遞區號");

            int errorCount = 0;
            int rowIndex = 1;
            var importErrorMessages = new List<string>();

            //檢查資料
            foreach (var row in excelContent)
            {
                var errorMessage = new StringBuilder();
                var ODATA = new OutboundData();

                ODATA.OrderID = row.OrderID;
                ODATA.Lisence = row.Lisence;
                ODATA.Declaration = row.Declaration;
                //ODATA.CreateDate = DateTime.Now;

                
                //=============================================================================
                if (errorMessage.Length > 0)
                {
                    errorCount += 1;
                    importErrorMessages.Add(string.Format(
                        "第 {0} 列資料發現錯誤：{1}{2}",
                        rowIndex,
                        errorMessage,
                        "<br/>"));
                }
                importOutbound.Add(ODATA);
                rowIndex += 1;
            }

            try
            {
                result.ID = Guid.NewGuid();
                result.Success = errorCount.Equals(0);
                result.RowCount = importOutbound.Count;
                result.ErrorCount = errorCount;

                string allErrorMessage = string.Empty;

                foreach (var message in importErrorMessages)
                {
                    allErrorMessage += message;
                }

                result.ErrorMessage = allErrorMessage;

                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        /// <summary>
        /// Saves the import data.
        /// </summary>
        /// <param name="importZipCodes">The import zip codes.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void SaveImportData(IEnumerable<OutboundData> importZipCodes)
        {
            try
            {
                //先砍掉全部資料
                using (var db = new TestDataEntities())
                {
                    foreach (var item in db.OutboundData.OrderBy(x => x.ID))
                    {
                        db.OutboundData.Remove(item);
                    }
                    db.SaveChanges();
                }

                //再把匯入的資料給存到資料庫
                using (var db = new TestDataEntities())
                {
                    foreach (var item in importZipCodes)
                    {
                        db.OutboundData.Add(item);
                    }
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}