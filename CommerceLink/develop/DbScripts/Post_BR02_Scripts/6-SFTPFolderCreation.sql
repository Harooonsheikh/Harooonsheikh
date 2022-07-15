ALTER TABLE Store
ADD IsSFTPDirTreeCreated [bit]; -- 0 means SFTP folder will be created by filesync service onstart automatically. 1 means don't create sftp folder for that store
GO

ALTER TABLE Store 
ADD CONSTRAINT [DF_Store_IsSFTPDirTreeCreated]  DEFAULT ((0)) FOR IsSFTPDirTreeCreated;
GO

-- update IsSFTPDirTreeCreated to 1 for all existing stores
update store set IsSFTPDirTreeCreated = 1;
GO