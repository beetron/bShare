# bShare

bShare is a personal project for uploading and sharing files on the fly.<br>
Instructions on how to clone and replicate a working bShare web app below.<br>
Automatic mysql data deletion for expired uploads, and stored file deletion via python script.<br>

<hr>

[Language] <br>
C#, Javascript, HTML, CSS/Boostrap 5 <br><br>

[Framework] <br> 
Asp.NET Core 7.0 MVC <br><br>

[Database] <br>
MySQL (ORM found below)

<hr>

[Features]<br>
- Anonymous file uploading with file size limit per upload <br>
- Physical file storage to server <br>
- Random short link generator <br>
- File expiration <br>
- Qr Image generator for short link <br>
- Download individual files or all as zip <br>
- Set password to delete upload record prior to expiry <br>

<br>
<hr>

[Update plans]<br>
<li>Adding files in addition to selected files <br>

<hr>

[Replicate working project]<br>

Steps in replicating a working bShare web app, <b>including</b> server side automation.<br>
Some information may not apply to your environment, as each server solution is different.
These steps are necessary if you want to run the app out of the box.<br>

1.) <b>Environment Variables</b><br>
```
bShare_AppUrl = "https://your-apps-base-url.com/"

bshare_DevConnectionString = "Your dev database connection string"

bshare_ProdConnectionString = "Your prod database connection string"

bshare_UploadLocation = "C:/your/file/storage/location/"

bshare_PyPath = "c:/your/script/location"
```

<br>

2.) <b>MySQL event scheduler</b><br><br>
Setup the event scheduler to query the database for any expired file uploads.
If DateExpired value is older than the datetime when scheduler is ran, 
the record(row(s) will be deleted.
<br>

2-1.) First enable the event scheduler.<br>
```
SET GLOBAL event_scheduler = on;
```

2-2.) Make sure event scheduler is enabled.<br>
```
SHOW VARIABLES LIKE 'event_scheulder';
```

2-3.) Create event.<br>
```
DELIMITER //
CREATE EVENT DeleteExpiredFiles
ON SCHEDULE EVERY 5 MINUTE
DO
BEGIN
DELETE FROM bshare.filedetails
WHERE DateExpire < NOW();
END;
//
DELIMITER ;
```

2-4) Check created event.<br>
```
SHOW EVENTS;
```
<br>

3.) <b>File deletion script (Python)</b><br><br>
I wrote a simple Python script for the file deletion.<br><br>
The script checks and compares current time and created time of the folders and removes anything older than X hours 
from creation.<br>
There are much more better ways to have files removed in a timely manner.
I may add this in the future.<br>

<a href="https://github.com/beetron/Python/blob/main/Delete-Expired-Folders/Delete-Expired-Folders.py" target="_BLANK">
Delete-Expired-Folders.py</a>

4.) <b>Add Python script to the Windows Task Schdeuler.</b><br><br>
Details on how on the link below.<br>
(In courtesy of JEAN-CHRISTOPHE CHOUINARD)<br>
<a href="https://www.jcchouinard.com/python-automation-using-task-scheduler/" target="_BLANK">
How to add and run scripts to Windows Task Scheduler
</a><br>
