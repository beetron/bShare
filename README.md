# bShare

<b>WORK IN PROGRESS!!!!</b>

bShare is a personal project for uploading and sharing files on the fly.<br><br>
Instructions on how to clone and replicate a working bShare web app below.<br><br>
Automatic mysql data deletion for expired uploads, and stored file deletion via python script.<br>

<hr>

[Language] <br>
-
C#, Javascript, HTML, CSS/Boostrap 5 <br><br>

[Framework] <br> 
-
Asp.NET Core 7.0 MVC <br><br>

[Database] <br>
-
MySQL (ORM found below)

<hr>

[Features]<br>
-
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
-
- Adding files in addition to selected files <br>

<hr>

[Replicate working project]<br>
-

Steps in replicating a working bShare web app.<br>
Some information may not apply to your environment, as each server solution is different.
These steps are necessary, if you are testing the web app out of the box.

1.) Required Environment Variables<br><br>
<code>
bShare_AppUrl = "https://your-apps-base-url.com/"<br>
<br>
bshare_DevConnectionString = "Your dev database connection string"<br>
<br>
bshare_ProdConnectionString = "Your prod database connection string"<br>
<br>
bshare_UploadLocation = "C:/inetpub/wwwroot/storage/bshare-uploads/"<br>
<br>
bshare_PyPath = "c:/python/scripts"<br>
</code>
