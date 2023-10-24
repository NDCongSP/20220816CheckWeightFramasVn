using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeightChecking
{
    public class AutoPostingHelper
    {
        /// <summary>
        /// Check truoc khi thực hiện.
        /// </summary>
        /// <param name="productNumber">Mã nguyên liệu.</param>
        /// <param name="mode">Mode In: 'ADD', Out: 'REMOVE', Transfer: 'TRANSFER'.</param>
        /// <param name="barcodeString"></param>
        /// <param name="fromWH">Từ kho nào đến.</param>
        /// <param name="toWH">Đến kho nào.</param>
        /// <param name="connection">chuôi connect.</param>
        /// <returns></returns>
        public static ResultPostingModel AutoTransfer(string productNumber, string barcodeString, int fromWH, int toWH, IDbConnection connection, IDbTransaction tran)
        {
            string machineName = System.Environment.MachineName;
            ResultPostingModel resultPostingModel = new ResultPostingModel();

            // check nếu QRCode hiện tại có nằm trong kho
            DynamicParameters para = new DynamicParameters();
            para.Add("@qr", barcodeString);
            para.Add("@userId", $"idc_autoposting");
            para.Add("@mode", "TRANSFER");
            // hàng đến từ kho (FFT)
            para.Add("@whFrom", fromWH);
            // sẽ vào kho (FFT)
            para.Add("@whTo", toWH);
            para.Add("@lock", 0);
            para.Add("@inputQuantity", null);

            (int Accept, string Message) = connection.Query<(int Accept, string Message)>("DOGE_WH.dbo.sp_lmpScannerClient_ScanningLabel_CheckLabel"
                , para, commandType: CommandType.StoredProcedure, transaction: tran).FirstOrDefault();

            resultPostingModel.Accept = Accept;
            resultPostingModel.Message = Message;

            // thùng hàng có trong kho -> có thể transfer
            if (Accept > 0)
            {
                para = new DynamicParameters();
                para.Add("@qr", barcodeString);
                para.Add("@userId", $"idc_autoposting");
                para.Add("@mode", "TRANSFER");
                // hàng đến từ kho (FFT)
                para.Add("@whFrom", fromWH);
                // sẽ vào kho (FFT)
                para.Add("@whTo", toWH);
                para.Add("@deviceId", machineName);
                para.Add("@scanTime", DateTime.Now);
                para.Add("@ipAdd", "");
                para.Add("@postingText", "");
                para.Add("@inputQuantity", null);
                para.Add("@id", null);

                var resInsertTransferRackStorage = connection.Execute("DOGE_WH.dbo.sp_lmpScannerClient_ScannedLabel_Insert"
                    , para, commandType: CommandType.StoredProcedure, transaction: tran);

                if (resInsertTransferRackStorage > 0)
                {
                    Debug.WriteLine($"ProductNumber: {productNumber} đã cập nhật kho.");
                    resultPostingModel.Message = $"Successful";
                }
                else
                {
                    Debug.WriteLine($"ProductNumber: {productNumber} cập nhật kho thất bại.");
                    resultPostingModel.Message = $"Fail";
                }

            }
            else
            {
                Debug.WriteLine($"Thông tin không hợp lệ: {Message}");
            }

            return resultPostingModel;
        }

        public static ResultPostingModel AutoStockIn(string productNumber, string barcodeString, int toWH, IDbConnection connection, IDbTransaction tran)
        {
            string machineName = System.Environment.MachineName;
            ResultPostingModel resultPostingModel = new ResultPostingModel();

            // check nếu QRCode hiện tại có nằm trong kho
            DynamicParameters para = new DynamicParameters();
            para.Add("@qr", barcodeString);
            para.Add("@userId", $"idc_autoposting"); //user sử dụng cho việc auto posting
            para.Add("@mode", "ADD");
            // hàng đến từ kho (FFT)
            para.Add("@whFrom", "");
            // sẽ vào kho (FFT)
            para.Add("@whTo", toWH);
            para.Add("@lock", 0);
            para.Add("@inputQuantity", null);

            (int Accept, string Message) = connection.Query<(int Accept, string Message)>("DOGE_WH.dbo.sp_lmpScannerClient_ScanningLabel_CheckLabel"
                , para, commandType: CommandType.StoredProcedure, transaction: tran).FirstOrDefault();

            resultPostingModel.Accept = Accept;
            resultPostingModel.Message = Message;

            // thùng hàng có trong kho -> có thể transfer
            if (Accept > 0)
            {
                para = new DynamicParameters();
                para.Add("@qr", barcodeString);
                para.Add("@userId", $"idc_autoposting");
                para.Add("@mode", "ADD");
                // hàng đến từ kho (FFT)
                para.Add("@whFrom", "");
                // sẽ vào kho (FFT)
                para.Add("@whTo", toWH);
                para.Add("@deviceId", machineName);
                para.Add("@scanTime", DateTime.Now);
                para.Add("@ipAdd", "");
                para.Add("@postingText", "");
                para.Add("@inputQuantity", null);
                para.Add("@id", null);

                var resInsertTransferRackStorage = connection.Execute("DOGE_WH.dbo.sp_lmpScannerClient_ScannedLabel_Insert", para, commandType: CommandType.StoredProcedure, transaction: tran);
                if (resInsertTransferRackStorage > 0)
                {
                    Debug.WriteLine($"ProductNumber: {productNumber} đã nhập kho.");
                    resultPostingModel.Message = $"Successful.";
                }
                else
                {
                    Debug.WriteLine($"ProductNumber: {productNumber} nhập kho thất bại.");
                    resultPostingModel.Message = $"Fail.";
                }

            }
            else
            {
                Debug.WriteLine($"Thông tin không hợp lệ: {Message}");

            }
            return resultPostingModel;
        }

        public static ResultPostingModel AutoStockOut(string productNumber, string barcodeString, int fromWH, IDbConnection connection, IDbTransaction tran)
        {
            string machineName = System.Environment.MachineName;
            ResultPostingModel resultPostingModel = new ResultPostingModel();

            // check nếu QRCode hiện tại có nằm trong kho
            DynamicParameters para = new DynamicParameters();
            para.Add("@qr", barcodeString);
            para.Add("@userId", $"idc_autoposting");
            para.Add("@mode", "REMOVE");
            // hàng đến từ kho (FFT)
            para.Add("@whFrom", fromWH);
            // sẽ vào kho (FFT)
            para.Add("@whTo", "");
            para.Add("@lock", 0);
            para.Add("@inputQuantity", null);

            (int Accept, string Message) = connection.Query<(int Accept, string Message)>("DOGE_WH.dbo.sp_lmpScannerClient_ScanningLabel_CheckLabel"
                , para, commandType: CommandType.StoredProcedure, transaction: tran).FirstOrDefault();

            resultPostingModel.Accept = Accept;
            resultPostingModel.Message = Message;

            // thùng hàng có trong kho -> có thể transfer
            if (Accept > 0)
            {
                para = new DynamicParameters();
                para.Add("@qr", barcodeString);
                para.Add("@userId", $"idc_autoposting");
                para.Add("@mode", "REMOVE");
                // hàng đến từ kho (FFT)
                para.Add("@whFrom", fromWH);
                // sẽ vào kho (FFT)
                para.Add("@whTo", "");
                para.Add("@deviceId", machineName);
                para.Add("@scanTime", DateTime.Now);
                para.Add("@ipAdd", "");
                para.Add("@postingText", "");
                para.Add("@inputQuantity", null);
                para.Add("@id", null);

                var resInsertTransferRackStorage = connection.Execute("DOGE_WH.dbo.sp_lmpScannerClient_ScannedLabel_Insert"
                    , para, commandType: CommandType.StoredProcedure, transaction: tran);
                if (resInsertTransferRackStorage > 0)
                {
                    Debug.WriteLine($"ProductNumber: {productNumber} đã cập nhật kho.");
                    resultPostingModel.Message = $"Successful.";
                }
                else
                {
                    Debug.WriteLine($"ProductNumber: {productNumber} cập nhật kho thất bại.");
                    resultPostingModel.Message = $"Fail.";
                }
            }
            else
            {
                Debug.WriteLine($"Thông tin không hợp lệ: {Message}");
            }

            return resultPostingModel;
        }
    }
}
