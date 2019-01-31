using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TayaIT.Enterprise.EMadbatah.Util;

namespace TayaIT.Enterprise.EMadbatah.DAL
{
    public static class UserHelper
    {
        public static long AddNewUser(string name, long roleID,  string domainUserName, string email, bool status)
        {
            try
            {
                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    if (context.Users.Count<User>() > 0)
                    {
                        User userTmp = context.Users.FirstOrDefault(c => c.DomainUserName == domainUserName);
                        if (userTmp != null)
                            return -1;
                    }
                    User user = new User
                    {
                        FName = name,
                        RoleID = roleID,
                        DomainUserName = domainUserName,
                        Status = status,
                        Email = email
                    };
                    context.Users.AddObject(user);
                    int result = context.SaveChanges();
                    return user.ID;
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.UserHelper.AddNewUser(" + name +")");
                return -1;
            }
        }

        public static int DeleteUserById(long userID)
        {
            try
            {
                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    User user = context.Users.FirstOrDefault(c => c.ID == userID);
                    //context.DeleteObject(user);
                    //SessionFileHelper.UnlockSessionFile(

                    if (!user.SessionFiles.IsLoaded)
                        user.SessionFiles.Load();

                    if (user.SessionFiles != null && user.SessionFiles.Count > 0)
                    {
                        foreach (SessionFile file in user.SessionFiles)
                        {
                            file.UserID = null;
                        }
                    }

                    var sessions = from session in context.Sessions
                                   where session.ReviewerID == userID
                                   select session;
                    if (sessions != null && sessions.Count<Session>() > 0)
                    {
                        foreach (Session s in sessions)
                        {
                            s.ReviewerID = null;
                        }
                    }

                    var sessionContentItems = from sci in context.SessionContentItems
                                              where sci.UserID == userID || sci.ReviewerUserID == userID
                                              select sci;

                    if (sessionContentItems != null && sessionContentItems.Count<SessionContentItem>() > 0)
                    {
                        foreach (SessionContentItem sci in sessionContentItems)
                        {
                            sci.ReviewerUserID = null;
                        }
                    }

                    var sessionStarts = from ss in context.SessionFiles
                                        where ss.UserID == userID &&
                                        ss.IsSessionStart == true
                                        select ss;
                    if (sessionStarts != null && sessionStarts.Count<SessionFile>() > 0)
                    {
                        foreach (SessionFile ss in sessionStarts)
                        {
                            ss.UserID = null;
                        }
                    }

                    user.Deleted = true;
                    int result = context.SaveChanges();
                    return result;
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.UserHelper.DeleteUserById(" + userID + ")");
                return -1;
            }
        }

        public static int UpdateUser(long userID,
                                             string name,
                                             string domainUserName,
                                             string email,
                                             bool status,
                                             long roleID)
        {
            try
            {
                User updated_user = null;
                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    updated_user = context.Users.FirstOrDefault(c => c.ID == userID);
                    if (updated_user != null)
                    {
                        //update the user attributes
                        updated_user.FName = string.IsNullOrEmpty(name) ? updated_user.FName : name;                        
                        updated_user.DomainUserName = string.IsNullOrEmpty(domainUserName) ? updated_user.DomainUserName : domainUserName;
                        updated_user.Email = string.IsNullOrEmpty(email) ? updated_user.Email : email;
                        updated_user.Status = status == null ? updated_user.Status : status;
                        updated_user.RoleID = roleID == -1 ? updated_user.RoleID : roleID;
                    }
                    else
                    {
                        return -1;
                    }
                    int res = context.SaveChanges();
                    return res;
                }
                
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.UserHelper.UpdateUser(" + userID + "," + name + "," + domainUserName + "," + email + "," + status + ")");
                return -1;
            }
            
        }


        public static User GetUserByID(long userID)
        {
            try
            {

                User user = null;
                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    user = context.Users.FirstOrDefault(c => c.ID == userID);
                }
                return user;
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.UserHelper.GetUserByID(" + userID + ")");
                return null;
            }
        }

       

        public static User GetUserByDomainName(string userDomainName)
        {
            try
            {
                User user = null;
                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    if (context.Users.Count<User>() > 0)
                        user = context.Users.FirstOrDefault(c => c.DomainUserName == userDomainName);
                }
                return user;
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.UserHelper.GetUserByDomainName(" + userDomainName + ")");
                return null;
            }
        }

        public static List<User> GetAllUsers()
        {
            try
            {
                List<User> users = null;
                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    //context.Users.ToList<User>();
                    users = (from user in context.Users
                             where (user.Deleted == false)
                             select user).ToList<User>();
                }
                return users;
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.UserHelper.GetAllUsers()");
                return null;
            }
        }

        public static List<User> GetACtiveUsers()
        {
            try
            {
                List<User> users = null;
                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    //context.Users.ToList<User>();
                    users = (from user in context.Users
                             where (user.Deleted == false && user.Status == true)
                             select user).ToList<User>();
                }
                return users;
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.UserHelper.GetAllUsers()");
                return null;
            }
        }


        public static int GetAllUsersCount()
        {
            try
            {
                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    return context.Users.Count<User>();
                }                
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.UserHelper.GetAllUsersCount()");
                return -1;
            }
        }

        public static int UpdateUserStatus(long userId, bool isActive)
        {
            try
            {
                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    User user = context.Users.FirstOrDefault(c => c.ID == userId);

                    if (user != null)
                    {
                        user.Status = isActive;
                        int res = context.SaveChanges();
                        return res;
                    }
                    else
                        return -1;

                }
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.UserHelper.UpdateUserStatus("+userId+ ","+isActive + ")");
                return -1;
            }
        }
        public static int UpdateUserRole(long userId, int roleID)
        {
            try
            {
                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    User user = context.Users.FirstOrDefault(c => c.ID == userId);

                    if (user != null)
                    {
                        user.RoleID = roleID;
                        int res = context.SaveChanges();
                        return res;
                    }
                    else
                        return -1;

                }
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.UserHelper.UpdateUserRole(" + userId + "," + roleID + ")");
                return -1;
            }
        }


        public static int UnDeleteUser(long userId, int userRoleId, string UserName, string UserDomainUserName, string UserEmail, bool isActive)
        {
            try
            {
                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    User user = context.Users.FirstOrDefault(c => c.ID == userId);

                    if (user != null)
                    {
                        user.Deleted = false;
                        user.Status = isActive;// false;
                        user.FName = UserName;
                        user.DomainUserName = UserDomainUserName;
                        user.Email = UserEmail;
                        user.RoleID = userRoleId;

                        int res = context.SaveChanges();
                        return res;
                    }
                    else
                        return -1;

                }
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.UserHelper.UnDeleteUser(" + userId  + ")");
                return -1;
            }
        }

    }
}
