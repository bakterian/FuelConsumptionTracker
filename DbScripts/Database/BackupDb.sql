USE [CarData] 
GO  

IF DB_ID('CarData') IS NOT NULL
BEGIN
BACKUP DATABASE CarData  
TO DISK = 'D:\5_GITHUB\0_FuelConsumptionTracker\DbFiles\CarData.Bak'  
   WITH FORMAT,  
      MEDIANAME = 'D_SQLServerBackups',  
      NAME = 'Full Backup of CarData';  
END
GO  