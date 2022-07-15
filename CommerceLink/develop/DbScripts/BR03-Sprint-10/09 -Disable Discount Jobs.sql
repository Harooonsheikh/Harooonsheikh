--Verify Discount Job IDs
Select * FROM Job WHERE JobId IN (19, 119)

--Disable Discount Jobs as discussed with Baidar
Update jobSchedule SET IsActive = 0 WHERE JobId IN (19, 119)