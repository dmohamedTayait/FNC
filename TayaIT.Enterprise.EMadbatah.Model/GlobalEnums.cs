using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TayaIT.Enterprise.EMadbatah.Model
{

    public enum SystemMailType
    {
        ApproveSession,//
        ChangeUserRole,//
        DeActivateUser,//
        DeleteUser,//
        FinalApproveSession,//
        NewUser,//
        ReAssignSessionFile,//
        ReAssignSessionRev,//
        SessionMarkedAsComplete,//code not implemented need revision
        UnlockSessionFile,//
        UnlockSessionRev,
        AssignSessionRev,//
        AssignSessionFile,//
        AssignSessionFileReviewer,//
        MadbatahFileCreated,
        FinalApproveSessionFail,
        MadbatahFilesCreationFailed,
        ReAssignSessionFileReviewer,
        UnlockSessionFileReviewer
    }

    public enum MadbatahFilesStatus
    {
        NotCreated = 1,
        InProgress = 2,
        DraftCreated = 3,
        FinalCreated = 4,
        DraftFail = 5,
        FinalFail = 6
    }

    public enum ErrorType
    {
        Generic = 1,
        Unauthorized = 2,
        UserinActive = 3,
        InvalidDomain = 4
    }

    public enum SignOutType
    {
        SignInAsDifferentUser = 1,
        SignOut = 2,
    }

    public enum EditorPageMode
    {
        Edit = 1,
        Correct = 2,
        Review = 3
    }

    public enum UserRole
    {
        Admin = 1,
        DataEntry = 2,
        Reviewer = 3,
        ReviewrDataEntry = 4,
        FileReviewer = 5
    }


    public enum AttendantState
    {
        Attended = 1,
        Absent = 2,
        Apology = 3,
        InMission = 4
    }

    public enum AttendantType
    {
        FromTheCouncilMembers = 1,
        FromOutsideTheCouncil = 2,
        GovernmentRepresentative = 3,
        Secretariat = 4,
        President = 5,
        NA = 6,
        UnKnown = 7
    }

    public enum AttendantTitle
    {
        M3alyPresident = 1,
        S3adetSessionPresident = 2,
        S3adet = 3,
        M3aly = 4,
        AlOstaz = 5,
        Almostashaar = 6,
        NoTitle = 7,
    }

    public enum SessionStatus
    {
        New = 1,
        InProgress = 2,
        Completed = 3,
        Approved = 4,
        FinalApproved = 5
    }

    public enum SessionFileStatus
    {
        New = 1,
        InProgress = 2,
        Completed = 3,//,
        SessionStartApproved = 4,
        SessionStartRejected = 5,
        SessionStartFixed = 6,
        SessionStartModifiedAfterApprove = 7
        //Approved = 4
    }

    //public enum SessionStartStatus
    //{
    //    New = 1,
    //    InProgress = 2,
    //    Completed = 3//,
    //    //Approved = 4
    //}

    public enum SessionContentItemStatus
    {
        Approved = 1,
        Rejected = 2,
        Fixed = 3,
        ModefiedAfterApprove = 4
    }

    public enum FileType//Attachement or Session File
    {
        Attachement = 1,
        SessionFile = 2
    }

    public enum FileExtensionType//Attachement or Session File
    {
        doc = 1,
        docx = 2,
        pdf = 3
    }

    public enum FileVersion//Attachement or Session File
    {
        draft = 1,
        final = 2
    }

    public enum TargetFormat
    {
        Pdf,
        Xps
    }

    public enum EmailType
    {
        AssignSessionFile, //give user session file
        RemoveSessionFileAssign, // take session file from user
        ChangeUserRole,

        MakeUserInActive,
        ActivateUser,

        AddUser,
        RemoveUser
    }

}
