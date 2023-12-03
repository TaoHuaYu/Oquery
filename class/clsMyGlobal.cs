using System;
//using System.DirectoryServices.AccountManagement;

namespace PostgreDB
{
    public class MyGlobal
    {
        public static string sTableNameMaster = "TEMP_LIST_VALUE_MASTER";
        public static string sTableNameDetail = "TEMP_LIST_VALUE_DETAIL";

        //public static bool CheckADAccount(string sAccount, string sPW)
        //{
        //    using (PrincipalContext pc = new PrincipalContext(ContextType.Domain, "ytec"))
        //    {
        //        sLoginAccount = sAccount.ToUpper();
        //        return pc.ValidateCredentials(sAccount, sPW);
        //    }
        //}

        //public static int CheckADAccount(string sAccount, string sPW)
        //{
        //    try
        //    {
        //        LdapConnection connection = new LdapConnection("LDAP://DC=ytec,DC=com,DC=tw");
        //        NetworkCredential credential = new NetworkCredential(sAccount, sPW);
        //        connection.Credential = credential;
        //        connection.Bind();

        //        sLoginAccount = sAccount.ToUpper();
        //        return 1;
        //    }
        //    catch (LdapException lexc)
        //    {
        //        //string error = lexc.ServerErrorMessage;
        //        //525​ user not found ​(1317)
        //        //52e​ invalid credentials ​(1326)
        //        //530​ not permitted to logon at this time​(1328)
        //        //531​ not permitted to logon at this workstation​(1329)
        //        //532​ password expired ​(1330)
        //        //533​ account disabled ​(1331)
        //        //701​ account expired ​(1793)
        //        //773​ user must reset password(1907)
        //        //775​ user account locked(1909)

        //        Console.WriteLine(lexc.ToString());
        //        return 0;
        //    }
        //    catch (Exception exc)
        //    {
        //        Console.WriteLine(exc.ToString());
        //        return 0;
        //    }
        //}

        public static void CloseApplication()
        {
            if (System.Windows.Forms.Application.MessageLoop)
            {
                //Use this since we are a WinForms app
                System.Windows.Forms.Application.Exit();
            }
            else
            {
                //Use this since we are a console app
                Environment.Exit(Environment.ExitCode);
            }
        }
    }
}