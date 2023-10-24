# bShare

<b>WORK IN PROGRESS!!!!</b>

bShare is a personal project for uploading and sharing files on the fly.<br><br>
Instructions on how to clone and replicate a working bShare web app below.<br><br>
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

1.) Environment Variables<br>
```
bShare_AppUrl = "https://your-apps-base-url.com/"

bshare_DevConnectionString = "Your dev database connection string"

bshare_ProdConnectionString = "Your prod database connection string"

bshare_UploadLocation = "C:/your/file/storage/location/"

bshare_PyPath = "c:/your/script/location"
```

<br>

2.) MySQL event scheduler<br>
Setup the event scheduler to query the database for any expired file uploads.
If DateExpired value is older than the datetime when scheduler is ran, 
the record(row(s) will be deleted.
<br>

2-1.) First enable the scheduler.<br>
```
set global event_scheduler = on;
```
<br>

2-2.) Make sure scheduler is enabled.<br>
```
show variables like 'event_scheulder';
```
<br>

```c#
Console.WriteLine("blahblah");
```