# 20220816CheckWeightFramasVn
Phần mềm kiểm tra khối lượng thùng hàng có đạt yêu cầu so với các thông số cấu hình hay không.


CÁCH NHẬN BIẾT THÙNG HÀNG PASS/FAIL
- Trong bảng tblScanData chú ý các cột: Pass,Satus,Deviation,ActualDeviation,ApprovedBy
- Pass = 1 --> PASS, không phải lăn tăn gì
- Pass = 0: có 2 trường hợp
  + Fail thật: Pass=0;Status=0;Deviation!=0;ApprovedBy=Empty
  + Fail giả: Pass=0;Status=0;Deviation!=0;ApprovedBy!=Empty
- Lưu ý:
  + Khi sử dụng Approved QR code , thì cột GrossWeight trong bảng tblScanData là giá trị cân của thùng hàng được đặt lên cân
  + Còn giá trị GrossWeight trong bảng tblApprovedPrintLabel là giá trị được lấy từ lần log trước đó bên bảng tblScanData mang qua để.
