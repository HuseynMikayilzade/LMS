using LearningManagementSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Application.Utilities.Extentions
{
	public static class EmailBodyCreator
	{
		public static string EmailBody(string link)
		{
			string body = $@"
            <!DOCTYPE html>
            <html lang='en'>
            <head>
                <meta charset='UTF-8'>
                <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                <title>Reset Password</title>
                <style>
                    body {{
                        font-family: Arial, sans-serif;
                        line-height: 1.6;
                        margin: 0;
                        padding: 0;
                    }}
                    .container {{
                        max-width: 600px;
                        margin: 20px auto;
                        padding: 20px;
                        border: 1px solid #ccc;
                        border-radius: 5px;
                        background-color: #f9f9f9;
                    }}
                    h2 {{
                        color: #333;
                    }}
                    p {{
                        margin-bottom: 15px;
                    }}
                    a {{
                        display: inline-block;
                        padding: 10px 20px;
                        background-color: #ffa500;
                        color: #fff;
                        text-decoration: none;
                        border-radius: 3px;
                    }}
                    a:hover {{
                        background-color: #ff8c00;
                    }}
                </style>
            </head>
            <body>
                <div class='container'>
                    <h2>Reset Password</h2>
                    <p>Hello,</p>
                    <p>We understand that you need to reset your password. You can follow the steps below to reset your password:</p>
                    <ol>
                        <li>Please click the link below to reset your password:</li>
                        <li><a href='{link}' target='_blank'>Password Reset Link</a></li>       
                        <li>After clicking the link, you will be given instructions to create a new password.</li>
                    </ol>
                    <p>If you did not request this email or have any other issues with your account, please let us know.</p>
                    <p>Best regards,<br><strong>[Edura] Team</strong></p>
                </div>
            </body>
            </html>";
			return body;
		}

//		public static string EmailBodyForTask(Assignment assignment, Student student)
//		{
//            string body = $@"<!DOCTYPE html>
//<html lang=""en"">
//<head>
//    <meta charset=""UTF-8"">
//    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
//    <title>Task Cevaplandırıldı</title>
//</head>
//<body>
//    <div style=""max-width: 600px; margin: 0 auto; padding: 20px;"">
//        <h2>Taskiniz Cevaplandırıldı</h2>
//        <p>Merhaba [Öğrenci İsmi],</p>
//        <p>Size iletmek istediğimiz bilgiye göre, taskiniz başarıyla cevaplandırıldı.</p>
//        <p>Cevap:</p>
//        <p>[Öğrenci cevabı buraya gelecek]</p>
//        <p>Eğer herhangi bir sorunuz veya ek bilgi talebiniz varsa lütfen bize ulaşın.</p>
//        <p>İyi günler dileriz,</p>
//        <p>[Öğretmen İsmi]</p>
//    </div>
//</body>
//</html>
//";

//		}
	}
}
